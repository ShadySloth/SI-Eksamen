import os

from fastapi import FastAPI
from app.version_1.routes import router as v1_router
from fastapi.middleware.cors import CORSMiddleware

# Sørg for at 'blob'-mappen findes i roden
if not os.path.exists("./blob"):
    os.makedirs("./blob")

app = FastAPI()

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],  # Add the URL of your frontend application
    allow_credentials=True,
    allow_methods=["*"],  # Or specify the methods you want to allow
    allow_headers=["*"],  # Or specify the headers you want to allow
    expose_headers=["Content-Disposition"],  # Explicitly expose the header
)

app.include_router(v1_router, prefix="/api")

