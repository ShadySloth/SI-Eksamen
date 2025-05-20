from sqlmodel import SQLModel
from sqlmodel.ext.asyncio.session import AsyncSession
from app.entities.models.ai_model import AIModelType
from app.entities.models.model_type_enums import default_model_types


async def seed_ai_model_types(session: AsyncSession):
    # Hvis tom, opret alle standard typer
    for model_type in default_model_types:
        ai_model_type = AIModelType(name=model_type["name"], type=model_type["type"])
        session.add(ai_model_type)
    try:
        await session.commit()
    except Exception as e:
        await session.rollback()
        raise RuntimeError(f"Failed to seed AIModelTypes: {str(e)}")


async def create_all_tables(session: AsyncSession):
    try:
        await drop_all_tables(session)
        await session.run_sync(lambda sync_session: SQLModel.metadata.create_all(bind=sync_session.get_bind()))
        await seed_ai_model_types(session)
    except Exception as e:
        raise RuntimeError(f"Error creating tables: {str(e)}")


async def drop_all_tables(session: AsyncSession):
    try:
        await session.run_sync(lambda sync_session: SQLModel.metadata.drop_all(bind=sync_session.get_bind()))
    except Exception as e:
        print(f"Error dropping tables: {e}")