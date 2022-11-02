#include "ParserCore.h"

CAFF parse(const UCHAR *raw, LONG64 size)
{
    std::vector<CaffFrame>;
    const UCHAR* pEnd = raw + size;

    ByteReader byteReader = ByteReader(raw, size);

    while (!byteReader.isFileEnd())
    {
        Block block = readBlock(byteReader);
        parseHeader(block.data);
        //TODO: call the other parsefiles
    }

    /*
    for (int i = 0; i < size; ++i)
    {
        printf("%02X ", raw[i]);
        if (i % 10 == 0 && i != 0)
        {
            std::cout << std::endl;
        }
    }
    */
    return CAFF();
}

Block readBlock(ByteReader byteReader)
{
    char id = (char) byteReader.pop(1).next();
    LONG64 length = 0;

    ByteSpan span = byteReader.pop(8);
    LONG64 length = span.readLittleEndian();

    ByteReader blockReader = byteReader.popAndGetBytes(length);
    return Block(id, length, blockReader);
}

//TODO:
LONG64 parseHeader(ByteReader bytes) { return 0; }
CaffCredits parseCredits() { return CaffCredits(); }

//TODO: CaffFrame constructor