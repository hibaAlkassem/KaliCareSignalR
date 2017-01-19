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

namespace RxSignalr
{
    [HubName("DataHub")]
    public class DataHub : Hub
    {

        public interface IUserIdProvider
        {
            string GetUserId(IRequest request);
        }

        public void getInsights(int id, int mid, int lastid)
        {

            rxenseEntities2 db = new rxenseEntities2();
            int ListCount = 25;

            while (true)
            {
                //RxenseAPI.Controllers.RxUserController controller = new RxenseAPI.Controllers.RxUserController();
                //var insights = controller.GetNewInsightsByUser(id, 2, mid);
                var insights =
                               
                          ((from i in db.Insight
                            join m in db.RxMonitor on i.RxMonitorId equals m.RxMonitorId 
                            where m.RxUser.RxUserId == id && m.RxMonitorId == (mid==-1?m.RxMonitorId:mid) && i.InsightId > lastid
                            select new InsightDTO
                            {
                                InsightId = i.InsightId,
                                DeviceId = i.Device.DeviceKey,
                                InsightTypeId = i.InsightTypeId,
                                TimeStamp = i.TimeStamp,
                                Summary = i.Summary,
                                Confidence = i.Confidence,
                                RuleName = i.Rule.RuleName,
                                RuleId = i.RuleId,
                                DeviceUserId = 1,
                                DeviceUserName = m.Patient.FirstName + " " + m.Patient.LastName,
                                AccountID = 0,
                                MonitorId = i.RxMonitorId,
                                PatientNumber = m.PatientIdentifier,
                            })
              
                          .Union
                          (from i in db.Insight
                           join m in db.RxMonitor on i.RxMonitorId equals m.RxMonitorId 
                           join permissions in db.MonitorPermission on m.RxMonitorId equals permissions.MonitorId
                           where (m.RxUser.RxUserId != id && permissions.RxUserId == id) 
                           && db.RolePermission.Any(rp => rp.RoleId == permissions.RoleId && rp.PermissionValue.Equals("true") && rp.PermissionId == 1)
                           && m.RxMonitorId == (mid == -1 ? m.RxMonitorId : mid) && i.InsightId > lastid
                             && permissions.PermissionStatus.Equals("Active")
                           select new InsightDTO
                           {
                               InsightId = i.InsightId,
                               DeviceId = i.Device.DeviceKey,
                               InsightTypeId = i.InsightTypeId,
                               TimeStamp = i.TimeStamp,
                               Summary = i.Summary,
                               Confidence = i.Confidence,
                               RuleName = i.Rule.RuleName,
                               RuleId = i.RuleId,
                               DeviceUserId = 1,
                               DeviceUserName = db.RolePermission.Any(rp => rp.RoleId == permissions.RoleId && rp.PermissionValue.Equals("true") && rp.PermissionId == 2) ? m.Patient.FirstName + " " + m.Patient.LastName : "Anonymous - " + m.PatientIdentifier,
                               AccountID = 0,
                               MonitorId = i.RxMonitorId,
                               PatientNumber = m.PatientIdentifier
                           })
                          ).Distinct().OrderByDescending(x => x.TimeStamp);
                foreach (InsightDTO ival in insights)
                {
                    //String insight = "{\"$id\":\"" + ival.InsightId + "\",\"InsightId\":" + ival.InsightId + ",\"DeviceId\":\"" + ival.DeviceId + "\",\"InsightTypeId\":" + ival.InsightTypeId + ",\"TimeStamp\":\"" + String.Format("{0:s}", ival.TimeStamp) + "\",\"Summary\":\"" + ival.Summary + "\",\"Confidence\":" +
                    //    ival.Confidence.ToString() + ",\"RuleName\":\"" + ival.RuleName + "\", \"RuleId\":\"" + ival.RuleId + "\", \"DeviceUserName\":\"" + ival.DeviceUserName + "\"}";
                    //insight = "[" + insight + "]";


                    //Response.Write(string.Format("data: {0}\n\n", insight));

                    
                    //System.Threading.Thread.Sleep(500);
                    if (ival.InsightId <= lastid)
                        continue;
                    Dictionary<string, string> data = new Dictionary<string, string> { };
                    data.Add("InsightId", ival.InsightId.ToString());
                    data.Add("DeviceId", ival.DeviceId.ToString());
                    data.Add("InsightTypeId", ival.InsightTypeId.ToString());
                    data.Add("TimeStamp", String.Format("{0:s}", ival.TimeStamp));
                    data.Add("Summary", ival.Summary.ToString());
                    data.Add("Confidence", ival.Confidence.ToString());
                    data.Add("RuleName", ival.RuleName.ToString());
                    data.Add("RuleId", ival.RuleId.ToString());
                    if (ival.DeviceUserName.ToString() == " ")
                        data.Add("DeviceUserName", ival.PatientNumber.ToString());
                    else
                    data.Add("DeviceUserName", ival.DeviceUserName.ToString());
                    data.Add("mid", ival.MonitorId.ToString());
               
                    var result = new[] { data };


                    Clients.Client(Context.ConnectionId).getData(result, mid);

                    if (lastid < ival.InsightId) lastid = ival.InsightId;
                }
                System.Threading.Thread.Sleep(5000);
            }
        }
    
    
    
    
    
    
    }
}







