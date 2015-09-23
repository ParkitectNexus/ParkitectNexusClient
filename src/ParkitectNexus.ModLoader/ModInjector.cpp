#include "ModInjector.h"

#include <string>
#include <loader.h>

using namespace System::Runtime::InteropServices;

int ParkitectNexus::ModLoader::ModInjector::Inject(System::String ^dllPath, System::String ^nameSpace,
  System::String ^className, System::String ^methodName)
{
  IntPtr p1 = Marshal::StringToHGlobalAnsi(dllPath);
  IntPtr p2 = Marshal::StringToHGlobalAnsi(nameSpace);
  IntPtr p3 = Marshal::StringToHGlobalAnsi(className);
  IntPtr p4 = Marshal::StringToHGlobalAnsi(methodName);

  char* s1 = static_cast<char*>(p1.ToPointer());
  char* s2 = static_cast<char*>(p2.ToPointer());
  char* s3 = static_cast<char*>(p3.ToPointer());
  char* s4 = static_cast<char*>(p4.ToPointer());

  int result = inject(s1, "Parkitect.exe", s2, s3, s4);

  Marshal::FreeHGlobal(p1);
  Marshal::FreeHGlobal(p2);
  Marshal::FreeHGlobal(p3);
  Marshal::FreeHGlobal(p4);

  return result;
}