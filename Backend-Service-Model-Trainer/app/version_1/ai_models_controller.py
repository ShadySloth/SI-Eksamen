import os
import shutil
from uuid import uuid4

import cv2
from fastapi import APIRouter, UploadFile, File
from sqlmodel.ext.asyncio.session import AsyncSession
from starlette.responses import FileResponse

from app.business_logic.runners import yolo_runner
from app.business_logic.services.ai_model_services.ai_model_service import get_model
from app.business_logic.services.tranings_data_service import train_model_on_set
from app.contexts.local_cloud_storage_context import CloudSession
from app.contexts.mariadb_session import get_session
from app.entities.schemes.ai_model import AIModelCreate, TrainingRequest

router = APIRouter()

from fastapi import Depends
from typing import List
from app.entities.models.ai_model import AIModel
from app.business_logic.services.ai_model_services import ai_model_service


@router.post("/", response_model=AIModel)
async def create(model: AIModelCreate, session: AsyncSession = Depends(get_session)):
    return await ai_model_service.create_model(session, model)

@router.get("/", response_model=List[AIModel])
async def read_all(session: AsyncSession = Depends(get_session)):
    return await ai_model_service.get_models(session)

@router.get("/{model_id}", response_model=AIModel)
async def read_one(model_id: int, session: AsyncSession = Depends(get_session)):
    return await ai_model_service.get_model(session, model_id)


@router.delete("/{model_id}")
async def delete(model_id: int, session: AsyncSession = Depends(get_session)):
    await ai_model_service.delete_model(session, model_id)
    return {"ok": True}


@router.post("/test/{model_id}", response_class=FileResponse)
async def run_model_on_img(
    model_id: int,
    uploaded_file: UploadFile = File(...),
    session: AsyncSession = Depends(get_session)
):
    model = await get_model(session, model_id)
    # ── 1. Gem den uploadede fil til disk ─────────────────────────────
    upload_dir = "uploads"
    os.makedirs(upload_dir, exist_ok=True)

    # Brug et unikt filnavn for at undgå konflikter
    filename = f"{uuid4()}_{uploaded_file.filename}"
    file_path = os.path.join(upload_dir, filename)

    with open(file_path, "wb") as buffer:
        shutil.copyfileobj(uploaded_file.file, buffer)

    # ── 2. Kør YOLO detektion med stien til den gemte fil ─────────────
    detected_image = await yolo_runner.detect(
        model_path= model.path,
        image=file_path  # ← bruger den gemte sti her
    )

    # ── 3. Gem output billede ─────────────────────────────────────────
    output_path = f"results/detected_{model_id}.png"
    cv2.imwrite(output_path, detected_image)

    # ── 4. Returnér billedet som download ─────────────────────────────
    return FileResponse(
        output_path,
        media_type="image/png",
        filename="detected.png"
    )


@router.post("/train/")
async def train_model(request: TrainingRequest, session_sql: AsyncSession = Depends(get_session)):
    # Mock CloudSession – i rigtig cloud ville du tilføje auth, bucket info etc.
    session_cloud = CloudSession()

    try:
        output_path = await train_model_on_set(request.new_model_name, request.training_data_name, request.epochs, request.image_size , session_cloud)

        model = AIModelCreate(
            name=request.new_model_name,
            path=output_path,
            type="object_detection",
            description="YOLOv8n model finetuned for detecting custom objects",
            model_type_id=1
        )

        await ai_model_service.create_model(session_sql, model)

        return {"message": "Fuck ja, du har lavet din egen AI min ven (NERD)"}
    except Exception as e:
        return {"error": str(e)}
