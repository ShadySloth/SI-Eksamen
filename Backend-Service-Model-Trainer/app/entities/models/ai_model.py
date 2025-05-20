from datetime import datetime
from sqlmodel import SQLModel, Field, Relationship
from typing import Optional, List


class AIModel(SQLModel, table=True):
    id: Optional[int] = Field(default=None, primary_key=True)
    name: str
    path: str
    trained_at: datetime = Field(default_factory=datetime.utcnow)
    description: Optional[str] = None
    # Relationer
    model_types: List["AIModelTypeLink"] = Relationship(back_populates="model")


class AIModelType(SQLModel, table=True):
    id: Optional[int] = Field(default=None, primary_key=True)
    name: str
    type: str
    models: List["AIModelTypeLink"] = Relationship(back_populates="model_type")


class AIModelTypeLink(SQLModel, table=True):
    model_id: int = Field(foreign_key="aimodel.id", primary_key=True)
    model_type_id: int = Field(foreign_key="aimodeltype.id", primary_key=True)

    model: "AIModel" = Relationship(back_populates="model_types")
    model_type: "AIModelType" = Relationship()

