# Base image
FROM python:3.11-slim

# Set workdir
WORKDIR /app

# Install dependencies
COPY req.txt .
RUN pip install --no-cache-dir -r req.txt

# Copy application code
COPY . .

# Expose port (FastAPI default via uvicorn)
EXPOSE 8000

# Start the app
CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8000"]
