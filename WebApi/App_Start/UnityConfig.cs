using System.Web.Http;
using Unity;
using Unity.WebApi;
using WebApi.Repository;
using WebApi.Services;

namespace WebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IEmpleadoRepository, EmpleadoRepository>();
            container.RegisterType<IEmpleadoService, EmpleadoService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}