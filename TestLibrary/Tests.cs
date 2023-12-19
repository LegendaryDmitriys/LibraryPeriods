using System;
using NUnit.Framework;
using SF2022UserNNLib;

namespace TestLibrary
{
    [TestFixture]
    public class Tests
    {
        [Test]
        // Тест для случая, когда консультации отсутствуют. Ожидается полный рабочий день.
        public void AvailablePeriods_NoConsultations_ReturnsFullWorkingDay()
        {
            TimeSpan[] startTimes = { };
            int[] durations = { };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 30;
            
            string[] result = Calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);
            
            CollectionAssert.AreEqual(new[] { "09:00-09:30", "09:30-10:00", "10:00-10:30", "10:30-11:00", "11:00-11:30", "11:30-12:00", "12:00-12:30", "12:30-13:00", "13:00-13:30", "13:30-14:00", "14:00-14:30", "14:30-15:00", "15:00-15:30", "15:30-16:00", "16:00-16:30", "16:30-17:00" }, result);
        }

        [Test]
        // Тест для случая с единственной консультацией.
        public void AvailablePeriods_SingleConsultation()
        {
            TimeSpan[] startTimes = { new TimeSpan(10, 0, 0) };
            int[] durations = { 30 };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 30;
            
            string[] result = Calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);
            
            CollectionAssert.AreEqual(new[] { "09:00-09:30", "09:30-10:00", "10:30-11:00", "11:00-11:30", "11:30-12:00", "12:00-12:30", "12:30-13:00", "13:00-13:30", "13:30-14:00", "14:00-14:30", "14:30-15:00", "15:00-15:30", "15:30-16:00", "16:00-16:30", "16:30-17:00" }, result);
        }
        
        [Test]
        // Тест для случая с консультациями утром.
        public void AvailablePeriods_ConsultationsInMorning()
        {
            TimeSpan[] startTimes = { new TimeSpan(9, 30, 0), new TimeSpan(10, 0, 0), new TimeSpan(11, 0, 0) };
            int[] durations = { 30, 30, 60 };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 30;
            
            string[] result = Calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);
            
            CollectionAssert.AreEqual(new[] { "09:00-09:30", "10:30-11:00", "12:00-12:30", "12:30-13:00", "13:00-13:30", "13:30-14:00", "14:00-14:30", "14:30-15:00","15:00-15:30","15:30-16:00","16:00-16:30","16:30-17:00" }, result);
        }
        
        [Test]
        // Тест для случая без рабочих часов. Ожидается отсутствие доступных периодов.
        public void AvailablePeriods_NoWorkingHours_NoAvailablePeriods()
        {
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(9, 0, 0);
            int consultationTime = 30;
            
            var result = Calculations.AvailablePeriods(new TimeSpan[0], new int[0], beginWorkingTime, endWorkingTime, consultationTime);
            
            CollectionAssert.IsEmpty(result);
        }
        
        [Test]
        // Тест для случая записи на конец рабочего дня. Ожидается отсутствие доступных периодов.
        public void AvailablePeriods_AppointmentAtTheEndOfTheDay_NoAvailablePeriods()
        {
            TimeSpan[] startTimes = { new TimeSpan(16, 30, 0) };
            TimeSpan beginWorkingTime = new TimeSpan(16, 30, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 30;
            int[] durations = { 30 };
            var result = Calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.IsEmpty(result);
        }
        
        [Test]
        // Тест для случая нескольких коротких консультаций между длинными. Ожидаются промежутки в расписании.
        public void AvailablePeriods_MultipleShortAppointmentsBetweenLongAppointments_GapsInSchedule()
        {
            TimeSpan[] startTimes = { new TimeSpan(10, 0, 0), new TimeSpan(11, 0, 0), new TimeSpan(12, 0, 0), new TimeSpan(13,0,0) };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 30;
            int[] durations = { 60, 15, 60, 30 };

            var result = Calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.AreEqual(new[] { "09:00-09:30", "09:30-10:00", "11:15-11:45", "11:45-12:15", "13:30-14:00", "14:00-14:30","14:30-15:00","15:00-15:30","15:30-16:00", "16:00-16:30","16:30-17:00" }, result);
        }
        
        [Test]
        // Тест для случая консультаций в нерегулярные интервалы. Ожидаются промежутки в расписании.
        public void AvailablePeriods_AppointmentsAtIrregularIntervals_GapsInSchedule() // Тест для случая консультаций в нерегулярные интервалы. Ожидаются промежутки в расписании.
        {
            TimeSpan[] startTimes = { new TimeSpan(10, 0, 0), new TimeSpan(12, 0, 0), new TimeSpan(13, 0, 0) };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 30;
            int[] durations = { 60, 30, 45 };

            var result = Calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.AreEqual(new[] { "09:00-09:30", "09:30-10:00", "11:00-11:30", "11:30-12:00", "12:30-13:00", "13:45-14:15","14:15-14:45", "14:45-15:15", "15:15-15:45","15:45-16:15", "16:15-16:45" }, result);
        }
        
        
        [Test]
        // Тест для случая консультации превышают рабочие часы. Ожидается отсутствие доступных периодов.
        public void AvailablePeriods_ConsultationTimeGreaterThanWorkingHours_NoAvailablePeriods()
        {
            TimeSpan[] startTimes = { new TimeSpan(9, 0, 0) };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 520;
            int[] durations = { 30 };
            var result = Calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.IsEmpty(result);
        }

        [Test]
        // Тест для случая, когда время консультации не равно времени начала консультации. Ожидается отсутствие доступных периодов.
        public void AvailablePeriods_СonsultationsIsNotEqualStartTime()
        {
            TimeSpan[] startTimes = { new TimeSpan(9, 0, 0) };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 30;
            int[] durations = { 30,30 };
            var result = Calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.IsEmpty(result);
        }
        
        [Test]
        // Тест для случая, когда время консультации превышает рабочие часы. Ожидается отсутствие доступных периодов.
        public void AvailablePeriods_ConsultationExceedsWorkingHours()
        {
            TimeSpan[] startTimes = { new TimeSpan(9, 0, 0) };
            TimeSpan beginWorkingTime = new TimeSpan(9, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(17, 0, 0);
            int consultationTime = 33;
            int[] durations = { 999 };
            var result = Calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            CollectionAssert.IsEmpty(result);
        }
    }
}