section .text
global main
main:
        push rbp
        mov rbp, rsp
        sub rsp, 112
        push RBX
        push R12
        push R13
        push R14
        push R15
        Mov RBX, QWORD 10
        Mov R12, RBX
        Mov R13, R12
        Mov R14, R13
        Mov R15, R14
        Mov QWORD [rbp-8], R15
        Mov RAX, QWORD [rbp-8]
        Mov QWORD [rbp-16], RAX
        Mov RAX, QWORD [rbp-16]
        Mov QWORD [rbp-24], RAX
        Mov RAX, QWORD [rbp-24]
        Mov QWORD [rbp-32], RAX
        Mov RAX, QWORD [rbp-32]
        Mov QWORD [rbp-40], RAX
        Mov RAX, QWORD [rbp-40]
        Mov QWORD [rbp-48], RAX
        Mov RAX, QWORD [rbp-48]
        Mov QWORD [rbp-56], RAX
        Mov RAX, QWORD [rbp-56]
        Mov QWORD [rbp-64], RAX
        Mov RAX, QWORD [rbp-64]
        Mov QWORD [rbp-72], RAX
        Mov RAX, QWORD [rbp-72]
        Mov QWORD [rbp-80], RAX
        Mov RAX, QWORD [rbp-80]
        Mov QWORD [rbp-88], RAX
        Mov RAX, QWORD [rbp-88]
        Mov QWORD [rbp-96], RAX
        Mov QWORD [rbp-104], QWORD 20
        Mov RAX, QWORD [rbp-104]
        Mov QWORD [rbp-112], RAX
        pop R15
        pop R14
        pop R13
        pop R12
        pop RBX
        mov rsp, rbp
        pop rbp
        ret
