from encoder import caff
from os import listdir

input_dir = 'inputs/'
creator = 'TBoyUltak'

if __name__ == "__main__":
    ins = [input_dir+x for x in listdir(input_dir)]
    out = caff(ins, creator)
    with open('out.caff', "bw") as f:
        f.write(out)
    print('Done!')
