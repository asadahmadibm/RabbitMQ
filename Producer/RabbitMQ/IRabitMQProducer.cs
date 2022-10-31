namespace Producer.RabbitMq
{
    public interface IRabitMQProducer
    {
        public void SendProductMessage<T>(T message);
    }
}
