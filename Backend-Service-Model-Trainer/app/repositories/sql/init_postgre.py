from sqlmodel import SQLModel
from sqlmodel.ext.asyncio.session import AsyncSession

from app.entities.models.trainingset import Label

default_categories = [
    {"name": "Person"},
    {"name": "Car"},
    {"name": "Dog"},
    {"name": "Cat"},
]

async def seed_categories_post(session: AsyncSession):
    try:
        for category_data in default_categories:
            category = Label(name=category_data["name"])
            session.add(category)
        await session.commit()
    except Exception as e:
        await session.rollback()
        raise RuntimeError(f"Failed to seed categories: {str(e)}")


async def create_all_tables_with_categories_post(session: AsyncSession):
    try:
        await drop_all_tables_post(session)
        await session.run_sync(lambda sync_session: SQLModel.metadata.create_all(bind=sync_session.get_bind()))
        await seed_categories_post(session)
    except Exception as e:
        raise RuntimeError(f"Error creating tables and seeding categories: {str(e)}")


async def drop_all_tables_post(session: AsyncSession):
    try:
        await session.run_sync(lambda sync_session: SQLModel.metadata.drop_all(bind=sync_session.get_bind()))
    except Exception as e:
        print(f"Error dropping tables: {e}")
