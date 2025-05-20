from sqlmodel import SQLModel


class AIModelTypeCreate(SQLModel):
    name: str
    type: str

class AIModelTypeRead(SQLModel):
    id: int
    name: str
    type: str