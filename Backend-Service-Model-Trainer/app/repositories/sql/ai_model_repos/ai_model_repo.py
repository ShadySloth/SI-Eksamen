from sqlmodel import select
from typing import List, Optional

from sqlmodel.ext.asyncio.session import AsyncSession

from app.entities.models.ai_model import AIModel
from app.entities.schemes.ai_model import AIModelCreate


from sqlalchemy.future import select
from sqlalchemy.orm import selectinload

async def create_ai_model(session: AsyncSession, data: AIModelCreate) -> AIModel:
    model = AIModel(**data.dict())
    session.add(model)
    await session.commit()
    await session.refresh(model)
    return model

async def get_ai_model(session: AsyncSession, model_id: int) -> Optional[AIModel]:
    return await session.get(AIModel, model_id)

async def get_all_ai_models(session: AsyncSession) -> List[AIModel]:
    result = await session.execute(select(AIModel))
    return result.scalars().all()

async def delete_ai_model(session: AsyncSession, model: AIModel):
    await session.delete(model)
    await session.commit()

