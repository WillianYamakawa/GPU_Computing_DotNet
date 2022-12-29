#include <CL/cl.h>
#include <cassert>
#include <iostream>
#include <fstream>

static std::string read_file(const char* fileName) {
	std::fstream f;
	f.open(fileName, std::ios_base::in);
	assert(f.is_open());

	std::string res;
	while (!f.eof()) {
		char c;
		f.get(c);
		res += c;
	}
	
	f.close();

	return std::move(res);
}

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

	cl_int contextResult;
	cl_context context = clCreateContext(nullptr, 1, &device, nullptr, nullptr, &contextResult);
	assert(contextResult == CL_SUCCESS);

	cl_int commandQueueResult;
	cl_command_queue queue = clCreateCommandQueue(context, device, 0, &commandQueueResult);
	assert(commandQueueResult == CL_SUCCESS);

	std::string s = read_file("C:\\Users\\willi\\Desktop\\vec.txt");
	const char* programSource = s.c_str();
	size_t length = 0;
	cl_int programResult;
	cl_program program = clCreateProgramWithSource(context, 1, &programSource, &length, &programResult);
	assert(programResult == CL_SUCCESS);

	cl_int programBuildResult = clBuildProgram(program, 1, &device, "", nullptr, nullptr);
	if (programBuildResult != CL_SUCCESS) {
		char log[256];
		size_t logLength;
		cl_int programBuildInfoResult = clGetProgramBuildInfo(program, device, CL_PROGRAM_BUILD_LOG, 256, log, &logLength);
		assert(programBuildInfoResult == CL_SUCCESS);
		std::cout << log << std::endl;
		assert(log);
	}

	cl_int kernelResult;
	cl_kernel kernel = clCreateKernel(program, "vector_sum", &kernelResult);
	assert(kernelResult == CL_SUCCESS);

	float vecaData[256];
	float vecbData[256];

	for (int i = 0; i < 256; ++i) {
		vecaData[i] = (float)(i * i);
		vecbData[i] = (float)i;
	}

	cl_int vecaResult;
	cl_mem veca = clCreateBuffer(context, CL_MEM_READ_ONLY, 256 * sizeof(float), nullptr, &vecaResult);
	assert(vecaResult == CL_SUCCESS);

	cl_int enqueueVecaResult = clEnqueueWriteBuffer(queue, veca, CL_TRUE, 0, 256 * sizeof(float), vecaData, 0, nullptr, nullptr);
	assert(enqueueVecaResult == CL_SUCCESS);

	cl_int vecbResult;
	cl_mem vecb = clCreateBuffer(context, CL_MEM_READ_ONLY, 256 * sizeof(float), nullptr, &vecbResult);
	assert(vecbResult == CL_SUCCESS);

	cl_int enqueueVecbResult = clEnqueueWriteBuffer(queue, vecb, CL_TRUE, 0, 256 * sizeof(float), vecbData, 0, nullptr, nullptr);
	assert(enqueueVecbResult == CL_SUCCESS);

	cl_int veccResult;
	cl_mem vecc = clCreateBuffer(context, CL_MEM_WRITE_ONLY, 256 * sizeof(float), nullptr, &veccResult);
	assert(veccResult == CL_SUCCESS);

	cl_int kernelArgaResult = clSetKernelArg(kernel, 0, sizeof(cl_mem), &veca);
	assert(kernelArgaResult == CL_SUCCESS);
	cl_int kernelArgbResult = clSetKernelArg(kernel, 1, sizeof(cl_mem), &vecb);
	assert(kernelArgaResult == CL_SUCCESS);
	cl_int kernelArgcResult = clSetKernelArg(kernel, 2, sizeof(cl_mem), &vecc);
	assert(kernelArgaResult == CL_SUCCESS);

	size_t globalWorkSize = 256;
	size_t localWorkSize = 64;
	cl_int enqueueKernelResult = clEnqueueNDRangeKernel(queue, kernel, 1, 0, &globalWorkSize, &localWorkSize, 0, nullptr, nullptr);
	assert(enqueueKernelResult == CL_SUCCESS);

	float veccData[256];
	cl_int enqueueReadBufferResult = clEnqueueReadBuffer(queue, vecc, CL_TRUE, 0, 256 * sizeof(float), veccData, 0, nullptr, nullptr);
	assert(enqueueReadBufferResult == CL_SUCCESS);

	clFinish(queue);

	std::cout << "Result: ";
	for (int i = 0; i < 256; ++i) {
		std::cout << veccData[i] << std::endl;
	}

	clReleaseMemObject(veca);
	clReleaseMemObject(vecb);
	clReleaseMemObject(vecc);
	clReleaseKernel(kernel);
	clReleaseProgram(program);
	clReleaseCommandQueue(queue);
	clReleaseContext(context);
	clReleaseDevice(device);
	

	return 0;
}