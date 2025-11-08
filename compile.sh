nasm -fwin64 output.S -o output.o
cc output.o -o output.exe -e main -lkernel32