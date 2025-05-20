# controllers/ai_model_type_controller.py
from fastapi import APIRouter, Depends
from sqlmodel.ext.asyncio.session import AsyncSession

from app.db_contexts.mariadb_session import get_session
from app.entities.schemes.ai_model_type_schema import AIModelTypeCreate, AIModelTypeRead
from app.services.ai_model_services.ai_model_type_service import create_model_type, read_model_type_by_id, \
    delete_model_type

router = APIRouter(prefix="/api/model-types", tags=["Model Types"])

@router.post("/", response_model=AIModelTypeRead)
async def create_model_type_endpoint(
    data: AIModelTypeCreate,
    session: AsyncSession = Depends(get_session)
):
    return await create_model_type(session, data)

@router.get("/{model_type_id}", response_model=AIModelTypeRead)
async def get_model_type_endpoint(
    model_type_id: int,
    session: AsyncSession = Depends(get_session)
):
    return await read_model_type_by_id(session, model_type_id)

@router.delete("/{model_type_id}", status_code=204)
async def delete_model_type_endpoint(
    model_type_id: int,
    session: AsyncSession = Depends(get_session)
):
    await delete_model_type(session, model_type_id)
