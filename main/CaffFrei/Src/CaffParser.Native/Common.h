#ifndef COMMON_H
#define COMMON_H

typedef unsigned char UCHAR;
typedef long long LONG64;

std::string getStringFromBytes(ByteSpan bytes, const LONG64 length)
{
    std::vector<UCHAR> byteVector;

    for (int i = 0; i < length; i++)
        byteVector.push_back(bytes.next());

    //byteVector.push_back('\0');

    return std::string(byteVector.begin(), byteVector.end());
}

#endif