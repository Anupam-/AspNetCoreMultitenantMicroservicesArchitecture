using Autofac.Multitenant;
using Microsoft.AspNetCore.Mvc;
using Proxies;
using ServiceResultModels;
using System.Collections.Generic;

namespace DefaultControllers
{
    public class HomeController : Controller
    {
        private ITenantIdentificationStrategy _tenantIdentificationStrategy;
        private IServiceProxy _service;
        public HomeController(IServiceProxy service,
            ITenantIdentificationStrategy tenantIdentificationStrategy)
        {
            _tenantIdentificationStrategy = tenantIdentificationStrategy;
            _service = service;
        }
        public IActionResult Index()
        {
            @ViewBag.Tenant = GetTenantId();
            @ViewBag.Service = _service.GetType().Name;
            @ViewBag.Controller = this.GetType().Assembly.GetName();
            return View(BuildIndexPage());
        }
        
        public virtual IList<UserModel> BuildIndexPage()
        {
            return _service.GetAll();
        }
        protected object GetTenantId()
        {
            var tenantId = (object)null;
            var success = _tenantIdentificationStrategy.TryIdentifyTenant(out tenantId);
            if (!success || tenantId == null)
            {
                return "";
            }
            return tenantId;
        }
    }
}
