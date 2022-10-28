# Szamitogep-biztonsag-VIHIMA06

## Start work

The source files for the project can be found at [main/CaffFrei](main/CaffFrei). Open the CaffFrei.sln solution with either VisualStudio 22 or JetBrains Rider. (Rider is basically an IntelliJ alternative for VS, it supports C# and C++ development)

In the solution there are 2 solution folders: `Src, Test`.

In the Src solution folder there are 2 projects for now. The cpp parser and the .NET wrapper for it. The web project will be added later.

## HappyParserPy

HappyParserPy is a CAFF parser prototype written in python. It does not handle errors, nor validates the CAFF file. It is useful to view CAFF files, and to better understand the underspecified CAFF format. It can be used as a guide for the cpp implementation.

Run the program by:

```PS
# Install the requirements
python -m pip install pillow
# Go the root of the program
cd .\main\HappyParserPy\
# Run the main.py
python main.py
```

If you want to play with the script in an IDE (eg.: VS Code or PyCharm), open the editor int the [main/HappyParserPy](main/HappyParserPy) folder.
