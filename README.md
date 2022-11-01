# RabbitMQ
RabbitMQ With .NET Core 6 Web API
# Step To Use
1- Install Elang from https://www.erlang.org/downloads

2- Install RabbitMQ from https://www.rabbitmq.com/download.html

3- Enable in command prompt : Go to sbin Directory that RabbitMQ is instsalled & Run rabbitmq-plugins enable rabbitmq_management
# Managment
http://localhost:15672/

# Using in .net

install-package rabbitmq.client for both producer & consumer

# create connection for both producer & consumer

var factory = new ConnectionFactory { HostName = "localhost" };

var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare("orders");

# producer : send message

var json = JsonConvert.SerializeObject(message);

var body = Encoding.UTF8.GetBytes(json);

channel.BasicPublish(exchange: "", routingKey: "orders", body: body);

# consumer : Recieve message

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, eventArgs) =>

{

    var body = eventArgs.Body.ToArray();

    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine(message);

};

channel.BasicConsume(queue: "orders", autoAck: true, consumer: consumer);


