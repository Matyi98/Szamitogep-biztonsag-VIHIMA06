#include <vector>
#include <string>
#include <stdexcept>
#include "Common.h"
#ifndef CAFF_H
#define CAFF_H

//Span of byte, call the next method to move one byte.
//Should be used for smaller arrays of bytes
class ByteSpan
{
    const UCHAR* pStart;
    const UCHAR* pEnd; // This points after the last byte
    int offset;

public:
    ByteSpan(const UCHAR* bytes, LONG64 size);

    UCHAR next();

    //TODO: This needs to be tested
    LONG64 readLittleEndian();

};

// ByteReader can track an array of bytes
// Should be used for big arrays of bytes
class ByteReader
{
    const UCHAR* data;
    LONG64 size;
    int offset;

public:
    ByteReader(const UCHAR* data, LONG64 size);

    ByteReader();

    // Copy constructor
    ByteReader(const ByteReader& b);

    // Doesn't move the pointer in the ByteReader
    //TODO: Maybe fix code redundacy here
    ByteSpan read(LONG64 spanSize);

    ByteReader popAsByteReader(LONG64 spanSize);

    // Moves the pointer in the ByteReader
    ByteSpan popAsSpan(LONG64 spanSize);

    bool isFileEnd();

    const UCHAR* getData();

    LONG64 getSize();

    /*
    const UCHAR* getCurrentPointer()
    {
        return data + offset;
    }
    */
};

struct Ciff
{
    LONG64 width;
    LONG64 height;
    std::string caption;
    std::vector<std::string> tags;
    ByteReader content;

    Ciff();

    Ciff(ByteReader* bytes);
};

struct CaffFrame
{
    LONG64 duration;
    Ciff ciff;

    CaffFrame(ByteReader block);
};

struct CaffCredits
{
    short YY;
    char M;
    char D;
    char h;
    char m;
    std::string Creator;

    CaffCredits();

    CaffCredits(ByteReader block);

    // Copy constructor
    CaffCredits(const CaffCredits& c);
};

struct CAFF {
	CaffCredits credits;
	std::vector<CaffFrame> frames;

    CAFF(CaffCredits credits, std::vector<CaffFrame> frames);
};

struct Block
{
    char id;
    LONG64 length;
    ByteReader data;

    Block(char id, LONG64 length, ByteReader data);
};

std::string getStringFromBytes(ByteSpan bytes, const LONG64 length);
std::vector<UCHAR> getVectorString(ByteSpan bytes, const LONG64 length);

#endif