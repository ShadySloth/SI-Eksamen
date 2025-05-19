from sqlmodel.ext.asyncio.session import AsyncSession
from app.repositories.session_test_repo import check_database_health


async def is_database_healthy(session: AsyncSession) -> bool:
    return await check_database_health(session)