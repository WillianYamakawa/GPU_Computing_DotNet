using GPUComputingDotNet;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Unicode;

namespace GPUComputing
{
    public class Program_
    {
        public static void Main(string[] args)
        {
            //Device[] devices = Device.GetAllDevices();

            //Console.WriteLine(devices[0].Type);
            //Console.WriteLine(devices[0].MaxComputeUnits);
            //Console.WriteLine(devices[0].MaxWorkItemDimensions);
            //Console.WriteLine(devices[0].MaxWorkGroupSize);
            //Console.WriteLine(devices[0].MaxWorkItemSizes);
            //Console.WriteLine(devices[0].MaxClockFrequency);
            //Console.WriteLine(devices[0].AddressBits);
            //Console.WriteLine(devices[0].GlobalMemorySize);
            //Console.WriteLine(devices[0].IsLittleEndian);
            //Console.WriteLine(devices[0].IsAvailable);
            //Console.WriteLine(devices[0].IsCompilerAvailable);
            //Console.WriteLine(devices[0].Name);
            //Console.WriteLine(devices[0].VendorName);
            //Console.WriteLine(devices[0].DriverVersion);
            //Console.WriteLine(devices[0].Profile);
            //Console.WriteLine(devices[0].Version);
            //Console.WriteLine(devices[0].Extensions);
            //Console.WriteLine(devices[0].Platform.Name);
            //Console.WriteLine("__________________________________________________________");
            //ErrorCode errorContext;
            //Context context = Binding.clCreateContext(IntPtr.Zero, 1, ref devices[0], IntPtr.Zero, IntPtr.Zero, out errorContext);
            //ErrorCode errorQueue;
            //CommandQueue queue = Binding.clCreateCommandQueue(context, devices[0], CommandQueueProperties.NONE, out errorQueue);

            string stringProgram = @"
            __kernel void vector_sum(__global float* a, __global float* b, __global float* c){
             int i = get_global_id(0);
             float sum = a[i] + b[i];
             c[i] = sum;
            }";

            //ErrorCode errorProgram;
            //GPUComputingDotNet._Program program = Binding.clCreateProgramWithSource(context, 1, new string[] {stringProgram}, IntPtr.Zero, out errorProgram);

            //ErrorCode errorBuilding = Binding.clBuildProgram(program, 1, ref devices[0], "", IntPtr.Zero, IntPtr.Zero);
            //if(errorBuilding != ErrorCode.CL_SUCCESS)
            //{
            //    nint size;
            //    Binding.clGetProgramBuildInfo(program, devices[0], ProgramBuildInfo.CL_PROGRAM_BUILD_LOG, 0, IntPtr.Zero, out size);
            //    IntPtr result = Marshal.AllocHGlobal(size);
            //    Binding.clGetProgramBuildInfo(program, devices[0], ProgramBuildInfo.CL_PROGRAM_BUILD_LOG, size, result, out size);
            //    string error = Marshal.PtrToStringAnsi(result);
            //    Console.WriteLine(error);
            //    Marshal.FreeHGlobal(result);
            //}

            //ErrorCode kernelError;
            //_Kernel kernel = Binding.clCreateKernel(program, "vector_sum", out kernelError);
            //Console.WriteLine(kernel.ArgsCount);
            //Console.WriteLine(kernel.FunctionName);

            //////memory1
            //nint memorySize = 5 * Marshal.SizeOf(typeof(float));
            //float[] floats1 = new float[5] { 1, 2, 3, 4, 5 };
            //ErrorCode errorBuffer1;
            //IntPtr ptrMem1 = Marshal.AllocHGlobal(memorySize);
            //Marshal.Copy(floats1, 0, ptrMem1, floats1.Length);
            //Mem mem1 = Binding.clCreateBuffer(context, MemFlags.CL_MEM_READ_ONLY, memorySize, IntPtr.Zero, out errorBuffer1);
            //Event ignoore;
            //ErrorCode errorWrite1 = Binding.clEnqueueWriteBuffer(queue, mem1, CLBool.CL_TRUE, 0, memorySize, ptrMem1, 0, IntPtr.Zero, IntPtr.Zero);
            //Marshal.FreeHGlobal(ptrMem1);

            ////memory2
            //float[] floats2 = new float[5] { 1, 2, 3, 4, 5 };
            //ErrorCode errorBuffer2;
            //IntPtr ptrMem2 = Marshal.AllocHGlobal(memorySize);
            //Marshal.Copy(floats2, 0, ptrMem2, floats2.Length);
            //Mem mem2 = Binding.clCreateBuffer(context, MemFlags.CL_MEM_READ_ONLY, memorySize, IntPtr.Zero, out errorBuffer2);
            //Event ignoore2;
            //ErrorCode errorWrite2 = Binding.clEnqueueWriteBuffer(queue, mem2, CLBool.CL_TRUE, 0, memorySize, ptrMem2, 0, IntPtr.Zero, IntPtr.Zero);
            //Marshal.FreeHGlobal(ptrMem2);

            ////resultmem
            //ErrorCode errorBufferReturn;
            //Mem memRes = Binding.clCreateBuffer(context, MemFlags.CL_MEM_WRITE_ONLY, memorySize, IntPtr.Zero, out errorBufferReturn);

            //ErrorCode errorArg1 = Binding.clSetKernelArg(kernel, 0, Marshal.SizeOf(typeof(Mem)), ref mem1);
            //ErrorCode errorArg2 = Binding.clSetKernelArg(kernel, 1, Marshal.SizeOf(typeof(Mem)), ref mem2);
            //ErrorCode errorArgResult = Binding.clSetKernelArg(kernel, 2, Marshal.SizeOf(typeof(Mem)), ref memRes);

            //ErrorCode errorND = Binding.clEnqueueNDRangeKernel(queue, kernel, 1, 0, new nint[] {5}, null, 0, IntPtr.Zero, IntPtr.Zero);

            //IntPtr resultF = Marshal.AllocHGlobal(memorySize);
            //ErrorCode errorRead = Binding.clEnqueueReadBuffer(queue, memRes, CLBool.CL_TRUE, 0, memorySize, resultF, 0, IntPtr.Zero, IntPtr.Zero);

            //float[] resultComplete = new float[5];
            //for(int i = 0; i < 5; i++)
            //{
            //    resultComplete[i] = Marshal.PtrToStructure<float>(resultF + (i * Marshal.SizeOf(typeof(float))));
            //}

            //Console.WriteLine(errorArg1.ToString(), errorArg2.ToString(), errorArgResult.ToString(), errorRead.ToString());

            Device device = Device.GetMostPerformantDevice(Device.GetAllDevices());
            Program program = Program.CreateProgramWithSource(device, stringProgram, out string compilerError);
            Kernel kernel = Kernel.CreateKernel(program, "vector_sum");
            kernel.Run();
            Console.WriteLine(compilerError);
        }
    }

}
