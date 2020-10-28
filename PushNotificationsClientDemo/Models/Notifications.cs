using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PushNotificationsClientDemo.Models
{
    public class Notification
    {
        public string title { get; set; }
        public string body { get; set; }
        public string icon { get; set; } = "assets/main-page-logo-small-hat.png";
        public List<int> vibrate { get; set; }
        public Data data { get; set; }
        public List<Action> actions { get; set; }
    }

    public class Data
    {
        public string dateOfArrival { get; set; } = "Date.now()";
        public int primaryKey { get; set; } = 1;
    }

    public class Action
    {
        public string action { get; set; } = "explore";
        public string title { get; set; } = "Go to the site";
    }

}
