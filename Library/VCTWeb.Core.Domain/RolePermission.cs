using System;

namespace VCTWeb.Core.Domain
{

    /// <summary>
    /// Role Permission Classs
    /// </summary>
    [Serializable]
	public class RolePermission : BaseEntityClass
	{
	
		#region "Member Variables"
			
		private long _roleid;
        private string _entity;        
		private string _permissioncode;
        private string _impliedpermissioncode;
        private bool _grantpermision;

		#endregion "Member Variables"
		
		#region "Default Constructor"

		public RolePermission()
		{
			_roleid = 0;
            _entity = string.Empty;
			_permissioncode = String.Empty;
            _impliedpermissioncode = string.Empty;
            _grantpermision = false;
		}
		
		#endregion "Default Constructor"
		
		#region "Public Properties"


        /// <summary>
        /// Gets or sets the role id.
        /// </summary>
        /// <value>The role id.</value>
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
        /// Gets or sets the entity.
        /// </summary>
        /// <value>The entity.</value>
        public string Entity
        {
            get { return _entity; }
            set 
            {
                if (_entity != value)
                {
                    _entity = value;
                    this.IsModified = true;
                }
            }
        }


        /// <summary>
        /// Gets or sets the permission code.
        /// </summary>
        /// <value>The permission code.</value>
		public string PermissionCode
		{
			get { return _permissioncode; }
			set	
			{
				if (_permissioncode != value)
				{
					_permissioncode = value;
					this.IsModified = true;
				}
			}
		}


        /// <summary>
        /// Gets or sets the implied permission code.
        /// </summary>
        /// <value>The implied permission code.</value>
        public string ImpliedPermissionCode
        {
            get { return _impliedpermissioncode; }
            set
            {
                if (_impliedpermissioncode != value)
                {
                    _impliedpermissioncode = value;
                    this.IsModified = true;
                }
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [grant permission].
        /// </summary>
        /// <value><c>true</c> if [grant permission]; otherwise, <c>false</c>.</value>
        public bool GrantPermission
        {
            get { return _grantpermision; }
            set
            {
                if (_grantpermision != value)
                {
                    _grantpermision = value;
                    this.IsModified = true;
                }
            }
        }
			
		#endregion "Public Properties" 
	
	}	
}
