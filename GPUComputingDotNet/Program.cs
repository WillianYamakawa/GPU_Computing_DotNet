using GPUComputingDotNet;
using System;
using System.Drawing;
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
        }
    }

}
