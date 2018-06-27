using Microsoft.Extensions.DependencyInjection;
using System;

namespace FirmwareServer
{
    internal class HangfireActivator : Hangfire.AspNetCore.AspNetCoreJobActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public HangfireActivator(IServiceProvider serviceProvider)
            : base(serviceProvider.GetService<IServiceScopeFactory>())
        {
            _serviceProvider = serviceProvider;
        }

        public override object ActivateJob(Type jobType)
        {
            return _serviceProvider.GetService(jobType) ?? base.ActivateJob(jobType);
        }
    }
}