using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using VCTWeb.Core.Domain.CustomValidators;

namespace VCTWeb.Core.Domain
{
    /// <summary>
    /// Role Class
    /// </summary>
    [Serializable]
    public class Role : BaseEntityClass
    {

        #region "Member Variables"

        private long _roleid;
        private string _description;
        private string _rolename;
        private bool _isActive;

        private bool _grantRole; //this field is for maintaining and displaying the state of selection

        #endregion "Member Variables"

        #region "Default Constructor"

        public Role()
        {
            _roleid = 0;
            _description = String.Empty;
            _rolename = String.Empty;
            _isActive = true;
        }

        #endregion "Default Constructor"

        #region "Public Properties"
        /// <summary>
        /// Gets or sets the role id
        /// </summary>
        /// <value>The role id</value>
        public long RoleId
        {
            get { return _roleid; }
            set
            {
                if (_roleid != value)
                {
                    _roleid = value;
                    this.IsModified = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        /// <value>The description</value>
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    this.IsModified = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of the role.
        /// </summary>
        /// <value>The name of the role.</value>
        [RequiredStringValidator(Ruleset = "Role", MessageTemplate = "valRoleName")]
        public string RoleName
        {
            get { return _rolename; }
            set
            {
                if (_rolename != value)
                {
                    _rolename = value;
                    this.IsModified = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Role"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive == value)
                {
                    _isActive = value;
                    this.IsModified = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [grant role].
        /// </summary>
        /// <value><c>true</c> if [grant role]; otherwise, <c>false</c>.</value>
        public bool GrantRole
        {
            get { return _grantRole; }
            set { _grantRole = value; }
        }

        public Role Copy()
        {
            Role tmp = new Role();
            tmp.RoleName = this._rolename;
            tmp.Description = this._description;
            tmp.IsActive = this._isActive;
            return tmp;
        }

        #endregion "Public Properties"
    }
}
