from app.business_logic.trainers.sam_trainer import SAMTrainer
from app.business_logic.interfaces.IAITrainer import IAITrainer
from app.business_logic.trainers.yolov8n import YOLO8NTrainer
from app.entities.models.model_type_enums import ModelTypeEnum


class AITrainingContext:
    def __init__(self, model_type: str):
        self.trainer = self._get_trainer(model_type)

    def _get_trainer(self, model_type: str) -> IAITrainer:
        #sam er bare mock for at vise hvordan man kan inlkudere nye
        if model_type == "sam":
            return SAMTrainer()
        elif model_type == ModelTypeEnum.YOLOV8N:
            return YOLO8NTrainer()
        else:
            raise ValueError(f"Unsupported model type: {model_type}")

    async def train(self, data: dict) -> str:
        return await self.trainer.train(data)
