#pragma once

#include <string>
#include <comdef.h>
#include "Caff.h"

extern "C" __declspec(dllexport) CaffCredits ParseMeta(const UCHAR* raw, LONG64 size);
extern "C" __declspec(dllexport) const UCHAR* ParsePreview(const UCHAR* raw, LONG64 size);