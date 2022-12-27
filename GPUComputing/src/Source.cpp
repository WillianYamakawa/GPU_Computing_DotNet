#include <CL/cl.h>
#include <cassert>
#include <iostream>

int main() {
	cl_platform_id platforms[64];
	cl_uint numPlatform;
	int result = clGetPlatformIDs(64, platforms, &numPlatform);
	assert(result == CL_SUCCESS);

	cl_device_id device = nullptr;
	for (int i = 0; i < numPlatform && device == nullptr; ++i) {
		cl_device_id devices[64];
		unsigned int deviceCount;
		cl_int deviceResult = clGetDeviceIDs(platforms[i], CL_DEVICE_TYPE_GPU, 64, devices, &deviceCount);

		if (deviceResult == CL_SUCCESS) {
			for (int j = 0; j < deviceCount; ++j) {
				char vendorName[256];
				size_t vendorNameLength;
				cl_int deviceInfoResult = clGetDeviceInfo(devices[j], CL_DEVICE_VENDOR, 256, vendorName, &vendorNameLength);
				if (deviceInfoResult == CL_SUCCESS && std::string(vendorName).substr(0, vendorNameLength) == "NVIDIA Corporation") {
					device = devices[j];
					break;
				}
			}
		}
	}

	assert(device != nullptr);

	cl_int clContextResult;
	cl_context context = clCreateContext(nullptr, 1, &device, nullptr, nullptr, &clContextResult);
	assert(clContextResult == CL_SUCCESS);

	cl_int commandQueueResult;
	cl_command_queue queue = clCreateCommandQueue(context, device, 0, &commandQueueResult);
	assert(commandQueueResult == CL_SUCCESS);

	const char* programSource = "";
	size_t length;
	cl_int createProgramResult;
	cl_program program = clCreateProgramWithSource(context, 1, &programSource, &length, &createProgramResult);
	assert(createProgramResult == CL_SUCCESS);

	cl_int programBuildResult = clBuildProgram(program, 1, &device, "", nullptr, nullptr);



	

	return 0;
}