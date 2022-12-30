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

        public DeviceType Type { 
            get {
                return (DeviceType)GetULong(DeviceInfo.CL_DEVICE_TYPE);
            } 
        }

        public uint MaxComputeUnits
        {
            get
            {
                return GetUint(DeviceInfo.CL_DEVICE_MAX_COMPUTE_UNITS);
            }
        }

        public uint MaxWorkItemDimensions
        {
            get
            {
                return GetUint(DeviceInfo.CL_DEVICE_MAX_WORK_ITEM_DIMENSIONS);
            }
        }

        public nint 

        private nint GetNInt(DeviceInfo info)
        {
            nint numReturned;
            ErrorCode error;
            int size = Marshal.SizeOf(typeof(nint));
            IntPtr buffer = Marshal.AllocHGlobal(size);
            error = Binding.clGetDeviceInfo(this, info, size, buffer, out numReturned);
            if (error != ErrorCode.CL_SUCCESS) throw new Exception("clGetDevice Returned a Error: " + Enum.GetName(error));
            nint _int = Marshal.PtrToStructure<nint>(buffer);
            Marshal.FreeHGlobal(buffer);
            return _int;
        }

        private ulong GetULong(DeviceInfo info)
        {
            nint numReturned;
            ErrorCode error;
            int size = Marshal.SizeOf(typeof(ulong));
            IntPtr buffer = Marshal.AllocHGlobal(size);
            error = Binding.clGetDeviceInfo(this, info, size, buffer, out numReturned);
            if (error != ErrorCode.CL_SUCCESS) throw new Exception("clGetDevice Returned a Error: " + Enum.GetName(error));
            ulong _int = Marshal.PtrToStructure<ulong>(buffer);
            Marshal.FreeHGlobal(buffer);
            return _int;
        }

        private uint GetUint(DeviceInfo info)
        {
            nint numReturned;
            ErrorCode error;
            int size = Marshal.SizeOf(typeof(uint));
            IntPtr buffer = Marshal.AllocHGlobal(size);
            error = Binding.clGetDeviceInfo(this, info, size, buffer, out numReturned);
            if (error != ErrorCode.CL_SUCCESS) return 0;
            uint _int = Marshal.PtrToStructure<uint>(buffer);
            Marshal.FreeHGlobal(buffer);
            return _int;
        }

        internal Device(IntPtr ptr)
        {
            this.ptr = ptr;
        }
    }
}
