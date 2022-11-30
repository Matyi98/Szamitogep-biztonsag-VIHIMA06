from encoder import caff
from os import listdir

input_dir = 'inputs/'

if __name__ == "__main__":
    ins = [input_dir+x for x in listdir(input_dir)]
    out = caff(ins)
    with open('out.caff', "bw") as f:
        f.write(out)
    print('Done!')