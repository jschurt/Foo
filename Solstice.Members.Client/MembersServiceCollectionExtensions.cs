﻿using Microsoft.Extensions.DependencyInjection;
using Solstice.Infrastructure;
using System;

namespace Solstice
{
    public static class MembersServiceCollectionExtensions
    {
        public static void AddMembersServiceClients(this IServiceCollection services, Action<ServiceClientOptions> configure)
        {
            services
                .AddServiceClientGroup("Members")
                .Configure(configure)
                .AddAutoGeneratedServiceClients(System.Reflection.Assembly.GetExecutingAssembly());
                ;
        }
    }
}