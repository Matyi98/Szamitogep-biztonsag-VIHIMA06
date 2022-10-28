#include "CaffParserFacade.h"

// Demo function
extern "C" __declspec(dllexport) int Add(int a, int b) {
	return a + b;
}


CaffCredits ParseMeta(const UCHAR* raw, LONG64 size)
{
	//TODO
	return CaffCredits();
}

const UCHAR* ParsePreview(const UCHAR* raw, LONG64 size)
{
	//TODO
	return nullptr;
}
