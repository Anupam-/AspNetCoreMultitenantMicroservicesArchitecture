using Autofac.Multitenant;
using Microsoft.AspNetCore.Http;
using System;

namespace AspNetCoreMultitenantMicroservicesArchitecture
{
    public class TenantResolverStrategy : ITenantIdentificationStrategy
    {
        private IHttpContextAccessor _httpContextAccessor;
        public TenantResolverStrategy(IHttpContextAccessor httpContextAccessor)
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
                    // get very first segment
                    var uriSeg = context.Request.Path.Value.Split('/');
                    tenantId = uriSeg[1];
                }
            }
            catch (Exception)
            {
                // Happens at app startup in IIS 7.0
            }

            return (tenantId != null || tenantId == (object)"" );
        }
    }
}
