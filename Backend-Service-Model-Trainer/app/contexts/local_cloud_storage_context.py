from contextlib import asynccontextmanager
from pathlib import Path
import boto3
from pydantic import BaseModel

from app.repositories.interfaces.icloud_storage_context import ICloudStorageContext

class CloudSession(BaseModel):
    user_id: str
    token: str
    storage_root: str = "/mnt/cloud-storage/datasets"


class LocalCloudStorageContext(ICloudStorageContext):
    def __init__(self, session: CloudSession):
        self.root_path = Path(session.config["base_path"])

    async def __aenter__(self) -> "LocalCloudStorageContext":
        # Ingen async ops i mock, men holder strukturen
        return self

    async def __aexit__(self, exc_type, exc_val, exc_tb):
        pass



    async def get_dataset_as_bytes(self, dataset_id: str) -> bytes:
        zip_path = self.root_path / f"{dataset_id}.zip"

        if not zip_path.exists():
            raise FileNotFoundError(f"Dataset ZIP not found at: {zip_path}")

        return zip_path.read_bytes()


