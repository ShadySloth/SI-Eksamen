from fastapi import APIRouter, Depends, HTTPException
from sqlmodel.ext.asyncio.session import AsyncSession

from app.business_logic.services.health_service import is_database_healthy, initialize_database
from app.contexts.mariadb_session import get_session

router = APIRouter()

@router.get("/is_healthy", status_code=200)
async def is_healthy(session: AsyncSession = Depends(get_session)):
    if await is_database_healthy(session):
        return {"status": "ok"}
    raise HTTPException(status_code=503, detail="Database not available")


@router.get("/init_db", status_code=200)
async def init_db_endpoint(session: AsyncSession = Depends(get_session)):
    try:
        await initialize_database(session)
        return {"status": "database initialized"}
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Database initialization failed: {str(e)}")