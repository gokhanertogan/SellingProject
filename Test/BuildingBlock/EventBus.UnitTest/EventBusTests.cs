using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Logging;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using EventBus.Base;
using EventBus.UnitTest.Events;

namespace EventBus.UnitTest
{
    [TestClass]
    public class EventBusTests
    {
        private ServiceCollection _services;

        public EventBusTests()
        {
            _services = new ServiceCollection();
            _services.AddLogging(configure => configure.AddConsole());
        }

        [TestMethod]
        public void subsribe_event_on_rabbitmq_test()
        {
            _services.AddSingleton<IEventBus>(sp =>
            {
                EventBusConfig config = GetRabbitMqConfig();
                return EventBusFactory.Create(config, sp);
            });

            var sp = _services.BuildServiceProvider();
            var eventBus = sp.GetRequiredService<IEventBus>();

            eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
            //eventBus.UnSubscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
        }

        [TestMethod]
        public void subsribe_event_on_azure_test()
        {
            _services.AddSingleton<IEventBus>(sp =>
            {
                EventBusConfig config = GetAzureConfig();

                return EventBusFactory.Create(config, sp);
            });

            var sp = _services.BuildServiceProvider();
            var eventBus = sp.GetRequiredService<IEventBus>();

            eventBus.Subscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
            //eventBus.UnSubscribe<OrderCreatedIntegrationEvent, OrderCreatedIntegrationEventHandler>();
        }


        [TestMethod]
        public void send_message_to_rabbitmq()
        {
            _services.AddSingleton<IEventBus>(sp =>
            {
                return EventBusFactory.Create(GetRabbitMqConfig(),sp);
            });

            var sp = _services.BuildServiceProvider();
            var eventBus = sp.GetRequiredService<IEventBus>();

            eventBus.Publish(new OrderCreatedIntegrationEvent(1));
        }


        [TestMethod]
        public void send_message_to_azure_test()
        {
            _services.AddSingleton<IEventBus>(sp =>
            {
                return EventBusFactory.Create(GetAzureConfig(), sp);
            });

            var sp = _services.BuildServiceProvider();
            var eventBus = sp.GetRequiredService<IEventBus>();

            eventBus.Publish(new OrderCreatedIntegrationEvent(1));
        }

        private EventBusConfig GetAzureConfig()
        {
            return new EventBusConfig()
            {
                ConnectionRetryCount = 5,
                SubscriberClientAppName = "EventBus.UniTest",
                DefaultTopicName = "SellingBuddyTopicName",
                EventBusType = EventBusType.AzureServiceBus,
                EventNameSuffix = "IntegrationEvent",
                EventBusConnectionString = "azure-connection"
            };
        }

        private EventBusConfig GetRabbitMqConfig()
        {
            return new EventBusConfig()
            {
                ConnectionRetryCount = 5,
                SubscriberClientAppName = "EventBus.UnitTest",
                DefaultTopicName = "SellingBuddyTopicName",
                EventBusType = EventBusType.RabbitMQ,
                EventNameSuffix = "IntegrationEvent"
            };
        }
    }
}
