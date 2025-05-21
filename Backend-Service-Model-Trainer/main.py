import os

from fastapi import FastAPI
from app.version_1.routes import router as v1_router

# Sørg for at 'blob'-mappen findes i roden
if not os.path.exists("blob"):
    os.makedirs("blob")


app = FastAPI()

app.include_router(v1_router, prefix="/api")

