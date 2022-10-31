using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Producer.RabbitMq;

namespace Producer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        IRabitMQProducer _rabitMQProducer;
        public ProductController(IRabitMQProducer rabitMQProducer)
        {
            _rabitMQProducer = rabitMQProducer;
        }

        [HttpPost]
        public IActionResult SendMessage(string name)
        {
            _rabitMQProducer.SendProductMessage(name);
            return Ok();
        }
    }
}
