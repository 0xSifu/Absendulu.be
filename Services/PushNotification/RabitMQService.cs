using System.Text;
using AbsenDulu.BE.Interfaces.IServices.PushNotification;
using RabbitMQ.Client;

namespace AbsenDulu.BE.Services.PushNotification;
public class RabbitMQService : IRabbitMQService
{
    private readonly ConnectionFactory _factory;

    public RabbitMQService(IConfiguration configuration)
    {
        _factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:HostName"],
            Port = int.Parse(configuration["RabbitMQ:Port"]),
            UserName = configuration["RabbitMQ:UserName"],
            Password = configuration["RabbitMQ:Password"]
        };
    }

    public IModel CreateChannel()
    {
        var connection = _factory.CreateConnection();
        return connection.CreateModel();
    }

    public void SendMessageToQueue(string queueName, string message)
    {
        using (var channel = CreateChannel())
        {
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: queueName,
                                 basicProperties: null,
                                 body: body);

            Console.WriteLine($"message sent: {message}");
        }
    }

    public void DeclareQueue(string queueName, bool durable = false)
    {
        using (var channel = CreateChannel())
        {
            // Deklarasikan antrian baru
            channel.QueueDeclare(
                queue: queueName,
                durable: durable,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            Console.WriteLine($"Antrian '{queueName}' telah dibuat.");
        }
    }


}