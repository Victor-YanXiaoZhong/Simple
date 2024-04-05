using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple.AspNetCore;

namespace Simple.CommonApplication.Events
{
    /// <summary></summary>
    public class InitHandle : INotificationHandler<AppDomainEvent<string>>
    {
        public Task Handle(AppDomainEvent<string> value, CancellationToken cancellationToken)
        {
            ConsoleHelper.Info("收到消息：" + value.Value);
            return Task.CompletedTask;
        }
    }
}