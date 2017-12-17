using ServiceResultModels;
using System.Collections.Generic;

namespace Proxies
{
    public interface IServiceProxy
    {
        IList<UserModel> GetAll();
    }
    
}
