from pathlib import Path
from typing import Optional

from pydantic import BaseModel

from app.repositories.interfaces.icloud_storage_context import ICloudStorageContext

# todo skal have rigtige værdier fra token ved rigtig cloud
class CloudSession(BaseModel):
    user_id: Optional[str] = None
    token: Optional[str] = None
    storage_root: str = "blob"  # Dette matcher din docker-compose mount


class LocalCloudStorageContext(ICloudStorageContext):
    def __init__(self, session: CloudSession):
        # ✅ Korrekt brug af Pydantic-model attribut
        self.root_path = Path(session.storage_root)

    async def __aenter__(self) -> "LocalCloudStorageContext":
        return self

    async def __aexit__(self, exc_type, exc_val, exc_tb):
        pass

    async def get_dataset_as_bytes(self, dataset_id: str) -> bytes:
        zip_path = self.root_path / f"{dataset_id}.zip"

        if not zip_path.exists():
            raise FileNotFoundError(f"Dataset ZIP not found at: {zip_path}")

        return zip_path.read_bytes()
