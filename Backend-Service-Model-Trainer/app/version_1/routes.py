#declares the route for this api version if included in main
from fastapi import APIRouter
from . import ai_trainer_controller, ai_models_controller, health_controller

router = APIRouter()

router.include_router(ai_trainer_controller.router, prefix="/ai/trainer", tags=["AI Trainer"])
router.include_router(ai_models_controller.router, prefix="/ai/models", tags=["AI Models"])
router.include_router(health_controller.router, prefix="/health", tags=["Health"])

