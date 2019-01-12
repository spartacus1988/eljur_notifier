﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using eljur_notifier.MsDbNS.CheckerNS;
using eljur_notifier.MsDbNS.RequesterNS;
using eljur_notifier.MsDbNS.CleanerNS;

namespace eljur_notifier.MsDbNS.FillerNS.Tests
{
    [TestClass()]
    public class MsDbFillerTests
    {
        [TestMethod()]
        public void FillOnlySchedulesTest()
        {
            MsDbFiller msDbFiller = new MsDbFiller("name=StaffContextTests");
            msDbFiller.FillOnlySchedules();

            EmptyChecker emptyChecker = new EmptyChecker("StaffContextTests");
            Assert.IsTrue(false == emptyChecker.IsTableEmpty("Schedules"));
        }

        [TestMethod()]
        public void FillOnlyPupilsTest()
        {
            PrepareTest();
            MsDbFiller msDbFiller = new MsDbFiller("name=StaffContextTests");
            var AllStaff = getStaffListTest();
            msDbFiller.FillOnlyPupils(AllStaff);
            MsDbRequester msDbRequester = new MsDbRequester("name=StaffContextTests");
            String FullFIO1 = msDbRequester.getFullFIOByPupilIdOld(5000);
            String FullFIO2 = msDbRequester.getFullFIOByPupilIdOld(5001);
            String FullFIO3 = msDbRequester.getFullFIOByPupilIdOld(5002);
            Assert.IsTrue(FullFIO1 == "Иванов Иван Иванович");
            Assert.IsTrue(FullFIO2 == "Петров Петр Петрович");
            Assert.IsTrue(FullFIO3 == "Сидоров Сидор Сидорович");
        }

        [TestMethod()]
        public void FillMsDbTest()
        {
            PrepareTest();
            MsDbFiller msDbFiller = new MsDbFiller("name=StaffContextTests");
            var AllStaff = getStaffListTest();
            msDbFiller.FillMsDb(AllStaff);
            EmptyChecker emptyChecker = new EmptyChecker("StaffContextTests");
            Assert.IsTrue(false == emptyChecker.IsTableEmpty("Schedules"));
            Assert.IsTrue(false == emptyChecker.IsTableEmpty("Pupils"));
        }

        public void PrepareTest()
        {
            MsDbCleaner msDbCleaner = new MsDbCleaner("name=StaffContextTests");
            msDbCleaner.clearTableDb("Pupils");
            msDbCleaner.clearTableDb("Schedules");
            msDbCleaner.clearTableDb("Events");

        }

        public List<object[]> getStaffListTest()
        {
            var AllStaff = new List<object[]>();
            object[] student1 = new object[23];
            student1[0] = 5000;
            student1[1] = "Иванов";
            student1[2] = "Иван";
            student1[3] = "Иванович";
            student1[22] = "Иванов Иван Иванович";
            AllStaff.Add(student1);
            object[] student2 = new object[23];
            student2[0] = 5001;
            student2[1] = "Петров";
            student2[2] = "Петр";
            student2[3] = "Петрович";
            student2[22] = "Петров Петр Петрович";
            AllStaff.Add(student2);
            object[] student3 = new object[23];
            student3[0] = 5002;
            student3[1] = "Сидоров";
            student3[2] = "Сидор";
            student3[3] = "Сидорович";
            student3[22] = "Сидоров Сидор Сидорович";
            AllStaff.Add(student3);
            return AllStaff;
        }


    }
}