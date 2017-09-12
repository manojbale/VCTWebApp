using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;

namespace VCTWeb.Core.Domain
{
    public class ContactRepository
    {
        private string _user;

        public ContactRepository()
        {
        }

        public ContactRepository(string user)
        {
            _user = user;
        }

        #region Public Methods


        public Boolean IsContactExists(Contact contact)
        {
            SafeDataReader reader = null;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_CONTACTEXISTS))
            {
               
                db.AddInParameter(cmd, "@Email", DbType.String, contact.Email);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    if (reader.Read())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
               
            }
        }
        public void SaveContact(Contact contact, string locationIds)
        {
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_SAVECONTACT))
            {
                db.AddInParameter(cmd, "@ContactId", DbType.Int32, contact.ContactId);
                db.AddInParameter(cmd, "@FirstName", DbType.String, contact.FirstName);
                db.AddInParameter(cmd, "@LastName", DbType.String, contact.LastName);
                db.AddInParameter(cmd, "@IsActive", DbType.Boolean, contact.IsActive);
                db.AddInParameter(cmd, "@Email", DbType.String, contact.Email);
                db.AddInParameter(cmd, "@Phone", DbType.String, contact.Phone);
                db.AddInParameter(cmd, "@Cell", DbType.String, contact.Cell);
                db.AddInParameter(cmd, "@Fax", DbType.String, contact.Fax);
                db.AddInParameter(cmd, "@LocationIds", DbType.String, locationIds);
                db.AddInParameter(cmd, "@UpdatedBy", DbType.String, _user);
                db.ExecuteNonQuery(cmd);
            }
        }

        /// <summary>
        /// Fetches all Contacts.
        /// </summary>
        /// <returns></returns>
        public List<Contact> FetchAllContacts()
        {
            SafeDataReader reader = null;
            Contact newContact = null;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLISTOFCONTACTS))
            {
                List<Contact> listOfContact = new List<Contact>();
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        newContact = this.LoadContact(reader);
                        listOfContact.Add(newContact);
                    }

                }
                return listOfContact;
            }
        }

        /// <summary>
        /// Fetches all Contacts.
        /// </summary>
        /// <returns></returns>
        public Contact FetchContactsByContactId(int contactId)
        {
            SafeDataReader reader = null;
            Contact newContact = null;
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETCONTACTBYCONTACTID))
            {
                db.AddInParameter(cmd, "@ContactId", DbType.Int32, contactId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    if (reader.Read())
                    {
                        newContact = this.LoadContact(reader);

                        if (reader.NextResult())
                        {
                            newContact.LocationContactList = new List<LocationContact>();
                            while (reader.Read())
                            {
                                newContact.LocationContactList.Add(new LocationContact()
                                {
                                    LocationId = reader.GetInt32("LocationId"),
                                    LocationName = reader.GetString("LocationName"),
                                    Selected = reader.GetBoolean("Selected")
                                });
                            }
                        }
                    }
                }
                return newContact;
            }
        }

        #endregion

        #region Private Methods


        private Contact LoadContact(SafeDataReader reader)
        {
            Contact newContact = new Contact();
            newContact.ContactId = reader.GetInt32("ContactId");
            newContact.FullName = reader.GetString("FullName").Trim();
            newContact.FirstName = reader.GetString("FirstName").Trim();
            newContact.LastName = reader.GetString("LastName").Trim();
            newContact.IsActive = reader.GetBoolean("IsActive");
            newContact.Email = reader.GetString("Email").Trim();
            newContact.Phone = reader.GetString("Phone").Trim();
            newContact.Cell = reader.GetString("Cell").Trim();
            newContact.Fax = reader.GetString("Fax").Trim();
            //newContact.LocationId = reader.GetNullableInt32("LocationId");
            return newContact;
        }

        #endregion


        public List<LocationContact> GetLocationContactByContactId(int contactId)
        {
            SafeDataReader reader = null;
            List<LocationContact> LocationContactList = new List<LocationContact>();
            Database db = DbHelper.CreateDatabase();
            using (DbCommand cmd = db.GetStoredProcCommand(Constants.USP_GETLOCATIONCONTACTBYCONTACTID))
            {
                db.AddInParameter(cmd, "@ContactId", DbType.Int32, contactId);
                using (reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        LocationContactList.Add(new LocationContact()
                        {
                            LocationId = reader.GetInt32("LocationId"),
                            LocationName = reader.GetString("LocationName"),
                            Selected = reader.GetBoolean("Selected")
                        });
                    }
                }
                return LocationContactList;
            }
        }
    }
}
