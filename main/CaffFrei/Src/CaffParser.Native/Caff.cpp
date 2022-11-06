#include <iostream>
#include "Caff.h"
#include <fstream>

std::string getStringFromBytes(ByteSpan bytes, const LONG64 length)
{
    std::string byteString;

    for (int i = 0; i < length; i++)
        byteString.push_back((char)bytes.next());

    return byteString;
}

std::vector<UCHAR> getVectorString(ByteSpan bytes, const LONG64 length)
{  
    std::vector<UCHAR> byteVector;

    for (int i = 0; i < length; i++)
        byteVector.push_back(bytes.next());

    return byteVector;
}

CAFF::CAFF(CaffCredits credits, std::vector<CaffFrame> frames)
{
    this->credits = credits;
    this->frames = frames;
}

void CAFF::writeToBinary(const char* filename, ByteReader content)
{
    auto myfile = std::fstream(filename, std::ios::out | std::ios::binary);
    myfile.write((char*)&content.getData()[0], content.getSize());
    myfile.close();
}

CaffFrame::CaffFrame(ByteReader block)
{
    ByteSpan durationSpan = block.popAsSpan(8);
    duration = durationSpan.readLittleEndian();

    ciff = Ciff(&block);
}

Ciff::Ciff() 
{
    height = 0;
    width = 0;
}

Ciff::Ciff(ByteReader* bytes)
{
    ByteSpan magic = bytes->popAsSpan(4);
    std::string magicString = getStringFromBytes(magic, 4);
    std::string ciff("CIFF");

    if (ciff.compare(magicString) != 0)
        throw std::invalid_argument("CIFF ctor: Magic string is not CIFF!");

    ByteSpan headersizeSpan = bytes->popAsSpan(8);
    LONG64 headersize = headersizeSpan.readLittleEndian();

    ByteSpan contentsizeSpan = bytes->popAsSpan(8);
    LONG64 contentsize = contentsizeSpan.readLittleEndian();

    ByteSpan widthSpan = bytes->popAsSpan(8);
    width = widthSpan.readLittleEndian();

    ByteSpan heightSpan = bytes->popAsSpan(8);
    height = heightSpan.readLittleEndian();

    if (height * width * 3 != contentsize)
        throw std::invalid_argument("CIFF ctor: Content size is invalid!");

    LONG64 onlyCapTagsSize = headersize - 36;
    ByteSpan capTagsSpan = bytes->popAsSpan(onlyCapTagsSize);
    std::vector<UCHAR> capTags = getVectorString(capTagsSpan, onlyCapTagsSize);

    std::vector<std::string> strings;
    int startIndex = 0;
    for (size_t i = 0; i < capTags.size(); i++)
    {
        if (capTags.at(i) == '\n' || capTags.at(i) == '\0')
        {
            strings.push_back(std::string(capTags.begin() + startIndex, capTags.begin() + i));
            startIndex = i + 1;
        }
    }

    for (size_t i = 0; i < strings.size(); i++)
    {
        if (i == 0)
            caption = strings.at(i);
        else
            tags.push_back(strings.at(i));
    }

    content = bytes->popAsByteReader(contentsize);

    if (bytes->getRemainingSize() != 0)
        throw std::invalid_argument("Ciff ctor: Contentsize doesn't match the byte size!");

}

CaffCredits::CaffCredits() 
{
    YY = 0;
    M = 0;
    D = 0;
    h = 0;
    m = 0;
    Creator = "";
}

CaffCredits::CaffCredits(ByteReader block)
{
    ByteSpan YYSpan = block.popAsSpan(2);
    YY = YYSpan.readLittleEndianTwoBytes();

    ByteSpan dateSpan = block.popAsSpan(4);
    M = dateSpan.next();
    D = dateSpan.next();
    h = dateSpan.next();
    m = dateSpan.next();

    ByteSpan creatorLengthSpan = block.popAsSpan(8);
    LONG64 creatorLength = creatorLengthSpan.readLittleEndian();

    ByteSpan creatorSpan = block.popAsSpan(creatorLength);

    Creator = getStringFromBytes(creatorSpan, creatorLength);

    if (Creator.size() != creatorLength)
        throw std::invalid_argument("CaffCredits ctor: Creator length is invalid!");
}

CaffCredits::CaffCredits(const CaffCredits& c)
{
    YY = c.YY;
    M = c.M;
    D = c.D;
    h = c.h;
    m = c.m;
    Creator = c.Creator;
}

Block::Block(char id, LONG64 length, ByteReader data)
{
    this->id = id;
    this->length = length;
    this->data = data;
}

ByteSpan::ByteSpan(const UCHAR* bytes, LONG64 size)
{
    pStart = bytes;
    pEnd = bytes + size;
    offset = 0;
}

UCHAR ByteSpan::next()
{
    if (pStart + offset == pEnd)
    {
        throw std::out_of_range("Next: No more bytes!");
    }

    UCHAR byte = pStart[offset];
    offset++;

    return byte;
}

LONG64 ByteSpan::readLittleEndian()
{
    LONG64 result = 0;
    result |= pStart[7]; result <<= 8;
    result |= pStart[6]; result <<= 8;
    result |= pStart[5]; result <<= 8;
    result |= pStart[4]; result <<= 8;
    result |= pStart[3]; result <<= 8;
    result |= pStart[2]; result <<= 8;
    result |= pStart[1]; result <<= 8;
    result |= pStart[0];
    return result;
}

short ByteSpan::readLittleEndianTwoBytes()
{
    short result = 0;
    result |= pStart[1]; result <<= 8;
    result |= pStart[0];
    return result;
}

ByteReader::ByteReader(const UCHAR* data, LONG64 size)
{
    this->data = data;
    this->size = size;
    offset = 0;
}

ByteReader::ByteReader() 
{
    data = 0;
    offset = 0;
    size = 0;
}

// Copy constructor
ByteReader::ByteReader(const ByteReader& b)
{
    data = b.data;
    size = b.size;
    offset = 0;
}

ByteSpan ByteReader::read(LONG64 spanSize)
{
    if ((data + offset + spanSize) - 1 >= data + size)
        throw std::out_of_range("Read: No more bytes");

    ByteSpan span = ByteSpan(data + offset, size);
    return span;
}

ByteReader ByteReader::popAsByteReader(LONG64 spanSize)
{
    if ((data + offset + spanSize) - 1 >= data + size)
        throw std::out_of_range("Read: No more bytes");

    ByteReader reader = ByteReader(data + offset, spanSize);
    popAsSpan(spanSize); // We don't need a return value
    return reader;
}

// Moves the pointer in the ByteReader
ByteSpan ByteReader::popAsSpan(LONG64 spanSize)
{
    if ((data + offset + spanSize) - 1 >= data + size)
        throw std::out_of_range("Pop: No more bytes");

    ByteSpan span = ByteSpan(data + offset, size);
    offset += spanSize;
    return span;
}

bool ByteReader::isFileEnd()
{
    return data + offset >= data + size;
}

const UCHAR* ByteReader::getData()
{
    return data;
}

LONG64 ByteReader::getSize()
{
    return size;
}

const LONG64 ByteReader::getRemainingSize()
{
    return size - offset;
}

void CAFF::persist_text(const char* fname) {
    std::ofstream outfile;

    outfile.open(fname, std::ios::out | std::ios::trunc);
    std::cout << "Creator: " << this->credits.Creator << std::endl;
    std::cout << "Creation date: " << +this->credits.YY << "." << +this->credits.M << "." << +this->credits.D << " " << +this->credits.h << ":" << +this->credits.m << std::endl;
    std::cout << "Number of frames: " << this->frames.size() << std::endl;

    outfile.close();
}

#include <sys/types.h>
#include <sys/stat.h>
#include <unistd.h>

void CAFF::persist_all(const char* base) {
    auto folder = std::string(base);

    struct stat st = {0};

    if (stat(folder.c_str(), &st) == -1)
    {
        mkdir(folder.c_str(), 0700);
    }

    auto manifest = folder + "/manifest";
    persist_text(manifest.c_str());
    for (size_t i = 0; i < this->frames.size(); i++)
    {
        auto frame = this->frames[i];
        std::cout << "frame_" << i << ":" << std::endl;
        std::cout << "\tDuration: " << frame.duration << std::endl;
        std::cout << "\tCaption: " << frame.ciff.caption << std::endl;
        std::cout << "\tTags: ";
        for (size_t i = 0; i < frame.ciff.tags.size(); i++)
            std::cout << frame.ciff.tags[i] << "; ";
        std::cout << std::endl;
        std::cout << "\tSize (width*height): " << frame.ciff.width << "*" << frame.ciff.height << std::endl;
        auto ciff_name = folder + "frame_" + std::to_string(i);
        auto &ciff_content = frame.ciff.content;
        this->writeToBinary(ciff_name.c_str(), ciff_content);
    }
}

