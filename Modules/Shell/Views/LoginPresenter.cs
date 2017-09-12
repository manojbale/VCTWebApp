using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public class LoginPresenter : Presenter<ILoginView>
    {

        #region Instance Variables

        private readonly Authentication authentication;
        private UserRepository userRepositoryService;
        private AddressRepository addressRepositoryService;
        #endregion

        #region Constructors

        public LoginPresenter() :
            this(new Authentication()) { }

        public LoginPresenter(Authentication service)
        {
            authentication = service;
            userRepositoryService = new UserRepository();
            addressRepositoryService = new AddressRepository();
        }

        #endregion

        #region Public Methods

        public bool AuthenticateAgainstDatabase(string userName, string password, out string fullname)
        {
            bool retval = false;
            retval = authentication.AuthenticateAgainstDatabase(userName, password);
            fullname = authentication.Fullname;
            return retval;
        }

        public bool AuthenticateAgainstFirstUser(string userName)
        {

            bool flag = false;
            try
            {
                Users currentUser = userRepositoryService.FetchUserByName(userName);
                if (currentUser.UserName != "root")
                {
                    if (currentUser.LastPasswordDate == null)
                    {
                        flag = true;
                    }
                }

            }
            catch
            {
                throw;
            }

            return flag;
        }

        public bool CheckLoginUserExiparity(string userName)
        {

            bool flag = false;

            try
            {
                Users currentUser = userRepositoryService.FetchUserByName(userName);
                if (currentUser.LastPasswordDate.HasValue)
                {
                    DateTime expdatetime = Convert.ToDateTime(currentUser.LastPasswordDate);
                    if (DateTime.UtcNow >= expdatetime.AddDays(90))
                    {
                        flag = true;
                    }
                }

            }
            catch
            {
                throw;
            }

            return flag;
        }

        public bool AuthenticateAgainstActiveDirectory(string userName, string password, string domain, out string fullname)
        {
            bool retval = false;
            retval = authentication.AuthenticateAgainstActiveDirectory(userName, password, domain);
            fullname = authentication.Fullname;
            return retval;

        }

        public void InsertLogggingDetails(string ipAddress, string userName, bool isSuccess)
        {
            userRepositoryService.InsertLogggingDetails(ipAddress, userName, isSuccess);
        }

        public string GetUserRoleByUserName(string userName)
        {
            return userRepositoryService.GetUserRoleByUserName(userName);
        }

        public Users FetchUserByUserName(string userName)
        {
            return userRepositoryService.FetchUserByName(userName);
        }

        //public void PopulateSalesOffices()
        //{
        //    View.SalesOfficeList = this.addressRepositoryService.FetchSalesOffices(View.SelectedRegion);
        //}

        #endregion

        #region Public Overrides

        public override void OnViewLoaded()
        {
            
        }

        public override void OnViewInitialized()
        {
            try
            {
                //View.RegionList = this.addressRepositoryService.FetchAllRegions();
                //View.SalesOfficeList = this.addressRepositoryService.FetchSalesOffices(View.SelectedRegion);
            }
            catch
            {
                throw;
            }
        }

        #endregion

    }
}




