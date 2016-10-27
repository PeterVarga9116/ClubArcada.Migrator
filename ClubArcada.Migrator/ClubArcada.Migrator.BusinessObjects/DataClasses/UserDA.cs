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

        public static List<Tournament> GetTournaments()
        {
            using (var appDC = new PDDCDataContext(ConfigurationManager.AppSettings["OldDB"]))
            {
                var tours = appDC.Tournaments.ToList();

                foreach(var t in tours)
                {
                    t.Detail = appDC.TournamentDetails.SingleOrDefault(td => td.TournamentId == t.TournamentId);
                }

                return tours;
            }
        }
    }
}
