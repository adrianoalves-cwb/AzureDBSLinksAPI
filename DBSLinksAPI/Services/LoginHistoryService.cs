using System;
using DBSLinksAPI.DBContext;
using DBSLinksAPI.Models;
using System.Net.Sockets;
using System.Net;

namespace DBSLinksAPI.Services
{
    public static class LoginHistoryService
    {
        public static void WriteToLoginHistory(Login model, string loginStatus, string logDescription, ApplicationDbContext _db)
        {
            LoginHistory loginHistory = new LoginHistory();

            loginHistory.UserName = model.UserName;
            loginHistory.ComputerName = model.ComputerName;
            loginHistory.LoginDateTimeUTC = System.DateTime.UtcNow;
            loginHistory.IPAddress = GetClientIPAddress();
            loginHistory.LoginStatus = loginStatus;
            loginHistory.UserAppVersion = model.UserAppVersion;
            loginHistory.LogDescription = logDescription;

            _db.LoginHistories.Add(loginHistory);
            _db.SaveChanges();
        }

        private static string GetClientIPAddress()
        {
            string IpAddress = "";

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    IpAddress = ip.ToString();
                }
            }
            return IpAddress;
        }
    }
}
