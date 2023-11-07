namespace AbsenDulu.BE.Interfaces.IServices.PushNotification;
public interface IRabbitMQService
{
    void SendMessageToQueue(string queueName, string message);
}