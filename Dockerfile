# Stage 1: Build .NET app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dotnet-build
WORKDIR /src
COPY *.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /out --no-restore

# Stage 2: Runtime image with .NET + Python
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Install Python
RUN apt-get update && \
    apt-get install -y python3 python3-pip && \
    rm -rf /var/lib/apt/lists/*

# Copy .NET published output
COPY --from=dotnet-build /out .

# Copy ML folder
COPY ml/ ./ml/

# Install CPU-only torch (much smaller than default CUDA build) + other deps
RUN pip3 install --no-cache-dir --break-system-packages \
    torch --index-url https://download.pytorch.org/whl/cpu && \
    pip3 install --no-cache-dir --break-system-packages \
    numpy scikit-learn fastapi "uvicorn[standard]" pydantic

# Copy and permission the startup script
COPY start.sh .
RUN chmod +x start.sh

CMD ["./start.sh"]
