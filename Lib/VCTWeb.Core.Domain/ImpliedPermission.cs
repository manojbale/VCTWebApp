using System;

namespace VCTWeb.Core.Domain
{

	public class ImpliedPermission : BaseEntityClass
	{
	
		#region "Member Variables"
			
		private string m_permissioncode; 
		private string m_impliedpermissioncode; 		

		#endregion "Member Variables"
		
		#region "Default Constructor"

		public ImpliedPermission()
		{
			m_permissioncode = String.Empty; 
			m_impliedpermissioncode = String.Empty; 
		}
		
		#endregion "Default Constructor"
		
		#region "Public Properties"
			
		/// <summary>
		/// 
		/// </summary>		
		public string Action
		{
			get { return m_permissioncode; }
			set	
			{
				if (m_permissioncode != value)
				{
					m_permissioncode = value;
					this.IsModified = true;
				}
			}
		}
			
		/// <summary>
		/// 
		/// </summary>		
		public string ImpliedPermissionCode
		{
			get { return m_impliedpermissioncode; }
			set	
			{
				if (m_impliedpermissioncode != value)
				{
					m_impliedpermissioncode = value;
					this.IsModified = true;
				}
			}
		}
			
		#endregion "Public Properties" 
	
	}	
}
