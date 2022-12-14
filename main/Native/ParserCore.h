#ifndef PARSER_CORE_H
#define PARSER_CORE_H

#include "Caff.h"
#include <iostream>
#include "Common.h"

CAFF parse(const UCHAR *, LONG64);

Block readBlock(ByteReader* byteReader);

LONG64 parseHeader(ByteReader block);

#endif