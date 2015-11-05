using System.Reflection;
using Autofac;
using MinimalEF.Commands.Common;
using MinimalEF.Domain.Actions;

namespace MinimalEF
{
    public static class IoC
    {
        public static IContainer HaveYouAnyIoC(string connectionString)
        {
            var builder = new ContainerBuilder();

            builder
                .Register(_ => new MyContext(connectionString))
                .As<IDbContext>()
                .InstancePerLifetimeScope();
            builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.IsAssignableTo<ICliCommand>())
                .As<ICliCommand>()
                .InstancePerDependency();
            builder
                .RegisterType<GetPortfolio>()
                .AsSelf();
            builder.RegisterType<AddAssetToPortfolio>().AsSelf();

            return builder.Build();
        }
    }
}