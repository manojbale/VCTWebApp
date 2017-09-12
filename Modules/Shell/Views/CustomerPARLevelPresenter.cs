using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;


namespace VCTWebApp.Shell.Views
{
    public class CustomerPARLevelPresenter : Presenter<ICustomerPARLevel>
    {
        private CustomerPARLevelRepository customerPARLevelRepositoryInstance;
        private ConfigurationRepository configurationRepositoryInstance;
        private CustomerRepository customerRepositoryInstance;
        private Helper helper = new Helper();

        #region Constructors

        public CustomerPARLevelPresenter()
            : this(new CustomerPARLevelRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public CustomerPARLevelPresenter(CustomerPARLevelRepository CustomerPARLevelRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "CustomerPARLevelPresenter", "Constructor is invoked.");
            this.customerPARLevelRepositoryInstance = CustomerPARLevelRepository;
            customerRepositoryInstance = new CustomerRepository(HttpContext.Current.User.Identity.Name);
            configurationRepositoryInstance = new ConfigurationRepository(HttpContext.Current.User.Identity.Name);
        }

        #endregion


        #region Override Methods

        public override void OnViewLoaded()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "CustomerPARLevelPresenter", "OnViewLoaded() is invoked.");
            try
            {

                helper.LogInformation(HttpContext.Current.User.Identity.Name, "CustomerPARLevelPresenter", "OnViewLoaded() is completed");
            }
            catch
            {
                throw;
            }
        }

        public override void OnViewInitialized()
        {
            PopulatePageSize();
            SetFieldsBlank();
            PopulateCustomerParLevel();
          
        }

        #endregion


        #region Public Methods

        private void PopulatePageSize()
        {
            View.PageSize = this.configurationRepositoryInstance.GetGridPageSize();
        }

        private void SetFieldsBlank()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "CustomerPARLevelPresenter", "SetFieldsBlank() is invoked.");
            View.CustomerPARLevelList = null;
            View.CustomerList = customerRepositoryInstance.FetchAllCustomer(false, HttpContext.Current.User.Identity.Name);
        }

        public bool Save(string AccountNumber, string RefNum, Int16 PARLevelQty)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "CustomerPARLevelPresenter", "Save() is invoked.");
            bool resultStatus = false;
            try
            {
                CustomerPARLevel objCustomerPARLevel = new CustomerPARLevel();
                objCustomerPARLevel.AccountNumber = View.SelectedAccountNumber;
                objCustomerPARLevel.RefNum = RefNum;
                objCustomerPARLevel.PARLevelQty = PARLevelQty;
                resultStatus = this.customerPARLevelRepositoryInstance.SaveCustomerPARLevel(objCustomerPARLevel);
            }
            catch
            {
                throw;
            }
            return resultStatus;
        }

        public void PopulateCustomerParLevel()
        {
            View.CustomerPARLevelList = customerPARLevelRepositoryInstance.FetchCustomerPARLevelByAccountNumber(View.SelectedAccountNumber.Trim());
        }

        public bool DeleteCustomerPARLevel(string AccountNumber, string RefNum)
        {
            return customerPARLevelRepositoryInstance.DeleteCustomerPARLevel(AccountNumber, RefNum);
        }

        #endregion

    }
}
