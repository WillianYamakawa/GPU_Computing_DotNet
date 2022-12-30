using GPUComputingDotNet;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Unicode;

namespace GPUComputing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            uint numPlat;
            IntPtr ptrToPlat = Marshal.AllocHGlobal(64 * Marshal.SizeOf(typeof(Platform)));
            Binding.clGetPlatformIDs(64, ptrToPlat, out numPlat);
            Platform[] platforms = new Platform[numPlat];
            for(int i = 0; i < numPlat; i++)
            {
                platforms[i] = Marshal.PtrToStructure<Platform>(ptrToPlat + (i * Marshal.SizeOf(typeof(Platform))));
            }

            Marshal.FreeHGlobal(ptrToPlat);
            IntPtr ptrToDevs = Marshal.AllocHGlobal(64 * Marshal.SizeOf(typeof(Device)));
            uint numDevices;
            Binding.clGetDeviceIDs(platforms[0], DeviceType.ALL, 10, ptrToDevs, out numDevices);
            Device[] devices = new Device[numDevices];
            for (int i = 0; i < numDevices; i++)
            {
                devices[i] = Marshal.PtrToStructure<Device>(ptrToDevs + (i * Marshal.SizeOf(typeof(Device))));
            }
            Marshal.FreeHGlobal(ptrToDevs);

            Console.WriteLine(devices[0].Type);
            Console.WriteLine(devices[0].MaxComputeUnits);
            Console.WriteLine(devices[0].MaxWorkItemDimensions);
            Console.WriteLine(devices[0].MaxWorkGroupSize);
            Console.WriteLine(devices[0].MaxWorkItemSizes);
            Console.WriteLine(devices[0].MaxClockFrequency);
            Console.WriteLine(devices[0].AddressBits);
            Console.WriteLine(devices[0].GlobalMemorySize);
            Console.WriteLine(devices[0].IsLittleEndian);
            Console.WriteLine(devices[0].IsAvailable);
            Console.WriteLine(devices[0].IsCompilerAvailable);
            Console.WriteLine(devices[0].Name);
            Console.WriteLine(devices[0].VendorName);
            Console.WriteLine(devices[0].DriverVersion);
            Console.WriteLine(devices[0].Profile);
            Console.WriteLine(devices[0].Version);
            Console.WriteLine(devices[0].Extensions);
            Console.WriteLine(devices[0].Platform.Name);
            Console.WriteLine("__________________________________________________________");
            ErrorCode errorContext;
            Context context = Binding.clCreateContext(IntPtr.Zero, 1, ref devices[0], IntPtr.Zero, IntPtr.Zero, out errorContext);
            ErrorCode errorQueue;
            CommandQueue queue = Binding.clCreateCommandQueue(context, devices[0], CommandQueueProperties.NONE, out errorQueue);

            string stringProgram = @"
            __kernel void vector_sum(__global float* a, __global float* b, __global float* c){
	            int i = get_global_id(0);
	            float sum = a[i] + b[i];
	            c[i] = sum;
            }";

            ErrorCode errorProgram;
            GPUComputingDotNet.Program program = Binding.clCreateProgramWithSource(context, 1, new string[] {stringProgram}, IntPtr.Zero, out errorProgram);

            ErrorCode errorBuilding = Binding.clBuildProgram(program, 1, ref devices[0], "", IntPtr.Zero, IntPtr.Zero);
            if(errorBuilding != ErrorCode.CL_SUCCESS)
            {
                nint size;
                Binding.clGetProgramBuildInfo(program, devices[0], ProgramBuildInfo.CL_PROGRAM_BUILD_LOG, 0, IntPtr.Zero, out size);
                IntPtr result = Marshal.AllocHGlobal(size);
                Binding.clGetProgramBuildInfo(program, devices[0], ProgramBuildInfo.CL_PROGRAM_BUILD_LOG, size, result, out size);
                string error = Marshal.PtrToStringAnsi(result);
                Console.WriteLine(error);
                Marshal.FreeHGlobal(result);
            }


        }
    }

}
