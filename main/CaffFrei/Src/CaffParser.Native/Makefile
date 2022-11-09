CC = g++

all: clean setup main

setup:
	mkdir target

main: 
	$(CC) -o target/CaffParser ParserCore.cpp ParserCore.h Common.h Caff.h Caff.cpp Main.cpp Main.h

clean: 
	rm -rf target