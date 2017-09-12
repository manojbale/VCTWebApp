using System;
using System.Collections;
using System.Collections.Generic;

namespace VCTWeb.Core.Domain
{

	public class Permission : BaseEntityClass
	{
	
		#region "Member Variables"
			
		private string _action; 
		private string _description; 
		private string _entityClass;

		#endregion "Member Variables"
		
		#region "Default Constructor"

		public Permission()
		{
			_action = String.Empty; 
			_description = String.Empty;
            _entityClass = String.Empty;
		}
		
		#endregion "Default Constructor"
		
		#region "Public Properties"
 
        /// <summary>
        /// Gets or sets the permission code
        /// </summary>
        /// <value>The permission code</value>
		public string Action
		{
			get { return _action; }
			set	
			{
				if (_action != value)
				{
					_action = value;
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
        /// Gets or sets the entity
        /// </summary>
        /// <value>The entity</value>
		public string EntityClass
		{
			get { return _entityClass; }
			set	
			{
				if (_entityClass != value)
				{
					_entityClass = value;
					this.IsModified = true;
				}
			}
		}
			
		#endregion "Public Properties" 

	
	}	
}
