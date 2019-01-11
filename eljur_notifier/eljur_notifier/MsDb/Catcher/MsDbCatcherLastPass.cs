﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eljur_notifier.AppCommonNS;
using System.Data.SqlClient;
using eljur_notifier.EljurNS;
using eljur_notifier.MsDbNS.SetterNS;
using eljur_notifier.StaffModel;

namespace eljur_notifier.MsDbNS.CatcherNS
{
    public class MsDbCatcherLastPass : EljurBaseClass
    {
        public MsDbCatcherLastPass() : base(new Message(), new StaffContext(), new MsDbSetter(), new EljurApiSender()) { }

        public void catchLastPass()
        {

            using (this.StaffCtx = new StaffContext())
            {
                TimeSpan timeNow = DateTime.Now.TimeOfDay;
                TimeSpan EventTimeNowSubstract15Min = timeNow.Add(new TimeSpan(0, -15, 0));
                var PupilIdOldAndTimeRows = from e in StaffCtx.Events
                                            where e.NotifyWasSend == false && e.EventName == "Вышел" && e.EventTime < EventTimeNowSubstract15Min
                                            orderby e.EventTime
                                            select new
                                            {
                                                PupilIdOld = e.PupilIdOld,
                                                EventTime = e.EventTime
                                            };
                foreach (var PupilIdOldAndTime in PupilIdOldAndTimeRows)
                {
                    var PupilIdOldAndTimeMassObjects = new object[2];
                    PupilIdOldAndTimeMassObjects[0] = PupilIdOldAndTime.PupilIdOld;
                    PupilIdOldAndTimeMassObjects[1] = PupilIdOldAndTime.EventTime;
                    Boolean result = eljurApiSender.SendNotifyLastPass(PupilIdOldAndTimeMassObjects);
                    if (result == true)
                    {
                        msDbSetter.SetStatusNotifyWasSend(Convert.ToInt32(PupilIdOldAndTime.PupilIdOld));
                    }
                }

            }


        }


    }
}
