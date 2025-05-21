import os
from typing import Union

import cv2
import numpy as np
from ultralytics import YOLO


async def detect_with_boxes(model_path: str, image: Union[str, np.ndarray]) -> dict:
    """
    Kører object detection på et billede med en trænet model.

    Args:
        model_path (str): Sti til .pt model
        image (str | np.ndarray): Filsti eller allerede-loaded billede som numpy array

    Returns:
        dict: indeholder detekterede objekter og billedet med boxes
    """

    if not os.path.isfile(model_path):
        raise ValueError("Model path is invalid")

    model = YOLO(model_path)

    # Load billedet
    if isinstance(image, str):
        img = cv2.imread(image)
        if img is None:
            raise ValueError("Could not read image from path")
    elif isinstance(image, np.ndarray):
        img = image
    else:
        raise TypeError("Image must be a file path or numpy array")

    # Kør detektion
    results = model(img)[0]

    detections = []
    for box in results.boxes:
        cls_id = int(box.cls[0].item())
        cls_name = model.names[cls_id]
        conf = float(box.conf[0].item())
        x1, y1, x2, y2 = map(int, box.xyxy[0].tolist())

        # Gem info
        detections.append({
            "class": cls_name,
            "confidence": conf,
            "box": [x1, y1, x2, y2]
        })

        # Tegn boks og label på billedet
        cv2.rectangle(img, (x1, y1), (x2, y2), (0, 255, 0), 2)
        cv2.putText(img, f"{cls_name} {conf:.2f}", (x1, y1 - 10),
                    cv2.FONT_HERSHEY_SIMPLEX, 0.6, (0, 255, 0), 2)

    return {
        "detections": detections,
        "image_with_boxes": img  # Kan vises med cv2.imshow eller gemmes med cv2.imwrite
    }


async def detect(model_path: str, image: Union[str, np.ndarray]) -> np.ndarray:
    """
    Kører object detection og viser billedet med detekterede objekter.

    Args:
        model_path (str): Sti til trænet model (.pt)
        image (str | np.ndarray): Billede som sti eller numpy array

    Returns:
        np.ndarray: Billedet med bokse tegnet på
    """
    print("runs detection")
    if not os.path.isfile(model_path):
        raise ValueError("Model path is invalid")

    model = YOLO(model_path)

    # Load billede
    if isinstance(image, str):
        img = cv2.imread(image)
        if img is None:
            raise ValueError("Could not read image from path")
    elif isinstance(image, np.ndarray):
        img = image
    else:
        raise TypeError("Image must be a file path or numpy array")

    # Inference
    results = model(img)[0]

    for box in results.boxes:
        cls_id = int(box.cls[0].item())
        cls_name = model.names[cls_id]
        conf = float(box.conf[0].item())
        x1, y1, x2, y2 = map(int, box.xyxy[0].tolist())

        # Tegn box og label
        cv2.rectangle(img, (x1, y1), (x2, y2), (0, 255, 0), 2)
        cv2.putText(img, f"{cls_name} {conf:.2f}", (x1, y1 - 10),
                    cv2.FONT_HERSHEY_SIMPLEX, 0.6, (0, 255, 0), 2)

    return img