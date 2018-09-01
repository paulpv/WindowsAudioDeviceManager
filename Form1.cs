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

/// <summary>
/// From Sang:
/// "You will use the CM_Get_Device_Interface_List API to obtain the list of device under a given interface class.
/// Walk that list to check for the DEVPKEY_DeviceInterface_Enabled (CM_Get_Device_Interface_Property),
/// if enabled you can attempt to disable it (CM_Set_Device_Interface_Property).
/// Disabling the interface in this manner will disable the “endpoint”, which will make user mode enumeration operations to skip the device."
/// https://www.pinvoke.net/default.aspx/cfgmgr32.CM_Get_Device_Interface_List
/// </summary>

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
            var listDeviceClasses = new List<DeviceClassWrapper>();
            listDeviceClasses.Add(new DeviceClassWrapper(CfgMgr32.KSCATEGORY_AUDIO, "KSCATEGORY_AUDIO"));
            listDeviceClasses.Add(new DeviceClassWrapper(CfgMgr32.KSCATEGORY_CAPTURE, "KSCATEGORY_CAPTURE"));
            listDeviceClasses.Add(new DeviceClassWrapper(CfgMgr32.KSCATEGORY_RENDER, "KSCATEGORY_RENDER"));
            comboBoxDeviceClass.BeginUpdate();
            comboBoxDeviceClass.Items.AddRange(listDeviceClasses.ToArray());
            comboBoxDeviceClass.SelectedIndex = 0;
            comboBoxDeviceClass.EndUpdate();
        }

        private void comboBoxDeviceClass_SelectedValueChanged(object sender, EventArgs e)
        {
            buttonRefresh.PerformClick();
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
            var deviceClass = comboBoxDeviceClass.SelectedItem as DeviceClassWrapper;
            var devicesInterfaces = CfgMgr32.GetDeviceInterfaces(deviceClass.deviceClass);

            textBox1.Clear();
            PrintDevices("devicesInterfaces", devicesInterfaces);

            comboBox1.BeginUpdate();
            comboBox1.Items.Clear();
            foreach (var deviceInterface in devicesInterfaces)
            {
                var name = CfgMgr32.GetDeviceInterfacePropertyFriendlyName(deviceInterface);
                var wrapper = new DeviceInterfaceWrapper(deviceInterface, name);
                comboBox1.Items.Add(wrapper);
                var enabled = CfgMgr32.GetDeviceInterfacePropertyEnabled(deviceInterface);
                if (false && name == "VoiceMeeter vaio")
                {
                    CfgMgr32.SetDeviceInterfacePropertyEnabled(deviceInterface, !enabled);
                }
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
            var selectedItem = comboBox1.SelectedItem as DeviceInterfaceWrapper;
            var enabled = CfgMgr32.GetDeviceInterfacePropertyEnabled(selectedItem.id);
            CfgMgr32.SetDeviceInterfacePropertyEnabled(selectedItem.id, !enabled);
        }
    }
}
