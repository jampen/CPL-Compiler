section .text
global main
main:
        push rbp
        mov rbp, rsp
        sub rsp, 16
        mov dword [rbp-4], 10
        mov dword [rbp-8], 20
        mov rsp, rbp
        pop rbp
        ret
