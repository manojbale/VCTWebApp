using System;
using System.DirectoryServices;
using System.Security.Principal;
//using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace VCTWeb.Core.Domain
{
    public class Authentication
    {
        //[DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        //public static extern int LogonUser(String lpszUserName, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider,
        //     ref IntPtr phToken);
        //public const int LOGON32_LOGON_INTERACTIVE = 2;
        //public const int LOGON32_PROVIDER_DEFAULT = 0;

        private string _fullname = string.Empty;

        

        #region Constructors

        public Authentication()
        {
        }

        #endregion Constructors

        #region Public properties

        public string Fullname
        {
            get { return _fullname; }
        }
        #endregion

        #region Private Methods

        ///<summary>
        ///Authenticates the against active directory.
        ///</summary>
        ///<param name="userName">Name of the user.</param>        
        ///<param name="domainName">Name of the domain.</param>
        ///<returns>If authenticated, returns true, else false.</returns>
        private bool ExistsInActiveDirectory(string userName, string domainName)
        {
            try
            {

                DirectoryEntry directoryEntry;

                string startSearchPath = Constants.SEARCH_PATH_PREFIX + domainName;

                directoryEntry = new DirectoryEntry(startSearchPath);

                //Bind to the native AdsObject to force authentication
                Object nativeObject = directoryEntry.NativeObject;

                DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry);
                directorySearcher.Filter = "(SAMAccountName=" + userName + ")";
                directorySearcher.PropertiesToLoad.Add("cn");

                //Get the result
                System.DirectoryServices.SearchResult result = directorySearcher.FindOne();

                if (result == null)
                    return false;

                directorySearcher.Dispose();
                directoryEntry.Dispose();

                //Set the authentication status to true                
                return true;
            }
            catch
            {
                return false;
            }
        }
       

 
        /// <summary>
        /// Authenticates the against active directory.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="domainName">Name of the domain.</param>
        /// <returns>
        /// If authenticated, returns true, else false.
        /// </returns>
        private bool ExistsInActiveDirectory(string userName, string password, string domainName)
        {
            try
            {

                DirectoryEntry directoryEntry;

                string startSearchPath = Constants.SEARCH_PATH_PREFIX + domainName;

                directoryEntry = new DirectoryEntry(startSearchPath, domainName + @"\" + userName, password, AuthenticationTypes.Secure);

                //Bind to the native AdsObject to force authentication
                Object nativeObject = directoryEntry.NativeObject;

                DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry);
                directorySearcher.Filter = "(SAMAccountName=" + userName + ")";
                directorySearcher.PropertiesToLoad.Add("cn");

                //Get the result
                System.DirectoryServices.SearchResult result = directorySearcher.FindOne();
                
                if (result == null)
                    return false;

                if (result.Properties.Contains("cn"))
                {
                    _fullname = result.Properties["cn"][0].ToString();
                }

                directorySearcher.Dispose();
                directoryEntry.Dispose();
                //Set the authentication status to true                
                return true;
            }
            catch 
            {                
                return false;
            }
           
        }

        #endregion Private Methods

        #region Public Methods
       

        /// <summary>
        /// Gets all the available groups from active directory for the given user in the domain.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="domainName">Name of the domain.</param>
        /// <returns>array of group names</returns>
        public string GetUserGroupsFromActiveDirectory(string userName, string password ,string domainName)
        {
            //ArrayList groupsList = new ArrayList();
            System.Text.StringBuilder groupsList = new System.Text.StringBuilder();

            try
            {

                DirectoryEntry directoryEntry;

                string startSearchPath = "LDAP://" + domainName;

                //directoryEntry = new DirectoryEntry(startSearchPath);
                directoryEntry = new DirectoryEntry(startSearchPath, userName, password, AuthenticationTypes.Secure);
                //Bind to the native AdsObject to force authentication
                Object nativeObject = directoryEntry.NativeObject;

                DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry);
                directorySearcher.Filter = "(SAMAccountName=" + userName + ")";   //Searches active directory for the login name             
                directorySearcher.PropertiesToLoad.Add("memberOf"); // Gets number of groups this user is a member of.

                //Get the result
                System.DirectoryServices.SearchResult result = directorySearcher.FindOne();

                if (result != null)
                {
                    int groupCount = result.Properties["memberOf"].Count;

                    for (int counter = 0; counter < groupCount; counter++)
                    {
                        string group = (string)result.Properties["memberOf"][counter];
                        if (counter == 0)
                        {
                            groupsList.Append("'" + group.Substring(3, group.Substring(3).IndexOf(',')) + "'");
                        }
                        else
                        {
                            groupsList.Append(",").Append("'" + group.Substring(3, group.Substring(3).IndexOf(',')) + "'");
                        }
                    }
                }


                directorySearcher.Dispose();
                directoryEntry.Dispose();

                //Set the authentication status to true                
                //return (string[])groupsList.ToArray(typeof(string));
                return groupsList.ToString();


            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Authenticate user against active directory.
        /// </summary>
        /// <returns>
        /// If authenticated, returns true, else false
        /// </returns>
        public bool AuthenticateAgainstActiveDirectory()
        {
            return AuthenticateAgainstActiveDirectory(GetSystemUserName());
        }

        /// <summary>
        /// Authenticate user against active directory.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        /// If authenticated, returns true, else false
        /// </returns>
        public bool AuthenticateAgainstActiveDirectory(string username)
        {
            string domain = ConfigSetting.GetValue(ConfigSetting.DOMAIN); //Helper.GetAppSettingsValue("Domain");
            return AuthenticateAgainstActiveDirectory(username, domain);
        }

        /// <summary>
        /// Authenticate user against active directory.
        /// </summary>
        /// <returns>
        /// If authenticated, returns true, else false
        /// </returns>
        public bool AuthenticateAgainstActiveDirectory(string username, string domain)
        {
            bool isValidUser = false;
            isValidUser = ExistsInActiveDirectory(username, domain);

            return isValidUser;
        }

        /// <summary>
        /// Authenticate user against active directory.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="domain">The domain.</param>
        /// <returns>
        /// If authenticated, returns true, else false
        /// </returns>
        public bool AuthenticateAgainstActiveDirectory(string username, string password, string domain)
        {
            bool isValidUser = false;
            isValidUser = ExistsInActiveDirectory(username, password, domain);
            //IntPtr token = IntPtr.Zero;            
            //if (LogonUser(username.Trim(), domain, password.Trim(), LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, ref token) != 0)
            //        isValidUser=true;
            return isValidUser;
        }

        /// <summary>
        /// Authenticates the against active directory for web user.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>If authenticated, returns true, else false</returns>
        public bool AuthenticateAgainstActiveDirectoryWithDefaultDomain(string username, string password)
        {
            string domain = ConfigSetting.GetValue(ConfigSetting.DOMAIN); //Helper.GetAppSettingsValue("Domain");
            return AuthenticateAgainstActiveDirectory(username, password, domain);
        }

        /// <summary>
        /// Authenticates the against database.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public bool AuthenticateAgainstDatabase(string userName, string password)
        {
            bool retval=false;
            Users user=null;
            AuthenticationRepository ar=new AuthenticationRepository();
            retval = ar.CheckUserNameAndPassword(userName, password);
            ar.RetrieveUserInformation(userName, out user);
            _fullname = user.FullName;
            user = null;
            ar = null;
            return retval;
        }

        /// <summary>
        /// Authenticates the against database with encrypted password.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password_hash">The password_hash.</param>
        /// <returns></returns>
        public bool AuthenticateAgainstDatabaseEncrypted(string userName, string password_hash)
        {
            return (new AuthenticationRepository()).CheckUserNameAndPasswordHash(userName, password_hash);
        }
        

        /// <summary>
        /// Gets the name of the system user.
        /// </summary>
        /// <returns>System user name</returns>
        public string GetSystemUserName()
        {
            int pos = 0;
            string username = string.Empty;
            WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();
            pos = currentIdentity.Name.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase);
            if (pos != -1)
            {
                username = currentIdentity.Name.Substring(currentIdentity.Name.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase) + 1);
            }
            else
            {
                username = string.Empty;
            }
            return username;
        }



        #endregion Public Methods

    }

}