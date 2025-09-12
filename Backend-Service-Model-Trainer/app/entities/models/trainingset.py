from sqlmodel import SQLModel, Field, Relationship
from typing import List, Optional



class Image(SQLModel, table=True):
    __tablename__ = "Images"

    id: Optional[int] = Field(default=None, primary_key=True, alias="Id")
    filename: str = Field(..., alias="FileName")


class Label(SQLModel, table=True):
    __tablename__ = "Labels"

    id: Optional[int] = Field(default=None, primary_key=True, alias="Id")
    name: str = Field(..., alias="Name")


class Segmentation(SQLModel, table=True):
    __tablename__ = "Segmentations"

    id: Optional[int] = Field(default=None, primary_key=True, alias="Id")

    first_coordinate_x: float = Field(..., alias="FirstCoordinateX")
    first_coordinate_y: float = Field(..., alias="FirstCoordinateY")
    second_coordinate_x: float = Field(..., alias="SecondCoordinateX")
    second_coordinate_y: float = Field(..., alias="SecondCoordinateY")

    label_id: int = Field(..., alias="LabelId")  # uden foreign_key her

    image_id: int = Field(..., alias="ImageId")  # uden foreign_key her
