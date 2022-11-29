from encoder import caff

if __name__ == "__main__":
    ins = ['inputs/1.png', 'inputs/2.png']
    out = caff(ins)
    with open('out.caff', "bw") as f:
        f.write(out)
