FROM debian:11
RUN apt update && apt install -y build-essential

COPY Caff.cpp /build/
COPY Caff.h /build/
COPY Common.h /build/
COPY Dockerfile /build/
COPY Main.cpp /build/
COPY Main.h /build/
COPY Makefile /build/
COPY ParserCore.cpp /build/
COPY ParserCore.h /build/

WORKDIR /build
RUN make
