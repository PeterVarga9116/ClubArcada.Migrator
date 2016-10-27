using System;
using ClubArcada.Common.BusinessObjects.DataClasses;
using System.Configuration;
using ClubArcada.Common;
using System.Linq;
using ClubArcada.Common.BusinessObjects;


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
                    AutoReturnType = u.IsAutoReturn.True() ? 1 : 0,
                    CreatedByUserId = new Guid(ConfigurationManager.AppSettings["PeterVargaUserId"]),
                    DateCreated = u.DateCreated,
                    Email = u.Email.Trim(),
                    FirstName = u.FirstName,
                    IsAdmin = u.IsAdmin,
                    IsBlocked = u.IsBlocked,
                    IsPersonal = u.IsPersonal,
                    IsTestUser = u.FirstName.ToLower().Contains("test"),
                    IsWallet = u.IsWallet.True(),
                    LastName = u.LastName.Trim(),
                    NickName = u.NickName.Trim(),
                    Password = u.Password.Trim(),
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
                    Description = u.Description.Trim(),
                    Status = u.Status,
                    UserId = u.UserId
                };

                ClubArcada.Common.BusinessObjects.Data.RequestData.Create(CR, newReq);
                Console.WriteLine(newReq.Description + " synced");
            }
        }

        public static void SynTournaments()
        {
            var oldTours = BusinessObjects.DataClasses.UserDA.GetTournaments();

            var filtered = oldTours.Where(t => !t.DateDeleted.HasValue && t.Detail.IsNotNull()).ToList();

            foreach (var u in filtered)
            {
                var newReq = new Tournament()
                {
                    Id = u.TournamentId,
                    Date = u.Date,
                    DateCreated = u.DateCreated,
                    DateDeleted = u.DateDeleted,
                    DateEnded = u.DateEnded.HasValue ? u.DateEnded.Value : DateTime.Now.AddYears(-1),
                    Description = u.Description.IsNullOrEmpty() ? string.Empty : u.Description,
                    Name = u.Name,
                    LeagueId = u.LeagueId,
                    IsLeague = u.Detail.IsLeague,
                    IsFullPointed = u.Detail.IsFullPointed,
                    IsFood =u.Detail.IsFood,
                    IsHidden = u.IsHidden.True(),
                    IsPercentageBonus = u.Detail.IsPercentageBonus,
                    IsRunning = u.IsRunning.True(),
                    
                    BountyDesc = u.Detail.BountyDesc.IsNullOrEmpty() ? string.Empty : u.Detail.BountyDesc,
                    BuyInPrize = u.Detail.BuyInPrize,
                    BuyInStack = u.Detail.BuyInStack,
                    CreatedByUserId = u.CreatedByUserId,

                    FullStackBonus = u.Detail.FullStackBonus.HasValue ? u.Detail.FullStackBonus.Value : 0,
                    GameType = (int)u.GameType.ToGameType(),
                    GTD = u.Detail.GTD.HasValue ? u.Detail.GTD.Value : 0,
                    LogicType = 0,
                    ReBuyCount = u.Detail.ReBuyCount.HasValue ? u.Detail.ReBuyCount.Value :0,
                    RebuyPrize = u.Detail.ReBuyPrize,
                    RebuyStack = u.Detail.ReBuyStack,
                    StructureId = u.Detail.StructureId,
                    SpecialAddonPrize = u.Detail.SpecialAddonPrize.HasValue ? u.Detail.SpecialAddonPrize.Value :0,
                    SpecialAddonStack = u.Detail.SpecialAddonStack.HasValue ? u.Detail.SpecialAddonStack.Value :0,
                    

                    AddOnPrize = u.Detail.AddOnPrize,
                    AddOnStack = u.Detail.AddOnStack,
                    BonusStack = u.Detail.BonusStack,
                    
                };

                ClubArcada.Common.BusinessObjects.Data.TournamentData.Save(CR, newReq);
                Console.WriteLine(newReq.Name + " synced");
            }
        }
    }



    public static class Ext
    {
        public static eGameType ToGameType(this char gametypeChar)
        {
            switch (gametypeChar)
            {
                case 'X':
                    return eGameType.NotSet;

                case 'F':
                    return eGameType.FreezeOut;

                case 'R':
                    return eGameType.RebuyUnlimited;

                case 'D':
                    return eGameType.DoubleChance;

                case 'T':
                    return eGameType.TripleChance;

                case 'C':
                    return eGameType.CashGame;

                case 'E':
                    return eGameType.Freeroll;

                case 'L':
                    return eGameType.RebuyLimited;

                case 'A':
                    return eGameType.DoubleTrouble;

                case 'Y':
                    return eGameType.Final;

                case 'Q':
                    return eGameType.Qualification;

                case 'Z':
                    return eGameType.QualFinal;

                default:
                    return eGameType.NotSet;
            }
        }
    }
}