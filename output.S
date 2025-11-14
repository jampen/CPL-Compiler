section .text
global main
main:
        Push RBP
        Mov RBP, RSP
        Sub RSP, 0
        Push RBX
        Push R12
        Mov EAX, DWORD 10
        Mov ECX, DWORD 20
        Cmp ECX, EAX
        SetL AL
        MovZX EBX, AL
        Mov EAX, EBX
        Mov ECX, DWORD 500
        Cmp ECX, EAX
        SetL AL
        MovZX R12D, AL
        Pop R12
        Pop RBX
        Mov RSP, RBP
        Pop RBP
        Ret