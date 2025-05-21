# Base image
FROM python:3.11-slim

# Installer systempakker
#libgl1 bruges af open cv i bla yolo trainings
RUN apt-get update && apt-get install -y libgl1 && rm -rf /var/lib/apt/lists/*

# Set workdir
WORKDIR /app

# Install Python dependencies
COPY req.txt .
RUN pip install --no-cache-dir -r req.txt

# Copy application code
COPY . .

# Expose port (FastAPI default via uvicorn)
EXPOSE 8000

# Start the app
CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8000"]
