#include "CaffParserFacade.h"
#include "ParserCore.h"

// Demo function
extern "C" __declspec(dllexport) int Add(int a, int b) {
	return a + b;
}

// Upload CAFF
CaffCredits ParseMeta(const UCHAR* raw, LONG64 size)
{
	auto ret = parse(raw);
	// transform ret to get CaffCredits
	return CaffCredits();
}

// Preview CAFF
const UCHAR* ParsePreview(const UCHAR* raw, LONG64 size)
{
	//TODO
	return nullptr;
}
