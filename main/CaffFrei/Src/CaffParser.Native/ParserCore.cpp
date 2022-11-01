#include "ParserCore.h"

CAFF parse(const UCHAR* raw, long size) {
    for (int i = 0; i < size; ++i)
    {
        printf("%02X ", raw[i]);
        if (i % 10 == 0 && i != 0)
        {
            std::cout << std::endl;
        }
    }
    return CAFF();
}