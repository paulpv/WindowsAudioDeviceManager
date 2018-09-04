using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace AudioEndpointManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        class DeviceClassWrapper
        {
            public Guid deviceClass { get; private set; }
            string name;

            public DeviceClassWrapper(Guid deviceClass, string name)
            {
                this.deviceClass = deviceClass;
                this.name = name;
            }

            public override string ToString()
            {
                return String.Format("{0}: {1}", name, deviceClass);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.IsMaximized)
            {
                WindowState = FormWindowState.Maximized;
            }
            else if (Screen.AllScreens.Any(screen => screen.WorkingArea.IntersectsWith(Properties.Settings.Default.WindowPosition)))
            {
                StartPosition = FormStartPosition.Manual;
                DesktopBounds = Properties.Settings.Default.WindowPosition;
                WindowState = FormWindowState.Normal;
            }

            var listDeviceClasses = new List<DeviceClassWrapper>();
            listDeviceClasses.Add(new DeviceClassWrapper(CfgMgr32.KSCATEGORY_CAPTURE, "KSCATEGORY_CAPTURE"));
            listDeviceClasses.Add(new DeviceClassWrapper(CfgMgr32.KSCATEGORY_RENDER, "KSCATEGORY_RENDER"));
            comboBoxDeviceClass.BeginUpdate();
            comboBoxDeviceClass.Items.AddRange(listDeviceClasses.ToArray());
            comboBoxDeviceClass.SelectedIndex = 0;
            comboBoxDeviceClass.EndUpdate();

            buttonRefresh.PerformClick();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.IsMaximized = WindowState == FormWindowState.Maximized;
            Properties.Settings.Default.WindowPosition = DesktopBounds;
            Properties.Settings.Default.Save();
        }

        class DeviceInterfaceWrapper
        {
            public string id { get; private set; }
            string name;

            public DeviceInterfaceWrapper(string id, string name)
            {
                this.id = id;
                this.name = name == null ? "" : name;
            }

            public override string ToString()
            {
                return String.Format("(id:\"{0}\", name:\"{1}\")", id, name);
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            var devicesInterfaces = CfgMgr32.GetDeviceInterfaces(CfgMgr32.KSCATEGORY_AUDIO);

            textBox1.Clear();
            PrintDevices("KSCATEGORY_AUDIO", devicesInterfaces);

            comboBox1.BeginUpdate();
            comboBox1.Items.Clear();
            foreach (var deviceInterface in devicesInterfaces)
            {
                var name = CfgMgr32.GetDeviceInterfacePropertyFriendlyName(deviceInterface);
                var wrapper = new DeviceInterfaceWrapper(deviceInterface, name);
                comboBox1.Items.Add(wrapper);
            }
            comboBox1.SelectedIndex = 0;
            comboBox1.EndUpdate();
        }

        private void PrintDevices(string title, string[] deviceInterfaces)
        {
            var text = String.Format("PrintDevices: \"{0}\"", title);
            textBox1.AppendText(text + "\r\n");
            Debug.WriteLine(text);
            foreach (string deviceInterface in deviceInterfaces)
            {
                var name = CfgMgr32.GetDeviceInterfacePropertyFriendlyName(deviceInterface);
                var enabled = CfgMgr32.GetDeviceInterfacePropertyEnabled(deviceInterface);
                text = String.Format("PrintDevices: (id:\"{0}\", name:\"{1}\", enabled:{2})", deviceInterface, name, enabled);
                textBox1.AppendText(text + "\r\n");
                Debug.WriteLine(text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var selectedDevice = comboBox1.SelectedItem as DeviceInterfaceWrapper;
            var deviceId = selectedDevice.id;

            var aliasClass = comboBoxDeviceClass.SelectedItem as DeviceClassWrapper;
            var alias = CfgMgr32.GetDeviceInterfaceAlias(deviceId, aliasClass.deviceClass);
            var enabled = CfgMgr32.GetDeviceInterfacePropertyEnabled(alias);
            if (enabled.HasValue)
            {
                CfgMgr32.SetDeviceInterfacePropertyEnabled(alias, !enabled.Value);
            }
        }
    }
}
