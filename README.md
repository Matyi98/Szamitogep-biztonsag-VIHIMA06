# Szamitogep-biztonsag-VIHIMA06

## Compile and test native parser

> Works only on Linux! Tested with Ubuntu 20.04

```bash
cd main/CaffFrei/Src/CaffParser.Native
make
# Test parser
target/CaffParser ../../../HappyParserPy/samples/2.caff $(pwd)/target/testrun
```

## Start work

The source files for the project can be found at [main/CaffFrei](main/CaffFrei). Open the CaffFrei.sln solution with VisualStudio 22.

In the solution there are 2 solution folders: `Src, Test`.

In the Src solution folder there are 3 projects. The cpp parser, the .NET wrapper for it and the webapp.

Debugging with breakpoints even in the cpp code invoked from C# should work.

In VS `rebuild solution` might help with errors.

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
