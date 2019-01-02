using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SparShaMap.DataService;
using SparShaMap.Models;

namespace SparShaMap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PushNotificationController : ControllerBase
    {
        public class Fcm
        {
            public string[] deviceTokens { get; set; }
            public string title { get; set; }
            public string body { get; set; }
            public object data { get; set; }
        }

        private static Uri FireBasePushNotificationsURL = new Uri("https://fcm.googleapis.com/fcm/send");
        private static string ServerKey;
        private readonly DataBaseService _db = new DataBaseService();
        public PushNotificationController()
        {
            ServerKey = _db.getServerKey();
        }
        public IActionResult Index()
        {
            return Ok("PushNotification Api");
        }
        
        [HttpGet("SearchData")]
        public async Task<IActionResult> SearchData(int startDateIndex)
        {
            try
            {
                var result = _db.SelectQueryNoAsync("SELECT [FIRST_NAME_TH],[LAST_NAME_TH],[CUS_ID] FROM [DB_01_UAT].[dbo].[M_CUSTOMER] WHERE REC_STATUS='Y'");
                var list = result.Skip(startDateIndex).Take(5);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceTokens">List of all devices assigned to a user</param>
        /// <param name="title">Title of notification</param>
        /// <param name="body">Description of notification</param>
        /// <param name="data">Object with all extra information you want to send hidden in the notification</param>
        /// <returns></returns>
        [HttpPost("SendPushNotification")]
        public async Task<IActionResult> SendPushNotification(Fcm fcm)
        {
            HttpResponseMessage result=null;
            if (fcm.deviceTokens.Count() > 0)
            {
                //Object creation

                var messageInformation = new Message()
                {
                    notification = new Notification()
                    {
                        title = fcm.title,
                        text = fcm.body
                    },
                    data = fcm.data,
                    registration_ids = fcm.deviceTokens
                };

                //Object to JSON STRUCTURE => using Newtonsoft.Json;
                string jsonMessage = JsonConvert.SerializeObject(messageInformation);
                
                //Create request to Firebase API
                var request = new HttpRequestMessage(HttpMethod.Post, FireBasePushNotificationsURL);
                if (ServerKey == null)
                {
                    Ok("Object is null");
                }
                request.Headers.TryAddWithoutValidation("Authorization", "key=" + ServerKey);
                request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    result = await client.SendAsync(request);
                }
            }

            return Ok(result);
        }

    }
}