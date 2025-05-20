from typing import Optional

from sqlmodel import SQLModel


class AIModelCreate(SQLModel):
    name: str
    path: str
    type: str
    description: str
    model_type_id: Optional[int]  # Hvis der er ForeignKey til AIModelType