from fastapi import APIRouter

router = APIRouter()

@router.get("/")
def get_training_status():
    return {"version": "v1", "trainer": "status OK"}



