using System;
using System.Web;
using Atelie.Infrastructure.Abstractions.Repositories;
using Atelie.Infrastructure.Abstractions.Services;
using Atelie.Infrastructure.Persistence;
using Atelie.Infrastructure.Repositories;
using Atelie.Infrastructure.Services;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using NHibernate;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Atelie.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Atelie.Web.App_Start.NinjectWebCommon), "Stop")]

namespace Atelie.Web.App_Start
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<ISessionFactory>().ToMethod(context => NHibernateHelper.GetSessionFactory()).InSingletonScope();
            kernel.Bind<ISession>().ToMethod(context => kernel.Get<ISessionFactory>().OpenSession()).InRequestScope();

            kernel.Bind<IAlunoRepository>().To<AlunoRepository>();
            kernel.Bind<ICursoRepository>().To<CursoRepository>();
            kernel.Bind<ITurmaRepository>().To<TurmaRepository>();
            kernel.Bind<IAulaRepository>().To<AulaRepository>();
            kernel.Bind<IPagamentoRepository>().To<PagamentoRepository>();
            kernel.Bind<IPresencaRepository>().To<PresencaRepository>();
            kernel.Bind<IViewRelatorioPagamentoRepository>().To<ViewRelatorioPagamentoRepository>();

            kernel.Bind<IAlunoService>().To<AlunoService>();
            kernel.Bind<ICursoService>().To<CursoService>();
            kernel.Bind<ITurmaService>().To<TurmaService>();
            kernel.Bind<IAulaService>().To<AulaService>();
            kernel.Bind<IPagamentoService>().To<PagamentoService>();
            kernel.Bind<IPresencaService>().To<PresencaService>();
            kernel.Bind<IViewRelatorioPagamentoService>().To<ViewRelatorioPagamentoService>();
            kernel.Bind<IRelatorioExcelService>().To<RelatorioExcelService>();

            QuartzScheduler.Start(kernel);
        }
    }
}