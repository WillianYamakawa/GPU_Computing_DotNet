using System;
using System.Runtime.InteropServices;


namespace GPUComputingDotNet
{
    internal class Binding
    {
        public const string Library = "opencl.dll";

        public static void CheckApiError(ErrorCode error)
        {
            if(error != ErrorCode.CL_SUCCESS) { throw new OpenCLAPIException(error); }
        }

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
        public static extern _Program clCreateProgramWithSource(Context context,
                                                               uint count,
                                                               [In][MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr, SizeParamIndex = 1)] string[] strings,
                                                               IntPtr lengths, //Always NULL because strings are null terminated
                                                               [Out] out ErrorCode error);

        //cl_int clBuildProgram(cl_program program,cl_uint num_devices,const cl_device_id* device_list,const char* options,void (* pfn_notify) (cl_program, void* user_data),void* user_data)
        [DllImport(Library)]
        public static extern ErrorCode clBuildProgram(_Program program,
                                                      uint numDevices, //Always 1
                                                      [In] ref Device device,
                                                      [In][MarshalAs(UnmanagedType.LPStr)] string options,
                                                      IntPtr callback, //Always null
                                                      IntPtr userData); //Always null


        //cl_int clGetProgramBuildInfo(cl_program program,cl_device_id device,cl_program_build_info param_name,size_t param_value_size,void* param_value,size_t* param_value_size_ret)
        [DllImport(Library)]
        public static extern ErrorCode clGetProgramBuildInfo(_Program program,
                                                             Device device,
                                                             ProgramBuildInfo info,
                                                             nint maxSize,
                                                             IntPtr value,
                                                             [Out] out nint valueSize);

        //cl_kernel clCreateKernel(cl_program program,const char* kernel_name,cl_int *errcode_ret)
        [DllImport(Library)]
        public static extern _Kernel clCreateKernel(_Program program,
                                                   [In][MarshalAs(UnmanagedType.LPStr)] string kernelName,
                                                   [Out] out ErrorCode error);


        //cl_int clGetKernelInfo(cl_kernel kernel,cl_kernel_info param_name,size_t param_value_size,void* param_value,size_t* param_value_size_ret)
        [DllImport(Library)]
        public static extern ErrorCode clGetKernelInfo(_Kernel platform,
                                                       KernelInfo info,
                                                       nint valueSizeMax,
                                                       IntPtr value,
                                                       [Out] out nint valueSize);

        //cl_mem clCreateBuffer(cl_context context,cl_mem_flags flags,size_t size,void* host_ptr,cl_int* errcode_ret)
        [DllImport(Library)]
        public static extern Mem clCreateBuffer(Context context,
                                                MemFlags flags,
                                                nint size,
                                                IntPtr hostPtr, //Always null
                                                [Out] out ErrorCode error);

        //cl_int clEnqueueWriteBuffer(cl_command_queue command_queue,cl_mem buffer,cl_bool blocking_write,size_t offset,size_t cb,const void* ptr,cl_uint num_events_in_wait_list,const cl_event* event_wait_list,cl_event *event)
        [DllImport(Library)]
        public static extern ErrorCode clEnqueueWriteBuffer(CommandQueue queue,
                                                            Mem buffer,
                                                            CLBool blocking, //Always true
                                                            nint offset,
                                                            nint size,
                                                            IntPtr pointer,
                                                            uint numEvents, //Always 0
                                                            IntPtr eventsList, //Always NULL
                                                            IntPtr _event); //Always Null


        //cl_int clSetKernelArg(cl_kernel kernel,cl_uint arg_index,size_t arg_size,const void* arg_value)
        [DllImport(Library)]
        public static extern ErrorCode clSetKernelArg(_Kernel kernel,
                                                      uint argIndex,
                                                      nint agrSize, //Always sizeof(Mem)
                                                      [In, Out] ref Mem memory);


        //cl_int clEnqueueNDRangeKernel(cl_command_queue command_queue,cl_kernel kernel,cl_uint work_dim,
        //const size_t* global_work_offset,const size_t* global_work_size,const size_t* local_work_size,cl_uint num_events_in_wait_list,
        //const cl_event* event_wait_list,cl_event *event)
        [DllImport(Library)]
        public static extern ErrorCode clEnqueueNDRangeKernel(CommandQueue queue,
                                                              _Kernel kernel,
                                                              uint workDim,
                                                              IntPtr globalWorkOffset, //Always NULL
                                                              [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] IntPtr[] globalWorkSize,
                                                              [In][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] IntPtr[] localWorkSize,
                                                              uint numEvents, //Always 0
                                                              IntPtr eventsList, //Always NULL
                                                              IntPtr _event); //Always null

        //cl_int clEnqueueReadBuffer(cl_command_queue command_queue,cl_mem buffer,cl_bool blocking_read,size_t offset,size_t cb,void* ptr,
        //cl_uint num_events_in_wait_list,const cl_event* event_wait_list,cl_event *event)
        [DllImport(Library)]
        public static extern ErrorCode clEnqueueReadBuffer(CommandQueue queue, 
                                                           Mem buffer,
                                                           CLBool blocking,
                                                           nint offset,
                                                           nint size,
                                                           IntPtr pointer,
                                                           uint numEvents, //Always 0
                                                           IntPtr eventsList, //Always NULL
                                                           IntPtr _event); //Always null

        //cl_int clFinish(cl_command_queue command_queue)
        [DllImport(Library)]
        public static extern ErrorCode clFinish(CommandQueue queue);


        [DllImport(Library)]
        public static extern ErrorCode clReleaseMemObject(Mem mem);

        [DllImport(Library)]
        public static extern ErrorCode clReleaseKernel(_Kernel kernel);

        [DllImport(Library)]
        public static extern ErrorCode clReleaseProgram(_Program program);

        [DllImport(Library)]
        public static extern ErrorCode clReleaseCommandQueue(CommandQueue commandQueue);

        [DllImport(Library)]
        public static extern ErrorCode clReleaseContext(Context context);


    }
}
