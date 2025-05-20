# repositories/ai_model_type_repo.py
from sqlmodel.ext.asyncio.session import AsyncSession
from sqlmodel import select
from fastapi import HTTPException, status

from app.entities.models.ai_model import AIModelType
from app.entities.schemes.ai_model_type_schema import AIModelTypeCreate


async def create_ai_model_type(session: AsyncSession, data: AIModelTypeCreate) -> AIModelType:
    model_type = AIModelType(**data.dict())
    session.add(model_type)
    await session.commit()
    await session.refresh(model_type)
    return model_type

async def get_ai_model_type_by_id(session: AsyncSession, model_type_id: int) -> AIModelType:
    result = await session.exec(select(AIModelType).where(AIModelType.id == model_type_id))
    model_type = result.first()
    if not model_type:
        raise HTTPException(status_code=404, detail="ModelType not found")
    return model_type

async def delete_ai_model_type(session: AsyncSession, model_type_id: int) -> None:
    model_type = await get_ai_model_type_by_id(session, model_type_id)
    await session.delete(model_type)
    await session.commit()
