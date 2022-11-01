CC = g++

all: clean setup main

setup:
	mkdir target

main: 
	$(CC) -o target/CaffParser Main.cpp Main.h

clean: 
	rm -rf target