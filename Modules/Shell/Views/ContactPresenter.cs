using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;

namespace VCTWebApp.Shell.Views
{
    public class ContactPresenter : Presenter<IContactView>
    {
        #region Instance Variables

        private ContactRepository contactRepositoryService;
        private DictionaryRepository dictionaryRepository;
        private AddressRepository addressRepositoryService;

        private Helper helper = new Helper();

        #endregion

        #region Constructors

        public ContactPresenter()
            : this(new ContactRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public ContactPresenter(ContactRepository contactRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ContactPresenter", "Constructor is invoked.");

            this.contactRepositoryService = contactRepository;
            this.dictionaryRepository = new DictionaryRepository();
            this.addressRepositoryService = new AddressRepository();
        }

        #endregion

        #region Private Methods

        private void SetFieldsBlank()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ContactPresenter", "SetFieldsBlank() is invoked.");

            View.FirstName = string.Empty;
            View.LastName = string.Empty;
            View.Email = string.Empty;
            View.Phone = string.Empty;
            View.Cell = string.Empty;
            View.Fax = string.Empty;
            View.Active = true;
            View.LocationContactList = null;

            View.LocationContactList = this.contactRepositoryService.GetLocationContactByContactId(0);
        }

        //private void PopulateSalesOffices()
        //{
        //    View.SalesOfficeList = this.addressRepositoryService.FetchSalesOffices();
        //}

        //private Contact GetSelectedContact()
        //{
        //    helper.LogInformation(HttpContext.Current.User.Identity.Name, "ContactPresenter", "GetSelectedContact() is invoked for selectedContactId: " + View.SelectedContactId.ToString());

        //    try
        //    {
        //        return View.ContactList.Find(delegate(Contact contactInList) { return contactInList.ContactId == View.SelectedContactId; });
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        #endregion

        #region Public Overrides

        public override void OnViewLoaded()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ContactPresenter", "OnViewLoaded is invoked.");

            try
            {
                Contact contact = this.contactRepositoryService.FetchContactsByContactId(View.SelectedContactId);
                if (contact != null)
                {
                    View.FirstName = contact.FirstName.Trim();
                    View.LastName = contact.LastName.Trim();
                    View.Email = contact.Email.Trim();
                    View.Phone = contact.Phone.Trim();
                    View.Cell = contact.Cell.Trim();
                    View.Fax = contact.Fax.Trim();
                    //View.LocationId = contact.LocationId;
                    View.Active = contact.IsActive;
                    View.LocationContactList = contact.LocationContactList;
                }
            }
            catch
            {
                throw;
            }
        }

        public override void OnViewInitialized()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ContactPresenter", "OnViewInitialized() is invoked.");
            try
            {
                View.ContactList = this.contactRepositoryService.FetchAllContacts();
                //this.PopulateSalesOffices();
                this.SetFieldsBlank();
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Public Methods

        public Constants.ResultStatus Save(string emailId)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "ContactPresenter", "Save() is invoked.");

            Constants.ResultStatus resultStatus = Constants.ResultStatus.Error;
             Contact contact = new Contact();
            try
            {
              
                contact.ContactId = View.SelectedContactId;
                contact.FirstName = View.FirstName;
                contact.LastName = View.LastName;
                contact.Email = View.Email;
                contact.Phone = View.Phone;
                contact.Cell = View.Cell;
                contact.Fax = View.Fax;
                contact.IsActive = View.Active;

                bool IsContactExists = false;
                if (View.SelectedContactId == 0)
                {
                    IsContactExists = this.contactRepositoryService.IsContactExists(contact);
                }
                else
                {
                    if (contact.Email != emailId)
                    {
                        IsContactExists = this.contactRepositoryService.IsContactExists(contact);
                    }
                }
                if (!IsContactExists)
                {

                    this.contactRepositoryService.SaveContact(contact, View.SelectedLocationIds);

                    helper.LogInformation(HttpContext.Current.User.Identity.Name, "ContactPresenter", "Contact '" + contact.ContactId + "' is saved successfully.");

                    if (View.SelectedContactId == 0)
                        resultStatus = Constants.ResultStatus.Created;
                    else
                        resultStatus = Constants.ResultStatus.Updated;
                }
                else
                { 
                     resultStatus = Constants.ResultStatus.DuplicateContact;
                }
            }
            catch
            {
                throw;
            }

            return resultStatus;
        }

        public Dictionary GetDictionaryRule(string key)
        {
            return this.dictionaryRepository.GetDictionaryRule(key);
        }

        #endregion
    }
}




