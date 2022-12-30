using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPUComputingDotNet
{
    public enum ErrorCode : int
    {
        CL_SUCCESS                                  = 0,
        CL_DEVICE_NOT_FOUND                         = -1,
        CL_DEVICE_NOT_AVAILABLE                     = -2,
        CL_COMPILER_NOT_AVAILABLE                   = -3,
        CL_MEM_OBJECT_ALLOCATION_FAILURE            = -4,
        CL_OUT_OF_RESOURCES                         = -5,
        CL_OUT_OF_HOST_MEMORY                       = -6,
        CL_PROFILING_INFO_NOT_AVAILABLE             = -7,
        CL_MEM_COPY_OVERLAP                         = -8,
        CL_IMAGE_FORMAT_MISMATCH                    = -9,
        CL_IMAGE_FORMAT_NOT_SUPPORTED               = -10,
        CL_BUILD_PROGRAM_FAILURE                    = -11,
        CL_MAP_FAILURE                              = -12,
        CL_MISALIGNED_SUB_BUFFER_OFFSET             = -13,
        CL_EXEC_STATUS_ERROR_FOR_EVENTS_IN_WAIT_LIST= -14,
        CL_COMPILE_PROGRAM_FAILURE                  = -15,
        CL_LINKER_NOT_AVAILABLE                     = -16,
        CL_LINK_PROGRAM_FAILURE                     = -17,
        CL_DEVICE_PARTITION_FAILED                  = -18,
        CL_KERNEL_ARG_INFO_NOT_AVAILABLE            = -19,
        CL_INVALID_VALUE                            = -30,
        CL_INVALID_DEVICE_TYPE                      = -31,
        CL_INVALID_PLATFORM                         = -32,
        CL_INVALID_DEVICE                           = -33,
        CL_INVALID_CONTEXT                          = -34,
        CL_INVALID_QUEUE_PROPERTIES                 = -35,
        CL_INVALID_COMMAND_QUEUE                    = -36,
        CL_INVALID_HOST_PTR                         = -37,
        CL_INVALID_MEM_OBJECT                       = -38,
        CL_INVALID_IMAGE_FORMAT_DESCRIPTOR          = -39,
        CL_INVALID_IMAGE_SIZE                       = -40,
        CL_INVALID_SAMPLER                          = -41,
        CL_INVALID_BINARY                           = -42,
        CL_INVALID_BUILD_OPTIONS                    = -43,
        CL_INVALID_PROGRAM                          = -44,
        CL_INVALID_PROGRAM_EXECUTABLE               = -45,
        CL_INVALID_KERNEL_NAME                      = -46,
        CL_INVALID_KERNEL_DEFINITION                = -47,
        CL_INVALID_KERNEL                           = -48,
        CL_INVALID_ARG_INDEX                        = -49,
        CL_INVALID_ARG_VALUE                        = -50,
        CL_INVALID_ARG_SIZE                         = -51,
        CL_INVALID_KERNEL_ARGS                      = -52,
        CL_INVALID_WORK_DIMENSION                   = -53,
        CL_INVALID_WORK_GROUP_SIZE                  = -54,
        CL_INVALID_WORK_ITEM_SIZE                   = -55,
        CL_INVALID_GLOBAL_OFFSET                    = -56,
        CL_INVALID_EVENT_WAIT_LIST                  = -57,
        CL_INVALID_EVENT                            = -58,
        CL_INVALID_OPERATION                        = -59,
        CL_INVALID_GL_OBJECT                        = -60,
        CL_INVALID_BUFFER_SIZE                      = -61,
        CL_INVALID_MIP_LEVEL                        = -62,
        CL_INVALID_GLOBAL_WORK_SIZE                 = -63,
        CL_INVALID_PROPERTY                         = -64,
        CL_INVALID_IMAGE_DESCRIPTOR                 = -65,
        CL_INVALID_COMPILER_OPTIONS                 = -66,
        CL_INVALID_LINKER_OPTIONS                   = -67,
        CL_INVALID_DEVICE_PARTITION_COUNT           = -68,
        CL_INVALID_PIPE_SIZE                        = -69,
        CL_INVALID_DEVICE_QUEUE                     = -70,
        CL_INVALID_SPEC_ID                          = -71,
        CL_MAX_SIZE_RESTRICTION_EXCEEDED            = -72,
    }
    public enum PlatformInfo : uint
    {
        CL_PLATFORM_PROFILE =                 0x0900,
        CL_PLATFORM_VERSION =                 0x0901,
        CL_PLATFORM_NAME =                    0x0902,
        CL_PLATFORM_VENDOR =                  0x0903,
        CL_PLATFORM_EXTENSIONS =              0x0904,
    }

    [Flags]
    public enum DeviceType : ulong
    {
        DEFAULT = (1 << 0),
        CPU = (1 << 1),
        GPU = (1 << 2),
        ACCELERATOR = (1 << 3),
        ALL = 0xFFFFFFFF,
    };

    public enum DeviceInfo : uint
    {
        CL_DEVICE_TYPE                                   = 0x1000,
        CL_DEVICE_MAX_COMPUTE_UNITS                      = 0x1002,
        CL_DEVICE_MAX_WORK_ITEM_DIMENSIONS               = 0x1003,
        CL_DEVICE_MAX_WORK_GROUP_SIZE                    = 0x1004,
        CL_DEVICE_MAX_WORK_ITEM_SIZES                    = 0x1005,
        CL_DEVICE_MAX_CLOCK_FREQUENCY                    = 0x100C,
        CL_DEVICE_ADDRESS_BITS                           = 0x100D,
        CL_DEVICE_GLOBAL_MEM_SIZE                        = 0x101F,
        CL_DEVICE_MAX_CONSTANT_BUFFER_SIZE               = 0x1020,
        CL_DEVICE_MAX_CONSTANT_ARGS                      = 0x1021,
        CL_DEVICE_LOCAL_MEM_TYPE                         = 0x1022,
        CL_DEVICE_LOCAL_MEM_SIZE                         = 0x1023,
        CL_DEVICE_ENDIAN_LITTLE                          = 0x1026,
        CL_DEVICE_AVAILABLE                              = 0x1027,
        CL_DEVICE_COMPILER_AVAILABLE                     = 0x1028,
        CL_DEVICE_NAME                                   = 0x102B,
        CL_DEVICE_VENDOR                                 = 0x102C,
        CL_DRIVER_VERSION                                = 0x102D,
        CL_DEVICE_PROFILE                                = 0x102E,
        CL_DEVICE_VERSION                                = 0x102F,
        CL_DEVICE_EXTENSIONS                             = 0x1030,
        CL_DEVICE_PLATFORM                               = 0x1031,
    };
}
