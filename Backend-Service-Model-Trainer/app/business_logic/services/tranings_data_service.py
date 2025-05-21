from app.business_logic.extractors.extract_training_zip import TrainingZipHandler
from app.business_logic.trainers.ai_training_context import AITrainingContext
from app.contexts.local_cloud_storage_context import CloudSession, LocalCloudStorageContext
from app.entities.models.model_type_enums import ModelTypeEnum
from app.repositories.cloud.cloud_storage_repo import CloudStorageRepository

async def fetch_zip_for_training(zip_filename: str, session: CloudSession) -> bytes:
    async with LocalCloudStorageContext(session) as cloud_context:
        repo = CloudStorageRepository(cloud_context)
        zip_bytes = await repo.get_zip_file(zip_filename)

        if zip_bytes is None:
            raise FileNotFoundError(f"Dataset not found: {zip_filename}")

        return zip_bytes


async def train_model_on_set(map_key: str, epochs: int, imgsz: int, session: CloudSession) -> str:
    # 1. Hent ZIP-filen som bytes fra din mock cloud storage
    zip_bytes = await fetch_zip_for_training(map_key, session)

    # 2. Brug context manager til at håndtere extraction og oprydning
    with TrainingZipHandler(zip_bytes) as handler:
        # 3. Få fat i stien til data.yaml
        yaml_path = handler.get_data_yaml()

        mock_data = {
            "data_yaml_path": yaml_path,
            "epochs": epochs,
            "imgsz": 416,
            "output_dir": "results"
        }

        context = AITrainingContext(model_type=ModelTypeEnum.YOLOV8N)
        model_path = await context.train(mock_data)
        print(f"Trænet model gemt her: {model_path}")

        # Returnér stien
        return str(model_path)