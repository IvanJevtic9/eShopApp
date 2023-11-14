using Autofac;
using eShopApp.MessageBroker.EventBus.Base;
using eShopApp.MessageBroker.EventBus.Implementation.RabbitMQ;
using eShopApp.MessageBroker.EventBus.Implementation.RabbitMQ.Base;
using eShopApp.MessageBroker.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace eShopApp.MessageBroker.Modules
{
    public class RabbitMQModule : Module
    {
        private readonly IConfiguration _configuration;

        public RabbitMQModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var rabbitMqSettings = new RabbitMQSettings();

            _configuration.GetSection(typeof(RabbitMQSettings).Name)
                .Bind(rabbitMqSettings);

            builder.RegisterInstance(rabbitMqSettings)
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<RabbitMQPersistentConnection>()
                .As<IRabbitMQPersistentConnection>()
                .WithParameter("connectionFactory", new ConnectionFactory()
                {
                    HostName = rabbitMqSettings.Host,
                    Port = rabbitMqSettings.Port
                })
                .WithParameter("retryCount", rabbitMqSettings.RetryCount)
                .SingleInstance();

            builder.RegisterType<RabbitMQEventBus>()
                .As<IEventBus>()
                .WithParameter("queueName", rabbitMqSettings.SubscriptionClientName)
                .WithParameter("retryCount", rabbitMqSettings.RetryCount)
                .SingleInstance();
        }
    }
}
