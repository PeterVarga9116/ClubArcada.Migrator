using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ClubArcada.Migrator.BusinessObjects.DataClasses
{
    public class UserDA
    {
        public static List<User> GetList()
        {
            using (var appDC = new PDDCDataContext(ConfigurationManager.AppSettings["OldDB"]))
            {
                return appDC.Users.ToList();
            }
        }

        public static List<Request> GetRequests()
        {
            using (var appDC = new PDDCDataContext(ConfigurationManager.AppSettings["OldDB"]))
            {
                return appDC.Requests.ToList();
            }
        }
    }
}
