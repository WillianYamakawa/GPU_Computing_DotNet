using GPUComputingDotNet;
using System;
using System.Runtime.InteropServices;

namespace GPUComputing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Platform[] platforms = new Platform[10];
            uint numPlat;
            Binding.clGetPlatformIDs(10, platforms, out numPlat);
            Device[] devices = new Device[10];
            uint numDevices;
            Binding.clGetDeviceIDs(platforms[0], DeviceType.ALL, 10, devices, out numDevices);
            Console.WriteLine(devices[0].MaxWorkItemDimensions);
        }
    }

}
