# OpenCL based Library for GPU computing

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




