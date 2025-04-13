

using Common;

namespace Crm.Kafka.Producer;

public interface IProducer 
{
    Task SendAsync(string topic, CustomerEvent customerEvent);
}
