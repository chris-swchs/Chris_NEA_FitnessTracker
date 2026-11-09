using NEA.Models.Entities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NEA.Services
{
    public static class ApplicationClock
    {
        private static DatabaseManager db;
        private static readonly Random random = new Random();

        public static DateTime SimulatedDate = DateTime.Now;

        public static void AdvanceDays(int days)
        {
            SimulatedDate = SimulatedDate + TimeSpan.FromDays(days);
        }

        public static void SetSimulatedDate(DateTime date)
        {
            SimulatedDate = date.Date;
        }

        public static DateTime GetCurrentDate()
        {
            return SimulatedDate;
        }

        public static void ResetDate()
        {
            SimulatedDate = DateTime.Now;
        }
    }
}