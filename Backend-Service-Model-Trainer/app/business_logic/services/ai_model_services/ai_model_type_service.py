# services/ai_model_type_service.py
from sqlmodel.ext.asyncio.session import AsyncSession

from app.entities.models.ai_model import AIModelType
from app.entities.schemes.ai_model_type_schema import AIModelTypeCreate
from app.repositories.sql.ai_model_repos import ai_model_type_repo
from app.repositories.sql.ai_model_repos.ai_model_type_repo import create_ai_model_type, get_ai_model_type_by_id, \
    delete_ai_model_type


async def create_model_type(session: AsyncSession, data: AIModelTypeCreate) -> AIModelType:
    return await create_ai_model_type(session, data)


async def read_model_type_by_id(session: AsyncSession, model_type_id: int) -> AIModelType:
    return await get_ai_model_type_by_id(session, model_type_id)

async def delete_model_type(session: AsyncSession, model_type_id: int) -> None:
    await delete_ai_model_type(session, model_type_id)


async def get_all_model_types(session: AsyncSession):
    return await ai_model_type_repo.get_all_model_types(session)