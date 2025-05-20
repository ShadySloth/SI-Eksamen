
from sqlalchemy.ext.asyncio import AsyncSession
from sqlalchemy.exc import SQLAlchemyError
from sqlalchemy import text  # ← Dette er vigtigt!

async def check_database_health(session: AsyncSession) -> bool:
    try:
        await session.execute(text("SELECT 1"))  # ← Pak SQL i `text()`
        return True
    except SQLAlchemyError as e:
        import logging
        logging.error(f"Database health check failed: {e}")
        return False
