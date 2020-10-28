using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PushNotificationsClientDemo.Context;
using PushNotificationsClientDemo.Entities;

namespace PushNotificationsClientDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscribersController : ControllerBase
    {
        public SubscribersController()
        {

        }

        [HttpGet]
        public List<Subscriber> FindAll()
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    return context.Subscribers.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public Subscriber Add(Subscriber subscriber)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    context.Subscribers.Add(subscriber);
                    context.SaveChanges();
                }
                return subscriber;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
