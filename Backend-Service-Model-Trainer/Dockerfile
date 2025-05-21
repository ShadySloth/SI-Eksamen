# Base image
FROM python:3.11-slim

# Installer systemafhængigheder
# libgl1 og libglib2.0-0 er nødvendige for OpenCV
# libsm6, libxext6, libxrender1 bruges ofte af cv2 GUI-bagends
RUN apt-get update && apt-get install -y --no-install-recommends \
    libgl1 \
    libglib2.0-0 \
    libsm6 \
    libxext6 \
    libxrender1 \
    && rm -rf /var/lib/apt/lists/*

# Set working directory
WORKDIR /app

# Copy requirements and install Python dependencies
COPY req.txt .
RUN pip install --no-cache-dir -r req.txt

# Copy application code
COPY . .

# Expose port (FastAPI default via uvicorn)
EXPOSE 8000

# Start the app
CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8000"]
