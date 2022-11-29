from PIL import Image

def _ciff_encode(fname: str) -> bytes:
    image = Image.open(fname, mode='r')
    content = []
    h = image.height
    w = image.width
    for j in range(h):
        for i in range(w):
            p = image.getpixel((i, j))
            content.extend(p)
    content = bytes(content)
    ret = _ciff(content, h, w)
    ret = _ciff_frame(ret)
    return ret


def _ciff(content: bytes, h: int, w: int) -> bytes:
    magic = 'CIFF'.encode('ASCII')
    caption = 'Generated\n'.encode('ASCII')
    tags = '\0'.encode('ASCII')
    _header_size = 4+8+8+8+8+len(caption)+len(tags)
    header_size = int.to_bytes(_header_size, 8, 'little')
    content_size = int.to_bytes(len(content), 8, 'little')
    width = int.to_bytes(w, 8, 'little')
    height = int.to_bytes(h, 8, 'little')
    content = content
    return magic+header_size+content_size+width+height+caption+tags+content


def _ciff_frame(ciff: bytes) -> bytes:
    duration = int.to_bytes(1000, 8, 'little')
    return duration+ciff


def _caff_header(num_anim: int) -> bytes:
    magic = 'CAFF'.encode('ASCII')
    header_size = int.to_bytes(20, 8, 'little')
    num_anim = int.to_bytes(num_anim, 8, 'little')
    return magic+header_size+num_anim

def _caff_credits() -> bytes:
    YY = int.to_bytes(2022, 2, 'little')
    M = int.to_bytes(11, 1, 'little')
    D = int.to_bytes(29, 1, 'little')
    h = int.to_bytes(23, 1, 'little')
    m = int.to_bytes(59, 1, 'little')
    creator_length = int.to_bytes(len('EncoderPy'), 8, 'little')
    creator = 'EncoderPy'.encode('ASCII')
    return YY+M+D+h+m+creator_length+creator


def _block(type: int, data: bytes) -> bytes:
    id = int.to_bytes(type, 1, 'little')
    length = int.to_bytes(len(data), 8, 'little')
    return id+length+data

def _caff(ciffs: list[bytes]):
    ret = _block(1, _caff_header(len(ciffs)))
    ret += _block(2, _caff_credits())
    for ciff in ciffs:
        ret += _block(3, ciff)
    return ret
    
def caff(ins: list[str]) -> bytes:
    ciffs = []
    for i in ins:
        ciffs.append(_ciff_encode(i))
    return _caff(ciffs)
