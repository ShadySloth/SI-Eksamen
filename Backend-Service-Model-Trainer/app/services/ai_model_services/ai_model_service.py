from fastapi import HTTPException
from sqlalchemy.ext.asyncio import AsyncSession
from typing import List
from app.entities.models.ai_model import AIModel
from app.entities.schemes.ai_model import AIModelCreate
from app.repositories.ai_model_repos import ai_model_repo


async def create_model(session: AsyncSession, model: AIModelCreate) -> AIModel:
    return await ai_model_repo.create_ai_model(session, model)

async def get_model(session: AsyncSession, model_id: int) -> AIModel:
    model = await ai_model_repo.get_ai_model(session, model_id)
    if not model:
        raise HTTPException(status_code=404, detail="Model not found")
    return model

async def get_models(session: AsyncSession) -> List[AIModel]:
    return await ai_model_repo.get_all_ai_models(session)

async def delete_model(session: AsyncSession, model_id: int):
    db_model = await ai_model_repo.get_ai_model(session, model_id)
    if not db_model:
        raise HTTPException(status_code=404, detail="Model not found")
    await ai_model_repo.delete_ai_model(session, db_model)

