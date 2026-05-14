#!/bin/bash
set -e

# Start Python LSTM service in background on port 8000
cd /app/ml
uvicorn serve:app --port 8000 &

# Give Python a moment to load the models before .NET starts
sleep 5

# Start .NET app on the Railway-provided port
cd /app
ASPNETCORE_URLS="http://0.0.0.0:${PORT:-8080}" exec dotnet Crowdlens-backend.dll
