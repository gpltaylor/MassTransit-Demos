namespace AspnetCoreAPI
{
    using System;
    using System.Threading.Tasks;
    //using EventContracts;
    using MassTransit;
    using Microsoft.AspNetCore.Mvc;

    public class ValueEntered {
        public string Value { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class ValueController :
        ControllerBase
    {
        readonly IPublishEndpoint _publishEndpoint;

        public ValueController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] string message)
        {
            await _publishEndpoint.Publish<ValueEntered>(new
            {
                Value = message
            });

            return Ok();
        }
    }
}