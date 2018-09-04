using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// https://docs.microsoft.com/en-us/windows/desktop/api/cfgmgr32/
/// </summary>
namespace AudioEndpointManager
{
    class CfgMgr32
    {
        /// <summary>
        /// From ksmedia.h per https://www.magnumdb.com/search?q=value%3A%22c166523c-fe0c-4a94-a586-f1a80cfbbf3e%22
        /// </summary>
        public static readonly Guid AUDIOENDPOINT_CLASS_UUID = new Guid("C166523C-FE0C-4A94-A586-F1A80CFBBF3E");
        public static readonly Guid KSCATEGORY_AUDIO = new Guid("6994AD04-93EF-11D0-A3CC-00A0C9223196");
        public static readonly Guid KSCATEGORY_CAPTURE = new Guid("65E8773D-8F56-11D0-A3B9-00A0C9223196");
        public static readonly Guid KSCATEGORY_RENDER = new Guid("65E8773E-8F56-11D0-A3B9-00A0C9223196");

        private const string CFGMGR32_DLL = "CfgMgr32.dll";
        private const CharSet CHARSET = CharSet.Unicode;

        /// <summary>
        /// https://github.com/tpn/winsdk-10/blob/master/Include/10.0.16299.0/um/cfgmgr32.h
        /// </summary>
        private const uint CR_SUCCESS                  = (0x00000000);
        private const uint CR_DEFAULT                  = (0x00000001);
        private const uint CR_OUT_OF_MEMORY            = (0x00000002);
        private const uint CR_INVALID_POINTER          = (0x00000003);
        private const uint CR_INVALID_FLAG             = (0x00000004);
        private const uint CR_INVALID_DEVNODE          = (0x00000005);
        private const uint CR_INVALID_DEVINST          = CR_INVALID_DEVNODE;
        private const uint CR_INVALID_RES_DES          = (0x00000006);
        private const uint CR_INVALID_LOG_CONF         = (0x00000007);
        private const uint CR_INVALID_ARBITRATOR       = (0x00000008);
        private const uint CR_INVALID_NODELIST         = (0x00000009);
        private const uint CR_DEVNODE_HAS_REQS         = (0x0000000A);
        private const uint CR_DEVINST_HAS_REQS         = CR_DEVNODE_HAS_REQS;
        private const uint CR_INVALID_RESOURCEID       = (0x0000000B);
        private const uint CR_DLVXD_NOT_FOUND          = (0x0000000C);   // WIN 95 ONLY
        private const uint CR_NO_SUCH_DEVNODE          = (0x0000000D);
        private const uint CR_NO_SUCH_DEVINST          = CR_NO_SUCH_DEVNODE;
        private const uint CR_NO_MORE_LOG_CONF         = (0x0000000E);
        private const uint CR_NO_MORE_RES_DES          = (0x0000000F);
        private const uint CR_ALREADY_SUCH_DEVNODE     = (0x00000010);
        private const uint CR_ALREADY_SUCH_DEVINST     = CR_ALREADY_SUCH_DEVNODE;
        private const uint CR_INVALID_RANGE_LIST       = (0x00000011);
        private const uint CR_INVALID_RANGE            = (0x00000012);
        private const uint CR_FAILURE                  = (0x00000013);
        private const uint CR_NO_SUCH_LOGICAL_DEV      = (0x00000014);
        private const uint CR_CREATE_BLOCKED           = (0x00000015);
        private const uint CR_NOT_SYSTEM_VM            = (0x00000016);   // WIN 95 ONLY
        private const uint CR_REMOVE_VETOED            = (0x00000017);
        private const uint CR_APM_VETOED               = (0x00000018);
        private const uint CR_INVALID_LOAD_TYPE        = (0x00000019);
        private const uint CR_BUFFER_SMALL             = (0x0000001A);
        private const uint CR_NO_ARBITRATOR            = (0x0000001B);
        private const uint CR_NO_REGISTRY_HANDLE       = (0x0000001C);
        private const uint CR_REGISTRY_ERROR           = (0x0000001D);
        private const uint CR_INVALID_DEVICE_ID        = (0x0000001E);
        private const uint CR_INVALID_DATA             = (0x0000001F);
        private const uint CR_INVALID_API              = (0x00000020);
        private const uint CR_DEVLOADER_NOT_READY      = (0x00000021);
        private const uint CR_NEED_RESTART             = (0x00000022);
        private const uint CR_NO_MORE_HW_PROFILES      = (0x00000023);
        private const uint CR_DEVICE_NOT_THERE         = (0x00000024);
        private const uint CR_NO_SUCH_VALUE            = (0x00000025);
        private const uint CR_WRONG_TYPE               = (0x00000026);
        private const uint CR_INVALID_PRIORITY         = (0x00000027);
        private const uint CR_NOT_DISABLEABLE          = (0x00000028);
        private const uint CR_FREE_RESOURCES           = (0x00000029);
        private const uint CR_QUERY_VETOED             = (0x0000002A);
        private const uint CR_CANT_SHARE_IRQ           = (0x0000002B);
        private const uint CR_NO_DEPENDENT             = (0x0000002C);
        private const uint CR_SAME_RESOURCES           = (0x0000002D);
        private const uint CR_NO_SUCH_REGISTRY_KEY     = (0x0000002E);
        private const uint CR_INVALID_MACHINENAME      = (0x0000002F);   // NT ONLY
        private const uint CR_REMOTE_COMM_FAILURE      = (0x00000030);   // NT ONLY
        private const uint CR_MACHINE_UNAVAILABLE      = (0x00000031);   // NT ONLY
        private const uint CR_NO_CM_SERVICES           = (0x00000032);   // NT ONLY
        private const uint CR_ACCESS_DENIED            = (0x00000033);   // NT ONLY
        private const uint CR_CALL_NOT_IMPLEMENTED     = (0x00000034);
        private const uint CR_INVALID_PROPERTY         = (0x00000035);
        private const uint CR_DEVICE_INTERFACE_ACTIVE  = (0x00000036);
        private const uint CR_NO_SUCH_DEVICE_INTERFACE = (0x00000037);
        private const uint CR_INVALID_REFERENCE_STRING = (0x00000038);
        private const uint CR_INVALID_CONFLICT_LIST    = (0x00000039);
        private const uint CR_INVALID_INDEX            = (0x0000003A);
        private const uint CR_INVALID_STRUCTURE_SIZE   = (0x0000003B);
        private const uint NUM_CR_RESULTS              = (0x0000003C);

        private const uint CM_GET_DEVICE_INTERFACE_LIST_PRESENT = 0x00000000;

        [DllImport(CFGMGR32_DLL, CharSet = CHARSET)]
        static extern uint CM_Get_Device_Interface_List_Size(out uint size, ref Guid interfaceClassGuid, string deviceID, uint flags);

        [DllImport(CFGMGR32_DLL, CharSet = CHARSET)]
        private static extern uint CM_Get_Device_Interface_List(ref Guid interfaceClassGuid, string deviceID, char[] buffer, uint bufferLength, uint flags);

        public static string[] GetDeviceInterfaces(Guid interfaceClassGuid)
        {
            var flags = CM_GET_DEVICE_INTERFACE_LIST_PRESENT;

            uint size;
            var result = CM_Get_Device_Interface_List_Size(out size, ref interfaceClassGuid, null, flags);
            if (result != CR_SUCCESS) throw new Exception(String.Format("Unable to retrieve device interface list size, result=0x{0:X8}", result));

            var buffer = new char[(int)size];
            result = CM_Get_Device_Interface_List(ref interfaceClassGuid, null, buffer, size, flags);
            if (result != CR_SUCCESS) throw new Exception(String.Format("Unable to retrieve device interface list, result=0x{0:X8}", result));

            return new string(buffer).Split(new char[] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// https://github.com/tpn/winsdk-10/blob/master/Include/10.0.16299.0/shared/devpropdef.h
        /// </summary>
        private const uint DEVPROP_TYPE_GUID                       = 0x0000000D;  // 128-bit unique identifier (GUID)
        private const uint DEVPROP_TYPE_BOOLEAN                    = 0x00000011;  // 8-bit boolean (DEVPROP_BOOLEAN)
        private const uint DEVPROP_TYPE_STRING                     = 0x00000012;  // null-terminated string

        private const byte DEVPROP_TRUE = unchecked((byte)-1);
        private const byte DEVPROP_FALSE = 0;

        /// <summary>
        /// https://docs.microsoft.com/en-us/windows-hardware/drivers/install/devpropkey
        /// </summary>
        private struct DEVPROPKEY
        {
            public Guid fmtid;
            public uint pid;

            public DEVPROPKEY(Guid fmtid, uint pid)
            {
                this.fmtid = fmtid;
                this.pid = pid;
            }
        }

        /// <summary>
        /// https://github.com/tpn/winsdk-10/blob/master/Include/10.0.16299.0/shared/devpkey.h
        /// </summary>
        private static DEVPROPKEY DEVPKEY_DeviceInterface_FriendlyName = new DEVPROPKEY(new Guid(0x026e516e, 0xb814, 0x414b, 0x83, 0xcd, 0x85, 0x6d, 0x6f, 0xef, 0x48, 0x22), 2); // DEVPROP_TYPE_STRING
        private static DEVPROPKEY DEVPKEY_DeviceInterface_Enabled = new DEVPROPKEY(new Guid(0x026e516e, 0xb814, 0x414b, 0x83, 0xcd, 0x85, 0x6d, 0x6f, 0xef, 0x48, 0x22), 3); // DEVPROP_TYPE_BOOLEAN

        [DllImport(CFGMGR32_DLL, CharSet = CHARSET)]
        static extern uint CM_Get_Device_Interface_Property(string pszDeviceInterface, ref DEVPROPKEY PropertyKey, ref uint PropertyType, byte[] PropertyBuffer, ref uint PropertyBufferSize, uint flags);

        private static byte[] GetDeviceInterfaceProperty(string pszDeviceInterface, DEVPROPKEY PropertyKey, uint PropertyType)
        {
            byte[] PropertyBuffer = { };
            uint PropertyBufferSize = 0;
            var result = CM_Get_Device_Interface_Property(pszDeviceInterface, ref PropertyKey, ref PropertyType, PropertyBuffer, ref PropertyBufferSize, 0);
            if (result == CR_NO_SUCH_VALUE) return null;
            if (result != CR_BUFFER_SMALL) throw new Exception(String.Format("Unable to retrieve device property length, result=0x{0:X8}", result));

            PropertyBuffer = new byte[PropertyBufferSize];
            result = CM_Get_Device_Interface_Property(pszDeviceInterface, ref PropertyKey, ref PropertyType, PropertyBuffer, ref PropertyBufferSize, 0);
            if (result != CR_SUCCESS) throw new Exception(String.Format("Unable to retrieve device property, result=0x{0:X8}", result));

            return PropertyBuffer;
        }

        [DllImport(CFGMGR32_DLL, CharSet = CHARSET)]
        static extern uint CM_Set_Device_Interface_Property(string pszDeviceInterface, ref DEVPROPKEY PropertyKey, uint PropertyType, byte[] PropertyBuffer, uint PropertyBufferSize, uint flags);

        private static void SetDeviceInterfaceProperty(string pszDeviceInterface, DEVPROPKEY PropertyKey, uint PropertyType, byte[] PropertyBuffer)
        {
            SetDeviceInterfaceProperty(pszDeviceInterface, PropertyKey, PropertyType, PropertyBuffer, (uint) PropertyBuffer.Length);
        }

        private static void SetDeviceInterfaceProperty(string pszDeviceInterface, DEVPROPKEY PropertyKey, uint PropertyType, byte[] PropertyBuffer, uint PropertyBufferSize)
        {
            var result = CM_Set_Device_Interface_Property(pszDeviceInterface, ref PropertyKey, PropertyType, PropertyBuffer, PropertyBufferSize, 0);
            if (result != CR_SUCCESS) throw new Exception(String.Format("Unable to set device property, result=0x{0:X8}", result));
        }

        [DllImport(CFGMGR32_DLL, CharSet = CHARSET)]
        static extern uint CM_Get_Device_Interface_Alias(string pszDeviceInterface, ref Guid AliasInterfaceGuid, char[] pszAliasDeviceInterface, ref uint pulLength, uint flags);

        public static string GetDeviceInterfaceAlias(string pszDeviceInterface, Guid AliasInterfaceGuid)
        {
            char[] szAliasDeviceInterface = { };
            uint ulLength = 0;
            var result = CM_Get_Device_Interface_Alias(pszDeviceInterface, ref AliasInterfaceGuid, szAliasDeviceInterface, ref ulLength, 0);
            if (result == CR_NO_SUCH_DEVICE_INTERFACE) return null;
            if (result != CR_BUFFER_SMALL) throw new Exception(String.Format("Unable to retrieve device alias length, result=0x{0:X8}", result));

            szAliasDeviceInterface = new char[ulLength];
            result = CM_Get_Device_Interface_Alias(pszDeviceInterface, ref AliasInterfaceGuid, szAliasDeviceInterface, ref ulLength, 0);
            if (result != CR_SUCCESS) throw new Exception(String.Format("Unable to retrieve device alias, result=0x{0:X8}", result));

            return new string(szAliasDeviceInterface);
        }

        //
        //
        //

        public static string GetDeviceInterfacePropertyFriendlyName(string pszDeviceInterface)
        {
            var PropertyBuffer = GetDeviceInterfaceProperty(pszDeviceInterface, DEVPKEY_DeviceInterface_FriendlyName, DEVPROP_TYPE_STRING);
            if (PropertyBuffer == null || PropertyBuffer.Length < 2) return null;
            var FriendlyName = Encoding.Unicode.GetString(PropertyBuffer, 0, PropertyBuffer.Length - 2);
            return FriendlyName;
        }

        public static bool? GetDeviceInterfacePropertyEnabled(string pszDeviceInterface)
        {
            var PropertyBuffer = GetDeviceInterfaceProperty(pszDeviceInterface, DEVPKEY_DeviceInterface_Enabled, DEVPROP_TYPE_BOOLEAN);
            if (PropertyBuffer == null || PropertyBuffer.Length != 1) return null;
            var Enabled = PropertyBuffer[0] == DEVPROP_TRUE;
            return Enabled;
        }

        public static void SetDeviceInterfacePropertyEnabled(string pszDeviceInterface, bool enabled)
        {
            byte[] PropertyBuffer = { enabled ? DEVPROP_TRUE : DEVPROP_FALSE };
            SetDeviceInterfaceProperty(pszDeviceInterface, DEVPKEY_DeviceInterface_Enabled, DEVPROP_TYPE_BOOLEAN, PropertyBuffer);
        }
    }
}
