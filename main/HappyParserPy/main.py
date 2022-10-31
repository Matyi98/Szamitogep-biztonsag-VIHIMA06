from data import parse
from bitmap import display

def main():
    with open("samples/1.caff", "rb") as f:
        caff_1 = f.read()
    with open("samples/2.caff", "rb") as f:
        caff_2 = f.read()
    with open("samples/3.caff", "rb") as f:
        caff_3 = f.read()

    caff = parse(caff_1)
    for frame in caff.frames:
        display(frame.ciff, True)

if __name__ == "__main__":
    main()
