#pragma once

#include <string>

using namespace System;

namespace ParkitectNexus {
    namespace ModLoader {

        public ref class ModInjector
        {
        public:
            static int Inject(System::String ^logPath, System::String ^dllPath, 
                System::String ^nameSpace, System::String ^className, 
                System::String ^methodName);
        };
    }
}