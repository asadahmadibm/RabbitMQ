// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Hello, World!");
string message = "";
var factory = new ConnectionFactory { HostName = "localhost" };

var connection = factory.CreateConnection();

using var channel = connection.CreateModel();
channel.QueueDeclare("product");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, eventArgs) =>

{

    var body = eventArgs.Body.ToArray();

    message = Encoding.UTF8.GetString(body);
    Console.WriteLine(message);

};

channel.BasicConsume(queue: "product", autoAck: true, noLocal:false,exclusive:false,arguments:null,consumer: consumer);
Console.ReadKey();


