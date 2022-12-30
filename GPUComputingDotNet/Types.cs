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
        public string Name { get { return GetInfo(PlatformInfo.CL_PLATFORM_NAME); } }

        public string Profile { get { return GetInfo(PlatformInfo.CL_PLATFORM_PROFILE); } }

        public string Version { get { return GetInfo(PlatformInfo.CL_PLATFORM_VERSION); } }

        public string Vendor { get { return GetInfo(PlatformInfo.CL_PLATFORM_VENDOR); } }

        public string Extensions { get { return GetInfo(PlatformInfo.CL_PLATFORM_EXTENSIONS); } }

        private string GetInfo(PlatformInfo info)
        {
            nint numReturned;
            ErrorCode error;
            error = Binding.clGetPlatformInfo(this, info, 0, IntPtr.Zero, out numReturned);
            if (error != ErrorCode.CL_SUCCESS) throw new Exception("clGetPlatformInfo Returned a Error: " + Enum.GetName(error));
            IntPtr buffer = Marshal.AllocHGlobal(numReturned);
            error = Binding.clGetPlatformInfo(this, info, numReturned, buffer, out numReturned);
            if (error != ErrorCode.CL_SUCCESS) throw new Exception("clGetPlatformInfo Returned a Error: " + Enum.GetName(error));
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

        public DeviceType Type { get { return (DeviceType)GetULong(DeviceInfo.CL_DEVICE_TYPE); } }

        public uint MaxComputeUnits { get { return GetUint(DeviceInfo.CL_DEVICE_MAX_COMPUTE_UNITS); } }

        public uint MaxWorkItemDimensions { get { return GetUint(DeviceInfo.CL_DEVICE_MAX_WORK_ITEM_DIMENSIONS); } }

        public nint MaxWorkGroupSize { get { return GetNInt(DeviceInfo.CL_DEVICE_MAX_WORK_GROUP_SIZE); } }

        public uint MaxClockFrequency { get { return GetUint(DeviceInfo.CL_DEVICE_MAX_CLOCK_FREQUENCY); } }

        public uint AddressBits { get { return GetUint(DeviceInfo.CL_DEVICE_ADDRESS_BITS); } }

        public ulong GlobalMemorySize { get { return GetULong(DeviceInfo.CL_DEVICE_GLOBAL_MEM_SIZE); } }

        public bool IsLittleEndian { get { return GetUint(DeviceInfo.CL_DEVICE_ENDIAN_LITTLE) == 1; } }

        public bool IsAvailable { get { return GetUint(DeviceInfo.CL_DEVICE_AVAILABLE) == 1; } }

        public bool IsCompilerAvailable { get { return GetUint(DeviceInfo.CL_DEVICE_COMPILER_AVAILABLE) == 1; } }

        public string Name { get { return GetString(DeviceInfo.CL_DEVICE_NAME); } }

        public string VendorName { get { return GetString(DeviceInfo.CL_DEVICE_VENDOR); } }

        public string DriverVersion { get { return GetString(DeviceInfo.CL_DRIVER_VERSION); } }

        public string Version { get { return GetString(DeviceInfo.CL_DEVICE_VERSION); } }

        public string Profile { get { return GetString(DeviceInfo.CL_DEVICE_PROFILE); } }

        public string[] Extensions { get { return GetString(DeviceInfo.CL_DEVICE_VERSION).Split(' '); } }

        public Platform Platform { get { return new Platform(GetNInt(DeviceInfo.CL_DEVICE_PLATFORM)); } }

        public nint[] MaxWorkItemSizes
        {
            get
            {
                nint numReturned;
                ErrorCode error;
                int dim = (int)this.MaxWorkItemDimensions;
                int size = Marshal.SizeOf(typeof(nint)) * dim;
                IntPtr buffer = Marshal.AllocHGlobal(size);
                error = Binding.clGetDeviceInfo(this, DeviceInfo.CL_DEVICE_MAX_WORK_ITEM_SIZES, size, buffer, out numReturned);
                if (error != ErrorCode.CL_SUCCESS) throw new Exception("clGetDeviceInfo Returned a Error: " + Enum.GetName(error));
                nint[] result = new nint[dim];
                int typeSize = Marshal.SizeOf(typeof(nint));
                for (int i = 0; i < dim; i++)
                {
                    result[i] = Marshal.PtrToStructure<nint>(buffer + (i * typeSize));
                }
                Marshal.FreeHGlobal(buffer);
                return result;
            }
        }

        private nint GetNInt(DeviceInfo info)
        {
            nint numReturned;
            ErrorCode error;
            int size = Marshal.SizeOf(typeof(nint));
            IntPtr buffer = Marshal.AllocHGlobal(size);
            error = Binding.clGetDeviceInfo(this, info, size, buffer, out numReturned);
            if (error != ErrorCode.CL_SUCCESS) throw new Exception("clGetDeviceInfo Returned a Error: " + Enum.GetName(error));
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
            if (error != ErrorCode.CL_SUCCESS) throw new Exception("clGetDeviceInfo Returned a Error: " + Enum.GetName(error));
            ulong _long = Marshal.PtrToStructure<ulong>(buffer);
            Marshal.FreeHGlobal(buffer);
            return _long;
        }

        private uint GetUint(DeviceInfo info)
        {
            nint numReturned;
            ErrorCode error;
            int size = Marshal.SizeOf(typeof(uint));
            IntPtr buffer = Marshal.AllocHGlobal(size);
            error = Binding.clGetDeviceInfo(this, info, size, buffer, out numReturned);
            if (error != ErrorCode.CL_SUCCESS) throw new Exception("clGetDeviceInfo Returned a Error: " + Enum.GetName(error));
            uint _int = Marshal.PtrToStructure<uint>(buffer);
            Marshal.FreeHGlobal(buffer);
            return _int;
        }

        private string GetString(DeviceInfo info)
        {
            nint numReturned;
            ErrorCode error;
            error = Binding.clGetDeviceInfo(this, info, 0, IntPtr.Zero, out numReturned);
            if (error != ErrorCode.CL_SUCCESS) throw new Exception("clGetDeviceInfo Returned a Error: " + Enum.GetName(error));
            IntPtr buffer = Marshal.AllocHGlobal(numReturned);
            error = Binding.clGetDeviceInfo(this, info, numReturned, buffer, out numReturned);
            if (error != ErrorCode.CL_SUCCESS) throw new Exception("clGetDeviceInfo Returned a Error: " + Enum.GetName(error));
            string name = Marshal.PtrToStringUTF8(buffer);
            Marshal.FreeHGlobal(buffer);
            return name;
        }

        internal Device(IntPtr ptr)
        {
            this.ptr = ptr;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Context
    {
        public IntPtr ptr;

        internal Context(IntPtr ptr)
        {
            this.ptr = ptr;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CommandQueue
    {
        public IntPtr ptr;

        internal CommandQueue(IntPtr ptr)
        {
            this.ptr = ptr;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Program
    {
        public IntPtr ptr;

        internal Program(IntPtr ptr)
        {
            this.ptr = ptr;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Kernel
    {
        public IntPtr ptr;

        public uint ArgsCount
        {
            get
            {
                nint numReturned;
                ErrorCode error;
                int size = Marshal.SizeOf(typeof(uint));
                IntPtr buffer = Marshal.AllocHGlobal(size);
                error = Binding.clGetKernelInfo(this, KernelInfo.CL_KERNEL_NUM_ARGS, size, buffer, out numReturned);
                if (error != ErrorCode.CL_SUCCESS) throw new Exception("clGetKernelInfo Returned a Error: " + Enum.GetName(error));
                uint _int = Marshal.PtrToStructure<uint>(buffer);
                Marshal.FreeHGlobal(buffer);
                return _int;
            }
        }

        public string FunctionName
        {
            get
            {
                nint numReturned;
                ErrorCode error;
                error = Binding.clGetKernelInfo(this, KernelInfo.CL_KERNEL_FUNCTION_NAME, 0, IntPtr.Zero, out numReturned);
                if (error != ErrorCode.CL_SUCCESS) throw new Exception("clGetKernelInfo Returned a Error: " + Enum.GetName(error));
                IntPtr buffer = Marshal.AllocHGlobal(numReturned);
                error = Binding.clGetKernelInfo(this, KernelInfo.CL_KERNEL_FUNCTION_NAME, numReturned, buffer, out numReturned);
                if (error != ErrorCode.CL_SUCCESS) throw new Exception("clGetKernelInfo Returned a Error: " + Enum.GetName(error));
                string name = Marshal.PtrToStringUTF8(buffer);
                Marshal.FreeHGlobal(buffer);
                return name;
            }
        }

        internal Kernel(IntPtr ptr)
        {
            this.ptr = ptr;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Mem
    {
        public IntPtr ptr;

        internal Mem(IntPtr ptr)
        {
            this.ptr = ptr;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Event
    {
        public IntPtr ptr;

        internal Event(IntPtr ptr)
        {
            this.ptr = ptr;
        }
    }
}
