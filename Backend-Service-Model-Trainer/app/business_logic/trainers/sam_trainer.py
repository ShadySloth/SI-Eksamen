from app.business_logic.interfaces.IAITrainer import IAITrainer

"""
bare mock for at vise setup for ny implimentation
"""


class SAMTrainer(IAITrainer):
    async def train(self, data: dict) -> str:
        # Træningslogik for SAM
        print("Training SAM with", data)
        return "sam_model_path.pt"