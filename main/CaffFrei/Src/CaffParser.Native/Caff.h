#include <vector>
#include "Common.h"
#ifndef CAFF_H
#define CAFF_H

struct CAFF {
	CaffCredits credits;
	std::vector<CaffFrame> frames;
};

struct CaffFrame
{
    LONG64 duration;
    Ciff ciff;
};

struct Ciff
{
    LONG64 width;
    LONG64 height;
    std::string caption;
    std::vector<std::string> tags;
    std::vector<UCHAR> content;
};


//TODO: Constructor
struct CaffCredits
{
    short YY;
    char M;
    char D;
    char h;
    char m;
    std::string Creator;
};

struct Block
{
    char id;
    LONG64 length;
    ByteReader data;

    Block(char id, LONG64 length, ByteReader data)
    {
        this->id = id;
        this->length = length;
        this->data = data;
    }
};

//Span of byte, call the next method to move one byte.
//Should be used for smaller arrays of bytes
class ByteSpan
{
    const UCHAR* pStart;
    const UCHAR* pEnd; // This points after the last byte
    int offset;

public:
    ByteSpan(const UCHAR* bytes, LONG64 size)
    {
        pStart = bytes;
        pEnd = bytes + size;
        offset = 0;
    }

    UCHAR next()
    {
        if (pStart + offset == pEnd)
        {
            throw std::out_of_range("Next: No more bytes!");
        }

        UCHAR byte = pStart[offset];
        offset++;

        return byte;
    }

    //TODO: This needs to be tested
    LONG64 readLittleEndian()
    {
        LONG64 length = 0;
        /*
        for (int i = 0; i < 8; i++)
        {
            length |= (LONG64)raw[i] << (i * 8);
        }
        */

        for (int i = 7; i >= 0; i--)
        {
            length = length | pStart[i];
            length = length << 8;
        }
    }

};

// ByteReader can track an array of bytes
// Should be used for big arrays of bytes
class ByteReader
{
    const UCHAR* data;
    LONG64 size;
    int offset;

public:
    ByteReader(const UCHAR* data, LONG64 size)  
    {
        this->data = data;
        this->size = size;
        offset = 0;
    }
    
    ByteReader();

    // Copy constructor
    ByteReader(const ByteReader& b)
    {
        data = b.data;
        size = b.size;
        offset = 0;
    }

    // Doesn't move the pointer in the ByteReader
    //TODO: Maybe fix code redundacy here
    ByteSpan read(LONG64 spanSize)
    {
        if (data + offset + spanSize >= data + size)
            throw std::out_of_range("Read: No more bytes");

        ByteSpan span = ByteSpan(data, size);
        return span;
    }

    ByteReader popAndGetBytes(LONG64 spanSize)
    {
        if (data + offset + spanSize >= data + size)
            throw std::out_of_range("Read: No more bytes");

        ByteReader reader = ByteReader(data, spanSize);
        pop(spanSize);
        return reader;
    }

    // Moves the pointer in the ByteReader
    ByteSpan pop(LONG64 spanSize)
    {
        if (data + offset + spanSize >= data + size)
            throw std::out_of_range("Pop: No more bytes");

        ByteSpan span = ByteSpan(data, size);
        offset += size;
        return span;
    }

    bool isFileEnd()
    {
        return data + offset >= data + size;
    }

    /*
    const UCHAR* getCurrentPointer()
    {
        return data + offset;
    }
    */
};

#endif