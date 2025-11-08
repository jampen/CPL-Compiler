section .text
global main
main:
        push rbp
        mov rbp, rsp
        sub rsp, 0
        push RBX
        mov AL, BYTE 10
        mov BX, WORD 20
        pop RBX
        mov rsp, rbp
        pop rbp
        ret
