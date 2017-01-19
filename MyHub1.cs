using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading;
using System.Web.Script.Serialization;

namespace KaliCareSignalR
{
    [HubName("MyHub1")]
    public class MyHub1 : Hub
    {
        int counter = 1;

        public void Hello(String message)
        {
            Clients.All.addMessage(message);
        }

        public void getNewInsights(string lastId) {
            


            while (true)
            {
                counter++;

                Dictionary<string, string> insight = new Dictionary<string, string> { };
                insight.Add("InsightId", lastId + counter);
                insight.Add("DeviceId", "163-117");
                insight.Add("InsightTypeId", "1");
                insight.Add("TimeStamp", "2014-12-18T21:27:56");
                insight.Add("Summary", "Took Larmabak");
                insight.Add("Confidence", "0.83");
                insight.Add("RuleName", "Kali Drop basic monitoring");
                insight.Add("RuleId", "1");
                insight.Add("DeviceUserName", "Someone");


                string myJsonString = (new JavaScriptSerializer()).Serialize(insight);

  

               // var result = new[] { "$id:" + lastId + counter, "InsightId:" + lastId + counter, "DeviceId:163-117", "InsightTypeId:1", "TimeStamp:2014-12-18T21:27:56", "Summary:Took Larmabak", "Confidence:0.83", "RuleName : Kali Drop basic monitoring", "RuleId:1", "DeviceUserName:Sina Fateh" };
                var result = new[] { insight };
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                //var json = serializer.Serialize(result);


                Clients.All.addInsight(result);
                System.Threading.Thread.Sleep(6000);



            } 

            


        
        
        }
    }
}