using Microsoft.AspNetCore.Mvc.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Autofac.Multitenant;

namespace AspNetCoreMultitenantMicroservicesArchitecture.UI
{
    public class TenantActionSelector : ActionSelector
    {
        private ITenantIdentificationStrategy _tenantIdentificationStrategy;
        public TenantActionSelector(
        IActionDescriptorCollectionProvider actionDescriptorCollectionProvider,
        ActionConstraintCache actionConstraintCache,
        ILoggerFactory loggerFactory, ITenantIdentificationStrategy tenantIdentificationStrategy) :base(actionDescriptorCollectionProvider, actionConstraintCache, loggerFactory)
        {
            _tenantIdentificationStrategy = tenantIdentificationStrategy;
        }
        protected override IReadOnlyList<ActionDescriptor> SelectBestActions(IReadOnlyList<ActionDescriptor> actions)
        {
            List<ActionDescriptor> result = null;
            var tid = GetTenantId();
            // find controller dependency for current tenant
            if (tid != null && (string)tid != "")
            {
                // here we will have full assembly name
                var tenantController = string.Format("{0}controllers", (string)tid).ToLower();
                result = actions.Where(x => x.DisplayName.ToLower().Contains(tenantController)).ToList();
            }
            // add default if not found
            if (result == null || result.Count() == 0)
            {
                result = actions.Where(x => x.DisplayName.ToLower().Contains("defaultcontrollers")).ToList();
            }
            return result;
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
