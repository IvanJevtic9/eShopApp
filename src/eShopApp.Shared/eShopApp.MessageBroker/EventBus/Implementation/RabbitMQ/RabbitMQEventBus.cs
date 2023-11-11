using Autofac;
using eShopApp.MessageBroker.EventBus.Base;
using eShopApp.MessageBroker.EventBus.Implementation.EventBusManager;
using eShopApp.MessageBroker.EventBus.Implementation.RabbitMQ.Base;
using eShopApp.MessageBroker.Extensions;
using eShopApp.MessageBroker.Models;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace eShopApp.MessageBroker.EventBus.Implementation.RabbitMQ
{
    public class RabbitMQEventBus : IEventBus, IDisposable
    {
        const string BROKER_NAME = "eshop_event_bus";
        const string AUTOFAC_SCOPE_NAME = "eshop_event_bus";

        private readonly int _retryCounter;
        private readonly ILifetimeScope _autofac;
        private readonly IEventBusManager _busManager;
        private readonly ILogger<RabbitMQEventBus> _logger;
        private readonly IRabbitMQPersistentConnection _persistentConnection;

        private string _queueName;
        private IModel _consumerChannel;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="busManager"></param>
        /// <param name="logger"></param>
        /// <param name="persistentConnection"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RabbitMQEventBus(
            string queueName,
            ILifetimeScope autofac,
            IEventBusManager busManager,
            ILogger<RabbitMQEventBus> logger,
            IRabbitMQPersistentConnection persistentConnection,
            int retryCount = 5)
        {
            _retryCounter = retryCount;

            _autofac = autofac;
            _queueName = queueName;
            _busManager = busManager ?? new InMemoryEventBusManager();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));

            _busManager.OnEventRemoved += BusManager_OnEventRemoved;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }

            _busManager.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="integrationEvent"></param>
        public void Publish(IntegrationEvent integrationEvent)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = Policy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCounter,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                        _logger.LogWarning(
                            ex,
                            "Could not publish event: {EventId} after {Timeout}s ({ExceptionMessage})",
                            integrationEvent.Id,
                            $"{time.TotalSeconds:n1}",
                            ex.Message);
                    });

            var eventName = integrationEvent.GetType().Name;
            _logger.LogTrace("Creating RabbitMQ channel to publish event: {EventId} ({EventName})", integrationEvent.Id, eventName);

            using var channel = _persistentConnection.CreateModel();
            _logger.LogTrace("Declaring RabbitMQ exchange to publish event: {EventId}", integrationEvent.Id);

            channel.ExchangeDeclare(
                exchange: BROKER_NAME,
                type: "direct");

            var body = JsonSerializer.SerializeToUtf8Bytes(
                integrationEvent,
                integrationEvent.GetType(),
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });

            policy.Execute(() =>
            {
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent

                _logger.LogTrace("Publishing event to RabbitMQ: {EventId}", integrationEvent.Id);
                channel.BasicPublish(
                    exchange: BROKER_NAME,
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _busManager.GetEventKey<T>();

            DoInternalSubscription(eventName);

            _logger.LogInformation(
                "Subscribing to event {EventName} with {EventHandler}",
                eventName,
                typeof(TH).GetGenericTypeName());

            _busManager.AddSubscription<T, TH>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TH"></typeparam>
        /// <param name="eventName"></param>
        public void SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            _logger.LogInformation(
                "Subscribing to dynamic event {EventName} with {EventHandler}",
                eventName, 
                typeof(TH).GetGenericTypeName());

            DoInternalSubscription(eventName);
            _busManager.AddDynamicSubscription<TH>(eventName);
            StartBasicConsume();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TH"></typeparam>
        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _busManager.GetEventKey<T>();

            _logger.LogInformation("Unsubscribing from event {EventName}", eventName);

            _busManager.RemoveSubscription<T, TH>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TH"></typeparam>
        /// <param name="eventName"></param>
        public void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            _logger.LogInformation("Unsubscribing from event {EventName}", eventName);

            _busManager.RemoveDynamicSubscription<TH>(eventName);
        }

        /// <summary>
        /// 
        /// </summary>
        private void StartBasicConsume()
        {
            _logger.LogTrace("Starting RabbitMQ basic consume");

            if (_consumerChannel is not null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += Consumer_Received;

                _consumerChannel.BasicConsume(
                    queue: _queueName,
                    autoAck: false,
                    consumer: consumer);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        /// <returns></returns>
        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

            try
            {
                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "----- ERROR Processing message \"{Message}\"", message);
            }

            // Even on exception we take the message off the queue.
            // in a REAL WORLD app this should be handled with a Dead Letter Exchange (DLX). 
            // For more information see: https://www.rabbitmq.com/dlx.html
            _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task ProcessEvent(string eventName, string message)
        {
            _logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);

            if (_busManager.HasSubscriptionForEvent(eventName))
            {
                await using var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME);
                var subscriptions = _busManager.GetHandlersForEvent(eventName);
                foreach (var subscription in subscriptions)
                {
                    if (subscription.IsDynamic)
                    {
                        if (scope.ResolveOptional(subscription.HandlerType) is not IDynamicIntegrationEventHandler handler)
                            continue;

                        using dynamic eventData = JsonDocument.Parse(message);
                        await Task.Yield();
                        await handler.Handle(eventData);
                    }
                    else
                    {
                        var handler = scope.ResolveOptional(subscription.HandlerType);
                        if (handler == null)
                            continue;

                        var eventType = _busManager.GetEventTypeByName(eventName);
                        var integrationEvent = JsonSerializer.Deserialize(
                            message,
                            eventType,
                            new JsonSerializerOptions()
                            {
                                PropertyNameCaseInsensitive = true
                            });
                        var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

                        await Task.Yield();
                        await (Task)concreteType.GetMethod("Handle")
                            .Invoke(handler, new object[] { integrationEvent });
                    }
                }
            }
            else
            {
                _logger.LogWarning("No subscription for RabbitMQ event: {EventName}", eventName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventName"></param>
        private void DoInternalSubscription(string eventName)
        {
            var containsKey = _busManager.HasSubscriptionForEvent(eventName);

            if (!containsKey)
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }

                _consumerChannel.QueueBind(
                    queue: _queueName,
                    exchange: BROKER_NAME,
                    routingKey: eventName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BusManager_OnEventRemoved(object sender, string eventName)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            using var channel = _persistentConnection.CreateModel();
            channel.QueueUnbind(
                queue: _queueName,
                exchange: BROKER_NAME,
                routingKey: eventName);

            if (_busManager.IsEmpty)
            {
                _queueName = string.Empty;
                _consumerChannel.Close();
            }
        }
    }
}
