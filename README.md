# RabbitMQ
RabbitMQ With .NET Core 6 Web API
# Step To Use
1- Install Elang from https://www.erlang.org/downloads

2- Install RabbitMQ from https://www.rabbitmq.com/download.html

3- Enable in command prompt : Go to sbin Directory that RabbitMQ is instsalled & Run rabbitmq-plugins enable rabbitmq_management
# Managment
http://localhost:15672/

# Using in .netsolution 1

install-package rabbitmq.client for both producer & consumer

# create connection for both producer & consumer

var factory = new ConnectionFactory { HostName = "localhost" };

var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare("product", exclusive: false);

# producer : send message

var json = JsonConvert.SerializeObject(message);

var body = Encoding.UTF8.GetBytes(json);

channel.BasicPublish(exchange: "", routingKey: "product", body: body);

# consumer : Recieve message

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, eventArgs) =>

{

    var body = eventArgs.Body.ToArray();

    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine(message);

};

channel.BasicConsume(queue: "product", autoAck: true, consumer: consumer);

# Using in .netsolution 2

        MassTransit
        MassTransit.RabbitMQ
        MassTransit.AspNetCore

publisher 
------------------------------------------------------------------------
in service

     services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((ctx, conf) =>
            {
                conf.Host(Configuration.GetValue<string>("EventBusSettings:HostAddress"));
            });
        });

	services.AddMassTransitHostedService();

in appsettings.json

     "EventBusSettings": {
        "HostAddress": "amqp://guest:guest@localhost:5672"
      },



in class

    private readonly ipublishendpoint _ipe
    _ipe.publish(message)
------------------------------------------------------------------------

subscriper
----------------------------------------------------------------------------
in service

    services.AddMassTransit(config =>
        {
            config.AddConsumer<BasketCheckoutConsumer>();

            config.UsingRabbitMq((ctx, conf) =>
            {
                conf.Host(Configuration.GetValue<string>("EventBusSettings:HostAddress"));
                conf.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
                {
                    c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
                });
            });
        });
	services.AddMassTransitHostedService();
	services.AddScoped<BasketCheckoutConsumer>();

in appsettings.json

     "EventBusSettings": {
        "HostAddress": "amqp://guest:guest@localhost:5672"
      },

make new class BasketCheckoutConsumer

    public class BasketCheckoutConsumer : IConsumer<BasketCheckoutEvent>
        {
            private readonly IMapper _mapper;
            private readonly IMediator _mediator;
            private readonly ILogger<BasketCheckoutConsumer> _logger;

            public BasketCheckoutConsumer(IMapper mapper, IMediator mediator, ILogger<BasketCheckoutConsumer> logger)
            {
                _mapper = mapper;
                _mediator = mediator;
                _logger = logger;
            }


            public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
            {
                var command = _mapper.Map<CheckoutOrderCommand>(context.Message);
                var result = await _mediator.Send(command);
                _logger.LogInformation($"order consumed successfully and order id is : {result}");
            }
    }
----------------------------------------------------------------------------











