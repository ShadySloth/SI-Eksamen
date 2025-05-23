import os
import asyncio
from ultralytics import YOLO

from app.business_logic.interfaces.IAITrainer import IAITrainer


class YOLO8NTrainer(IAITrainer):
    async def train(self, data: dict) -> str:
        """
        Træner YOLOv8n med det givne data.
        Kræver at data indeholder:
        - data_yaml_path: sti til YAML fil
        - epochs: antal epochs
        - imgsz: billedstørrelse
        - output_dir: hvor modellen skal gemmes
        """
        data_yaml_path = data.get("data_yaml_path")
        epochs = data.get("epochs", 10)
        imgsz = data.get("imgsz", 640)
        output_dir = data.get("output_dir", "results")

        if not os.path.isfile(data_yaml_path):
            raise ValueError("Invalid or missing data_yaml_path")

        # Sikr output directory findes
        os.makedirs(output_dir, exist_ok=True)

        model = YOLO("yolov8n.yaml")  # starter fra scratch — eller brug "yolov8n.pt" for fine-tuning

        def train_sync():
            model.train(
                data=data_yaml_path,
                epochs=epochs,
                imgsz=imgsz,
                project=output_dir,
                name="yolo8n_model",
                exist_ok=True
            )

        loop = asyncio.get_event_loop()
        await loop.run_in_executor(None, train_sync)

        final_model_path = os.path.join(output_dir, "yolo8n_model", "weights", "best.pt")
        return final_model_path
