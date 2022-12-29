using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace GPUComputingDotNet
{
    internal class Binding
    {
        public const string Library = "opencl.dll";

        //cl_int clGetPlatformIDs(cl_uint num_entries,cl_platform_id* platforms,cl_uint* num_platforms)
        [DllImport(Library)]
        public static extern ErrorCode clGetPlatformIDs(uint maxEntries,
                                                     [Out][MarshalAs(UnmanagedType.LPArray)] Platform[] platforms,
                                                     out uint numPlatforms);


        //cl_int clGetPlatformInfo(cl_platform_id platform,cl_platform_info param_name,size_t param_value_size,void* param_value,size_t* param_value_size_ret)
        [DllImport(Library)]
        public static extern ErrorCode clGetPlatformInfo(Platform platform, 
                                                         PlatformInfo info, 
                                                         nint valueSizeMax, 
                                                         IntPtr value, 
                                                         out nint valueSize);


        //cl_int clGetDeviceIDs(cl_platform_id platform,cl_device_type device_type,cl_uint num_entries,cl_device_id* devices,cl_uint* num_devices)
        [DllImport(Library)]
        public static extern ErrorCode clGetDeviceIDs(Platform platform, DeviceType type, uint maxEntries, 
                                                      [Out][MarshalAs(UnmanagedType.LPArray)] Device[] devices,
                                                      out uint numDevices);
    }
}
