# OpenCL based Library for GPU computing (C#)
Developed by Willian Yamakawa Souza - Computer Engineering Student

This software is free for use (Under MIT License)

Requires GPU driver with OpenCL support

## Example

```cs
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
OutputArgument outputArg = kernel.SetOutputArgument(2, typeof(float), 5);
kernel.Run(new nint[] { 5 });
float[] arr = kernel.GetOutputArgument<float>(outputArg); //Returns { 2, 4, 6, 8, 10 }
```

## Platform Type

```cs
public struct Platform
```
### Properties
```cs
//Gets platform name string
public string Name { get { } };

/*Returns the profile name supported by the implementation. The profile name returned can be one of the following strings:
FULL_PROFILE - if the implementation supports the OpenCL specification.
EMBEDDED_PROFILE - if the implementation supports the OpenCL embedded profile. The embedded profile is defined
to be a subset for each version of OpenCL.*/
public string Profile { get {  } };

/*Returns the OpenCL version supported by the implementation. This version string has the following format:

OpenCL<space><major_version.minor_version><space><platform-specific information>*/
public string Version { get {  } };

//Gets platform vendor string
public string Vendor { get {  } };

//Returns a space-separated list of extension names supported by the platform. 
//Extensions defined here must be supported by all devices associated with this platform.
public string Extensions { get {  } };
```

### Methods

```cs
//Gets all platforms
public static Platform[] GetAllPlatforms(uint max = 64);
```


## Device Type

```cs
public struct Device
```

### Properties
```cs
//The OpenCL device type
public DeviceType Type { get { } };

//The number of parallel compute cores on the OpenCL device. The minimum value is 1.
public uint MaxComputeUnits { get { } };

//Maximum dimensions that specify the global and local work-item IDs used by
//the data parallel execution model. The minimum value is 3.
public uint MaxWorkItemDimensions { get {  } };

//Maximum number of work-items in a work-group executing a kernel using the
//data parallel execution model. The minimum value is 1.
public nint MaxWorkGroupSize { get {  } };

//Maximum configured clock frequency of the device in MHz.
public uint MaxClockFrequency { get {  } };

//The default compute device address space size specified as an unsigned
//integer value in bits. Currently supported values are 32 or 64 bits.
public uint AddressBits { get {  } };

//Size of global device memory in bytes.
public ulong GlobalMemorySize { get {  } };

//Is [true] if the OpenCL device is a little endian device and [false] otherwise.
public bool IsLittleEndian { get {  } };

//Is [true] if the device is available and [false] if the device is not available.
public bool IsAvailable { get {  } };

//Is [false] if the implementation does not have a compiler 
//available to compile the program source. Is [true] if the 
//compiler is available. This can be [false] for the embededed platform profile only.
public bool IsCompilerAvailable { get {  } };

//The device name string
public string Name { get {  } };

//The device vendor name string
public string VendorName { get {  } };

//OpenCL software driver version string in the form major_number.minor_number.
public string DriverVersion { get {  } };

//OpenCL version string. Returns the OpenCL version supported by the device.
//This version string has the following format:
//OpenCL<space><major_version.minor_version><space><vendor-specific information>
public string Version { get {  } };

/*OpenCL profile string. Returns the profile name supported by the device (see note).
The profile name returned can be one of the following strings:
FULL_PROFILE - if the device supports the OpenCL specification.
EMBEDDED_PROFILE - if the device supports the OpenCL embedded profile.*/
public string Profile { get {  } };

//Returns a space separated list of extension names
public string[] Extensions { get {  } };

//Gets device platform
public Platform Platform { get {  } };

//Returns n entries, where n is the value returned by the query for 
//MaxWorkItemDimensions. The minimum value is (1, 1, 1).
public nint[] MaxWorkItemSizes { get {  } };
```

### Methods

```cs
//Gets all devices
public static Device[] GetAllDevices(Platform platform, DeviceType deviceType = DeviceType.ALL);
public static Device[] GetAllDevices(Platform platform, DeviceType deviceType = DeviceType.ALL);

//Gets the most performant device (Clock * ComputeUnits)
public static Device GetMostPerformantDevice(Device[] devices);
```

## Program Type

```cs
public class Program
```

### Methods
```cs
/*Creates a program
Params:
  device: A valid device
  source: A valid OpenCL code
  compilerError: An appropriate error code. If compilerError is [null], no error code is returned.*/
public static Program CreateProgramWithSource(Device device, string[] source, out string compilerError);
public static Program CreateProgramWithSource(Device device, string source, out string compilerError);
```

## Kernel Type

```cs
public class Kernel
```

### Properties
```cs
//Returns the number of arguments that needs to be set
public uint ArgsCount

//Gets the function name
public string FunctionName
```

### Methods
```cs
//Creates a kernel. The kernelName must be equal to OpenCL code function name
public static Kernel CreateKernel(Program program, string kernelName)

/*Sets function argument value by index (starting from 0 - left to right)
When setting a single value as argument, pass the array with 1 value
Ex: (int value) with value 3, would be passed as (new int[] { 3 })*/
public void SetInputArgument<T>(int argumentIndex, T[] array) where T : struct

//Sets output argument size by index (starting from 0 - left to right).
//If argument is a single value, set size = 1
//Returns: an index for later access to its value
public OutputArgument SetOutputArgument(int argumentIndex, Type type, int size)

//When kernel finish executing, gets the output of argument
//If argument is single value, an array with Length = 1 is returned
//The argument outputArgument must be the one returned by SetOutputArgument
public T[] GetOutputArgument<T>(OutputArgument outputArgument) where T : struct

//Executes the kernel
//WorkSizes.Length cannot be greater than device MaxWorkItemDimensions.
//All arguments must be set before calling this method
public void Run(nint[] workSizes)
```
