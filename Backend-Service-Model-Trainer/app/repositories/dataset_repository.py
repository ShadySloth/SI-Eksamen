import random
import shutil
from pathlib import Path
from typing import List

import yaml
from sqlalchemy import text
from sqlmodel.ext.asyncio.session import AsyncSession
from app.entities.models.trainingset import Label, Image, Segmentation


class DatasetRepository:
    def __init__(self, session: AsyncSession):
        self.session = session

    async def get_or_create_label(self, name: str) -> Label:
        # Manuelt SELECT
        query = 'SELECT * FROM "Labels" WHERE "Name" = :name LIMIT 1'
        result = await self.session.execute(text(query), {"name": name})
        row = result.first()

        if row:
            label = Label(id=row.Id, name=row.Name)
            return label

        # Manuelt INSERT
        insert_query = 'INSERT INTO "Labels" ("Name") VALUES (:name) RETURNING "Id", "Name"'
        result = await self.session.execute(text(insert_query), {"name": name})
        await self.session.commit()
        row = result.first()
        label = Label(id=row.Id, name=row.Name)
        return label

    async def create_image(self, filename: str, label_ids: list[int] | None = None) -> Image:
        insert_image = 'INSERT INTO "Images" ("FileName") VALUES (:filename) RETURNING "Id", "FileName"'
        result = await self.session.execute(text(insert_image), {"filename": filename})
        await self.session.commit()
        row = result.first()
        image = Image(id=row.Id, filename=row.FileName)

        if label_ids:
            # Manuelt indsæt links i ImageLabel
            for label_id in label_ids:
                insert_link = (
                    'INSERT INTO "ImageLabel" ("image_id", "label_id") VALUES (:image_id, :label_id) '
                    'ON CONFLICT DO NOTHING'
                )
                await self.session.execute(text(insert_image, insert_link), {"image_id": image.id, "label_id": label_id})
            await self.session.commit()

            # Hvis du ønsker at opdatere image.labels (men normalt man holder det ORM), kan du vælge at hente labels her.

        return image

    async def create_segmentation(
            self,
            image_id: int,
            label_id: int,
            first_x: float,
            first_y: float,
            second_x: float,
            second_y: float,
    ) -> Segmentation:
        insert_seg = '''
        INSERT INTO "Segmentations" 
            ("FirstCoordinateX", "FirstCoordinateY", "SecondCoordinateX", "SecondCoordinateY", "LabelId", "ImageId")
        VALUES
            (:first_x, :first_y, :second_x, :second_y, :label_id, :image_id)
        RETURNING "Id", "ImageId", "LabelId", "FirstCoordinateX", "FirstCoordinateY", "SecondCoordinateX", "SecondCoordinateY"
        '''
        result = await self.session.execute(
            text(insert_seg),
            {
                "image_id": image_id,
                "label_id": label_id,
                "first_x": first_x,
                "first_y": first_y,
                "second_x": second_x,
                "second_y": second_y,
            },
        )
        await self.session.commit()
        row = result.first()
        segmentation = Segmentation(
            id=row.Id,
            image_id=row.ImageId,
            label_id=row.LabelId,
            first_coordinate_x=row.FirstCoordinateX,
            first_coordinate_y=row.FirstCoordinateY,
            second_coordinate_x=row.SecondCoordinateX,
            second_coordinate_y=row.SecondCoordinateY,
        )
        return segmentation



    async def get_all_labels(self) -> List[Label]:
        query = 'SELECT "Id", "Name" FROM "Labels" ORDER BY "Id"'
        result = await self.session.execute(text(query))
        labels = [Label(id=row.Id, name=row.Name) for row in result.fetchall()]
        return labels

    async def get_all_images_with_segmentations(self) -> List[dict]:
        # Returner liste af dicts med billede og tilhørende segmenteringer
        query = '''
            SELECT 
                i."Id" AS image_id, 
                i."FileName" AS filename,
                s."Id" AS seg_id,
                s."LabelId" AS label_id,
                s."FirstCoordinateX" AS fx,
                s."FirstCoordinateY" AS fy,
                s."SecondCoordinateX" AS sx,
                s."SecondCoordinateY" AS sy
            FROM "Images" i
            LEFT JOIN "Segmentations" s ON s."ImageId" = i."Id"
            ORDER BY i."Id"
        '''
        result = await self.session.execute(text(query))
        rows = result.fetchall()

        images = {}
        for row in rows:
            img_id = row.image_id
            if img_id not in images:
                images[img_id] = {
                    "filename": row.filename,
                    "segmentations": []
                }
            if row.seg_id:
                images[img_id]["segmentations"].append({
                    "label_id": row.label_id,
                    "first_x": row.fx,
                    "first_y": row.fy,
                    "second_x": row.sx,
                    "second_y": row.sy,
                })
        return list(images.values())

    async def create_yolo_dataset(
        self,
        base_path: str,
        train_split: float = 0.7,
        val_split: float = 0.2,
        test_split: float = 0.1
    ):
        # Valider splits
        assert abs(train_split + val_split + test_split - 1.0) < 1e-6, "Splits skal give 1.0"

        base_path = Path(base_path)
        (base_path / "images" / "train").mkdir(parents=True, exist_ok=True)
        (base_path / "images" / "val").mkdir(parents=True, exist_ok=True)
        (base_path / "images" / "test").mkdir(parents=True, exist_ok=True)
        (base_path / "labels" / "train").mkdir(parents=True, exist_ok=True)
        (base_path / "labels" / "val").mkdir(parents=True, exist_ok=True)
        (base_path / "labels" / "test").mkdir(parents=True, exist_ok=True)

        labels = await self.get_all_labels()
        label_id_to_idx = {label.id: idx for idx, label in enumerate(labels)}  # YOLO class ids 0..N-1

        # Skriv YAML-fil med klasser og stier
        yaml_content = {
            'path': str(base_path.resolve()),
            'train': 'images/train',
            'val': 'images/val',
            'test': 'images/test',
            'names': [label.name for label in labels]
        }
        with open(base_path / "dataset.yaml", "w", encoding="utf-8") as f:
            yaml.dump(yaml_content, f, sort_keys=False)

        # Hent alle billeder + segmenteringer
        images = await self.get_all_images_with_segmentations()
        random.shuffle(images)

        n = len(images)
        n_train = int(n * train_split)
        n_val = int(n * val_split)
        # Rest er test

        splits = {
            'train': images[:n_train],
            'val': images[n_train:n_train + n_val],
            'test': images[n_train + n_val:]
        }

        def save_label_file(label_path: Path, segmentations: List[dict]):
            with open(label_path, "w", encoding="utf-8") as f:
                for seg in segmentations:
                    cls_idx = label_id_to_idx[seg['label_id']]
                    # Beregn bbox i YOLO format (relative)
                    x_center = (seg['first_x'] + seg['second_x']) / 2
                    y_center = (seg['first_y'] + seg['second_y']) / 2
                    width = abs(seg['second_x'] - seg['first_x'])
                    height = abs(seg['second_y'] - seg['first_y'])
                    f.write(f"{cls_idx} {x_center:.6f} {y_center:.6f} {width:.6f} {height:.6f}\n")

        for split_name, imgs in splits.items():
            for img in imgs:
                src = Path(img['filename'])
                if not src.exists():
                    print(f"Billedfil ikke fundet: {src}, springer over")
                    continue
                dest_img_path = base_path / "images" / split_name / src.name
                shutil.copy(src, dest_img_path)

                label_file = dest_img_path.with_suffix('.txt')
                save_label_file(label_file, img['segmentations'])

        print(f"Dataset oprettet i {base_path}")

