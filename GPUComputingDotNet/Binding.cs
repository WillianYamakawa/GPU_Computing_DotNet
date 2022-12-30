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
                                                     IntPtr platforms,
                                                     [Out] out uint numPlatforms);


        //cl_int clGetPlatformInfo(cl_platform_id platform,cl_platform_info param_name,size_t param_value_size,void* param_value,size_t* param_value_size_ret)
        [DllImport(Library)]
        public static extern ErrorCode clGetPlatformInfo(Platform platform, 
                                                         PlatformInfo info, 
                                                         nint valueSizeMax, 
                                                         IntPtr value,
                                                         [Out] out nint valueSize);


        //cl_int clGetDeviceIDs(cl_platform_id platform,cl_device_type device_type,cl_uint num_entries,cl_device_id* devices,cl_uint* num_devices)
        [DllImport(Library)]
        public static extern ErrorCode clGetDeviceIDs(Platform platform, DeviceType type, uint maxEntries, 
                                                      IntPtr devices,
                                                      [Out] out uint numDevices);

        //cl_int clGetPlatformInfo(cl_platform_id platform,cl_platform_info param_name,size_t param_value_size,void* param_value,size_t* param_value_size_ret)
        [DllImport(Library)]
        public static extern ErrorCode clGetDeviceInfo(Device platform,
                                                         DeviceInfo info,
                                                         nint valueSizeMax,
                                                         IntPtr value,
                                                         [Out] out nint valueSize);


        //cl_context clCreateContext(cl_context_properties* properties,cl_uint num_devices,const cl_device_id* devices,void* pfn_notify(const char* errinfo,const void* private_info,size_t cb, void* user_data),void* user_data,cl_int *errcode_ret)
        [DllImport(Library)]
        public static extern Context clCreateContext(IntPtr properties, //NULL
                                                     uint numDevices, //Always 1
                                                     [In] ref Device device,  //Should be and array, but since always 1
                                                     IntPtr callback,  //NULL
                                                     IntPtr userData,  //NULL
                                                     [Out] out ErrorCode error);


        //cl_command_queue clCreateCommandQueue(cl_context context,cl_device_id device,cl_command_queue_properties properties,cl_int* errcode_ret)
        [DllImport(Library)]
        public static extern CommandQueue clCreateCommandQueue(Context context, 
                                                               Device device, 
                                                               CommandQueueProperties properties, 
                                                               [Out] out ErrorCode error);


        //cl_program clCreateProgramWithSource(cl_context context,cl_uint count,const char** strings,const size_t* lengths,cl_int *errcode_ret)
        [DllImport(Library)]
        public static extern Program clCreateProgramWithSource(Context context,
                                                               uint count,
                                                               [In][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 1)] string[] strings,
                                                               IntPtr lengths, //Always NULL because strings are null terminated
                                                               [Out] out ErrorCode error);

        //cl_int clBuildProgram(cl_program program,cl_uint num_devices,const cl_device_id* device_list,const char* options,void (* pfn_notify) (cl_program, void* user_data),void* user_data)
        [DllImport(Library)]
        public static extern ErrorCode clBuildProgram(Program program,
                                                      uint numDevices, //Always 1
                                                      [In] ref Device device,
                                                      [In][MarshalAs(UnmanagedType.LPStr)] string options,
                                                      IntPtr callback, //Always null
                                                      IntPtr userData); //Always null


        //cl_int clGetProgramBuildInfo(cl_program program,cl_device_id device,cl_program_build_info param_name,size_t param_value_size,void* param_value,size_t* param_value_size_ret)
        [DllImport(Library)]
        public static extern ErrorCode clGetProgramBuildInfo(Program program,
                                                             Device device,
                                                             ProgramBuildInfo info,
                                                             nint maxSize,
                                                             IntPtr value,
                                                             [Out] out nint valueSize);
    }
}
