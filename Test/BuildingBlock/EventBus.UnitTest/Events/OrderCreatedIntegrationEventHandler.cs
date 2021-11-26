using EventBus.Base.Abstraction;
using System.Threading.Tasks;

namespace EventBus.UnitTest.Events
{
    public class OrderCreatedIntegrationEventHandler : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
    {
        public Task Handle(OrderCreatedIntegrationEvent @event)
        {
            System.Console.WriteLine("Handle method worked with id:" + @event.Id);
            return Task.CompletedTask;
        }
    }
}
