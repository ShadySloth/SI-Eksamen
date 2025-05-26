import zipfile
import tempfile
import yaml
import os
from pathlib import Path
import asyncio

from sqlmodel.ext.asyncio.session import AsyncSession

from app.repositories.dataset_repository import DatasetRepository


# Antag at DatasetRepository er importeret og tilgængelig
# from your_module import DatasetRepository

import os
import zipfile
import tempfile
import shutil
import yaml
from pathlib import Path

async def print_and_store_yolo8_labels(zip_path, session):
    def load_data_yaml(path_to_yaml):
        with open(path_to_yaml, 'r') as f:
            return yaml.safe_load(f)

    def read_label_file(label_path):
        boxes = []
        if not os.path.isfile(label_path):
            return boxes
        with open(label_path, 'r') as f:
            for line in f:
                line = line.strip()
                if line == '':
                    continue
                parts = line.split()
                if len(parts) != 5:
                    continue
                boxes.append(parts)
        return boxes

    async def process_split(base_path, split_name, repo, categories):
        images_dir = os.path.join(base_path, split_name, 'images')
        labels_dir = os.path.join(base_path, split_name, 'labels')

        print(f"\n--- {split_name.upper()} ---")
        if not os.path.isdir(images_dir):
            print(f"Directory not found: {images_dir}")
            return

        target_dir = Path("../blob/temp")
        target_dir.mkdir(parents=True, exist_ok=True)

        images = [f for f in os.listdir(images_dir) if f.lower().endswith(('.jpg', '.jpeg', '.png'))]
        for img in sorted(images):
            label_file = os.path.join(labels_dir, os.path.splitext(img)[0] + '.txt')
            boxes = read_label_file(label_file)
            if not boxes:
                print(f"{img}: No boxes")
                continue

            print(f"Image: {img}")

            source_img_path = Path(images_dir) / img
            target_img_path = target_dir / img

            # Kopier kun hvis fil ikke findes i target
            if not target_img_path.exists():
                shutil.copy(source_img_path, target_img_path)
                print(f"Copied {img} to {target_img_path}")

            # Gem i repo med den korrekte sti (relativ eller absolut)
            image_obj = await repo.create_image(filename=str(target_img_path))

            for box in boxes:
                class_idx = int(box[0])
                x_center, y_center, w, h = map(float, box[1:])
                category = categories[class_idx]
                print(f"  Class {class_idx} ({category}): {x_center} {y_center} {w} {h}")

                await repo.create_segmentation(
                    image_id=image_obj.id,
                    first_x=x_center,
                    first_y=y_center,
                    second_x=w,
                    second_y=h,
                    label_id=class_idx + 1  # Typisk label_id starter ved 1
                )

    with tempfile.TemporaryDirectory() as tmpdirname:
        with zipfile.ZipFile(zip_path, 'r') as zip_ref:
            zip_ref.extractall(tmpdirname)

        data_yaml_path = os.path.join(tmpdirname, 'data.yaml')
        if not os.path.isfile(data_yaml_path):
            print("data.yaml not found in the zip archive")
            return

        data = load_data_yaml(data_yaml_path)

        print("Categories:")
        categories = data.get('names', [])
        repo = DatasetRepository(session)

        # Opret labels i databasen
        category_objects = []
        for cat_name in categories:
            category_obj = await repo.get_or_create_label(cat_name)
            category_objects.append(category_obj)
            print(f"  - {cat_name}")

        # Kør igennem splits
        for split in ['train', 'val', 'test']:
            await process_split(tmpdirname, split, repo, categories)

async def loadDataSet(session: AsyncSession):
    repo = DatasetRepository(session)

    await repo.create_yolo_dataset(base_path='blob')