from fastapi import APIRouter, Query

from app.business_logic.services.tranings_data_service import get_training_data
from app.contexts.local_cloud_storage_context import CloudSession

router = APIRouter()


@router.get("/test-training-yaml")
async def test_training_yaml(map_key: str = Query(..., description="Navn eller path til zip-filen")):
    # Mock CloudSession – i rigtig cloud ville du tilføje auth, bucket info etc.
    session = CloudSession()

    try:
        yaml_path = await get_training_data(map_key, session)
        return {"message": "YAML fundet og udskrevet", "yaml_path": yaml_path}
    except Exception as e:
        return {"error": str(e)}