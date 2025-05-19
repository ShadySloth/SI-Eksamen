from sqlalchemy.ext.asyncio import AsyncSession
from sqlalchemy.exc import SQLAlchemyError

async def check_database_health(session: AsyncSession) -> bool:
    try:
        await session.execute("SELECT 1")
        return True
    except SQLAlchemyError:
        return False