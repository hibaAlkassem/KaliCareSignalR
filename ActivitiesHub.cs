using System;
using RxenseAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading;
using System.Web.Script.Serialization;
using System.Data.SqlClient;
using Microsoft.AspNet.SignalR.Client;
using System.Data;

namespace KaliCareSignalR
{
    [HubName("ActivitiesHub")]
    public class ActivitiesHub : Hub
    {



        public void getActivites(int lastId)
        {






            rxenseEntities2 db = new rxenseEntities2();


            while (true)
            {
                DateTime startDate = DateTime.Now;

                //var activities = from act in db.Activity
                //                 where act.ActivityId > lastId
                //                 select act;

                var activities = from i in db.Activity
                                 join sd in db.SensorData on i.SensorDataId equals sd.SensorDataId
                                 join s in db.Sensor on sd.SensorId equals s.SensorId
                                 where i.ActivityId > lastId
                                 select new ActivityDTO()
                                  {
                                      ActivityId = i.ActivityId,
                                      TimeStamp = i.TimeStamp,
                                      Value = i.Value,
                                      TimeStart = i.TimeStart,
                                      TimeEnd = i.TimeEnd,
                                      ActivitySummary = i.ActivitySummary,
                                      ActivityTypeId = i.ActivityTypeId,
                                      InsightId = i.InsightId,
                                      ActivityTypeName = i.ActivityType.ActivityTypeName,
                                      DeviceId = s.DeviceId,
                                      DeviceHardwareId = i.SensorData.Sensor.Device.DeviceHardwareId,
                                      SensorId = s.SensorId,
                                      VirtualDevice = s.Device.VirtualDevice,
                                      LoggedImportedDataId = sd.LoggedImportedDataId
                                  };

                
                foreach (ActivityDTO ival in activities)
                {





                    String activity = "{\"$id\":\"" + ival.ActivityId + "\",\"ActivityId\":" + ival.ActivityId + ",\"TimeStamp\":\"" + String.Format("{0:s}", DateTime.UtcNow) + "\",\"Value\":\"" + ival.Value + "\",\"ActivitySummary\":\"" + ival.ActivitySummary + "\", \"DeviceId\":\"" + ival.DeviceId + "\", \"SensorId\":\"" + ival.SensorId + "\", \"LoggedImportedDataId\":\"" + ival.LoggedImportedDataId + "\"}";
                    activity = "[" + activity + "]";
                    Clients.All.pushActivities(string.Format("data: {0}\n\n", activity));


                    if (lastId < ival.ActivityId) lastId = ival.ActivityId;
                }


                System.Threading.Thread.Sleep(6000);
            }




        }
    }
}







