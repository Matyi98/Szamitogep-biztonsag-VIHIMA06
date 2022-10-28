#include "CaffParserFacade.h"

// Demo function
extern "C" __declspec(dllexport) int Add(int a, int b) {
	return a + b;
}


CaffCredits ParseMeta(const UCHAR* raw)
{
	return CaffCredits();
}

const UCHAR* ParsePreview(const UCHAR* raw)
{
	return nullptr;
}
