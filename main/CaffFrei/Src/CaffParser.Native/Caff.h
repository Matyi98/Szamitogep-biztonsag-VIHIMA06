#include <vector>
#include "Common.h"
#ifndef CAFF_H
#define CAFF_H

struct CAFF {
	CaffCredits credits;
	std::vector<CaffFrame> frames;

    CAFF(CaffCredits credits, std::vector<CaffFrame> frames)
    {
        this->credits = credits;
        this->frames = frames;
    }
};

struct CaffFrame
{
    LONG64 duration;
    Ciff ciff;

    CaffFrame(ByteReader block)
    {
        ByteSpan durationSpan = block.popAsSpan(8);
        duration = durationSpan.readLittleEndian();

        ciff = Ciff(block);
        
    }
};

struct Ciff
{
    LONG64 width;
    LONG64 height;
    std::string caption;
    std::vector<std::string> tags;
    ByteReader content;

    Ciff();

    Ciff(ByteReader bytes)
    {
        ByteSpan magic = bytes.popAsSpan(4);
        std::string magicString = getStringFromBytes(magic, 4);
        std::string ciff("CIFF");

        if (ciff.compare(magicString) != 0)
            throw std::invalid_argument("CIFF ctor: Magic string is not CIFF!");
    
        ByteSpan headersizeSpan = bytes.popAsSpan(8);
        LONG64 headersize = headersizeSpan.readLittleEndian();

        ByteSpan contentsizeSpan = bytes.popAsSpan(8);
        LONG64 contentsize = contentsizeSpan.readLittleEndian();

        ByteSpan widthSpan = bytes.popAsSpan(8);
        width = widthSpan.readLittleEndian();

        ByteSpan heightSpan = bytes.popAsSpan(8);
        height = heightSpan.readLittleEndian();

        if (height * width * 3 != contentsize)
            throw std::invalid_argument("CIFF ctor: Content size is invalid!");
        
        ByteSpan capTagsSpan = bytes.popAsSpan(headersize);

        std::string capTags = getStringFromBytes(capTagsSpan, headersize);

        //TODO: CHECK THIS
        std::string delimEnter = "\n";
        std::string delimZero = "\0";
        auto start = 0U;
        auto end = capTags.find(delimEnter);
        bool isTags = false;
        while (end != std::string::npos)
        {
            if (!isTags)
            {
                caption = capTags.substr(start, end - start);
                isTags = true;
                start = end + delimEnter.length();
                end = capTags.find(delimZero, start);
            }
            else
            {
                tags.push_back(capTags.substr(start, end - start));
                start = end + delimZero.length();
                end = capTags.find(delimZero, start);
            }
        }

        //TODO: Check if contentsize is the same as content's size
        content = bytes.popAsByteReader(contentsize);

    }
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

    CaffCredits(ByteReader block)
    {
        ByteSpan YYSpan = block.popAsSpan(2);
        YY = (short) YYSpan.readLittleEndian();

        ByteSpan dateSpan = block.popAsSpan(4);
        M = dateSpan.next();
        D = dateSpan.next();
        h = dateSpan.next();
        m = dateSpan.next();

        ByteSpan creatorLengthSpan = block.popAsSpan(8);
        LONG64 creatorLength = creatorLengthSpan.readLittleEndian();

        ByteSpan creatorSpan = block.popAsSpan(creatorLength);

        Creator = getStringFromBytes(creatorSpan, creatorLength);

        if(Creator.size() != creatorLength)
            throw std::invalid_argument("CaffCredits ctor: Creator length is invalid!");
    }

    // Copy constructor
    CaffCredits(const CaffCredits& c)
    {
        YY = c.YY;
        M = c.M;
        D = c.D;
        h = c.h;
        m = c.m;
        Creator = c.Creator;
    }
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

    ByteReader popAsByteReader(LONG64 spanSize)
    {
        if (data + offset + spanSize >= data + size)
            throw std::out_of_range("Read: No more bytes");

        ByteReader reader = ByteReader(data, spanSize);
        popAsSpan(spanSize); // We don't need a return value
        return reader;
    }

    // Moves the pointer in the ByteReader
    ByteSpan popAsSpan(LONG64 spanSize)
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