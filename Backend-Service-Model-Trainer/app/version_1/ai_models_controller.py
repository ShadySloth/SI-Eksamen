from fastapi import APIRouter
from sqlmodel.ext.asyncio.session import AsyncSession

from app.db_contexts.mariadb_session import get_session

router = APIRouter()

from fastapi import Depends
from typing import List
from app.entities.models.ai_model import AIModel
from app.services.ai_model_services import ai_model_service


@router.post("/", response_model=AIModel)
async def create(model: AIModel, session: AsyncSession = Depends(get_session)):
    return await ai_model_service.create_model(session, model)

@router.get("/", response_model=List[AIModel])
async def read_all(session: AsyncSession = Depends(get_session)):
    return await ai_model_service.get_models(session)

@router.get("/{model_id}", response_model=AIModel)
async def read_one(model_id: int, session: AsyncSession = Depends(get_session)):
    return await ai_model_service.get_model(session, model_id)

@router.put("/{model_id}", response_model=AIModel)
async def update(model_id: int, model: AIModel, session: AsyncSession = Depends(get_session)):
    return await ai_model_service.update_model(session, model_id, model)

@router.delete("/{model_id}")
async def delete(model_id: int, session: AsyncSession = Depends(get_session)):
    await ai_model_service.delete_model(session, model_id)
    return {"ok": True}
