using GPUComputingDotNet;
using System;

namespace GPUComputing
{
    public class Program_
    {
        public static void Main(string[] args)
        {
            //=======================EXAMPLE====================
            string stringProgram = @"
            __kernel void vector_sum(__global float* a, __global float* b, __global float* c){
             int i = get_global_id(0);
             float sum = a[i] + b[i];
             c[i] = sum;
            }";

            Device device = Device.GetMostPerformantDevice(Device.GetAllDevices());
            Program program = Program.CreateProgramWithSource(device, stringProgram, out string compilerError);
            Kernel kernel = Kernel.CreateKernel(program, "vector_sum");
            kernel.SetInputArgument(0, new float[5] { 1, 2, 3, 4, 5 });
            kernel.SetInputArgument(1, new float[5] { 1, 2, 3, 4, 5 });
            int outputArg = kernel.SetOutputArgument(2, typeof(float), 5);
            kernel.Run(1, new nint[] {5});
            float[] arr = kernel.GetOutputArgument<float>(outputArg);
            Console.WriteLine(compilerError);
        }
    }

}
