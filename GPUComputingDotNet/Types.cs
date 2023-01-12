using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GPUComputingDotNet
{
    public class OpenCLAPIException : Exception
    {
        public OpenCLAPIException(string message, ErrorCode code) : base(message + Enum.GetName(typeof(ErrorCode), code)) {}
        public OpenCLAPIException(ErrorCode code) : base("API " + Binding.Library + " returned" + Enum.GetName(typeof(ErrorCode), code)) { }
    }

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

        public static Platform[] GetAllPlatforms(uint max = 64)
        {
            uint numPlat;
            IntPtr ptrToPlat = Marshal.AllocHGlobal((int)max * Marshal.SizeOf(typeof(Platform)));
            Binding.CheckApiError(Binding.clGetPlatformIDs(max, ptrToPlat, out numPlat));
            Platform[] platforms = new Platform[numPlat];
            for (int i = 0; i < numPlat; i++)
            {
                platforms[i] = Marshal.PtrToStructure<Platform>(ptrToPlat + (i * Marshal.SizeOf(typeof(Platform))));
            }
            Marshal.FreeHGlobal(ptrToPlat);
            return platforms;
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

        public static Device[] GetAllDevices(Platform platform, DeviceType deviceType = DeviceType.ALL)
        {
            IntPtr ptrToDevs = Marshal.AllocHGlobal(64 * Marshal.SizeOf(typeof(Device)));
            uint numDevices;
            Binding.CheckApiError(Binding.clGetDeviceIDs(platform, DeviceType.ALL, 10, ptrToDevs, out numDevices));
            Device[] devices = new Device[numDevices];
            for (int i = 0; i < numDevices; i++)
            {
                devices[i] = Marshal.PtrToStructure<Device>(ptrToDevs + (i * Marshal.SizeOf(typeof(Device))));
            }
            Marshal.FreeHGlobal(ptrToDevs);
            return devices;
        }

        public static Device[] GetAllDevices(DeviceType deviceType = DeviceType.ALL)
        {
            List<Device> devices = new List<Device>();
            Platform[] platforms = Platform.GetAllPlatforms();
            foreach(var platform in platforms){
                Device[] _devices = Device.GetAllDevices(platform, deviceType);
                devices.AddRange(_devices);
            }
            return devices.ToArray();
        }

        public static Device GetMostPerformantDevice(Device[] devices)
        {
            if (devices == null || devices.Length == 0) throw new Exception("Devices Array is Empty");
            Device selectedDevice = default;
            int selectedValue = -1;
            foreach(var device in devices)
            {
                uint value = device.MaxClockFrequency * device.MaxComputeUnits;
                if(value > selectedValue)
                {
                    selectedValue = (int)value;
                    selectedDevice = device;
                }
            }
            return selectedDevice;
        }

        internal Device(IntPtr ptr)
        {
            this.ptr = ptr;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Context
    {
        public IntPtr ptr;

        internal Context(IntPtr ptr)
        {
            this.ptr = ptr;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct CommandQueue
    {
        public IntPtr ptr;

        internal CommandQueue(IntPtr ptr)
        {
            this.ptr = ptr;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct _Program
    {
        public IntPtr ptr;

        internal _Program(IntPtr ptr)
        {
            this.ptr = ptr;
        }
    }

    public class Program
    {
        internal Context context;
        internal CommandQueue queue;
        internal Device device;
        internal _Program program;

        public static Program CreateProgramWithSource(Device device, string[] source, out string compilerError)
        {
            compilerError = null;
            ErrorCode errorContext;
            Context context = Binding.clCreateContext(IntPtr.Zero, 1, ref device, IntPtr.Zero, IntPtr.Zero, out errorContext);
            Binding.CheckApiError(errorContext);
            ErrorCode errorQueue;
            CommandQueue queue = Binding.clCreateCommandQueue(context, device, CommandQueueProperties.NONE, out errorQueue);
            Binding.CheckApiError(errorQueue);

            ErrorCode errorProgram;
            GPUComputingDotNet._Program program = Binding.clCreateProgramWithSource(context, 1, source, IntPtr.Zero, out errorProgram);
            Binding.CheckApiError(errorProgram);

            ErrorCode errorBuilding = Binding.clBuildProgram(program, 1, ref device, "", IntPtr.Zero, IntPtr.Zero);
            if (errorBuilding != ErrorCode.CL_SUCCESS)
            {
                nint size;
                Binding.clGetProgramBuildInfo(program, device, ProgramBuildInfo.CL_PROGRAM_BUILD_LOG, 0, IntPtr.Zero, out size);
                IntPtr result = Marshal.AllocHGlobal(size);
                Binding.clGetProgramBuildInfo(program, device, ProgramBuildInfo.CL_PROGRAM_BUILD_LOG, size, result, out size);
                compilerError = Marshal.PtrToStringAnsi(result);
                Marshal.FreeHGlobal(result);
                return null;
            }

            return new Program(context, queue, program, device);

        }

        public static Program CreateProgramWithSource(Device device, string source, out string compilerError)
        {
            return Program.CreateProgramWithSource(device, new string[] { source }, out compilerError);
        }

        private Program(Context context, CommandQueue queue, _Program program, Device device)
        {
            this.context = context;
            this.queue = queue;
            this.program = program;
            this.device = device;
        }

        ~Program()
        {
            Binding.CheckApiError(Binding.clReleaseContext(context));
            Binding.CheckApiError(Binding.clReleaseCommandQueue(queue));
            Binding.CheckApiError(Binding.clReleaseProgram(program));
        }
    }

    public struct OutputArgument
    {
        internal int index;
        internal OutputArgument(int index)
        {
            this.index = index;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct _Kernel
    {
        public IntPtr ptr;

        internal _Kernel(IntPtr ptr)
        {
            this.ptr = ptr;
        }
    }

    public class Kernel
    {
        Program program;
        _Kernel kernel;
        List<Mem> inputMemories;
        List<Mem> outputMemories;
        List<int> outputSizes;
        List<int> argumentIndexes;

        public uint ArgsCount
        {
            get
            {
                nint numReturned;
                ErrorCode error;
                int size = Marshal.SizeOf(typeof(uint));
                IntPtr buffer = Marshal.AllocHGlobal(size);
                error = Binding.clGetKernelInfo(this.kernel, KernelInfo.CL_KERNEL_NUM_ARGS, size, buffer, out numReturned);
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
                error = Binding.clGetKernelInfo(this.kernel, KernelInfo.CL_KERNEL_FUNCTION_NAME, 0, IntPtr.Zero, out numReturned);
                if (error != ErrorCode.CL_SUCCESS) throw new Exception("clGetKernelInfo Returned a Error: " + Enum.GetName(error));
                IntPtr buffer = Marshal.AllocHGlobal(numReturned);
                error = Binding.clGetKernelInfo(this.kernel, KernelInfo.CL_KERNEL_FUNCTION_NAME, numReturned, buffer, out numReturned);
                if (error != ErrorCode.CL_SUCCESS) throw new Exception("clGetKernelInfo Returned a Error: " + Enum.GetName(error));
                string name = Marshal.PtrToStringUTF8(buffer);
                Marshal.FreeHGlobal(buffer);
                return name;
            }
        }

        public static Kernel CreateKernel(Program program, string kernelName)
        {
            if (program == null) throw new ArgumentNullException("Program cannot be null");
            ErrorCode error;
            _Kernel kernel = Binding.clCreateKernel(program.program, kernelName, out error);
            Binding.CheckApiError(error);
            return new Kernel(program, kernel);
        }

        private Kernel(Program program, _Kernel kernel)
        {
            this.program = program;
            this.kernel = kernel;
            inputMemories = new();
            outputMemories = new();
            outputSizes = new();
            argumentIndexes = new();
        }

        public void SetInputArgument<T>(int argumentIndex, T[] array) where T : struct
        {
            if(argumentIndex >= this.ArgsCount) { throw new Exception("Argument out of bounds"); }
            nint memorySize = array.Length * Marshal.SizeOf(typeof(T));
            IntPtr ptr = Marshal.AllocHGlobal(memorySize);
            for(int i = 0; i < array.Length; i++)
            {
                Marshal.StructureToPtr(array[i], ptr + (Marshal.SizeOf(typeof(T)) * i), false);
            }
            ErrorCode errorBuffer;
            Mem mem = Binding.clCreateBuffer(program.context, MemFlags.CL_MEM_READ_ONLY, memorySize, IntPtr.Zero, out errorBuffer);
            Binding.CheckApiError(errorBuffer);
            Binding.CheckApiError(Binding.clEnqueueWriteBuffer(program.queue, mem, CLBool.CL_TRUE, 0, memorySize, ptr, 0, IntPtr.Zero, IntPtr.Zero));
            Marshal.FreeHGlobal(ptr);
            this.inputMemories.Add(mem);
            Binding.CheckApiError(Binding.clSetKernelArg(kernel, (uint)argumentIndex, Marshal.SizeOf(typeof(Mem)), ref mem));
            argumentIndexes.Add(argumentIndex);
        }

        public OutputArgument SetOutputArgument(int argumentIndex, Type type, int size)
        {
            if (argumentIndex >= this.ArgsCount) { throw new Exception("Index out of bounds"); }
            ErrorCode errorBufferReturn;
            Mem mem = Binding.clCreateBuffer(program.context, MemFlags.CL_MEM_WRITE_ONLY, size * Marshal.SizeOf(type), IntPtr.Zero, out errorBufferReturn);
            Binding.CheckApiError(errorBufferReturn);
            Binding.CheckApiError(Binding.clSetKernelArg(kernel, 2, Marshal.SizeOf(typeof(Mem)), ref mem));
            this.outputMemories.Add(mem);
            argumentIndexes.Add(argumentIndex);
            outputSizes.Add(size);
            return new OutputArgument(this.outputMemories.Count - 1);
        }

        public void Run(nint[] workSizes)
        {
            if(workSizes.Length > this.program.device.MaxWorkItemDimensions)
            {
                throw new Exception("Invalid Arguments: make sure workSizes.Length is <= device.MaxWorkItemDimensions");
            }
            int argsCount = (int)this.ArgsCount;
            for (int i = 0; i < argsCount; i++)
            {
                if (!argumentIndexes.Contains(i))
                {
                    throw new Exception("Argument missing: " + i);
                }
            }

            Binding.CheckApiError(Binding.clEnqueueNDRangeKernel(this.program.queue, this.kernel, (uint)workSizes.Length, 0, workSizes, null, 0, IntPtr.Zero, IntPtr.Zero));
            Binding.CheckApiError(Binding.clFinish(program.queue));
        }

        public T[] GetOutputArgument<T>(OutputArgument outputArgument) where T : struct
        {
            int memoryIndex = outputArgument.index;
            if (memoryIndex >= outputMemories.Count) { throw new Exception("Index out of bounds"); }
            int size = outputSizes[memoryIndex] * Marshal.SizeOf(typeof(T));
            IntPtr resultPtr = Marshal.AllocHGlobal(size);
            Binding.CheckApiError(Binding.clEnqueueReadBuffer(program.queue, outputMemories[memoryIndex], CLBool.CL_TRUE, 0, size, resultPtr, 0, IntPtr.Zero, IntPtr.Zero));

            T[] resultArr = new T[outputSizes[memoryIndex]];
            for (int i = 0; i < outputSizes[memoryIndex]; i++)
            {
                resultArr[i] = Marshal.PtrToStructure<T>(resultPtr + (i * Marshal.SizeOf(typeof(T))));
            }
            return resultArr;
        }

        ~Kernel()
        {
            foreach(Mem mem in inputMemories)
            {
                Binding.CheckApiError(Binding.clReleaseMemObject(mem));
            }
            foreach (Mem mem in outputMemories)
            {
                Binding.CheckApiError(Binding.clReleaseMemObject(mem));
            }
            Binding.CheckApiError(Binding.clReleaseKernel(kernel));
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Mem
    {
        public IntPtr ptr;

        internal Mem(IntPtr ptr)
        {
            this.ptr = ptr;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct Event
    {
        public IntPtr ptr;

        internal Event(IntPtr ptr)
        {
            this.ptr = ptr;
        }
    }
}
