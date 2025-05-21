import tempfile
import zipfile
import os
from pathlib import Path


class TrainingZipHandler:
    def __init__(self, zip_bytes: bytes):
        self.zip_bytes = zip_bytes
        self._temp_dir = tempfile.TemporaryDirectory()
        self.temp_path = Path(self._temp_dir.name)
        self.zip_path = self.temp_path / "dataset.zip"
        self.extracted_path: Path | None = None
        self.data_yaml_path: Path | None = None

    def extract(self):
        # Gem zip midlertidigt
        with open(self.zip_path, "wb") as f:
            f.write(self.zip_bytes)

        # Unzip til temp dir
        with zipfile.ZipFile(self.zip_path, "r") as zip_ref:
            zip_ref.extractall(self.temp_path)

        self.extracted_path = self.temp_path
        self.data_yaml_path = self._find_data_yaml(self.extracted_path)

    @staticmethod
    def _find_data_yaml(folder: Path) -> Path:
        for root, _, files in os.walk(folder):
            if "data.yaml" in files:
                return Path(root) / "data.yaml"
        raise FileNotFoundError("data.yaml not found in extracted zip")

    def get_data_yaml(self) -> Path:
        if not self.data_yaml_path:
            raise ValueError("Zip not yet extracted or YAML not found")
        return self.data_yaml_path

    def get_dataset_root(self) -> Path:
        if not self.extracted_path:
            raise ValueError("Zip not yet extracted")
        return self.extracted_path

    def cleanup(self):
        self._temp_dir.cleanup()

    def __enter__(self):
        self.extract()
        return self

    def __exit__(self, exc_type, exc_val, exc_tb):
        self.cleanup()
