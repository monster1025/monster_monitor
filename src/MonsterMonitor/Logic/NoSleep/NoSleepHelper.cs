﻿using System;
using System.Runtime.InteropServices;

namespace MonsterMonitor.Logic.NoSleep
{
    public class NoSleepHelper
    {
        #region prevent screensaver, display dimming and automatically sleeping

        POWER_REQUEST_CONTEXT _powerRequestContext;
        IntPtr _powerRequest; //HANDLE

        // Availability Request Functions
        [DllImport("kernel32.dll")]
        static extern IntPtr PowerCreateRequest(ref POWER_REQUEST_CONTEXT Context);

        [DllImport("kernel32.dll")]
        static extern bool PowerSetRequest(IntPtr PowerRequestHandle, PowerRequestType RequestType);

        [DllImport("kernel32.dll")]
        static extern bool PowerClearRequest(IntPtr PowerRequestHandle, PowerRequestType RequestType);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        internal static extern int CloseHandle(IntPtr hObject);

        // Availablity Request Enumerations and Constants
        enum PowerRequestType
        {
            PowerRequestDisplayRequired = 0,
            PowerRequestSystemRequired,
            PowerRequestAwayModeRequired,
            PowerRequestMaximum
        }

        const int POWER_REQUEST_CONTEXT_VERSION = 0;
        const int POWER_REQUEST_CONTEXT_SIMPLE_STRING = 0x1;
        const int POWER_REQUEST_CONTEXT_DETAILED_STRING = 0x2;

        // Availablity Request Structures
        // Note:  Windows defines the POWER_REQUEST_CONTEXT structure with an
        // internal union of SimpleReasonString and Detailed information.
        // To avoid runtime interop issues, this version of 
        // POWER_REQUEST_CONTEXT only supports SimpleReasonString.  
        // To use the detailed information,
        // define the PowerCreateRequest function with the first 
        // parameter of type POWER_REQUEST_CONTEXT_DETAILED.
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct POWER_REQUEST_CONTEXT
        {
            public UInt32 Version;
            public UInt32 Flags;

            [MarshalAs(UnmanagedType.LPWStr)] public string
                SimpleReasonString;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PowerRequestContextDetailedInformation
        {
            public IntPtr LocalizedReasonModule;
            public UInt32 LocalizedReasonId;
            public UInt32 ReasonStringCount;
            [MarshalAs(UnmanagedType.LPWStr)] public string[] ReasonStrings;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct POWER_REQUEST_CONTEXT_DETAILED
        {
            public UInt32 Version;
            public UInt32 Flags;
            public PowerRequestContextDetailedInformation DetailedInformation;
        }

        #endregion

        /// <summary>
        /// Prevent screensaver, display dimming and power saving. This function wraps PInvokes on Win32 API. 
        /// </summary>
        /// <param name="enableConstantDisplayAndPower">True to get a constant display and power - False to clear the settings</param>
        public void EnableConstantDisplayAndPower(bool enableConstantDisplayAndPower)
        {
            if (enableConstantDisplayAndPower)
            {
                // Set up the diagnostic string
                _powerRequestContext.Version = POWER_REQUEST_CONTEXT_VERSION;
                _powerRequestContext.Flags = POWER_REQUEST_CONTEXT_SIMPLE_STRING;
                _powerRequestContext.SimpleReasonString =
                    "Continuous measurement"; // your reason for changing the power settings;

                // Create the request, get a handle
                _powerRequest = PowerCreateRequest(ref _powerRequestContext);

                // Set the request
                PowerSetRequest(_powerRequest, PowerRequestType.PowerRequestSystemRequired);
                PowerSetRequest(_powerRequest, PowerRequestType.PowerRequestDisplayRequired);
            }
            else
            {
                // Clear the request
                PowerClearRequest(_powerRequest, PowerRequestType.PowerRequestSystemRequired);
                PowerClearRequest(_powerRequest, PowerRequestType.PowerRequestDisplayRequired);

                CloseHandle(_powerRequest);
            }
        }
    }
}
