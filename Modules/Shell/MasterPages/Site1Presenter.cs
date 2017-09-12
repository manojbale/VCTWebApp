using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.MasterPages
{
    public class Site1Presenter : Presenter<ISite1View>
    {
        private static UserRepository userRepositoryService;
        public Site1Presenter()
        {
            userRepositoryService = new UserRepository();
        }
        #region Public Overrides


        public override void OnViewInitialized()
        {
            View.SetVersion(ConfigSetting.GetValue(ConfigSetting.VERSION));
        }

        public  bool IsNewUser(string username)
        {
            bool flag = false;
            try
            {
                VCTWeb.Core.Domain.Users currentUser = userRepositoryService.FetchUserByName(username);
                //if (currentUser.LastPasswordDate.HasValue)
                //{
                    DateTime expdatetime = Convert.ToDateTime(currentUser.LastPasswordDate);

                    //Validate the passwords
                    if (currentUser.LastPasswordDate == null || DateTime.UtcNow >= expdatetime.AddDays(90))
                    {
                        flag = true;
                    }
                //}

            }
            catch
            {
                throw;
            }
            return flag;
        }
        #endregion
    }
}




