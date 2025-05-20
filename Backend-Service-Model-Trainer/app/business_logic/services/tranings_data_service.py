from pathlib import Path
import yaml

from app.business_logic.extractors.extract_training_zip import TrainingZipHandler
from app.contexts.local_cloud_storage_context import CloudSession, LocalCloudStorageContext
from app.repositories.cloud.cloud_storage_repo import CloudStorageRepository


async def fetch_zip_for_training(zip_filename: str, session: CloudSession) -> bytes:
    async with LocalCloudStorageContext(session) as cloud_context:
        repo = CloudStorageRepository(cloud_context)
        zip_bytes = await repo.get_zip_file(zip_filename)

        if zip_bytes is None:
            raise FileNotFoundError(f"Dataset not found: {zip_filename}")

        return zip_bytes


async def get_training_data(map_key: str, session: CloudSession) -> str:
    # 1. Hent ZIP-filen som bytes fra din mock cloud storage
    zip_bytes = await fetch_zip_for_training(map_key, session)

    # 2. Brug context manager til at håndtere extraction og oprydning
    with TrainingZipHandler(zip_bytes) as handler:
        # 3. Få fat i stien til data.yaml
        yaml_path = handler.get_data_yaml()

        # 4. Læs og print indholdet
        with open(yaml_path, "r") as f:
            yaml_content = yaml.safe_load(f)
            print("✅ YAML-indhold:", yaml_content)

        # Returnér stien (valgfrit – du kan også returnere yaml_content direkte)
        return str(yaml_path)