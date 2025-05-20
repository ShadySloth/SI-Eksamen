from enum import Enum

from enum import Enum
from typing import Optional, List
from sqlmodel import SQLModel, Field, Relationship
from sqlalchemy import Column, Enum as SAEnum


class ModelTypeEnum(str, Enum):
    YOLOV5S = "yolov5s"      # Small version af YOLOv5
    YOLOV5N = "yolov5n"      # Nano - endnu mindre end small
    YOLOV8N = "yolov8n"      # YOLOv8 Nano
    YOLOV8S = "yolov8s"      # Small version af YOLOv8
    YOLOV6 = "yolov6"
    YOLOV7 = "yolov7"
    YOLOV10 = "yolov10"


# Opret liste baseret på enum
default_model_types = [
    {"name": "YOLOv5 Small", "type": ModelTypeEnum.YOLOV5S},
    {"name": "YOLOv5 Nano", "type": ModelTypeEnum.YOLOV5N},
    {"name": "YOLOv6", "type": ModelTypeEnum.YOLOV6},
    {"name": "YOLOv7", "type": ModelTypeEnum.YOLOV7},
    {"name": "YOLOv8 Nano", "type": ModelTypeEnum.YOLOV8N},
    {"name": "YOLOv8 Small", "type": ModelTypeEnum.YOLOV8S},
    {"name": "YOLOv10", "type": ModelTypeEnum.YOLOV10},
]
