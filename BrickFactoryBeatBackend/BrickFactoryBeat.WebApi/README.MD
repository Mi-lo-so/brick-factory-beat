# BrickFactoryBeat.WebApi

## Swagger
See the swagger rendition for the API at:
```bash
http://localhost:5100/swagger/index.html
```

## SQL Server Database
### Create server
Run 
```bash
dotnet ef migrations add InitialCreate --project .\BrickFactoryBeat.Infrastructure --startup-project .\BrickFactoryBeat.WebApi
```
to ensure the AppDbContextFactory creates a server
### Apply Migrations
Run
```bash
 dotnet ef database update   --project .\BrickFactoryBeat.Infrastructure --startup-project .\BrickFactoryBeat.WebApi  
```
to apply migrations to the database.

## Usage
Below are examples of how to use the API via curl commands.
### Create Equipment

```bash
curl -vk -X POST https://localhost:7097/Equipment/Create -H "Content-Type: application/json"  -d "{\"Name\":\"Drill\",\"Type\":\"PowerTool\"}"
```

### Get All Equipment

```bash
curl -vk -X GET https://localhost:7097/Equipment
```

### Get Equipment by ID

```bash
TBD
```

### Update Equipment

```bash
curl -X POST "https://localhost:7243/api/equipment/{id}/state?state=Red"
```
### Assign Order to Equipment

```bash
curl -X POST "https://localhost:7243/api/equipment/{id}/order/{orderId}"
```

### Get Order by equipment id

```bash
curl -vk -X GET https://localhost:7097/Equipment/421897ec-5a41-4b26-8459-137efa86d555/order
```

### Get history by equipment id

```bash
curl -vk -X GET https://localhost:7097/Equipment/421897ec-5a41-4b26-8459-137efa86d555/history
```