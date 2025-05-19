from fastapi import APIRouter, Depends, HTTPException
from sqlmodel.ext.asyncio.session import AsyncSession

from app.db_contexts.mariadb_session import get_session
from app.services.health_service import is_database_healthy

router = APIRouter()

@router.get("/is_healthy", status_code=200)
async def is_healthy(session: AsyncSession = Depends(get_session)):
    if await is_database_healthy(session):
        return {"status": "ok"}
    raise HTTPException(status_code=503, detail="Database not available")