using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;


namespace Continuum_Windows_Testing_Agent
{
    public partial class Main : Form
    {
        private CTM_Agent et;

        public Main()
        {
            InitializeComponent();
            this.et = new CTM_Agent();
        }

        private Boolean _setHostname(String host)
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\CTM");

            key.SetValue("hostname", host);

            return true;

        }

        private Boolean _setMachineName(String machineName)
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\CTM");

            key.SetValue("machineName", machineName);

            return true;

        }

        private Boolean _setHostIp(String hostIp)
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\CTM");

            key.SetValue("host_ip", hostIp);

            return true;

        }


        private String _getHostname()
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\CTM");

            if (key.GetValue("hostname") != null)
            {
                return key.GetValue("hostname").ToString();
            }

            return "";

        }

        private String _getMachineName()
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\CTM");
            if (key.GetValue("machineName") != null)
            {
                return key.GetValue("machineName").ToString();
            }

            String machineName = Environment.MachineName;

            if (machineName.Length > 0)
            {
                key.SetValue("machineName", machineName);
            }

            key.Close();
            return machineName;
        
        }

        private String _getGuid()
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\CTM");
            if (key.GetValue("guid") != null)
            {
                return key.GetValue("guid").ToString();
            }

            String guid = System.Guid.NewGuid().ToString();

            if (guid.Length > 0)
            {
                key.SetValue("guid", guid);
            }

            key.Close();
            return guid;
        }

        private String _getIp()
        {
            // try to load this from the registry.
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\CTM");

            if (key.GetValue("host_ip") != null)
            {
                return key.GetValue("host_ip").ToString();
            }

            String ip = "";

            IPAddress[] localIps = Dns.GetHostAddresses(Dns.GetHostName());

            foreach (IPAddress hostIp in localIps)
            {
                if (!IPAddress.IsLoopback(hostIp))
                {
                    ip = hostIp.ToString();
                }
            }

            if (ip.Length > 0)
            {
                key.SetValue("host_ip", ip);
            }

            return ip;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            this.ipBox.Text = this._getIp();
            this.hostnameBox.Text = this._getHostname();
            this.machineNameBox.Text = this._getMachineName();
            this.ieVersionBox.Text = et.ie.getVersion();
            this.chromeVersionBox.Text = et.googlechrome.getVersion();
            this.firefoxVersionBox.Text = et.firefox.getVersion();
            this.safariVersionBox.Text = et.safari.getVersion();
            this.osVersionBox.Text = et.determineWindowsVersion();
            this.guidBox.Text = this._getGuid();
            this.buildBox.Text = this._getBuild();
            
            
            
            // hook up the onLog event handler.
            // this.et.log.logMessage += new CTM_Agent_Log_Message_Handler(agentLogHandler);
        }

        private String _getBuild()
        {
            System.Version v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            
            return v.ToString();

        }

        /*
        public void agentLogHandler(object sender, CTM_Agent_Log_Message_Handler_Args e)
        {
            this.lastRunLogBox.Text += e.message + "\r\n";
        }
        */

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void activePolling()
        {
            if (this.et.ctmBgWorker.IsBusy == true)
            {
                // Quiet down while we're working on the other threads.
                this.ctmStatusLabel.Text = "Running tests..";
                this.callHomeTimer.Interval = 60 * 5 * 1000;
                return;
            }
            else
            {
                // We get noisy when we're not loaded.
                this.callHomeTimer.Interval = 30000;
            }

            // make sure we have a guid.
            this.et.setGuid(this._getGuid());

            this.et.setCTMHostname(this.hostnameBox.Text);
            this.et.setLocalIp(this.ipBox.Text);
            this.et.setMachineName(this.machineNameBox.Text);
            this.et.setUseVerboseTestLogs(this.useVerboseTestLogsCheckbox.Checked);
            this.et.run();

            if (this.et.getIsRegistered() == true)
            {
                DateTime now = DateTime.Now;
                this.ctmStatusLabel.Text = "Last check in: " + String.Format("{0:r}", now);
            }
            else
            {
                this.ctmStatusLabel.Text = "Phoning home now";
            }

            this.lastRunLogBox.Text = this.et.log.getLastLogLines();

        }

        private void configSaveSettingsBtn_Click(object sender, EventArgs e)
        {

            this._setHostname(this.hostnameBox.Text);
            this._setHostIp(this.ipBox.Text);
            this._setMachineName(this.machineNameBox.Text);
            this.activePolling();

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.activePolling();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void logRefresh_Tick(object sender, EventArgs e)
        {
            
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void regenerateGuidBtn_Click(object sender, EventArgs e)
        {
           Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\CTM");
           if (key.GetValue("guid") != null)
           {
               key.DeleteValue("guid");
           }
           this.guidBox.Text = this._getGuid();
        }

        private void forcePollBtn_Click(object sender, EventArgs e)
        {
            this.activePolling();
        }

        private void ctmLogTimer_Tick(object sender, EventArgs e)
        {
           
        }

        private void ctmStatusBar_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
    }
}
