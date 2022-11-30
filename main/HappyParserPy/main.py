from data import parse
from bitmap import display

def main():
    with open("samples/out.caff", "rb") as f:
        caff_bytes = f.read()

    caff = parse(caff_bytes)
    for frame in caff.frames:
        display(frame.ciff, True)

if __name__ == "__main__":
    main()
