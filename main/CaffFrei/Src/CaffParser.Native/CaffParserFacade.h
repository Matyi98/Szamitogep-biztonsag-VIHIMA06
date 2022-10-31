#pragma once

#include <string>
#include <comdef.h>

class CaffCredits
{
    short YY;
    char M;
    char D;
    char h;
    char m;
    BSTR Creator;
};

extern "C" __declspec(dllexport) CaffCredits ParseMeta(const UCHAR* raw, LONG64 size);
extern "C" __declspec(dllexport) const UCHAR* ParsePreview(const UCHAR* raw, LONG64 size);