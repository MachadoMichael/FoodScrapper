# FoodScrapper

FoodScrapper is a .NET Core Web API project designed to scrape and manage food composition data. It provides endpoints for managing foods and their nutritional components, along with scraping capabilities to collect data from external sources.

## Features

- **Food Management**: CRUD operations for food items
- **Component Management**: CRUD operations for food components/nutrients
- **Data Scraping**: Automated scraping of food and component data
- **PostgreSQL Database**: Persistent storage using Entity Framework Core
- **Swagger Documentation**: Interactive API documentation
- **CORS Support**: Configured for cross-origin requests

## API Endpoints

### Food Endpoints

- `POST /api/foods` - Create a new food item
- `GET /api/foods` - Get all food items
- `GET /api/foods/{id}` - Get a specific food item
- `PUT /api/foods/{id}` - Update a food item
- `DELETE /api/foods/{id}` - Delete a food item
- `DELETE /api/foods/deleteAll` - Delete all food items

### Component Endpoints

- `POST /api/components` - Create a new component
- `GET /api/components` - Get all components
- `GET /api/components/{id}` - Get a specific component
- `GET /api/components/food/{foodId}` - Get components by food ID
- `PUT /api/components/{id}` - Update a component
- `DELETE /api/components/{id}` - Delete a component
- `DELETE /api/components/deleteAll` - Delete all components

### Scrapper Endpoints

- `POST /api/scrapper/foods` - Trigger food data scraping
- `POST /api/scrapper/components` - Trigger component data scraping

## Data Models

### Food
{
    "id": int,
    "code": string,
    "name": string,
    "scientificName": string,
    "group": string,
    "brand": string
}

### Component
{
    "id": int,
    "name": string,
    "units": string,
    "valuePer100g": float,
    "standardDeviation": string,
    "minValue": float?,
    "maxValue": float?,
    "numberOfDataUsed": int?,
    "references": string,
    "dataType": enum["Calculated", "Analytical", "Assumed"],
    "foodId": int
}

## Technical Details

- **Framework**: .NET Core
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core
- **Documentation**: Swagger/OpenAPI
- **HTTP Client**: Configured with 5-minute timeout for scraping operations
- **Port**: Runs on port 4000

## Setup

1. Ensure PostgreSQL is installed and running
2. Configure the connection string in your application settings
3. Run database migrations
4. Start the application

## Configuration

The application includes:
- CORS configuration allowing all origins
- Swagger UI available at the root URL
- Entity Framework Core with PostgreSQL
- HTTP client with extended timeout for scraping operations

## Error Handling

The API implements consistent error handling with:
- Detailed system messages for debugging
- User-friendly messages for client applications
- Appropriate HTTP status codes
- Request timeout handling for long-running operations

## Dependencies

- Microsoft.EntityFrameworkCore
- Npgsql.EntityFrameworkCore.PostgreSQL
- Swashbuckle.AspNetCore
- HtmlAgilityPack (for web scraping)
