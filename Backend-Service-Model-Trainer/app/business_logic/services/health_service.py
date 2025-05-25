from sqlmodel.ext.asyncio.session import AsyncSession

from app.repositories.init_db_repo import create_all_tables
from app.repositories.session_test_repo import check_database_health
from app.repositories.sql.init_postgre import create_all_tables_with_categories_post


async def is_database_healthy(session: AsyncSession) -> bool:
    return await check_database_health(session)


async def initialize_database(session: AsyncSession, session_post: AsyncSession):
    await create_all_tables(session)
    await create_all_tables_with_categories_post(session_post)

