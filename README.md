# OpenCL based Library for GPU computing

## Platform Type

```cs
public struct Platform
```
### Properties
```cs
//Gets platform's name string
public string Name { get { ... } }

/*Returns the profile name supported by the implementation. The profile name returned can be one of the following strings:
FULL_PROFILE - if the implementation supports the OpenCL specification (functionality defined as part of the core specification and does not require any extensions to be supported).
EMBEDDED_PROFILE - if the implementation supports the OpenCL embedded profile. The embedded profile is defined to be a subset for each version of OpenCL.*/
public string Profile { get { ... } }

/*Returns the OpenCL version supported by the implementation. This version string has the following format:

OpenCL<space><major_version.minor_version><space><platform-specific information>*/
public string Version { get { ... } }

//Gets platform vendor string
public string Vendor { get { ... } }

//Returns a space-separated list of extension names (the extension names themselves do not contain any spaces) supported by the platform. 
//Extensions defined here must be supported by all devices associated with this platform.
public string Extensions { get { ... } }
```

