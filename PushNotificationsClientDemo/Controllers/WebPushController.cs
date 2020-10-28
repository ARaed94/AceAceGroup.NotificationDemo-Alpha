using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebPush;

namespace PushNotificationsClientDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WebPushController : ControllerBase
    {
        public WebPushController()
        {
        }

        [HttpGet]
        [Route("get-keys")]
        public IEnumerable<string> Get()
        {
            VapidDetails vapidKeys = VapidHelper.GenerateVapidKeys();

            // Prints 2 URL Safe Base64 Encoded Strings
            Console.WriteLine("Public {0}", vapidKeys.PublicKey);
            Console.WriteLine("Private {0}", vapidKeys.PublicKey);

            List<string> result = new List<string>()
            {
               string.Format($"Public: {vapidKeys.PublicKey}"),
               string.Format($"Private: {vapidKeys.PrivateKey}"),
            };

            return result;
        }


        [HttpPost]
        [Route("send")]
        public string Send()
        {
            var pushEndpoint = @"https://fcm.googleapis.com/fcm/send/cpXeECsP1KA:APA91bG7YajDkMmI19jW2mPranhGyV2vVwSRSjeHUD7rQHKiZuS_2s2Sw6LZQ9I5zkWc7_WAyLkj58mmvUP3fZi3K7fhyTKv2E8jPmCQXndQWBolzDQO9xEUxbQiwEDaM0m3He8vqiR7";
            var p256dh = @"BKS6rjnACQK29uumBvOlNfuE2IYTI6Jvv4GN_fxMrsiVxgI9rJ8LuplTt1b0uVytAHZSwQ0F3e1KxIWUSW6_BU0";
            var auth = @"XtsczBpNu3z7jRJZRcY62Q";

            var subject = @"https://www.google.com";
            var publicKey = @"BJdErwJmwoVV3kZ0liZ9BVimTnKIXK57Zh7FW6X9CFHnXnD9wyV4D2uBR2krZzYHiALGG3dRJmCnpbRsTt3iM1M";
            var privateKey = @"9u0E1ZeOlnrqeaCG6C39gN7FMyTPruZwOZj_gl3HeM4";


            var p256dhb64bytes = System.Text.Encoding.UTF8.GetBytes(p256dh);
            string p256dhb64 = System.Convert.ToBase64String(p256dhb64bytes);

            var authbytes = System.Text.Encoding.UTF8.GetBytes(auth);
            string authb64 = System.Convert.ToBase64String(authbytes);

            var subscription = new PushSubscription(pushEndpoint, p256dh, auth);
            var vapidDetails = new VapidDetails(subject, publicKey, privateKey);
            //var gcmAPIKey = @"[your key here]";

            var webPushClient = new WebPushClient();
            try
            {
                Model m = new Model()
                {
                    Id = 1,
                    Name = "Raed"
                };
                string json;
                using (StreamReader reader = new StreamReader("test.json"))
                {
                     json = reader.ReadToEnd();
                }

                var options = new Dictionary<string, object>
                {
                    ["vapidDetails"] = new VapidDetails(subject, publicKey, privateKey)
                };
                //var json = JsonConvert.SerializeObject(m);
                webPushClient.SendNotification(subscription, json, options);
                return "sent";
            }
            catch (WebPushException ex)
            {
                Console.WriteLine("Http STATUS code" + ex.StatusCode);
                throw new Exception("error");
            }
        }
    }
}
