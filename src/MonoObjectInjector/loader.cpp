
#include <string>
#include <algorithm>
#include <iostream>
#include <fstream>
#include <Process/Process.h>
#include <Process/RPC/RemoteFunction.hpp>
#include <Misc/Utils.h>

bool FileExists(const std::wstring& name) {
  std::ifstream f(name);
  if (f.good()) {
    f.close();
    return true;
  }
  else {
    f.close();
    return false;
  }
}

std::vector<char> FileReadAllBytes(const std::wstring& name) {
  std::ifstream input(name, std::ios::binary);
  if (input.is_open()) {

    std::vector<char> buffer((
      std::istreambuf_iterator<char>(input)),
      (std::istreambuf_iterator<char>()));

    return buffer;
  }
  return std::vector<char>();
}

int ExecuteGetDomain(blackbone::Process& process) {
  typedef int(__cdecl*  mono_domain_get) ();

  auto mono_get_root_domain_address = process.modules().GetExport(process.modules().GetModule(L"mono.dll"), "mono_domain_get");
  if (mono_get_root_domain_address.procAddress == 0) {
    return 0;
  }

  blackbone::RemoteFunction<mono_domain_get> mono_domain_get_function(process, (mono_domain_get)mono_get_root_domain_address.procAddress);

  int domain_result;
  mono_domain_get_function.Call(domain_result, process.threads().getMain());

  return domain_result;
}
int ExecuteImageOpenFromDataFull(blackbone::Process& process, std::vector<char>& data) {
  typedef int(__cdecl* mono_image_open_from_data_full) (int data, unsigned int data_len, int need_copy, int *status, int refonly);

  auto mono_image_open_from_data_full_address = process.modules().GetExport(process.modules().GetModule(L"mono.dll"), "mono_image_open_from_data");
  if (mono_image_open_from_data_full_address.procAddress == 0) {
    return 0;
  }

  auto memblock = process.memory().Allocate(data.size(), PAGE_READWRITE);
  memblock.Write(0, data.size(), data.data());

  int status;
  blackbone::RemoteFunction<mono_image_open_from_data_full> mono_image_open_from_data_full_function(process, (mono_image_open_from_data_full)mono_image_open_from_data_full_address.procAddress, memblock.ptr<int>(), data.size(), 1, &status, 0);

  int image_data_get_result;
  mono_image_open_from_data_full_function.Call(image_data_get_result, process.threads().getMain());

  memblock.Free();

  return image_data_get_result;
}
int ExecuteAssemblyLoadFromFull(blackbone::Process& process, int image) {
  typedef int(__cdecl* mono_assembly_load_from_full) (int image, int *fname, int *status, bool refonly);

  auto mono_assembly_load_from_full_address = process.modules().GetExport(process.modules().GetModule(L"mono.dll"), "mono_assembly_load_from_full");
  if (mono_assembly_load_from_full_address.procAddress == 0) {
    return 0;
  }

  int status;
  int *fname = nullptr;
  bool refonly = false;
  blackbone::RemoteFunction<mono_assembly_load_from_full> mono_assembly_load_from_full_function(process, (mono_assembly_load_from_full)mono_assembly_load_from_full_address.procAddress, image, fname, &status, refonly);

  int assembly;
  mono_assembly_load_from_full_function.Call(assembly, process.threads().getMain());

  return assembly;
}
int ExecuteAssemblyGetImage(blackbone::Process& process, int assembly) {
  typedef int(__cdecl*  mono_assembly_get_image) (int assembly);

  auto mono_assembly_get_image_address = process.modules().GetExport(process.modules().GetModule(L"mono.dll"), "mono_assembly_get_image");
  if (mono_assembly_get_image_address.procAddress == 0) {
    return 0;
  }

  blackbone::RemoteFunction<mono_assembly_get_image> mono_assembly_get_image_function(process, (mono_assembly_get_image)mono_assembly_get_image_address.procAddress, assembly);

  int image;
  mono_assembly_get_image_function.Call(image, process.threads().getMain());

  return image;
}
int ExecuteGetClassFromName(blackbone::Process& process, int image, const char* name_space, const char* name) {
  typedef int(__cdecl*  mono_class_from_name) (int image, const char* name_space, const char *name);

  auto mono_class_from_name_address = process.modules().GetExport(process.modules().GetModule(L"mono.dll"), "mono_class_from_name");
  if (mono_class_from_name_address.procAddress == 0) {
    return 0;
  }

  blackbone::RemoteFunction<mono_class_from_name> mono_class_from_name_function(process, (mono_class_from_name)mono_class_from_name_address.procAddress, image, name_space, name);

  int klass;
  mono_class_from_name_function.Call(klass, process.threads().getMain());

  return klass;
}
int ExecuteGetMethodFromName(blackbone::Process& process, int klass, const char* name) {
  typedef int(__cdecl*  mono_class_get_method_from_name) (int klass, const char *name, int param_count);

  auto mono_class_get_method_from_name_address = process.modules().GetExport(process.modules().GetModule(L"mono.dll"), "mono_class_get_method_from_name");
  if (mono_class_get_method_from_name_address.procAddress == 0) {
    return 0;
  }

  blackbone::RemoteFunction<mono_class_get_method_from_name> mono_class_get_method_from_name_function(process, (mono_class_get_method_from_name)mono_class_get_method_from_name_address.procAddress, klass, name, 0);

  int method;
  mono_class_get_method_from_name_function.Call(method, process.threads().getMain());

  return method;
}
int ExecuteRuntimeInvoke(blackbone::Process& process, int method) {
  typedef int(__cdecl*   mono_runtime_invoke) (int method, void *obj, void **params, int **exc);

  auto mono_runtime_invoke_address = process.modules().GetExport(process.modules().GetModule(L"mono.dll"), "mono_runtime_invoke");
  if (mono_runtime_invoke_address.procAddress == 0) {
    return 0;
  }

  void *obj = nullptr;
  void **params = nullptr;
  int **exc = nullptr;

  blackbone::RemoteFunction<mono_runtime_invoke> mono_runtime_invoke_function(process, (mono_runtime_invoke)mono_runtime_invoke_address.procAddress, method, obj,params,exc);

  int invoke_result;
  mono_runtime_invoke_function.Call(invoke_result, process.threads().getMain());

  return invoke_result;
}

int UseAssembly(std::ofstream& log, blackbone::Process& process, std::wstring dll, std::wstring name_space, std::wstring class_name, std::wstring method_name) {

  log << "ExecuteGetDomain\n";
  int domain = ExecuteGetDomain(process);
  if (!domain) {
    log << "return 1\n";
    return 1;
  }

  log << "FileReadAllBytes\n";
  auto data = FileReadAllBytes(dll);

  if (data.size() == 0) {
    log << "return 2\n";
    return 2;
  }

  log << "ExecuteImageOpenFromDataFull\n";
  int raw_image = ExecuteImageOpenFromDataFull(process, data);

  if (!raw_image) {
    log << "return 3\n";
    return 3;
  }

  log << "ExecuteAssemblyLoadFromFull\n";
  int assembly = ExecuteAssemblyLoadFromFull(process, raw_image);
  if (!assembly) {
    log << "return 4\n";
    return 4;
  }

  log << "ExecuteAssemblyGetImage\n";
  int image = ExecuteAssemblyGetImage(process, assembly);
  if (!assembly) {
    log << "return 5\n";
    return 5;
  }

  log << "ExecuteGetClassFromName\n";
  int klass = ExecuteGetClassFromName(process, image, blackbone::Utils::WstringToUTF8(name_space).c_str(), blackbone::Utils::WstringToUTF8(class_name).c_str());
  if (!klass) {
    log << "return 6\n";
    return 6;
  }

  log << "ExecuteGetMethodFromName\n";
  int method = ExecuteGetMethodFromName(process, klass, blackbone::Utils::WstringToUTF8(method_name).c_str());

  if (!method) {
    log << "return 7\n";
    return 7;
  }

  log << "ExecuteRuntimeInvoke\n";
  ExecuteRuntimeInvoke(process, method);

  log << "return 0\n";
  return 0;
}

int inject(char *dll, char *target, char *name_space, char *classname, char *methodname) {
  const size_t strbuf = 100;

  wchar_t wdll[strbuf];
  wchar_t wtarget[strbuf];
  wchar_t wname_space[strbuf];
  wchar_t wclassname[strbuf];
  wchar_t wmethodname[strbuf];

  size_t convertedChars = 0;
  mbstowcs_s(&convertedChars, wdll, strlen(dll) + 1, dll, _TRUNCATE);
  mbstowcs_s(&convertedChars, wtarget, strlen(target) + 1, target, _TRUNCATE);
  mbstowcs_s(&convertedChars, wname_space, strlen(name_space) + 1, name_space, _TRUNCATE);
  mbstowcs_s(&convertedChars, wclassname, strlen(classname) + 1, classname, _TRUNCATE);
  mbstowcs_s(&convertedChars, wmethodname, strlen(methodname) + 1, methodname, _TRUNCATE);
  
  std::ofstream log;
  log.open("MonoObjectInjector.log");
  log << "inject(\"" 
      << dll << "\", \"" 
      << target << "\", \"" 
      << name_space << "\", \"" 
      << classname << "\", \"" 
      << methodname << "\")\n";

  blackbone::Process target_process;
  std::vector<DWORD> found;

  log << "EnumByName\n";
  blackbone::Process::EnumByName(wtarget, found);

  if (found.size() > 0) {
    log << "Attach\n";
    if (target_process.Attach(found.back()) == STATUS_SUCCESS) {

      log << "GetWow64Barrier\n";
      auto barrier = target_process.core().native()->GetWow64Barrier().type;

      if (barrier != blackbone::wow_32_32 && barrier != blackbone::wow_64_64)
      {
        log << "return 8\n";
        return 8;
      }

      log << "UseAssembly\n";
      bool r = UseAssembly(log, target_process, wdll, wname_space, wclassname, wmethodname);


      log << "return r\n"; 
      log.close();
      return r;
    }
    else {
      log << "return 9\n";
      log.close();
      return 9;
    }
  }
  else {
    log << "return 10\n";
    log.close();
    return 10;
  }
}