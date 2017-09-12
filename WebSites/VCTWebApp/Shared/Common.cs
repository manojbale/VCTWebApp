using System;
using System.Collections.Generic;
using System.Web;
using VCTWeb.Core.Domain;
using System.Text;
using System.Web.Security;
using System.Configuration;
using System.Globalization;

namespace VCTWebApp.Web
{
    public class Common
    {
        #region Constants

        public const string UNAUTHORIZED_PAGE = "~/ErrorPage.aspx?ErrorKey=Common_msgUnauthorizedPage";
        public const string INVALID_ACTION = "~/ErrorPage.aspx?ErrorKey=Common_msgInvalidAction";
        public const string ERROR_PAGE = "~/ErrorPage.aspx";
        public const string ERROR_KEY = "ErrorKey";
        public const string PCT_FILE_UPLOAD_EDIT = "PCTFileUploadEdit";
        public const string FILE_UPLOAD_DOCUMENT_TYPE = "FileUploadDocumentType";
        //for Report
        public const string REPORT_SHIPMENT_BY_PRODUCT_VIEW = "ReportShipmentByProduct.View";
        public const string REPORT_SHIPMENT_BY_SHIPPER_VIEW = "ReportShipmentByShipper.View";
        public const string REPORT_ADVANCED_SHIP_NOTICE_VIEW = "ReportAdvancedShipNotice.View";
        public const string REPORT_TICKET_VIEW = "ReportTicket.View";
        public const string REPORT_ADJUST_TICKET_VIEW = "ReportAdjustTicket.View";
        public const string REPORT_DEFECT_TICKET_VIEW = "ReportDefectTicket.View";
        public const string REPORT_RECEIVED_SHIPMENT_VIEW = "ReportReceivedShipment.View";
        public const string REPORT_RECONCILE_SHIPMENT_VIEW = "ReportReconcileShipment.View";
        public const string REPORT_TICKET_CONSUME_VIEW = "ReportTicketConsume.View";
        public const string REPORT_TICKET_PRODUCTION_VIEW = "ReportTicketProduction.View";
        public const string REPORT_PRODUCT_TYPE_VIEW = "ReportProductType.View";
        public const string REPORT_RECALL_VIEW = "ReportRecall.View";

        public const string DATE_END = "DateEnd";
        public const string DATE_START = "DateStart";
        public const string SHIPPER_ID = "ShipperID";
        public const string SHIPPER_NAME = "ShipperName";
        public const string REPORT_ID = "ReportID";
        public const string PARTY_TYPE_NAME = "PartyTypeName";
        public const string REPORT_CRITERIA_ALL = "ALL";
        public const string LOT_NUMBER = "LotNumber";
        public const string PRODUCT_CODE = "ProductCode";
        public const string SHIP_ORDER_NO = "ShipOrderNo";
        public const string DOCUMENT_ID = "DocumentID";
        public const string SHIPMENT_ID = "ShipmentID";
        public const string SHIP_DATE = "ShipDate";
        public const string PRINT_DATE = "PrintDate";
        public const string IMAGE_NAME = "ImageName";
        public const string IMAGE_PATH = "ImagePath";
        public const string IMAGE_PATH_URL = "Images\\";
        public const string TICKET_TYPE = "TicketType";
        public const string LOCATION = "Location";
        public const string LOCATION_NAME = "LocationName";
        public const string DEFECT_REASON = "DefectReason";
        public const string VENDOR_CODE = "VendorCode";
        public const string MSG_VAL_EXISTS = "msgValExists";
        public const string MSG_BADGE_EXISTS = "msgBadgeExists";
        public const string MSG_PRO_VAL_EXISTS = "msgProValExists";
        public const string MSG_VAL_IS_IN_USE = "msgValIsInUse";
        public const string MSG_VAL_IS_IN_PRODUCTION = "msgValIsInProduction";
        public const string MSG_ASN_UNAVAILABLE = "msgASNUnavailable";
        public const string MSG_VAL_IS_IN_CLOSE = "msgValIsInClose";
        public const string PRODUCT_TYPE_NAME = "ProductTypeName";
        public const string PRODUCT_TYPE_ID = "ProductTypeID";
        public const string MSG_VAL_COMPANY_PREFIX_IS_IN_USE = "msgComanyPrefixIsInUse";

        public const string CAN_ADD = "CanAdd";
        public const string CAN_EDIT = "CanEdit";
        public const string CAN_VIEW = "CanView";
        public const string CAN_DELETE = "CanDelete";
        public const string CAN_PRINT = "CanPrint";
        public const string CAN_CONSUME = "CanConsume";
        public const string CAN_REVERT_CONSUME = "CanRevertConsume";
        public const string CAN_ADJUST = "CanAdjust";

        public const string PER_MATCHED = "PerMatched";
        public const string PER_SHORTAGE = "PerShortage";
        public const string PER_OVERAGE = "PerOverage";
        public const string PRINTING_SIDE = "PrintingSide";
        public const string SERVER = "Server";
        public const string CLIENT = "Client";
        #endregion

        #region Public Methods
        /// <summary>
        /// Strings to list.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        private List<string> StringToList(string str)
        {
            const char list_element_seperator = ',';
            List<string> list = new List<string>();
            string[] data2 = str.Trim().Split(list_element_seperator);
            foreach (string str1 in data2)
            {
                list.Add(str1.Trim());
            }
            return list;
        }

        /// <summary>
        /// Lists to string.
        /// </summary>
        /// <param name="listOfString">The list of string.</param>
        /// <returns></returns>
        private string ListToString(List<string> listOfString)
        {
            const char list_element_seperator = ',';
            StringBuilder sb = new StringBuilder();
            string tmp = string.Empty;
            foreach (string str in listOfString)
            {
                tmp = str.Trim();
                if (!tmp.Equals(string.Empty))
                {
                    tmp += list_element_seperator;
                }
                sb.Append(tmp);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <returns>return current user</returns>
        public static string GetCurrentUser()
        {
            string username = string.Empty;
            // Try to retrieve it from session
            if (HttpContext.Current.Session != null)
            {
                if (!(HttpContext.Current.Session["username"] == null ||
                    HttpContext.Current.Session["username"].ToString() == string.Empty))
                {
                    username = HttpContext.Current.Session["username"].ToString().Trim();
                }
                else
                {
                    // If not found in session try to retrieve it from Context
                    if (HttpContext.Current.User != null)
                    {
                        username = HttpContext.Current.User.Identity.Name;
                        // Save it in session for later use
                        if (username != string.Empty)
                        {
                            HttpContext.Current.Session["username"] = username;
                        }
                        else
                        {
                            //For debug purpose only
                            //throw new Exception("User.Identity.Name found blank in Context.");
                        }
                    }
                    else
                    {
                        // user missing from context, an authentication issue raise exception
                        throw new Exception("User not found in Context.");
                    }
                }
            }
            else
            {
                // Session not supported then retrieve it from Context
                if (HttpContext.Current.User != null)
                        username = HttpContext.Current.User.Identity.Name;
                //For debug purpose only
                //throw new Exception("Session not avaiable in Context.");
            }
            return username;
        }

        /// <summary>
        /// Gets the full name of user.
        /// </summary>
        /// <param name="user">The user.</param>
        public static string GetFullNameOfUser(string username)
        {
         
            string fullname = string.Empty;

            if (!String.IsNullOrEmpty(username))
            {
                if (HttpContext.Current.Session != null)
                {
                    if (!(HttpContext.Current.Session["fullname"] == null ||
                        HttpContext.Current.Session["fullname"].ToString() == string.Empty))
                    {
                        fullname = HttpContext.Current.Session["fullname"].ToString().Trim();
                    }
                    else
                    {
                        // If not found in session try to retrieve it from Ticket
                        if (!String.IsNullOrEmpty(username))
                        {
                            Security sec = new Security();
                            fullname = sec.FullName;
                        }
                    }
                }
                else
                {
                    // Session not supported then retrieve it from Ticket
                    if (!String.IsNullOrEmpty(username))
                    {
                        Security sec = new Security();
                        fullname = sec.FullName;
                    }

                }
            }
            return fullname;            
        }

        public static bool IsNewUser(string username)
        {

            string fullname = string.Empty;
            Boolean isnewuser = false;

            if (!String.IsNullOrEmpty(username))
            {
                Security sec = new Security();
                if (sec.Password == "iris")
                {
                    isnewuser = true;
                }

                
            }
            return isnewuser;
        }
        /// <summary>
        /// Sets the current user.
        /// </summary>
        /// <returns></returns>
        public static void SetCurrentUserInSession(string username, string fullname)
        {
            if (HttpContext.Current.Session != null)
            {
                if (username != string.Empty)
                {
                    HttpContext.Current.Session["username"] = username;
                    HttpContext.Current.Session["fullname"] = fullname;
                }
                else
                    throw new Exception("Do you really want to store blank user name in session?");
            }
            else
            {
                // No issue we will it from Context

                //For debug purpose only
                //throw new Exception("Session not avaiable in Context.");
            }
        }
        #endregion Public Methods

        #region Other

        public enum ReportType : int
        {
            SHIPMENT_BY_SHIPPER = 1,
            SHIPMENT_BY_PRODUCT = 2,
            ADVANCED_SHIP_NOTICE = 3,
            TICKET = 4,
            DEFECTIVE_TICKET = 5,
            ADJUSTMENT_TICKET = 6,
            RECEIVED_SHIPMENT = 7,
            CONSUMPTION_BY_PRODUCT = 8,
            PRODUCTION_BY_PRODUCT = 9,
            RECONCILE_SHIPMENT = 10,
            PRODUCT_TYPE = 11,
            RECALL = 12
        }

        #endregion

        #region Public Properties

        public static bool IsPrintingServerSide
        {
            get
            {
                string printingSide = ConfigurationManager.AppSettings[Common.PRINTING_SIDE];
                if (!string.IsNullOrEmpty(printingSide))
                {
                    if (string.Compare(printingSide, Common.SERVER, true, CultureInfo.InvariantCulture) == 0)
                        return true;
                    else
                        return false;

                }
                else
                    return false;
            }
        }

        #endregion

    }

    /// <summary>
    /// Security Class to check permissions
    /// </summary>
    public class Security
    {
        #region Private Fields

        private string _user = string.Empty;
        private string _password = string.Empty;
        private List<Permission> _permissions = new List<Permission>();
        private string _domain = string.Empty;
        private bool _isActiveDirectory = false;
        private string _fullname = string.Empty;

        #endregion

        #region Constants

        const char data_seperator = '\t';
        

        #endregion

        #region Constructors

        public Security()
        {

            FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;
            if (!string.IsNullOrEmpty(identity.Ticket.UserData))
            {
                LoadData(identity.Ticket.UserData);
                if (_permissions.Count == 0)
                {
                    if (HttpContext.Current.Session != null)
                    {
                        if (HttpContext.Current.Session["UserPermissions"] == null)
                        {
                            LoadPermissions();
                            HttpContext.Current.Session["UserPermissions"] = _permissions;
                            HttpContext.Current.Session["username"] = _user;
                            HttpContext.Current.Session["fullname"] = _fullname;
                        }
                        else
                        {
                            _permissions = (List<Permission>)HttpContext.Current.Session["UserPermissions"];
                        }
                    }
                    else
                    {
                        LoadPermissions();
                    }
                }
            }
        }

        public Security(string user, string password)
        {
            _user = user;
            _password = password;
            _isActiveDirectory = false;
            LoadPermissions();
        }

        public Security(string user, string password, string domain)
        {
            _user = user;
            _password = password;
            _domain = domain;
            _isActiveDirectory = true;
            LoadPermissions();
        }

        #endregion

        #region Public Properties

        public string Domain
        {
            get { return _domain; }
        }

        public string User
        {
            get { return _user; }
        }

        public string Password
        {
            get { return _password; }
        }

        public List<Permission> Permissions
        {
            get { return _permissions; }
        }

        public bool IsActiveDirectory
        {
            get { return _isActiveDirectory; }
        }
        public string FullName
        {
            set { _fullname = value; }
            get { return _fullname; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the data as string.
        /// </summary>
        /// <returns></returns>
        public string GetDataAsString()
        {
            //return string.Format("{0}\t{1}\t{2}", _user, _password, _permissions);
            return string.Format("{0}\t{1}\t{2}\t{3}\t{4}", _user, _password, _isActiveDirectory.ToString(), _domain,_fullname);
        }

        /// <summary>
        /// Determines whether the user has specified permission.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns>
        /// 	<c>true</c> if the user has specified permission; otherwise, <c>false</c>.
        /// </returns>
        public bool HasPermission(string permission)
        {
            return _permissions.Exists(obj => obj.Action == permission);
        }

        /// <summary>
        /// Determines whether the user can access specified entity name can be accessed.
        /// </summary>
        /// <param name="entiryName">Name of the entiry.</param>
        /// <returns>
        /// 	<c>true</c> if the specified entity name can be accessed; otherwise, <c>false</c>.
        /// </returns>
        public bool HasAccess(string entityName)
        {
            return _permissions.Exists(obj => obj.EntityClass == entityName);
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// Loads the user data corrosponding to UserData field in FormsAuthenticationTicket.
        /// </summary>
        /// <param name="userData">The user data.</param>
        private void LoadData(string userData)
        {
            string permission_data = string.Empty;

            string[] data1 = userData.Split(data_seperator);

            switch (data1.Length)
            {
                case 1:
                    {
                        _user = data1[0].Trim();
                        break;
                    }
                case 2:
                    {
                        _user = data1[0].Trim();
                        _password = data1[1].Trim();
                        break;
                    }
                case 3:
                    {
                        _user = data1[0].Trim();
                        _password = data1[1].Trim();
                        _isActiveDirectory = bool.Parse(data1[2].Trim());
                        break;
                    }
                case 4:
                    {
                        _user = data1[0].Trim();
                        _password = data1[1].Trim();
                        _isActiveDirectory = bool.Parse(data1[2].Trim());
                        _domain = data1[3].Trim();
                        break;
                    }
                case 5:
                    {
                        _user = data1[0].Trim();
                        _password = data1[1].Trim();
                        _isActiveDirectory = bool.Parse(data1[2].Trim());
                        _domain = data1[3].Trim();
                        _fullname = data1[4].Trim();
                        break;
                    }
                default:
                    {
                        _user = string.Empty;
                        _password = string.Empty;
                        _permissions = new List<Permission>();
                        break;
                    }
            }
        }


        /// <summary>
        /// Loads the permissions.
        /// </summary>
        private void LoadPermissions()
        {
            //PermissionRepository pr = new PermissionRepository();
            Authentication authentication = new Authentication();
            PermissionRepository permissionRepository = new PermissionRepository();
            _permissions.Clear();
            string strPermission = string.Empty;
            if (_isActiveDirectory)
            {
                string adGroupList = string.Empty;
                adGroupList = authentication.GetUserGroupsFromActiveDirectory(_user,_password, _domain);
                _permissions = permissionRepository.FetchByRoles(adGroupList);
            }
            else
            {
                _permissions = permissionRepository.FetchByUserName(_user);
            }
        }

        #endregion


    }
}
