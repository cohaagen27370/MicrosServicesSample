using Gateway.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers;

[ApiController]
[Route("[controller]")]
public class ReinitController(IRabbitMqProducer rabbitMqProducer) : ControllerBase
{
    private readonly IRabbitMqProducer _rabbitMqProducer = rabbitMqProducer;

    [HttpPost("send")]
    public IActionResult SendMessage([FromBody] string messageContent)
    {
        if (string.IsNullOrWhiteSpace(messageContent))
        {
            return BadRequest("Message content cannot be empty.");
        }

        try
        {
            _rabbitMqProducer.SendMessage(messageContent);
            return Ok($"Message '{messageContent}' sent to RabbitMQ.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending message to RabbitMQ: {ex.Message}");
            return StatusCode(500, "Failed to send message to RabbitMQ.");
        }
    }
}