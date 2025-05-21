from typing import Optional

from pydantic import BaseModel, Field
from sqlmodel import SQLModel


class AIModelCreate(SQLModel):
    name: str
    path: str
    type: str
    description: str
    model_type_id: Optional[int]  # Hvis der er ForeignKey til AIModelType


class TrainingRequest(BaseModel):
    new_model_name: str
    selected_model: str
    training_data_name: str
    epochs: Optional[int] = Field(default=10, description="Number of training epochs")
    image_size: Optional[int] = Field(default=640, description="Size of training images")