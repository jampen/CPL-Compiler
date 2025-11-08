section .text
global main
main:
        push rbp
        mov rbp, rsp
        sub rsp, 0
        push RBX
        mov AL, BYTE 10
        mov BX, WORD 20
        cmp BX, AX
        jge L0.Else
        L0:
        mov CX, WORD 30
        jmp L0.Done
        L1:
        mov CX, WORD 40
        L0.Done:
        pop RBX
        mov rsp, rbp
        pop rbp
        ret
