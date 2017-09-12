using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VCTWeb.Core.Domain;

namespace VCTWebApp.Shell.Views
{
    public interface IeParPlusNotificationSetup
    {
        List<Customer> CustomerList { set; }
        List<NotificationDetail> NotificationDetailList { set; get; }
        List<Notification> ListNotification { set; get; }
        string ReceiverType { get; set; }
        string ReceiverName { get; set; }
        string ReceiverEmailId { get; set; }
        string Time { get; set; }
        string ConfigurationSetting { get; set; }
        Int64 SelectedNotificationDetailId { get; set; }
        string SelectedAccountNumber { get; }
        string SelectedNotificationType { get; set; }
        int PageSize { get; set; }
    }
}
