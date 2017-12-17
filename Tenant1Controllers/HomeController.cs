using Autofac.Multitenant;
using Microsoft.AspNetCore.Mvc;
using Proxies;
using ServiceResultModels;
using System.Collections.Generic;

namespace Tenant1Controllers
{
    public class HomeController : DefaultControllers.HomeController
    {
        private IServiceProxy _service;
        public HomeController(IServiceProxy service,
            ITenantIdentificationStrategy tenantIdentificationStrategy) 
            : base(service, tenantIdentificationStrategy)
        {
            _service = service;
        }
        public IActionResult NewAction()
        {
            return View("New Action");
        }
        public override IList<UserModel> BuildIndexPage()
        {
            return _service.GetAll();
        }
    }
}
