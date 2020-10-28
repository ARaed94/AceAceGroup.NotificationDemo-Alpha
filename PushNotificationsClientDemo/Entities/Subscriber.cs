using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PushNotificationsClientDemo.Entities
{
    public class Subscriber
    {
        [Key]
        public Guid Id { get; set; }
        public string Device { get; set; }
        public string IPAddress { get; set; }
        public string OperatingSystem { get; set; }
        public DateTime SubscribedOn { get; set; }
        public DateTime LastNotificationSentOn { get; set; }
        public int SuccessfullPushNotificationsCount { get; set; }
        public int FailedPushNotificationsCount { get; set; }
        public string Endpoint { get; set; }
        public string P256DH { get; set; }
        public string Auth { get; set; }
        [NotMapped]
        public bool SentLastMsg { get; set; }
    }
}
