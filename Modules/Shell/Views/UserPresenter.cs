using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using VCTWeb.Core.Domain;
using System.Web;
using System.Transactions;

namespace VCTWebApp.Shell.Views
{
    public class UserPresenter : Presenter<IUserView>
    {
        #region Instance Variables

        private RoleRepository roleRepositoryService;
        private UserRepository userRepositoryService;
        private DictionaryRepository dictionaryRepository;

        private Helper helper = new Helper();

        #endregion

        #region Constructors

        public UserPresenter()
            : this(new RoleRepository(), new UserRepository(HttpContext.Current.User.Identity.Name))
        {
        }

        public UserPresenter(RoleRepository roleRepository, UserRepository userRepository)
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "UserPresenter", "Constructor is invoked.");

            this.roleRepositoryService = roleRepository;
            this.userRepositoryService = userRepository;
            this.dictionaryRepository = new DictionaryRepository();
        }

        #endregion

        #region Private Methods

        private void SetFieldsBlank()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "UserPresenter", "SetFieldsBlank() is invoked.");

            View.UserId = string.Empty;
            View.FirstName = string.Empty;
            View.LastName = string.Empty;
            View.SecurityQuestion = string.Empty;
            View.SecurityAnswer = string.Empty;
            View.LocationId = 0;
            View.IsSystemUser = true;
            View.IsDomainUser = true;
            View.Domain = string.Empty;
            View.Email = string.Empty;
            View.Phone = string.Empty;
            View.Cell = string.Empty;
            View.Fax = string.Empty;
            View.ResetPassword = false;
            View.Active = true;
        }

        private Users GetSelectedUser()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "UserPresenter", "GetSelectedUser() is invoked for selectedUserId: " + View.SelectedUserId);

            try
            {
                return View.UserList.Find(delegate(Users userInList) { return userInList.UserName.Trim().ToLower() == View.SelectedUserId.Trim().ToLower(); });
            }
            catch
            {
                return null;
            }
        }

        private bool CheckIfAtleastOneRoleIsSelected()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "UserPresenter", "CheckIfAtleastOneRoleIsSelected() is invoked.");
            foreach (Role role in View.RoleList)
            {
                if (role.GrantRole)
                    return false;
            }

            return true;
        }

        private void SendEmail()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "UserPresenter", "SendEmail() is invoked.");

            try
            {
                string alertMailSubject = "User Details";
                string alertMailMessage = "The following user details are created/updated as under: "
                    + Environment.NewLine + "User ID: " + View.UserId
                    + Environment.NewLine + "Password: " + ConfigSetting.GetValue(ConfigSetting.DEFAULT_PASSWORD);

                int mailPort = 0;
                if (int.TryParse(ConfigSetting.GetValue(ConfigSetting.ALERT_MAIL_PORT), out mailPort))
                {
                    EmailSender emailSender = new EmailSender(ConfigSetting.GetValue(ConfigSetting.ALERT_MAIL_HOST), mailPort, false);
                    emailSender.Send(ConfigSetting.GetValue(ConfigSetting.ALERT_MAIL_FROM), View.Email, alertMailSubject, alertMailMessage);
                }
                else
                {
                    helper.LogInformation(HttpContext.Current.User.Identity.Name, "UserPresenter", "Mail sending failed.");
                }
            }
            catch
            {
                //Note: This exception is not be thrown as this has nothing to do with the transaction.
                //So, this is logged here.  
                helper.LogInformation(HttpContext.Current.User.Identity.Name, "UserPresenter", "Email sending failed.");
            }
        }

        #endregion

        #region Public Overrides

        public override void OnViewLoaded()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "UserPresenter", "OnViewLoaded is invoked.");

            try
            {
                Users user = this.GetSelectedUser();
                if (user != null)
                {
                    View.UserId = user.UserName.Trim();
                    View.FirstName = user.FirstName.Trim();
                    View.LastName = user.LastName.Trim();
                    View.SecurityQuestion = user.SecurityQuestion.Trim();
                    View.SecurityAnswer = user.SecurityAnswer.Trim();
                    View.LocationId = user.LocationId;
                    View.IsSystemUser = user.IsSystemUser;
                    View.IsDomainUser = user.IsDomainUser;
                    View.Domain = user.Domain.ToString();
                    View.Email = user.EmailID.Trim();
                    View.Phone = user.Phone.Trim();
                    View.Cell = user.Cell.Trim();
                    View.Fax = user.Fax.Trim();
                    View.ResetPassword = false;
                    View.Active = user.IsActive;
                    List<Role> userRoleList = this.roleRepositoryService.FetchRolesByUserName(user.UserName.Trim());
                    helper.LogInformation(HttpContext.Current.User.Identity.Name, "UserPresenter", "# records returned by FetchRolesByUserName() method: " + userRoleList.Count.ToString());

                    List<Role> allRoleList = this.roleRepositoryService.FetchAll();


                    //Set the grant status for the current user roles
                    foreach (Role userRole in userRoleList)
                    {
                        foreach (Role role in allRoleList)
                        {
                            if (userRole.RoleId == role.RoleId)
                            {
                                role.GrantRole = true;
                            }
                        }
                    }

                    View.RoleList = allRoleList;
                }
            }
            catch
            {
                throw;
            }
        }

        public override void OnViewInitialized()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "UserPresenter", "OnViewInitialized() is invoked.");
            try
            {
                View.UserList = this.userRepositoryService.FetchAllUsers();
                View.RoleList = this.roleRepositoryService.FetchAll();
                View.LocationList = new LocationRepository().FetchAllLocations();
                this.SetFieldsBlank();
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Public Methods

        public Constants.ResultStatus Save()
        {
            helper.LogInformation(HttpContext.Current.User.Identity.Name, "UserPresenter", "Save() is invoked.");

            Constants.ResultStatus resultStatus;
            try
            {
                resultStatus = CheckStatusForUser(View.UserId);
                
                if (resultStatus == Constants.ResultStatus.Ok)
                {
                    string mode = "Edit";
                    bool useDefaultPassword = View.ResetPassword;

                    Users user = this.GetSelectedUser();
                    if (user == null)
                    {
                        user = new Users();
                        mode = "Add";
                        useDefaultPassword = true; //In case of add mode default password is to be generated automatically
                    }
                    user.UserName = View.UserId;
                    user.FirstName = View.FirstName;
                    user.LastName = View.LastName;
                    user.SecurityQuestion = View.SecurityQuestion;
                    user.SecurityAnswer = View.SecurityAnswer;
                    user.LocationId = View.LocationId;
                    user.IsSystemUser = View.IsSystemUser;
                    user.IsDomainUser = View.IsDomainUser;
                    user.Domain = View.Domain;
                    user.EmailID = View.Email;
                    user.Phone = View.Phone;
                    user.Cell = View.Cell;
                    user.Fax = View.Fax;
                    user.IsActive = View.Active;

                    //Get the selected roles
                    List<Role> selectedRoleList = new List<Role>();
                    foreach (Role role in View.RoleList)
                    {
                        if (role.GrantRole)
                        {
                            selectedRoleList.Add(role);
                        }
                    }

                    string roleMembershipXmlString = string.Empty;
                    if (selectedRoleList.Count > 0)
                    {
                        roleMembershipXmlString = "<root>";
                        foreach (Role role in selectedRoleList)
                        {
                            roleMembershipXmlString += "<RoleMembership><RoleId>";
                            roleMembershipXmlString += role.RoleId;
                            roleMembershipXmlString += "</RoleId></RoleMembership>";
                        }
                        roleMembershipXmlString += "</root>";
                    }
                    this.userRepositoryService.SaveUserAndUserRole(user, mode, useDefaultPassword, roleMembershipXmlString);

                    helper.LogInformation(HttpContext.Current.User.Identity.Name, "UserPresenter", "User '" + user.UserName + "' is saved successfully.");

                    if (mode == "Add")
                        resultStatus = Constants.ResultStatus.Created;
                    else
                        resultStatus = Constants.ResultStatus.Updated;

                    if (resultStatus == Constants.ResultStatus.Created || resultStatus == Constants.ResultStatus.Updated)
                    {
                        if (!string.IsNullOrEmpty(View.Email))
                        {
                            this.SendEmail();
                        }
                        else
                        {
                            helper.LogInformation(HttpContext.Current.User.Identity.Name, "UserPresenter", "No EmailId mentioned. So no mail can be sent.");
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

            return resultStatus;
        }

        private Constants.ResultStatus CheckStatusForUser(string userId)
        {
            if (CheckIfAtleastOneRoleIsSelected())
            {
                return Constants.ResultStatus.SelectAtleastOneItem;
            }
            else if (!View.Active && this.userRepositoryService.CheckInUseUser(userId))
            {
                View.Active = true;
                return Constants.ResultStatus.InUse;
            }
            else if (View.LocationId == 0)
            {
                return Constants.ResultStatus.SelectLocation;
            }
            return Constants.ResultStatus.Ok;
        }

        public Dictionary GetDictionaryRule(string key)
        {
            return this.dictionaryRepository.GetDictionaryRule(key);
        }

        #endregion
    }
}




