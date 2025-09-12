# controllers/ai_model_type_controller.py
from pathlib import Path
from typing import List

from fastapi import APIRouter, Depends
from sqlmodel.ext.asyncio.session import AsyncSession

from app.business_logic.services.ai_model_services.ai_model_type_service import create_model_type, \
    read_model_type_by_id, delete_model_type
from app.business_logic.services.dataset_service import print_and_store_yolo8_labels, loadDataSet
from app.contexts.local_cloud_storage_context import LocalCloudStorageContext, CloudSession
from app.contexts.mariadb_session import get_session
from app.contexts.postgre_session import get_postgre_session
from app.entities.models.ai_model import AIModelType
from app.entities.schemes.ai_model_type_schema import AIModelTypeCreate, AIModelTypeRead
from app.business_logic.services.ai_model_services import ai_model_type_service
from app.repositories.dataset_repository import DatasetRepository

router = APIRouter()

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


@router.get("/", response_model=List[AIModelType])
async def read_all_model_types(session: AsyncSession = Depends(get_session)):
    return await ai_model_type_service.get_all_model_types(session)


@router.post("/upload-yolo/")
async def upload_yolo(
    path: str,
    session_cloud: CloudSession,
    session: AsyncSession = Depends(get_postgre_session),
):
    await print_and_store_yolo8_labels(path, session)
    return {"status": "success"}

@router.post("/create-yolo-set/")
async def upload_yolo(
    session: AsyncSession = Depends(get_postgre_session),
):
    await loadDataSet(session)
    return {"status": "success"}
