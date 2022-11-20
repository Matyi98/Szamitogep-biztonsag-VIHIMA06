#ifdef _MSC_VER
#define _CRT_SECURE_NO_WARNINGS
#endif

#include "Main.h"

int main(int argc, char* argv[])
{
    if (!(argc == 2 || argc == 3)) {
        std::cerr << "No argument provided! Usage: caffparser <caff_file_name> <OPTIONAL output_folder>" << std::endl;
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
        fclose(in_file);
        return 1;
    }

    LONG64 size = sb.st_size;
    UCHAR *file_contents = new UCHAR[size];

    fread(file_contents, size, 1, in_file);
    fclose(in_file);

    try {
        auto caff = parse(file_contents, size);
        std::cout << "Creator: " << caff.credits.Creator << std::endl;
        std::cout << "Creation date: " << caff.credits.YY << "." << +caff.credits.M << "." << +caff.credits.D << " " << +caff.credits.h << ":" << +caff.credits.m << std::endl;
        std::cout << "Number of frames: " << caff.frames.size() << std::endl;
        for (size_t i = 0; i < caff.frames.size(); i++)
        {
            auto frame = caff.frames[i];
            std::cout << "Frame " << i << ":" << std::endl;
            std::cout << "\tDuration: " << frame.duration << std::endl;
            std::cout << "\tCiff: " << std::endl;
            std::cout << "\t\tCaption: " << frame.ciff.caption << std::endl;
            std::cout << "\t\tTags: ";
            for (size_t i = 0; i < frame.ciff.tags.size(); i++)
                std::cout << frame.ciff.tags[i] << "; ";
            std::cout << std::endl;
            std::cout << "\t\tSize (width*height): " << frame.ciff.width << "*" << frame.ciff.height << std::endl;

            if (argc == 3) {
                const char* output_dir = argv[2];
                caff.persist_all(output_dir);
            }

        }

        delete[] file_contents;
        return 0;
    }
    catch (std::exception) {
        std::cout << "Error occured, can't parse caff" << std::endl;
        return 1;
    }
}
