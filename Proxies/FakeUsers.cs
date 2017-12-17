using ServiceResultModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proxies
{
    public class FakeUsers
    {
        public static IList<UserModel> All(string tenant)
        {
            return new List<UserModel> {
                new UserModel{ Id = 1, FirstName = string.Format("{0} {1}",tenant, "anupam"), LastName= "singh", Profile="s\\w developer"   },
                new UserModel{ Id = 2, FirstName = string.Format("{0} {1}",tenant, "rohit"), LastName= "kumar", Profile="s\\w developer"   },
                new UserModel{ Id = 3, FirstName = string.Format("{0} {1}",tenant, "ajay"), LastName= "singh", Profile="sr. s\\w developer"   },
                new UserModel{ Id = 4, FirstName = string.Format("{0} {1}",tenant, "rahul"), LastName= "kumar", Profile="sr. s\\w developer"   },
            };
        } 
    }
}
