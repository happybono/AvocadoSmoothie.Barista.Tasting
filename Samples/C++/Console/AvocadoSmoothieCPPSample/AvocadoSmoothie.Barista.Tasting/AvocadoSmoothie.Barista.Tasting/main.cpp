#include "pch.h"
#include "AvocadoSmoothie.Barista.Tasting.h"

using namespace System;

int main(array<System::String ^> ^args)
{
	Tasting::Run();
	Console::WriteLine("Press Enter to exit.");
	Console::ReadLine();
	return 0;
}