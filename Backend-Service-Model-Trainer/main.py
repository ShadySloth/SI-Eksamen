from fastapi import FastAPI
from app.version_1.routes import router as v1_router

app = FastAPI()

app.include_router(v1_router, prefix="/api", tags=["api_routes"])

