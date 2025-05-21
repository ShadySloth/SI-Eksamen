from fastapi import APIRouter
from sqlmodel.ext.asyncio.session import AsyncSession

from app.business_logic.services.tranings_data_service import train_model_on_set
from app.contexts.local_cloud_storage_context import CloudSession
from app.contexts.mariadb_session import get_session
from app.entities.schemes.ai_model import AIModelCreate, TrainingRequest

router = APIRouter()

from fastapi import Depends
from typing import List
from app.entities.models.ai_model import AIModel
from app.business_logic.services.ai_model_services import ai_model_service


@router.post("/", response_model=AIModel)
async def create(model: AIModelCreate, session: AsyncSession = Depends(get_session)):
    return await ai_model_service.create_model(session, model)

@router.get("/", response_model=List[AIModel])
async def read_all(session: AsyncSession = Depends(get_session)):
    return await ai_model_service.get_models(session)

@router.get("/{model_id}", response_model=AIModel)
async def read_one(model_id: int, session: AsyncSession = Depends(get_session)):
    return await ai_model_service.get_model(session, model_id)


@router.delete("/{model_id}")
async def delete(model_id: int, session: AsyncSession = Depends(get_session)):
    await ai_model_service.delete_model(session, model_id)
    return {"ok": True}


@router.post("/train/{model_id}")
async def train_model(request: TrainingRequest, session_sql: AsyncSession = Depends(get_session)):
    # Mock CloudSession – i rigtig cloud ville du tilføje auth, bucket info etc.
    session_cloud = CloudSession()

    try:
        output_path = await train_model_on_set(request.training_data_name, request.epochs, request.image_size , session_cloud)

        model = AIModelCreate(
            name=request.new_model_name,
            path=output_path,
            type="object_detection",
            description="YOLOv8n model finetuned for detecting custom objects",
            model_type_id=request.selected_model  # eller None, hvis du ikke vil knytte til en modeltype
        )
        await ai_model_service.create_model(session_sql, model)

        return {"message": "Fuck ja, du har lavet din egen AI min ven (NERD) og den ligger lige her: ", "path": output_path}
    except Exception as e:
        return {"error": str(e)}

