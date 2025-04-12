using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PresentationLayer.Helpers
{
    public class Notification
    {
        public  NotificationType Type { get; set; }
        public  string Text { get; set; }
        public  string CssClass { get; set; }

        public static Notification Success(string Text, string CssClass)
        {           
            Notification notification = new Notification();           
            notification.Text = Text;
            notification.CssClass = CssClass;
            return notification; 
        }
        public static Notification Erorr(string Text, string CssClass)
        {
            Notification notification = new Notification();
            notification.Text = Text;
            notification.CssClass = CssClass;
            return notification;
        }
    }

    public enum NotificationType
    {
        Success, 
        Info,
        Error,       
        Worning     
    }
    public enum NotificationCssType
    {
        success,
        info,
        danger,
        warning
    }
}
