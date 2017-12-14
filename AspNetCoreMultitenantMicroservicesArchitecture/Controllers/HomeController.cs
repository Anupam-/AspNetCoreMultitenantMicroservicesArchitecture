using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreMultitenantMicroservicesArchitecture.Models;
using AspNetCoreMultitenantMicroservicesArchitecture.UI.Proxies;
using Autofac.Multitenant;
using Microsoft.Extensions.DependencyInjection;
using Autofac.Extensions.DependencyInjection;
using Autofac;

namespace AspNetCoreMultitenantMicroservicesArchitecture.Controllers
{
    public class HomeController : Controller
    {
        private ITenantIdentificationStrategy _tenantIdentificationStrategy;
        private IServiceProxy _service;
        public HomeController(ITenantIdentificationStrategy tenantIdentificationStrategy, IServiceProxy service)
        {
            _tenantIdentificationStrategy = tenantIdentificationStrategy;
            _service = service;
        }
        public IActionResult Index()
        {
            Console.WriteLine(_service.GetType());
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        private object GetTenantId()
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
