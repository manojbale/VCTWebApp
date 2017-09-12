using System;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Text;


namespace VCTWebApp.Shell.Views
{
    public class CustomerPresenter : Presenter<ICustomerView>
    {
        #region Instance Variables

        private CustomerRepository customerRepositoryService;
        private UserRepository userRepositoryService;
        private EParPlusRepository eParPlusRepositoryService;
        private DictionaryRepository dictionaryRepositoryService;
        private StateRepository stateRepositoryService;
        private Helper helper = new Helper();

        #endregion

        #region Constructors

        public CustomerPresenter()
            : this(new CustomerRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public CustomerPresenter(CustomerRepository customerRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "CustomerPresenter", "Constructor is invoked.");
            this.customerRepositoryService = customerRepository;
            this.userRepositoryService = new UserRepository();
            eParPlusRepositoryService = new EParPlusRepository();
            dictionaryRepositoryService = new DictionaryRepository();
            stateRepositoryService = new StateRepository();
        }

        #endregion

        #region Private Methods

        private void SetFieldsBlank()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "CustomerPresenter", "SetFieldsBlank() is invoked.");
            View.AccountNumber = string.Empty;
            View.Active = true;
            View.BranchAgency = string.Empty;
            View.City = string.Empty;
            View.CustomerName = string.Empty;
            View.ManagementStructure = string.Empty;
            View.Manager = string.Empty;
            View.OwnershipStructure = string.Empty;
            View.QtyOfORs = 0;
            View.SpineOnlyMultiSpecialty = string.Empty;
            View.State = string.Empty;
            View.StreetAddress = string.Empty;
            View.Zip = string.Empty;
            View.ConsumptionInterval = Convert.ToInt16(dictionaryRepositoryService.GetDictionaryRule("DefaultConsumptionInterval_Mins").KeyValue);
            View.AssetNearExpiryDays = Convert.ToInt16(dictionaryRepositoryService.GetDictionaryRule("AssetNearExpiryDays").KeyValue);
            View.CustomerProductLineList = customerRepositoryService.FetchCustomerProductLineByProductLine(string.Empty);
            View.SalesRepresentative= string.Empty;
            View.SpecialistRep = string.Empty;
        }

        #endregion

        #region Public Overrides

        public override void OnViewLoaded()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "CustomerPresenter", "OnViewLoaded is invoked.");
            SetFieldsBlank();
            try
            {
                Customer customer = this.customerRepositoryService.FetchCustomerByAccountNumber(View.SelectedCustomerAccountNumber);
                if (customer != null)
                {
                    View.AccountNumber = customer.AccountNumber.Trim();
                    View.Active = customer.IsActive;
                    View.BranchAgency = customer.BranchAgency.Trim();
                    View.City = customer.City.Trim();
                    View.SalesRepresentative = customer.SalesRepresentative;
                    View.SpecialistRep = customer.SpecialistRep;
                    View.CustomerName = customer.CustomerName.Trim();
                    View.ManagementStructure = customer.ManagementStructure.Trim();
                    View.Manager = customer.Manager.Trim();
                    View.OwnershipStructure = customer.OwnershipStructure.Trim();
                    View.QtyOfORs = customer.QtyOfORs;
                    View.SpineOnlyMultiSpecialty = customer.SpineOnlyMultiSpecialty.Trim();
                    View.State = customer.State.Trim();
                    View.StreetAddress = customer.StreetAddress.Trim();
                    View.Zip = customer.Zip.Trim();
                    View.ConsumptionInterval = customer.ConsumptionInterval;
                    View.AssetNearExpiryDays = customer.AssetNearExpiryDays;
                    View.CustomerProductLineList = customerRepositoryService.FetchCustomerProductLineByProductLine(View.AccountNumber);
                }
            }
            catch
            {
                throw;
            }
        }

        public override void OnViewInitialized()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "CustomerPresenter", "OnViewInitialized() is invoked.");
            try
            {
                View.CustomerList = this.customerRepositoryService.FetchAllCustomer(true);
                this.SetFieldsBlank();

                List<string> lstOwnershipStructure = this.eParPlusRepositoryService.FetchDistinctColumnValueForTable("Customer", "OwnershipStructure");
                View.OwnershipStructureList = lstOwnershipStructure;

                List<string> lstManagementStructure = this.eParPlusRepositoryService.FetchDistinctColumnValueForTable("Customer", "ManagementStructure");
                View.ManagementStructureList = lstManagementStructure;

                List<string> lstSpineOnlyMultiSpecialty = this.eParPlusRepositoryService.FetchDistinctColumnValueForTable("Customer", "SpineOnlyMultiSpecialty");
                View.SpineOnlyMultiSpecialtyList = lstSpineOnlyMultiSpecialty;

                List<string> lstBranchAgency = this.eParPlusRepositoryService.FetchDistinctColumnValueForTable("Customer", "BranchAgency");
                View.BranchAgencyList = lstBranchAgency;

                View.ManagerList = this.userRepositoryService.FetchListOfSystemUsersByRoleName("ePar+ Manager");
                View.SalesRepresentativeList = this.userRepositoryService.FetchListOfSystemUsersByRoleName("ePar+ Sales Rep");
                View.SpecialistRepList = this.userRepositoryService.FetchListOfSystemUsersByRoleName("ePar+ Specialist Rep");
                


                List<State> lstState = this.stateRepositoryService.GetStateList("US");
                View.StateList = lstState;

            }
            catch
            {
                throw;
            }
        }

        List<CustomerProductLine> ListCustomerProductLine = null;

        public List<CustomerProductLine> FilterCustomerForProductLine(string ProductLineName)
        {
            List<CustomerProductLine> list = new List<CustomerProductLine>();
            if (ListCustomerProductLine == null || ListCustomerProductLine.Count <= 0)
                ListCustomerProductLine = this.customerRepositoryService.FetchAllCustomerProductLine();
            list = ListCustomerProductLine.FindAll(c => c.ProductLineName == ProductLineName);
            return list;
        }

        #endregion

        #region Public Methods

        public void ResetCustometList()
        {
            View.CustomerList = this.customerRepositoryService.FetchAllCustomer(true);
        }

        public Constants.ResultStatus Save()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "CustomerPresenter", "Save() is invoked.");
            Constants.ResultStatus resultStatus = Constants.ResultStatus.Error;
            try
            {
                Customer customer = new Customer();
                customer.AccountNumber = View.AccountNumber;
                customer.City = View.City;
                customer.SalesRepresentative = View.SalesRepresentative;
                customer.SpecialistRep = View.SpecialistRep;
                customer.CustomerName = View.CustomerName;
                customer.IsActive = View.Active;
                customer.State = View.State;
                customer.StreetAddress = View.StreetAddress;
                customer.Zip = View.Zip;
                customer.QtyOfORs = View.QtyOfORs;
                customer.SpineOnlyMultiSpecialty = View.SpineOnlyMultiSpecialty;
                customer.ManagementStructure = View.ManagementStructure;
                customer.Manager = View.Manager;
                customer.OwnershipStructure = View.OwnershipStructure;
                customer.BranchAgency = View.BranchAgency;
                customer.ConsumptionInterval = View.ConsumptionInterval;
                customer.AssetNearExpiryDays = View.AssetNearExpiryDays;
                customer.ProductLines = View.SelectedProductLines;

                if (this.customerRepositoryService.SaveCustomer(customer, GetXMLForCustomerProductLineList()))
                {
                    helper.LogInformation(HttpContext.Current.User.Identity.Name, "CustomerPresenter", "Contact '" + customer.AccountNumber + "' is saved successfully.");
                    if (string.IsNullOrEmpty(View.SelectedCustomerAccountNumber))
                        resultStatus = Constants.ResultStatus.Created;
                    else
                        resultStatus = Constants.ResultStatus.Updated;
                }
                else
                    resultStatus = Constants.ResultStatus.Failed;
            }
            catch
            {
                throw;
            }

            return resultStatus;
        }

        public void FilterCustomer()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "FilterCustomer", "FilterCustomer() is invoked.");
            try
            {
                View.CustomerList = this.customerRepositoryService.FetchAllFilteredCustomer(View.SelectedCustomerNameFilter, View.SelectedAccountNumberFilter, View.SelectedOwnershipStructureFilter,
                    View.SelectedManagementStructureFilter, View.SelectedSpineonlyMultiSpecialtyFilter, View.SelectedBranchAgencyFilter, View.SelectedManagerFilter, View.SelectedSalesRepresentativeFilter, View.SelectedActiveInactiveFilter);
                this.SetFieldsBlank();
            }
            catch
            {
                throw;
            }
        }


        private string GetXMLForCustomerProductLineList()
        {
            string XMLString = string.Empty;
            try
            {
                List<CustomerProductLine> ProductLineList = View.CustomerProductLineList;

                //XmlDocument xml = new XmlDocument();
                //XmlElement root = xml.CreateElement("root");
                //xml.AppendChild(root);
                //foreach (var cust in ProductLineList)
                //{
                //    XmlElement child = xml.CreateElement("CustomerProductLine");
                //    child.SetAttribute("ProductLine", cust.ProductLineName.ToString());
                //    child.SetAttribute("AccountNumber", cust.AccountNumber);                    
                //    root.AppendChild(child);
                //}
                //XMLString = xml.OuterXml;

                StringBuilder builder = new StringBuilder();
                //XmlTextWriter writer = new XmlTextWriter("CustomerProductLine.xml", System.Text.Encoding.UTF8);

                XmlWriter writer = XmlWriter.Create(builder);
                writer.WriteStartDocument(true);
                //writer.Formatting = Formatting.Indented;
                //writer.Indentation = 2;
                writer.WriteStartElement("root");


                foreach (var cust in ProductLineList)
                {
                    createNode(cust.ProductLineName,cust.AccountNumber, writer);
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();

                XMLString = builder.ToString();

                XMLString = XMLString.Substring(XMLString.IndexOf("<root>"));
            }
            catch (Exception ex)
            {
                XMLString= string.Empty;
            }
            return XMLString;
        }

        private void createNode(string ProductLine, string AccountNumber, XmlWriter writer)
        {
            writer.WriteStartElement("CustomerProductLine");

            writer.WriteStartElement("ProductLine");
            writer.WriteString(ProductLine);
            writer.WriteEndElement();

            writer.WriteStartElement("AccountNumber");
            writer.WriteString(AccountNumber);
            writer.WriteEndElement();
                        
            writer.WriteEndElement();
        }
        #endregion Public Methods
    }
}
