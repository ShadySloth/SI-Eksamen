from pathlib import Path

from app.repositories.interfaces.icloud_storage_context import ICloudStorageContext


class CloudStorageRepository:
    def __init__(self, context: ICloudStorageContext):
        self.context = context

    async def get_zip_file(self, zip_filename: str) -> bytes:
        """
        Returnerer path til ZIP-filen i det mockede cloud storage.
        """
        dataset_id = Path(zip_filename).stem  # fjerner .zip hvis tilføjet
        return await self.context.get_dataset_as_bytes(dataset_id)
