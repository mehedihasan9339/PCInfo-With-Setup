using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Management;
using System.Drawing.Imaging;
using System.Threading;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PCInfo
{
    public partial class Form1 : Form
    {
        //IFirebaseClient client;
        //IFirebaseConfig config = new FirebaseConfig
        //{
        //    AuthSecret = "m5XGdevDrbLO33eKDipLUVgMJpKeZjEpEZK8XSbU",
        //    BasePath = "https://pcinfodb1.firebaseio.com/"
        //};
        public Form1()
        {
            InitializeComponent();
            textBox34.Visible = false;
            textBox36.Visible = false;
            textBox37.Visible = false;
            textBox43.Visible = false;
            textBox15.Visible = false;
            textBox35.Visible = false;
            textBox38.Visible = false;
            textBox44.Visible = false;
        }
        string driveDetails = "";
        object phy = null;
        IDictionary<string, object> operatingsystem = new Dictionary<string, object>();

        private void Form1_Load(object sender, EventArgs e)
        {
            string drives = "";
            string tsize = "";
            string fsize = "";
            string drivetype = "";
            string driveformat = "";
            double tsizecount = 0;
            double fsizecount = 0;
            int totalram = 0;
            string country = "";
            string localIpAddress = "";
            string macAddress = "";
            string wifiName = "";
            string wifiStrength = "";


            //CPU Information
            IDictionary<string, object> cpudic = new Dictionary<string, object>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_Processor");
            foreach (ManagementObject item in searcher.Get())
            {
                foreach (PropertyData PC in item.Properties)
                {
                    if (!cpudic.TryGetValue(PC.Name, out phy))
                    {
                        cpudic.Add(PC.Name, PC.Value);
                    }
                    else
                    {
                        cpudic.Add(PC.Name + "1", PC.Value);
                    }
                }
            }

            //Physical Memory Information
            IDictionary<string, object> physicalmemory = new Dictionary<string, object>();
            ManagementObjectSearcher searcher1 = new ManagementObjectSearcher("select * from Win32_PhysicalMemory");
            foreach (ManagementObject item in searcher1.Get())
            {
                foreach (PropertyData PC in item.Properties)
                {
                    if (!physicalmemory.TryGetValue(PC.Name, out phy))
                    {
                        physicalmemory.Add(PC.Name, PC.Value);
                    }
                    else
                    {
                        if (!physicalmemory.TryGetValue(PC.Name + "1", out phy))
                        {
                            physicalmemory.Add(PC.Name + "1", PC.Value);
                        }
                    }
                }
            }

            //Operating System Information
            IDictionary<string, object> operatingsystem = new Dictionary<string, object>();
            ManagementObjectSearcher searcher2 = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            foreach (ManagementObject item in searcher2.Get())
            {
                foreach (PropertyData PC in item.Properties)
                {
                    if (!operatingsystem.TryGetValue(PC.Name, out phy))
                    {
                        operatingsystem.Add(PC.Name, PC.Value);
                    }
                    else
                    {
                        if (!operatingsystem.TryGetValue(PC.Name + "1", out phy))
                        {
                            operatingsystem.Add(PC.Name + "1", PC.Value);
                        }
                    }
                }
            }

            //Network Adapter Information
            IDictionary<string, object> network = new Dictionary<string, object>();
            ManagementObjectSearcher searcher3 = new ManagementObjectSearcher("select * from Win32_NetworkAdapter");
            foreach (ManagementObject item in searcher3.Get())
            {
                foreach (PropertyData PC in item.Properties)
                {
                    if (!network.TryGetValue(PC.Name, out phy))
                    {
                        if (PC.Value != null)
                        {
                            network.Add(PC.Name, PC.Value);
                        }
                    }
                    else
                    {
                        if (!network.TryGetValue(PC.Name + "1", out phy))
                        {
                            network.Add(PC.Name + "1", PC.Value);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            //MAC Address
            if (network.TryGetValue("MACAddress", out phy))
            {
                macAddress = network["MACAddress"].ToString();
            }
            else
            {
                macAddress = "Cannot get the value";
            }


            //Process p = new Process();
            //p.StartInfo.FileName = "cmd.exe";
            //p.StartInfo.CreateNoWindow = true;
            //p.StartInfo.RedirectStandardInput = true;
            //p.StartInfo.RedirectStandardOutput = true;
            //p.StartInfo.UseShellExecute = false;
            //p.Start();
            //p.StandardInput.WriteLine("systeminfo");
            //p.StandardInput.Flush();
            //p.StandardInput.Close();
            //p.WaitForExit();


            //Machine Name
            textBox1.Text = Environment.MachineName;
            //User Name
            textBox2.Text = Environment.UserName;
            //Operating System Bit
            if (Environment.Is64BitOperatingSystem)
            {
                textBox3.Text = "64 Bit OS";
            }
            else
            {
                textBox3.Text = "32 Bit OS";
            }

            if (SystemInformation.PowerStatus.PowerLineStatus.ToString() == "Online" || SystemInformation.PowerStatus.PowerLineStatus.ToString() == "Offline")
            {
                textBox34.Visible = true;
                textBox36.Visible = true;
                textBox37.Visible = true;
                textBox43.Visible = true;
                textBox15.Visible = true;
                textBox35.Visible = true;
                textBox38.Visible = true;
                textBox44.Visible = true;


                //Charge Status
                textBox34.Text = SystemInformation.PowerStatus.BatteryChargeStatus.ToString();
                //var power = SystemInformation.PowerStatus.BatteryFullLifetime.ToString(); //-1

                //Remaining Battery
                textBox36.Text = (SystemInformation.PowerStatus.BatteryLifePercent * 100).ToString() + "%";

                //Power Status and Battery Remaining
                var powerStatus = SystemInformation.PowerStatus.PowerLineStatus.ToString();
                if (powerStatus == "Online")
                {
                    textBox37.Text = "Plugged In";
                    textBox43.Text = "Battery is charging";
                }
                else
                {
                    textBox37.Text = "Not Plugged In";
                    textBox43.Text = (Convert.ToInt32(SystemInformation.PowerStatus.BatteryLifeRemaining) / 3600).ToString() + " hour " + ((Convert.ToInt32(SystemInformation.PowerStatus.BatteryLifeRemaining) % 3600) / 60).ToString() + " minute";
                }
            }

            //Screen Size in px
            textBox41.Text = Screen.PrimaryScreen.Bounds.Width.ToString() + " x " + Screen.PrimaryScreen.Bounds.Height.ToString();

            //Screen Size in inch
            textBox39.Text = (Convert.ToDouble(Screen.PrimaryScreen.Bounds.Width) / 96).ToString("#.#") + " x " + (Convert.ToDouble(Screen.PrimaryScreen.Bounds.Height) / 96).ToString("#.#"); //14.2 x 8

            //Screen Type
            if (Screen.PrimaryScreen.Primary)
            {
                textBox45.Text = "Primary";
            }
            else
            {
                textBox45.Text = "Secondary";
            }

            //Country Name
            TimeZone localZone = TimeZone.CurrentTimeZone;
            var result = localZone.StandardName;
            var s = result.Split(' ');
            country = s[0];

            //Local IP Address
            localIpAddress = GetLocalIpAddress();

            //Disabled for CMD
            //--------------------------------------------------
            //wifiName = showConnectedId(s).Item1; //may be ""
            //wifiStrength = showConnectedId(s).Item2; //may be ""
            //--------------------------------------------------


            label21.Text = country + localIpAddress + macAddress;


            //Machine Start Time
            textBox4.Text = Environment.TickCount / 1000 / 60 + " minute or " + (Convert.ToDouble(Environment.TickCount) / 1000 / 60 / 60).ToString("#.#") + " hour";

            textBox6.Text = "Drive Name";
            textBox7.Text = "Total Space";
            textBox8.Text = "Free Space";
            textBox30.Text = "Drive Type";
            textBox33.Text = "Drive Format";

            //Get Drive Details
            foreach (var item in DriveInfo.GetDrives())
            {
                if (item.IsReady)
                {
                    drives += item + Environment.NewLine;
                    tsize += item.TotalSize / 1024 / 1024 / 1024 + " GB" + Environment.NewLine;
                    fsize += item.TotalFreeSpace / 1024 / 1024 / 1024 + " GB" + Environment.NewLine;
                    drivetype += item.DriveType + Environment.NewLine;
                    driveformat += item.DriveFormat + Environment.NewLine;
                    tsizecount += item.TotalSize / 1024 / 1024 / 1024;
                    fsizecount += item.TotalFreeSpace / 1024 / 1024 / 1024;

                    driveDetails += item + "\t\t" +
                                   item.TotalSize / 1024 / 1024 / 1024 + " GB\t\t" +
                                   item.TotalFreeSpace / 1024 / 1024 / 1024 + " GB\t\t" +
                                   item.DriveType + "\t\t" +
                                   item.DriveFormat + "\n";
                }
            }

            //Drive names
            textBox5.Text = drives;
            //Drive available sizes
            textBox9.Text = tsize;
            //Drive free size
            textBox10.Text = fsize;
            //Drive type
            textBox29.Text = drivetype;
            //Drive format
            textBox32.Text = driveformat;
            textBox12.Text = "Total";
            //Drive total size
            textBox13.Text = tsizecount.ToString() + " GB";
            //Drive total free size
            textBox14.Text = fsizecount.ToString() + " GB";
            //Product name
            textBox16.Text = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\", "ProductName", "").ToString();
            //OS Version
            textBox11.Text = Environment.OSVersion.VersionString;
            //System product name
            textBox17.Text = Registry.GetValue(@"HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\SYSTEM\BIOS\", "SystemProductName", "").ToString();
            //BIOS version
            textBox18.Text = Registry.GetValue(@"HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\SYSTEM\BIOS\", "BIOSVersion", "").ToString();
            //Release date
            textBox19.Text = Registry.GetValue(@"HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\SYSTEM\BIOS\", "BIOSReleaseDate", "").ToString();
            //System BIOS date
            if (operatingsystem.TryGetValue("InstallDate", out phy))
            {
                textBox20.Text = operatingsystem["InstallDate"].ToString().Substring(6, 2) + "/" +
                                 operatingsystem["InstallDate"].ToString().Substring(4, 2) + "/" +
                                 operatingsystem["InstallDate"].ToString().Substring(0, 4) + "   " +
                                 operatingsystem["InstallDate"].ToString().Substring(8, 2) + ":" +
                                 operatingsystem["InstallDate"].ToString().Substring(10, 2) + ":" +
                                 operatingsystem["InstallDate"].ToString().Substring(12, 2);
            }
            else
            {
                textBox20.Text = "Cannot get the value";
            }


            //textBox20.Text = Registry.GetValue(@"HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\SYSTEM\", "SystemBiosDate", "").ToString();
            //CPU name
            textBox21.Text = cpudic["Name"].ToString();

            //RAM 1
            if (physicalmemory.TryGetValue("Capacity", out phy))
            {
                textBox22.Text = "Capacity: " + (Convert.ToDouble(physicalmemory["Capacity"]) / 1000000000).ToString("#") + " GB" + Environment.NewLine +
                            "Clock Speed: " + physicalmemory["ConfiguredClockSpeed"].ToString() + " MHz" + Environment.NewLine +
                            "Device Location: " + physicalmemory["DeviceLocator"].ToString() + Environment.NewLine +
                            "Manufacturer: " + physicalmemory["Manufacturer"].ToString() + Environment.NewLine;
                totalram += Convert.ToInt32(Convert.ToDouble(physicalmemory["Capacity"]) / 1000000000);

            }
            else
            {
                textBox22.Text = "Cannot get the value";
            }

            //RAM 2
            if (physicalmemory.TryGetValue("Capacity1", out phy))
            {
                textBox23.Text = "Capacity: " + (Convert.ToDouble(physicalmemory["Capacity1"]) / 1000000000).ToString("#") + " GB" + Environment.NewLine +
                            "Clock Speed: " + physicalmemory["ConfiguredClockSpeed1"].ToString() + " MHz" + Environment.NewLine +
                            "Device Location: " + physicalmemory["DeviceLocator1"].ToString() + Environment.NewLine +
                            "Manufacturer: " + physicalmemory["Manufacturer1"].ToString() + Environment.NewLine;
                totalram += Convert.ToInt32(Convert.ToDouble(physicalmemory["Capacity"]) / 1000000000);
            }
            else
            {
                textBox23.Text = "Cannot get the value";
            }

            //Total RAM
            textBox24.Text = totalram.ToString() + " GB";

            //Serial number
            if (operatingsystem.TryGetValue("SerialNumber", out phy))
            {
                textBox25.Text = operatingsystem["SerialNumber"].ToString();
            }
            else
            {
                textBox25.Text = "Cannot get the value";
            }

            //Free physical memory
            if (operatingsystem.TryGetValue("FreePhysicalMemory", out phy))
            {
                textBox26.Text = (Convert.ToDouble(operatingsystem["FreePhysicalMemory"]) / 1024 / 1024).ToString("#.##") + " GB";
            }
            else
            {
                textBox26.Text = "Cannot get the value";
            }

            //Last boot time
            if (operatingsystem.TryGetValue("LastBootUpTime", out phy))
            {
                textBox27.Text = operatingsystem["LastBootUpTime"].ToString().Substring(6, 2) + "/" +
                                 operatingsystem["LastBootUpTime"].ToString().Substring(4, 2) + "/" +
                                 operatingsystem["LastBootUpTime"].ToString().Substring(0, 4) + "   " +
                                 operatingsystem["LastBootUpTime"].ToString().Substring(8, 2) + ":" +
                                 operatingsystem["LastBootUpTime"].ToString().Substring(10, 2) + ":" +
                                 operatingsystem["LastBootUpTime"].ToString().Substring(12, 2);
            }
            else
            {
                textBox27.Text = "Cannot get the value";
            }

            // Save to Database
            //client = new FirebaseClient(config);
            //var data = new UserInfoTbl
            //{
            //    MachineName = country,
            //    UserName = textBox2.Text,
            //    MACAddress = macAddress,
            //    IpAddress = localIpAddress,
            //    Country = country,
            //    ProductName = textBox17.Text,
            //    OSBit = textBox3.Text,
            //    TotalDriveSize = textBox13.Text,
            //    FreeDriveSpace = textBox14.Text,
            //    OSName = textBox16.Text,
            //    OSVersion = textBox11.Text,
            //    SerialNumber = textBox25.Text,
            //    StartDate = textBox27.Text,
            //    BIOSVersion = textBox18.Text,
            //    BIOSRelease = textBox19.Text,
            //    CPUInfo = textBox21.Text,
            //    TotalRAM = textBox24.Text,
            //    WifiName = wifiName,
            //    WifiStrength = wifiStrength,
            //    Note = DateTime.Now.ToString()
            //};

            //if (client != null)
            //{
            //    //Save to Database
            //    try
            //    {
            //        PushResponse response = client.Push("UserInfoTbl/", data);
            //        data.Id = response.Result.name;
            //        SetResponse setResponse = client.Set("UserInfoTbl/" + data.Id, data);



            //        //Visitor
            //        FirebaseResponse response1 = client.Get("UserInfoTbl");
            //        var data1 = JsonConvert.DeserializeObject<dynamic>(response1.Body);
            //        var list1 = new List<UserInfoTbl>();
            //        foreach (var item in data1)
            //        {
            //            list1.Add(JsonConvert.DeserializeObject<UserInfoTbl>(((JProperty)item).Value.ToString()));
            //        }
            //        label22.Text = "Visited: " + (from v in list1
            //                                      select v.MACAddress).Distinct().Count().ToString();
            //    }
            //    catch (Exception)
            //    {
            //        label22.Text = "";
            //    }
            //}

            

        }

        private static Tuple<string, string> showConnectedId(string[] s)
        {
            string s1 = "", s2 = "";
            Process p = new Process();
            p.StartInfo.FileName = "netsh.exe";
            p.StartInfo.Arguments = "wlan show interfaces";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.Start();
            string so = p.StandardOutput.ReadToEnd();
            p.Close();

            if (so.IndexOf("SSID") >= 0)
            {
                s1 = so.Substring(so.IndexOf("SSID"));
                s1 = s1.Substring(s1.IndexOf(":"));
                s1 = s1.Substring(2, s1.IndexOf("\n")).Trim();

                s2 = so.Substring(so.IndexOf("Signal"));
                s2 = s2.Substring(s2.IndexOf(":"));
                s2 = s2.Substring(2, s2.IndexOf("\n")).Trim();
            }
            return Tuple.Create(s1, s2);
            //p.WaitForExit();
        }

        private static string GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://facebook.com/mehedi9340");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = @"C:\Users\" + Environment.UserName + @"\Desktop";
            //if (!File.Exists(path + @"\PCInfo.txt"))
            //{
            //    File.Create(path + @"\PCInfo.txt");
            //}
            //Thread.Sleep(1000);
            TextWriter txt = new StreamWriter(path + @"\PCInfo.txt");
            txt.WriteLine("Hello " + textBox2.Text);
            txt.WriteLine("Here is your PC information.");
            txt.WriteLine("-----------------------------");
            txt.WriteLine("Machine name: " + textBox1.Text);
            txt.WriteLine("User Name: " + textBox2.Text);
            txt.WriteLine("OS Bit: " + textBox3.Text);
            txt.WriteLine("Run Time: " + textBox4.Text);
            txt.WriteLine();
            txt.WriteLine("Drive Info: ");
            txt.WriteLine("Name\t\tSize\t\tFree\t\tType\t\tFormat");
            txt.WriteLine("----\t\t----\t\t----\t\t----\t\t------");
            txt.WriteLine(driveDetails);
            txt.WriteLine("OS Name: " + textBox16.Text);
            txt.WriteLine("OS Version: " + textBox11.Text);
            txt.WriteLine("Serial Number: " + textBox25.Text);
            txt.WriteLine("PC Start Time: " + textBox27.Text);
            txt.WriteLine("Product Name: " + textBox17.Text);
            txt.WriteLine("BIOS Version: " + textBox18.Text);
            txt.WriteLine("BIOS Release: " + textBox19.Text);
            txt.WriteLine("Last Boot Date: " + textBox20.Text);
            txt.WriteLine("CPU Info: " + textBox21.Text);
            txt.WriteLine("Total RAM: " + textBox24.Text);
            txt.WriteLine();
            txt.WriteLine("RAM 1: ");
            txt.WriteLine(textBox22.Text);
            txt.WriteLine("RAM 2: ");
            txt.WriteLine(textBox23.Text);
            txt.WriteLine("Free RAM: " + textBox26.Text);
            txt.WriteLine();
            txt.WriteLine();
            txt.WriteLine("Display Information");
            txt.WriteLine("--------------------");
            txt.WriteLine("Screen Size (in px): " + textBox41.Text);
            txt.WriteLine("Screen Size (in inch): " + textBox39.Text);
            txt.WriteLine("Screen Type: " + textBox45.Text);
            txt.WriteLine();
            txt.WriteLine();

            if (SystemInformation.PowerStatus.PowerLineStatus.ToString() == "Online" || SystemInformation.PowerStatus.PowerLineStatus.ToString() == "Offline")
            {
                txt.WriteLine("Battery Information");
                txt.WriteLine("--------------------");
                txt.WriteLine("Charge Status: " + textBox34.Text);
                txt.WriteLine("Remaining Charge: " + textBox36.Text);
                txt.WriteLine("Charger Plugged: " + textBox37.Text);
                txt.WriteLine("Battery Life Remain: " + textBox43.Text);
                txt.WriteLine();
                txt.WriteLine();
            };
            
            txt.WriteLine("------------------------------");
            txt.WriteLine("Regards,");
            txt.WriteLine("Mehedi Hasan");
            txt.WriteLine("mehedihasan9339@gmail.com");
            txt.WriteLine("facebook.com/mehedi9340");
            txt.WriteLine("linkedin.com/in/mehedi9339");
            txt.WriteLine("------------------------------");
            txt.Close();

            MessageBox.Show("The information saved successfully. Please find it from your Desktop. File name 'PCInfo.txt'", "Success", MessageBoxButtons.OK);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Rectangle bounds = this.Bounds;
            using (Bitmap bitmap = new Bitmap(bounds.Width - 20, bounds.Height - 10))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(new Point(bounds.Left + 10, bounds.Top), Point.Empty, bounds.Size);
                }
                bitmap.Save("C:\\Users\\" + Environment.UserName + "\\Desktop\\PCInfo.png", ImageFormat.Png);
                MessageBox.Show("The information saved successfully. Please find it from your Desktop. File name 'PCInfo.png'", "Success", MessageBoxButtons.OK);
            }
        }

        private void chargerPlugged_Tick(object sender, EventArgs e)
        {
            //Power Status and Battery Remaining
            var powerStatus = SystemInformation.PowerStatus.PowerLineStatus.ToString();
            if (powerStatus == "Online")
            {
                textBox37.Text = "Plugged In";
                textBox43.Text = "Battery is charging";
            }
            else
            {
                textBox37.Text = "Not Plugged In";
                textBox43.Text = (Convert.ToInt32(SystemInformation.PowerStatus.BatteryLifeRemaining) / 3600).ToString() + " hour " + ((Convert.ToInt32(SystemInformation.PowerStatus.BatteryLifeRemaining) % 3600) / 60).ToString() + " minute";
            }

            //Charge Status
            textBox34.Text = SystemInformation.PowerStatus.BatteryChargeStatus.ToString();

            //Remaining Battery
            textBox36.Text = (SystemInformation.PowerStatus.BatteryLifePercent * 100).ToString() + "%";

            //Screen Type
            if (Screen.PrimaryScreen.Primary)
            {
                textBox45.Text = "Primary";
            }
            else
            {
                textBox45.Text = "Secondary";
            }

            
        }

        private void freeRAM_Tick(object sender, EventArgs e)
        {
            ManagementObjectSearcher searcher2 = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            operatingsystem.Clear();
            foreach (ManagementObject item in searcher2.Get())
            {
                foreach (PropertyData PC in item.Properties)
                {
                    if (!operatingsystem.TryGetValue(PC.Name, out phy))
                    {
                        operatingsystem.Add(PC.Name, PC.Value);
                    }
                    else
                    {
                        if (!operatingsystem.TryGetValue(PC.Name + "1", out phy))
                        {
                            operatingsystem.Add(PC.Name + "1", PC.Value);
                        }
                    }
                }
            }

            //Free physical memory
            if (operatingsystem.TryGetValue("FreePhysicalMemory", out phy))
            {
                textBox26.Text = (Convert.ToDouble(operatingsystem["FreePhysicalMemory"]) / 1024 / 1024).ToString("#.##") + " GB";
            }
            else
            {
                textBox26.Text = "Cannot get the value";
            }
        }
    }
}
