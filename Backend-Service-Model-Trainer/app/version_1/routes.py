#declares the route for this api version if included in main
from fastapi import APIRouter
from . import ai_models_controller, health_controller, ai_model_type_controller

router = APIRouter()

router.include_router(ai_model_type_controller.router, prefix="/ai/models/type", tags=["AI Model Types"])
router.include_router(ai_models_controller.router, prefix="/ai/models", tags=["AI Models"])
router.include_router(health_controller.router, prefix="/health", tags=["Health"])

