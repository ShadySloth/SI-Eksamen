from fastapi import APIRouter, HTTPException

from app.business_logic.trainers.ai_training_context import AITrainingContext

router = APIRouter()


@router.post("/train-model/")
async def train_model_endpoint(payload: dict):
    # todo skal kunne hente trainings folder fra id, unpack og finde yaml filen

    # todo skal lave requst object der kan mappes om til den rigtige json format se train metoder for opbygning

    # todo sørg for at den gemmer det korrekt og opretter model i db med rigtige path

    # todo lav endpoint til simpel training

    model_type = "yolov8n"
    if not model_type:
        raise HTTPException(status_code=400, detail="Missing model_type")

    try:
        training_context = AITrainingContext(model_type)
        model_path = await training_context.train(payload)
        return {"message": "Training completed", "path": model_path}
    except ValueError as ve:
        raise HTTPException(status_code=400, detail=str(ve))
