from dataclasses import dataclass

@dataclass
class CaffCredits:
    def __init__(self, block_data: bytes) -> None:
        self.YY = int.from_bytes(block_data[0:2], 'little')
        self.M  = block_data[2]
        self.D  = block_data[3]
        self.h  = block_data[4]
        self.m  = block_data[5]
        creator_length = int.from_bytes(block_data[6:14], 'little')
        # len creator must be == creator_length
        self.creator = block_data[13:].decode('ASCII')

@dataclass
class CaffFrame:
    def __init__(self, block_data: bytes) -> None:
        self.duration = int.from_bytes(block_data[0:8], 'little')
        self.ciff = Ciff(block_data[8:])

@dataclass
class Caff:
    credits: CaffCredits
    frames: list[CaffFrame]

@dataclass
class Ciff:
    def __init__(self, ciff_bytes: bytes) -> None:
        magic = ciff_bytes[0:4].decode('ASCII')  # must be the string: CIFF
        header_size = int.from_bytes(ciff_bytes[4:12], 'little')
        content_size = int.from_bytes(ciff_bytes[12:20], 'little')
        self.width = int.from_bytes(ciff_bytes[20:28], 'little')
        self.height = int.from_bytes(ciff_bytes[28:36], 'little')
        # height * width * 3 == content_size
        cap_tags = ciff_bytes[36:header_size]

        self.caption = cap_tags.split(b'\n')[0].decode('ASCII')
        self.tags = [x.decode('ASCII') for x in cap_tags.split(b'\n')[1].split(b'\x00') if x]
        self.content = ciff_bytes[header_size:]  # len must be content_size

@dataclass
class Block:
    id: int
    length: int
    data: bytes


def read_block(raw: bytes) -> tuple[bytes, Block]:
    id = raw[0]
    length = int.from_bytes(raw[1:9], 'little')
    data = raw[9:9+length]
    return raw[9+length:], Block(id, length, data)


def parse_header(block_data: bytes) -> int:
    magic = block_data[0:4].decode('ASCII')  # must be the string: CAFF
    header_size = int.from_bytes(block_data[4:12], 'little') # should be 20=4+8+8 every time
    num_anim = int.from_bytes(block_data[12:20], 'little')
    return num_anim


def parse(raw: bytes) -> Caff:
    frames = []
    while raw != b'':
        raw, block = read_block(raw)
        # validate that only the first block is a header.
        if block.id == 1:
            num_anim = parse_header(block.data)
        # validate that there are only one credits
        elif block.id == 2:
            credits = CaffCredits(block.data)            
        elif block.id == 3:            
            frames.append(CaffFrame(block.data))
    # validate num_anim == len frames
    return Caff(credits, frames)
