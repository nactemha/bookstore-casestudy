#!/bin/sh

# Navigate to your project directory (if needed)
cd /app

# Run migrations
dotnet ef database update

# Start your app
dotnet ecommerce-casestudy.dll
