using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCInfo
{
    class UserInfoTbl
    {
        public string Id { get; set; }
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public string OSBit { get; set; }
        public string TotalDriveSize { get; set; }
        public string FreeDriveSpace { get; set; }
        public string OSName { get; set; }
        public string OSVersion { get; set; }
        public string SerialNumber { get; set; }
        public string StartDate { get; set; }
        public string ProductName { get; set; }
        public string BIOSVersion { get; set; }
        public string BIOSRelease { get; set; }
        public string CPUInfo { get; set; }
        public string TotalRAM { get; set; }
        public string Country { get; set; }
        public string MACAddress { get; set; }
        public string IpAddress { get; set; }
        public string WifiName { get; set; }
        public string WifiStrength { get; set; }
        public string Note { get; set; }
    }
}
