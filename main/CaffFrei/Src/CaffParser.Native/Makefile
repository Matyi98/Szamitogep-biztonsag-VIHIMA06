CC = g++

all: clean setup main

setup:
	mkdir target

main: 
	$(CC) -o target/CaffParser ParserCore.cpp ParserCore.h Common.h Caff.h Main.cpp Main.h

clean: 
	rm -rf target