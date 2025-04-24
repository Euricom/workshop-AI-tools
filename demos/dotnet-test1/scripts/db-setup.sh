#!/bin/bash

# Function to check if Docker is running
check_docker() {
    if ! docker info > /dev/null 2>&1; then
        echo "Docker is not running. Please start Docker and try again."
        exit 1
    fi
}

# Function to wait for SQL Server to be ready
wait_for_sqlserver() {
    echo "Waiting for SQL Server to be ready..."
    while ! docker exec webshop-sqlserver /opt/mssql-tools/bin/sqlcmd \
        -S localhost -U sa -P "Your_Strong_Password123" \
        -Q "SELECT 1" > /dev/null 2>&1; do
        echo -n "."
        sleep 1
    done
    echo "SQL Server is ready!"
}

# Start Docker containers
start_containers() {
    echo "Starting Docker containers..."
    docker-compose up -d
    wait_for_sqlserver
}

# Stop Docker containers
stop_containers() {
    echo "Stopping Docker containers..."
    docker-compose down
}

# Create database migrations
create_migration() {
    if [ -z "$1" ]; then
        echo "Please provide a migration name"
        exit 1
    fi
    echo "Creating migration: $1"
    dotnet ef migrations add $1 \
        --project src/MyApp.Infrastructure \
        --startup-project src/MyApp.API \
        --output-dir Persistence/Migrations
}

# Apply database migrations
apply_migrations() {
    echo "Applying migrations..."
    dotnet ef database update \
        --project src/MyApp.Infrastructure \
        --startup-project src/MyApp.API
}

# Remove database migrations
remove_migrations() {
    echo "Removing last migration..."
    dotnet ef migrations remove \
        --project src/MyApp.Infrastructure \
        --startup-project src/MyApp.API
}

# Show help
show_help() {
    echo "Database Management Script"
    echo "Usage:"
    echo "  ./db-setup.sh [command]"
    echo ""
    echo "Commands:"
    echo "  start         Start Docker containers"
    echo "  stop          Stop Docker containers"
    echo "  migrate       Apply all pending migrations"
    echo "  add [name]    Create a new migration"
    echo "  remove        Remove the last migration"
    echo "  help          Show this help message"
}

# Main script
check_docker

case "$1" in
    "start")
        start_containers
        ;;
    "stop")
        stop_containers
        ;;
    "migrate")
        apply_migrations
        ;;
    "add")
        create_migration "$2"
        ;;
    "remove")
        remove_migrations
        ;;
    "help"|"")
        show_help
        ;;
    *)
        echo "Unknown command: $1"
        show_help
        exit 1
        ;;
esac