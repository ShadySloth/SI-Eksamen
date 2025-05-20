from abc import ABC, abstractmethod
from typing import Protocol
from pathlib import Path


class ICloudStorageContext(ABC):
    @abstractmethod
    async def __aenter__(self) -> "ICloudStorageContext":
        ...

    @abstractmethod
    async def __aexit__(self, exc_type, exc_val, exc_tb):
        ...

    @abstractmethod
    async def get_dataset_as_bytes(self, dataset_id: str) -> bytes:
        ...
