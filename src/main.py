from fastapi import FastAPI
from datetime import datetime, timezone

app = FastAPI(
    title="Alumni Tracking System API",
    description="Backend API for managing university alumni, careers, and networks.",
    version="0.1.0",
)


@app.get("/")
def read_root():
    return {
        "project": "Alumni Tracking System",
        "status": "online",
        "current_week": "01 - Project inception & fundamentals",
        "message": "Welcome to the Alumni Tracking System API.",
    }


@app.get("/health")
def health_check():
    return {
        "status": "healthy",
        "timestamp": datetime.now(timezone.utc).isoformat(),
        "service": "alumni-web",
    }
