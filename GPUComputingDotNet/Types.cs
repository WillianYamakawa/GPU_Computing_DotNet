using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace GPUComputingDotNet
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Platform
    {
        nint ptr;
        public string Name { 
            get
            {
                return GetInfo(PlatformInfo.CL_PLATFORM_NAME);
            }
        }

        public string Profile
        {
            get
            {
                return GetInfo(PlatformInfo.CL_PLATFORM_PROFILE);
            }
        }

        public string Version
        {
            get
            {
                return GetInfo(PlatformInfo.CL_PLATFORM_VERSION);
            }
        }

        public string Vendor
        {
            get
            {
                return GetInfo(PlatformInfo.CL_PLATFORM_VENDOR);
            }
        }

        public string Extensions
        {
            get
            {
                return GetInfo(PlatformInfo.CL_PLATFORM_EXTENSIONS);
            }
        }

        private string GetInfo(PlatformInfo info)
        {
            nint numReturned;
            ErrorCode error;
            error = Binding.clGetPlatformInfo(this, info, 0, IntPtr.Zero, out numReturned);
            if (error != ErrorCode.CL_SUCCESS) return null;
            IntPtr buffer = Marshal.AllocHGlobal(numReturned);
            error = Binding.clGetPlatformInfo(this, info, numReturned, buffer, out numReturned);
            if (error != ErrorCode.CL_SUCCESS) return null;
            string name = Marshal.PtrToStringUTF8(buffer);
            Marshal.FreeHGlobal(buffer);
            return name;
        }


        
        internal Platform(IntPtr ptr)
        {
            this.ptr = ptr;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Device
    {
        nint ptr;

        internal Device(IntPtr ptr)
        {
            this.ptr = ptr;
        }
    }
}
