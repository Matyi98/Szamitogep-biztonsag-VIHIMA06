#include "Main.h"

int main(int argc, char* argv[])
{
    if (argc != 2) {
        std::cerr << "No argument provided! Usage: caffparser <caff_file_name>" << std::endl;
        return 1;
    }
        
    const char* filename = argv[1];

    auto in_file = fopen(filename, "rb");

    if (!in_file)
    {
        std::cerr << "Can't open file" << std::endl;
        return 1;
    }

    struct stat sb {};

    if (stat(filename, &sb) == -1)
    {
        std::cerr << "Can't read file stat" << std::endl;
        return 1;
    }

    long size = sb.st_size;
    UCHAR *file_contents = new UCHAR[size];
    fread(file_contents, size, 1, in_file);

    auto caff = parse(file_contents, size);

    delete[] file_contents;
    return 0;
}
