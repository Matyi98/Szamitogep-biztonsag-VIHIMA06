#include <fstream>
#include "ParserCore.h"

CAFF parse(const UCHAR *raw, LONG64 size)
{
    std::vector<CaffFrame> frames;

    ByteReader byteReader = ByteReader(raw, size);
    LONG64 numAnim = 0;
    CaffCredits credits;

    while (!byteReader.isFileEnd())
    {
        Block block = readBlock(&byteReader);

        if (block.id == 1)
            numAnim = parseHeader(block.data);

        else if (block.id == 2)
            credits = CaffCredits(block.data);

        else if (block.id == 3)
            frames.push_back(CaffFrame(block.data));
    }
    if (numAnim != frames.size())
        throw std::invalid_argument("Parse: Animation size is invalid!");

    /*
    * With this test code, you can write a binary file to CaffFrei.Web folder with the first frame in it.
    auto myfile = std::fstream("file.binary", std::ios::out | std::ios::binary);
    myfile.write((char*)&frames.at(0).ciff.content.getData()[0], frames.at(0).ciff.content.getSize());
    myfile.close();
    */

    return CAFF(credits, frames);
}

Block readBlock(ByteReader* byteReader)
{
    char id = (char) byteReader->popAsSpan(1).next();

    ByteSpan span = byteReader->popAsSpan(8);
    LONG64 length = span.readLittleEndian();

    ByteReader blockReader = byteReader->popAsByteReader(length);
    return Block(id, length, blockReader);
}

LONG64 parseHeader(ByteReader block) 
{ 
    ByteSpan magic = block.popAsSpan(4);
    std::string magicString = getStringFromBytes(magic, 4);
    std::string caff("CAFF");

    if (caff.compare(magicString) != 0)
        throw std::invalid_argument("parseHeader: Magic string is not CAFF!");

    ByteSpan headerSpan = block.popAsSpan(8);
    LONG64 headerSize = headerSpan.readLittleEndian();

    if (headerSize != 20)
        throw std::invalid_argument("parseHeader: Header size is not 20 bytes!");

    ByteSpan picturesSizeSpan = block.popAsSpan(8);
    LONG64 pictureSize = picturesSizeSpan.readLittleEndian();

    return pictureSize;
}