from abc import ABC, abstractmethod

class IAITrainer(ABC):
    @abstractmethod
    async def train(self, data: dict) -> str:
        """Train the model with provided data."""
        pass