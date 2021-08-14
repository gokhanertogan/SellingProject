﻿using EventBus.Base.Abstraction;
using EventBus.Base.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PaymentService.Api.IntegrationEvents.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentService.Api.IntegrationEvents.EventHandlers
{
    public class OrderStartedIntegrationEventHandler : IIntegrationEventHandler<OrderStartedIntegrationEvent>
    {
        private readonly IConfiguration configuration;
        private readonly IEventBus eventBus;
        private readonly ILogger<OrderStartedIntegrationEventHandler> logger;

        public OrderStartedIntegrationEventHandler(IConfiguration configuration, IEventBus eventBus, ILogger<OrderStartedIntegrationEventHandler> logger)
        {
            this.configuration = configuration;
            this.eventBus = eventBus;
            this.logger = logger;
        }

        public Task Handle(OrderStartedIntegrationEvent @event)
        {
            string keyword = "PaymentSucess";
            bool paymentSuccessFiag = configuration.GetValue<bool>(keyword);

            IntegrationEvent paymentEvent = paymentSuccessFiag
                ? new OrderPaymentSuccessIntegrationEvent(@event.OrderId)
                : new OrderPaymentFailedIntegrationEvent(@event.OrderId, "This is a fake error message");

            logger.LogInformation($"OrderCreatedIntegrationEventHandler in PaymentService is fired with PaymentSuccess : {paymentSuccessFiag}, orderId: {@event.OrderId}");

            eventBus.Publish(paymentEvent);

            return Task.CompletedTask;
        }
    }
}
