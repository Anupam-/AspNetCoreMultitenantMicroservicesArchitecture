using Autofac.Multitenant;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMultitenantMicroservicesArchitecture
{
    public class RequestParameterStrategy : ITenantIdentificationStrategy
    {
        private IHttpContextAccessor _httpContextAccessor;
        public RequestParameterStrategy(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public bool TryIdentifyTenant(out object tenantId)
        {
            tenantId = null;
            try
            {
                var context = _httpContextAccessor.HttpContext;
                if (context != null && context.Request != null)
                {
                    tenantId = context.Request.HttpContext.Items["tenant"];
                }
            }
            catch (Exception)
            {
                // Happens at app startup in IIS 7.0
            }

            return tenantId != null;
        }
    }
}
