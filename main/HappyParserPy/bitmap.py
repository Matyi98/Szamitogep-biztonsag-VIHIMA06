from PIL import Image

def read_content(x, o):
    return x[o], x[o+1], x[o+2]

def display(content, show: bool):
    offset=0
    img = Image.new('RGB', (content.width, content.height), "magenta")
    pixels = img.load()
    for j in range(img.size[1]):
        for i in range(img.size[0]):
            pixels[i, j] = read_content(content.content, offset)
            offset += 3
    if show:
        img.show()
