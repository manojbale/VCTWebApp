using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VCTWeb.Core.Domain
{
    public class NotificationRepository
    {
        public List<Notification> FetchAllNotificationType()
        {
            var listNotification = new List<Notification>();
            var db = DbHelper.CreateDatabase();
            using (var cmd = db.GetStoredProcCommand("Usp_FetchAllNotificationType"))
            {                
                using (var reader = new SafeDataReader(db.ExecuteReader(cmd)))
                {
                    while (reader.Read())
                    {
                        listNotification.Add(Load(reader));
                    }
                }
            }
            return listNotification;
        }
        
        private Notification Load(SafeDataReader reader)
        {
            Notification newNotification = new Notification();
            newNotification.NotificationType = reader.GetString("NotificationType");
            newNotification.Description = reader.GetString("Description");
            newNotification.LabelText = reader.GetString("LabelText");
            newNotification.TextBoxDefaultValue = reader.GetString("TextBoxDefaultValue");
            newNotification.ShowDays = reader.GetBoolean("ShowDays");
            newNotification.IsTextBoxEditable = reader.GetBoolean("IsTextBoxEditable");
            newNotification.IsTimerEditable = reader.GetBoolean("IsTimerEditable");
            newNotification.ConfigurationSettingHeader = reader.GetString("ConfigurationSettingHeader");
            return newNotification;
        }
    }
}
