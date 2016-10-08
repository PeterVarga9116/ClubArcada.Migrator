using System;
using ClubArcada.Common.BusinessObjects.DataClasses;
using System.Configuration;
using ClubArcada.Common;

namespace ClubArcada.Migrator.Win.DA
{
    public class BU
    {

        public static Credentials CR
        {
            get
            {
                return new Credentials(new Guid(ConfigurationManager.AppSettings["PeterVargaUserId"]), 0, ConfigurationManager.AppSettings["NewDB"]);
            }
            private set { }
        }

        public static void SyncUsers()
        {
            var oldUsers = BusinessObjects.DataClasses.UserDA.GetList();

            foreach (var u in oldUsers)
            {
                var user = new User()
                {
                    Id = u.UserId,
                    AdminLevel = u.AdminLevel,
                    AutoReturnType = 0,
                    CreatedByUserId = new Guid(ConfigurationManager.AppSettings["PeterVargaUserId"]),
                    DateCreated = u.DateCreated,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    IsAdmin = u.IsAdmin,
                    IsBlocked = u.IsBlocked,
                    IsPersonal = u.IsPersonal,
                    IsTestUser = u.FirstName.ToLower().Contains("test"),
                    IsWallet = u.IsWallet.True(),
                    LastName = u.LastName,
                    NickName = u.NickName,
                    Password = u.Password,
                    PhoneNumber = u.PhoneNumber
                };

                ClubArcada.Common.BusinessObjects.Data.UserData.Save(CR, user);
                Console.WriteLine(u.NickName + " synced");
            }
        }

        public static void SyncRequests()
        {
            var oldReqs = BusinessObjects.DataClasses.UserDA.GetRequests();

            foreach (var u in oldReqs)
            {
                var newReq = new Request()
                {
                    Id = u.RequestId,
                    AssignedTo = u.AssignedTo,
                    DateCompleted = u.DateCompleted,
                    DateCreated = u.DateCreated,
                    DateDeleted = u.DateDeleted,
                    Description = u.Description,
                    Status = u.Status,
                    UserId = u.UserId
                };

                ClubArcada.Common.BusinessObjects.Data.RequestData.Create(CR, newReq);
                Console.WriteLine(u.Description + " synced");
            }
        }
    }
}