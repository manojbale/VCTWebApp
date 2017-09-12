
/****************************************************************************
 * User.cs                                                        
 *                                                                          
 * Description:     Describes business logic for User.
 *
 * Author:          Vishal Tanwar
 * Date:            22/12/2008                                               
 *                                                                          
 ****************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using VCTWeb.Core.Domain.CustomValidators;

namespace VCTWeb.Core.Domain
{
    [Serializable]
    public class Users : BaseEntityClass
    {
        #region "instance variables"

        private string _userName;
        private string _fullName;
        private string _firstName;
        private string _lastName;
        private string _securityQuestion;
        private string _securityAnswer;
        private bool _isSystemUser;
        private bool _isDomainUser;
        private string _domain;
        private bool _isActive;
        private string _password;
        private string _newPassword;
        private string _verifyPassword;
        private string _emailID;
        private string _phone;
        private string _cell;
        private string _fax;
        private List<Role> _selectedRole;
        #endregion

        #region "Default Constructor"

        /// <summary>
        /// Initializes a new instance of the <see cref="Users"/> class.
        /// </summary>
        public Users()
        {
            _userName = string.Empty;
            _fullName = string.Empty;
            _firstName = string.Empty;
            _lastName = string.Empty;
            _securityQuestion = string.Empty;
            _securityAnswer = string.Empty;
            _isSystemUser = false;
            _isDomainUser = false;
            _domain = string.Empty;
            _isActive = true;
            _password = string.Empty;
            _newPassword = string.Empty;
            _verifyPassword = string.Empty;
            _emailID = string.Empty;
            _phone = string.Empty;
            _cell = string.Empty;
            _fax = string.Empty;
            _selectedRole = new List<Role>();
            
        }
        #endregion

        #region "public Properties"

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        [StringLengthValidator(1, 100, Ruleset = "User", MessageTemplate = "valUserName")]
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    this.IsModified = true;
                }
            }
        }


        /// <summary>
        /// Gets or sets the Full name.
        /// </summary>
        /// <value>The Full name.</value>
        public string FullName
        {
            get
            {
                return _fullName;
            }
            set
            {
                if (_fullName != value)
                {
                    _fullName = value;
                    this.IsModified = true;
                }
            }
        }


        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        [StringLengthValidator(1, 50, Ruleset = "User", MessageTemplate = "valFirstName")]
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    this.IsModified = true;
                }
            }
        }


        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        [StringLengthValidator(1, 50, Ruleset = "User", MessageTemplate = "valLastName")]
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    this.IsModified = true;
                }
            }
        }


        /// <summary>
        /// Gets or sets the security question.
        /// </summary>
        /// <value>The security question.</value>
        [StringLengthValidator(1, 250, Ruleset = "User", MessageTemplate = "valSecurityQuestion")]
        public string SecurityQuestion
        {
            get
            {
                return _securityQuestion;
            }
            set
            {
                if (_securityQuestion != value)
                {
                    _securityQuestion = value;
                    this.IsModified = true;
                }
            }
        }


        /// <summary>
        /// Gets or sets the security answer.
        /// </summary>
        /// <value>The security answer.</value>
        [StringLengthValidator(1, 50, Ruleset = "User", MessageTemplate = "valSecurityAnswer")]
        public string SecurityAnswer
        {
            get
            {
                return _securityAnswer;
            }
            set
            {
                if (_securityAnswer != value)
                {
                    _securityAnswer = value;
                    this.IsModified = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Users"/> is System User.
        /// </summary>
        /// <value><c>true</c> if System User; otherwise, <c>false</c>.</value>
        public bool IsSystemUser
        {
            get
            {
                return _isSystemUser;
            }
            set
            {
                if (_isSystemUser != value)
                {
                    _isSystemUser = value;
                    this.IsModified = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Users"/> is Domain User.
        /// </summary>
        /// <value><c>true</c> if Domain User; otherwise, <c>false</c>.</value>
        public bool IsDomainUser
        {
            get
            {
                return _isDomainUser;
            }
            set
            {
                if (_isDomainUser != value)
                {
                    _isDomainUser = value;
                    this.IsModified = true;
                }
            }
        }


        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>The domain.</value>
        public string Domain
        {
            get
            {
                return _domain;
            }
            set
            {
                if (_domain != value)
                {
                    _domain = value;
                    this.IsModified = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Users"/> is Active.
        /// </summary>
        /// <value><c>true</c> if Active; otherwise, <c>false</c>.</value>
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    this.IsModified = true;
                }
            }
        }


        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        //[StringLengthValidator(1, 64, Ruleset = "User", MessageTemplate = "valPassword")]
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    this.IsModified = true;
                }
            }
        }
        public string NewPassword
        {
            get
            {
                return _newPassword;
            }
            set
            {
                if (_newPassword != value)
                {
                    _newPassword = value;
                    this.IsModified = true;
                }
            }
        }
        //[StringLengthValidator(1, 64, Ruleset = "User", MessageTemplate = "valVerifyPassword")]
        public string VerifyPassword
        {
            get
            {
                return _verifyPassword;
            }
            set
            {
                if (_verifyPassword != value)
                {
                    _verifyPassword = value;
                    this.IsModified = true;
                }
            }
        }

        [RegexValidator(Constants.EMAIL_VALIDATION, Ruleset = "User",MessageTemplate="valInvalidEmailAddress")] 
        public string EmailID
        {
            get
            {
                return _emailID;
            }
            set
            {
                if (_emailID != value)
                {
                    _emailID = value;
                    this.IsModified = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Phone.
        /// </summary>
        /// <value>The Phone.</value>
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    this.IsModified = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Cell.
        /// </summary>
        /// <value>The Cell.</value>
        public string Cell
        {
            get
            {
                return _cell;
            }
            set
            {
                if (_cell != value)
                {
                    _cell = value;
                    this.IsModified = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Fax.
        /// </summary>
        /// <value>The Fax.</value>
        public string Fax
        {
            get
            {
                return _fax;
            }
            set
            {
                if (_fax != value)
                {
                    _fax = value;
                    this.IsModified = true;
                }
            }
        }

        public DateTime? LastPasswordDate { get; set; }
        public int LocationId { get; set; }
        public string Location { get; set; }
        public string LocationType { get; set; }

        /// <summary>
        /// Gets or sets the selected role.
        /// </summary>
        /// <value>The selected role.</value>
        public List<Role> SelectedRole
        {
            get { return _selectedRole; }
            set { _selectedRole = value; }
        }
       
        /// <summary>
        /// Copies this instance.
        /// </summary>
        /// <returns></returns>
        public Users Copy()
        {
            Users tmp = new Users();
            tmp.UserName = this._userName;
            tmp.FirstName = this._firstName;
            tmp.LastName = this._lastName;
            tmp.SecurityQuestion = this._securityQuestion;
            tmp.SecurityAnswer = this._securityAnswer;
            tmp.IsSystemUser = this._isSystemUser;
            tmp.IsDomainUser = this._isDomainUser;
            tmp.Domain = this._domain;
            tmp.IsActive = this._isActive;
            tmp.Password = this._password;
            tmp.EmailID = this._emailID;
            tmp.Phone = this._phone;
            tmp.Cell = this._cell;
            tmp.Fax = this._fax;

            return tmp;
        }
        #endregion
        
    }

    
}

