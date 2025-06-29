# OrderManagement

## Overview
OrderManagement is a .NET 8 Web API for managing customers and orders, featuring:
- Discounting system with customer segment rules
- Order status tracking with valid state transitions
- Order analytics endpoint (average value, fulfillment time)
- Swagger/OpenAPI documentation
- Unit and integration tests

## Features
- **Discount System:** Applies different promotion rules based on customer segments and order history.
- **Order Status Tracking:** Supports valid state transitions (Pending → Shipped → Delivered).
- **Order Analytics:** Provides average order value and fulfillment time via `/api/orders/analytics`.

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Running the API
1. Navigate to the project directory:
   ```sh
   cd OrderManagement
   ```
2. Run the API:
   ```sh
   dotnet run
   ```
3. Open your browser at the URL shown in the console (e.g., `https://localhost:7122/swagger`).

### API Documentation
- Swagger UI is available at `/swagger` when the API is running.
- All endpoints are documented with XML comments and response types.

### Running Tests
1. Navigate to the test project directory:
   ```sh
   cd ../OrderManagement.Tests
   ```
2. Run all tests:
   ```sh
   dotnet test
   ```
   - Unit tests for discount logic and DTOs will run.
   - Integration test is skipped unless the API is running.

## API Examples

### Create a Customer
```http
POST /api/customers
Content-Type: application/json

{
  "name": "John Doe"
}
```

**Response:**
```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "name": "John Doe",
  "orderCount": 0,
  "totalSpent": 0,
  "createdAt": "2024-01-01T00:00:00Z"
}
```

### Create an Order
```http
POST /api/orders
Content-Type: application/json

{
  "customerId": "123e4567-e89b-12d3-a456-426614174000",
  "total": 150.00
}
```

**Response:**
```json
{
  "id": "456e7890-e89b-12d3-a456-426614174000",
  "customerId": "123e4567-e89b-12d3-a456-426614174000",
  "total": 150.00,
  "status": "Pending",
  "createdAt": "2024-01-01T00:00:00Z",
  "deliveredAt": null
}
```

### Update Order Status
```http
PUT /api/orders/456e7890-e89b-12d3-a456-426614174000/status
Content-Type: application/json

"Shipped"
```

### Get Order Analytics
```http
GET /api/orders/analytics
```

**Response:**
```json
{
  "averageOrderValue": 125.50,
  "totalOrders": 10,
  "averageFulfillmentTime": "2.5 days"
}
```

### Calculate Discount
```http
POST /api/orders/456e7890-e89b-12d3-a456-426614174000/discount
```

**Response:**
```json
15.00
```

## Performance Optimization

### Current Optimizations
- **Use of `.AsNoTracking()`:**
  - All read-only queries use `.AsNoTracking()` for improved performance by disabling change tracking in Entity Framework Core.
  - Example:
    ```csharp
    var customers = await _context.Customers.AsNoTracking().ToListAsync();
    ```

### Future Optimizations
- **Caching:** Implement Redis or in-memory caching for frequently accessed data (customer profiles, analytics).
- **Pagination:** Add pagination for large datasets (e.g., `/api/orders?page=1&pageSize=20`).
- **Database Indexing:** Add indexes on frequently queried fields (customerId, status, createdAt).
- **Async/Await:** All database operations are already async for better scalability.
- **Compression:** Enable response compression for large datasets.

### Performance Considerations
- The API uses an in-memory database for demonstration, which is fast but not persistent.
- For production, consider using a real database with proper indexing and connection pooling.
- Monitor query performance and add logging for slow operations.

## Assumptions
- The API uses an in-memory database for demonstration and testing.
- Discount strategies can be extended by implementing `IDiscountStrategy`.
- Integration tests require the API to be running or the `Program` class to be made public for full automation.
- All monetary values are stored as `decimal` for precision.
- Order status transitions follow a specific flow: Pending → Shipped → Delivered.

## Project Structure
- `Controllers/` - API endpoints with full Swagger documentation
- `Dtos/` - Data transfer objects with validation attributes
- `Models/` - Entity models with business logic
- `Services/` - Business logic and analytics
- `Tests/` - Unit and integration tests
- `Mappings/` - AutoMapper profiles for object mapping

## API Endpoints Summary

### Customers
- `GET /api/customers` - Get all customers
- `GET /api/customers/{id}` - Get customer by ID
- `POST /api/customers` - Create new customer
- `PUT /api/customers/{id}` - Update customer

### Orders
- `GET /api/orders` - Get all orders (with optional customer filter)
- `PUT /api/orders/{id}/status` - Update order status
- `GET /api/orders/analytics` - Get order analytics
- `POST /api/orders/{id}/discount` - Calculate discount for order

## Contact
For questions or suggestions, please open an issue or submit a pull request.

## API Documentation (Swagger UI)

Below is a screenshot of the automatically generated Swagger UI for this project:

![Swagger UI Screenshot](swagger.png)