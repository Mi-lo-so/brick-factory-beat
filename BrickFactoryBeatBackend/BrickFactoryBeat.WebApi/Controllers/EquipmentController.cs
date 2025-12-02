using BrickFactoryBeat.Application.Services;
using BrickFactoryBeat.Domain.Equipment;
using BrickFactoryBeat.Domain.Orders;
using Microsoft.AspNetCore.Mvc;

namespace BrickFactoryBeat.WebApi.Controllers;

public class CreateEquipmentRequest
{
    public string Name { get; set; }
    public EquipmentType Type { get; set; } = EquipmentType.BrickMold; // default
}

public class UpdateStateRequest
{
    public EquipmentState State { get; set; }
}

public class OrderDto
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public OrderType OrderType { get; set; }
    public string? EquipmentId { get; set; }
    public string? Status { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

}

public class CreateOrderRequest
{
    public OrderDto order { get; set; }
}

[ApiController]
[Route("[controller]")]
public class EquipmentController(IEquipmentService service) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await service.GetAllEquipmentAsync());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id) => Ok(await service.GetEquipmentByIdAsync(id.ToString()));

    
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateEquipmentRequest request)
    {
        try
        {
            var result = await service.CreateEquipmentAsync(request.Name, request.Type);
            Console.WriteLine(result);
            return Ok(result);
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    
    
    [HttpPost("{id:guid}/state")]
    public async Task<IActionResult> UpdateState(Guid id, [FromBody] UpdateStateRequest request)
    {
        await service.UpdateStateAsync(id.ToString(), request.State);
        return Ok();
    }
    
    [HttpPost("{id:guid}/order")]
    public async Task<IActionResult> Order(Guid id, [FromBody] CreateOrderRequest request)
    {
        var order = new Order
        {
            Id = Guid.NewGuid().ToString(),
            Title = request.order.Title ?? "Order " + Guid.NewGuid().ToString(),
            OrderType = request.order.OrderType,
            EquipmentId = id.ToString(),
            Status = "Pending",
            StartedAt = DateTime.UtcNow
        };

        await service.AddOrderToEquipmentAsync(id.ToString(), order);

        return Ok(order);
    }
    


    [HttpPost("{id:guid}/order/{orderId}")]
    public async Task<IActionResult> AssignOrder(Guid id, string orderId, string? title)
    {
        Order order = new Order
        {
            Id = orderId,
            Title = title ?? "Order " + orderId
        };
        await service.AddOrderToEquipmentAsync(id.ToString(), order);
        return Ok();
    }
    
    [HttpPost("{id:guid}/startOrders")]
    public async Task<IActionResult> StartOrder(Guid id)
    {
        await service.StartNextOrderAsync(id.ToString());
        return Ok();
    }
    
    [HttpPost("{id:guid}/order/{orderId}/start")]
    public async Task<IActionResult> StartOrder(Guid id, string orderId)
    {
        await service.StartNextOrderAsync(id.ToString(), orderId);
        return Ok();
    }
    
    [HttpGet("{id:guid}/order")]
    public async Task<IActionResult> GetOrders(Guid id) => Ok(await service.GetAllOrdersForEquipmentAsync(id.ToString()));

    [HttpGet("{id:guid}/history")]
    public async Task<IActionResult> GetHistory(Guid id) => Ok(await service.GetHistoryForEquipmentAsync(id.ToString()));

    
}