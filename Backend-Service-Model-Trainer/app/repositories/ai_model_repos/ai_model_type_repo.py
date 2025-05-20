# repositories/ai_model_type_repo.py
from fastapi import HTTPException, status
from sqlmodel import select
from sqlmodel.ext.asyncio.session import AsyncSession

from app.entities.models.ai_model import AIModelType
from app.entities.schemes.ai_model_type_schema import AIModelTypeCreate


async def create_ai_model_type(session: AsyncSession, data: AIModelTypeCreate) -> AIModelType:
    model_type = AIModelType(**data.dict())
    session.add(model_type)
    await session.commit()
    await session.refresh(model_type)
    return model_type


async def get_ai_model_type_by_id(session: AsyncSession, model_type_id: int) -> AIModelType:
    result = await session.get(AIModelType, model_type_id)
    if not result:
        raise HTTPException(status_code=404, detail="ModelType not found")
    return result


async def delete_ai_model_type(session: AsyncSession, model_type_id: int) -> None:
    model_type = await get_ai_model_type_by_id(session, model_type_id)
    await session.delete(model_type)
    await session.commit()


async def get_all_model_types(session: AsyncSession) -> list[AIModelType]:
    result = await session.execute(select(AIModelType))
    return result.scalars().all()