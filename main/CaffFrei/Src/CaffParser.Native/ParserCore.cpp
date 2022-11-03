#include "ParserCore.h"

CAFF parse(const UCHAR *raw, LONG64 size)
{
    std::vector<CaffFrame> frames;

    ByteReader byteReader = ByteReader(raw, size);
    LONG64 numAnim;
    CaffCredits credits;

    while (!byteReader.isFileEnd())
    {
        Block block = readBlock(byteReader);

        if (block.id == 1)
            numAnim = parseHeader(block.data);

        else if (block.id == 2)
            CaffCredits credits = CaffCredits(block.data);

        else if (block.id == 3)
            frames.push_back(CaffFrame(block.data));

        //TODO: call the other parsefiles
    }
    if (numAnim != frames.size())
        throw std::invalid_argument("Parse: Animation size is invalid!");

    return CAFF(credits, frames);
}

Block readBlock(ByteReader byteReader)
{
    char id = (char) byteReader.popAsSpan(1).next();

    ByteSpan span = byteReader.popAsSpan(8);
    LONG64 length = span.readLittleEndian();

    ByteReader blockReader = byteReader.popAsByteReader(length);
    return Block(id, length, blockReader);
}

//TODO:
LONG64 parseHeader(ByteReader block) 
{ 
    ByteSpan magic = block.popAsSpan(4);

    //TODO: Rewrire with function
    char magicArray[4+1];
    for (int i = 0; i < sizeof(magicArray); i++)
    {
        magicArray[i] = magic.next();
    }
    magicArray[4 + 1] = '\0';
    std::string magicString2(magicArray, sizeof(magicArray));
    std::string caff("CAFF");

    if (caff.compare(magicString2) != 0)
        throw std::invalid_argument("parseHeader: Magic string is not CAFF!");

    ByteSpan headerSpan = block.popAsSpan(8);
    LONG64 headerSize = headerSpan.readLittleEndian();

    if (headerSize != 20)
        throw std::invalid_argument("parseHeader: Header size is not 20 bytes!");

    ByteSpan picturesSizeSpan = block.popAsSpan(8);
    LONG64 pictureSize = picturesSizeSpan.readLittleEndian();

    return pictureSize;
}
CaffCredits parseCredits() { return CaffCredits(); }

//TODO: CaffFrame constructor