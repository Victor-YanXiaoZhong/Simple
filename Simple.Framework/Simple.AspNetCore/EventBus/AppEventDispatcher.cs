using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Simple.AspNetCore
{
    /// <summary>app 领域应用内的事件分发</summary>
    public class AppDomainEventDispatcher
    {
        private static readonly IMediator _mediator;

        static AppDomainEventDispatcher()
        {
            _mediator = HostServiceExtension.ServiceProvider.GetService<IMediator>();
        }

        /// <summary>发送事件</summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <param name="event"></param>
        /// <returns></returns>
        public static async Task PublishEvent<TEvent>(TEvent @event) where TEvent : INotification
        {
            await _mediator.Publish(@event);
        }
    }

    /// <summary>App领域事件 用于传递 Vaule</summary>
    /// <typeparam name="T"></typeparam>
    public class AppDomainEvent<T> : INotification
    {
        /// <summary>App领域事件 用于传递 Vaule</summary>
        /// <param name="name">事件名称</param>
        /// <param name="value">事件值</param>
        public AppDomainEvent(T value, string name = "")
        {
            Name = name;
            Value = value;
        }

        /// <summary>事件参数</summary>
        public T Value { get; set; }

        /// <summary>事件名称</summary>
        public string Name { get; set; }

        /// <summary>事件时间</summary>
        public DateTime EventTime { get; } = DateTime.Now;
    }
}