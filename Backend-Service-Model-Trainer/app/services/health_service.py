from sqlmodel.ext.asyncio.session import AsyncSession

from app.repositories.init_db_repo import create_all_tables
from app.repositories.session_test_repo import check_database_health


async def is_database_healthy(session: AsyncSession) -> bool:
    return await check_database_health(session)


async def initialize_database(session: AsyncSession):
    await create_all_tables(session)