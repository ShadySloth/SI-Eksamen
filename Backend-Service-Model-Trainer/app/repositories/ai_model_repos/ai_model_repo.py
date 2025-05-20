from sqlmodel import select
from typing import List, Optional

from sqlmodel.ext.asyncio.session import AsyncSession

from app.entities.models.ai_model import AIModel

async def create_ai_model(session: AsyncSession, model: AIModel) -> AIModel:
    session.add(model)
    await session.commit()
    await session.refresh(model)
    return model

async def get_ai_model(session: AsyncSession, model_id: int) -> Optional[AIModel]:
    return await session.get(AIModel, model_id)

async def get_all_ai_models(session: AsyncSession) -> List[AIModel]:
    result = await session.execute(select(AIModel))
    return result.scalars().all()

async def update_ai_model(session: AsyncSession, db_model: AIModel, updated: AIModel) -> AIModel:
    db_model.name = updated.name
    db_model.path = updated.path
    db_model.description = updated.description
    db_model.trained_at = updated.trained_at
    await session.commit()
    await session.refresh(db_model)
    return db_model

async def delete_ai_model(session: AsyncSession, model: AIModel):
    await session.delete(model)
    await session.commit()

