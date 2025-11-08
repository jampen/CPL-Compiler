using CPL.IR;
using System.Linq;
using static CPL.BE.X64Allocator;

namespace CPL.BE;

internal sealed class AlreadyAllocatedException(IR.Value value) : Exception($"{nameof(value)} was already allocated");
internal sealed class NotAllocatedException(IR.Value value) : Exception($"{nameof(value)} is not allocated");
internal sealed class InvalidRegisterException(string name) : Exception($"{name} is not a valid register");


/// <summary>
/// Allocates a MemoryLocation for IR Values
/// </summary>
// TODO: X64Platform class to determine what is a callee saved register.
internal sealed class X64Allocator : IAllocator
{
    public int StackSize { get; private set; } = 0;

    public enum Register
    {
        RAX, EAX, AX, AL,
        RBX, EBX, BX, BL,
        RCX, ECX, CX, CL,
        RDX, EDX, DX, DL,
        R8, R8D, R8W, R8B,
        R9, R9D, R9W, R9B,
        R10, R10D, R10W, R10B,
        R11, R11D, R11W, R11B,
        R12, R12D, R12W, R12B,
        R13, R13D, R13W, R13B,
        R14, R14D, R14W, R14B,
        R15, R15D, R15W, R15B,
    }

    // Must be in size-order smallest to largest
    public enum RegisterSize
    {
        BYTE,
        WORD,
        DWORD,
        QWORD,
    }

    // Maps x64 register sizes to their family of registers
    private static readonly Dictionary<RegisterSize, Register[]> registers = new()
    {
        [RegisterSize.QWORD] = [Register.RAX, Register.RBX, Register.RCX, Register.RDX, Register.R9, Register.R10, Register.R11, Register.R12, Register.R13, Register.R14, Register.R15],
        [RegisterSize.DWORD] = [Register.EAX, Register.EBX, Register.ECX, Register.EDX, Register.R9D, Register.R10D, Register.R11D, Register.R12D, Register.R13D, Register.R14D, Register.R15D],
        [RegisterSize.WORD] = [Register.AX, Register.BX, Register.CX, Register.DX, Register.R9W, Register.R10W, Register.R11W, Register.R12W, Register.R13W, Register.R14W, Register.R15W],
        [RegisterSize.BYTE] = [Register.AL, Register.BL, Register.CL, Register.DL, Register.R9B, Register.R10B, Register.R11B, Register.R12B, Register.R13B, Register.R14B, Register.R15B],
    };


    // In smallest to largest order
    private static readonly Dictionary<Register, Register[]> registerFamilies = new()
    {
        [Register.RAX] = [Register.AL, Register.AX, Register.EAX, Register.RAX],
        [Register.RBX] = [Register.BL, Register.BX, Register.EBX, Register.RBX],
        [Register.RCX] = [Register.CL, Register.CX, Register.ECX, Register.RCX],
        [Register.RDX] = [Register.DL, Register.DX, Register.EDX, Register.RDX],
        [Register.R8] =  [Register.R8B, Register.AX, Register.EAX, Register.RAX],
        [Register.R9] =  [Register.R9B, Register.R9W, Register.R9D, Register.R9],
        [Register.R10] = [Register.R10B, Register.R10W, Register.R10D, Register.R10],
        [Register.R11] = [Register.R11B, Register.R11W, Register.R11D, Register.R11],
        [Register.R12] = [Register.R12B, Register.R12W, Register.R12D, Register.R12],
        [Register.R13] = [Register.R13B, Register.R13W, Register.R13D, Register.R13],
        [Register.R14] = [Register.R14B, Register.R13W, Register.R14D, Register.R14],
        [Register.R15] = [Register.R15B, Register.R14W, Register.R15D, Register.R15],
    };


    private HashSet<Register> usedRegisters = new();
    private Dictionary<IR.Value, MemoryLocation> valueLocations = new();

    public HashSet<Register> CalleeSavedRegisters { get; } = new();

    private static Register PromoteRegister(Register register, RegisterSize size)
    {
        return registerFamilies[ToLargestRegister(register)][((int)size)];
    }

    public MemoryLocation Promote(MemoryLocation location, int size)
    {
        if (location is not RegisterLocation registerLocation)
        {
            return location;
        }

        var register = Enum.Parse<Register>(registerLocation.Name);
        return new RegisterLocation(PromoteRegister(register, ByteSizeToRegisterSize(size).Value).ToString());
    }


    // Promotes a register to its largest size in the family
    private static Register ToLargestRegister(Register reg)
    {
        switch (reg)
        {
            // RAX
            case Register.RAX: return Register.RAX;
            case Register.EAX: return Register.RAX;
            case Register.AX: return Register.RAX;
            case Register.AL: return Register.RAX;

            case Register.RBX: return Register.RBX;
            case Register.EBX: return Register.RBX;
            case Register.BX: return Register.RBX;
            case Register.BL: return Register.RBX;

            case Register.RCX: return Register.RCX;
            case Register.ECX: return Register.RCX;
            case Register.CX: return Register.RCX;
            case Register.CL: return Register.RCX;

            case Register.RDX: return Register.RDX;
            case Register.EDX: return Register.RDX;
            case Register.DX: return Register.RDX;
            case Register.DL: return Register.RDX;

            case Register.R8: return Register.R8;
            case Register.R8D: return Register.R8;
            case Register.R8W: return Register.R8;
            case Register.R8B: return Register.R8;

            case Register.R9: return Register.R9;
            case Register.R9D: return Register.R9;
            case Register.R9W: return Register.R9;
            case Register.R9B: return Register.R9;

            case Register.R10: return Register.R10;
            case Register.R10D: return Register.R10;
            case Register.R10W: return Register.R10;
            case Register.R10B: return Register.R10;

            case Register.R11: return Register.R11;
            case Register.R11D: return Register.R11;
            case Register.R11W: return Register.R11;
            case Register.R11B: return Register.R11;

            case Register.R12: return Register.R12;
            case Register.R12D: return Register.R12;
            case Register.R12W: return Register.R12;
            case Register.R12B: return Register.R12;

            case Register.R13: return Register.R13;
            case Register.R13D: return Register.R13;
            case Register.R13W: return Register.R13;
            case Register.R13B: return Register.R13;

            case Register.R14: return Register.R14;
            case Register.R14D: return Register.R14;
            case Register.R14W: return Register.R14;
            case Register.R14B: return Register.R14;

            case Register.R15: return Register.R15;
            case Register.R15D: return Register.R15;
            case Register.R15W: return Register.R15;
            case Register.R15B: return Register.R15;
        }

        throw new ArgumentException(nameof(reg));
    }

    public static RegisterSize? ByteSizeToRegisterSize(int numBytes)
    {
        if (numBytes <= 1) return RegisterSize.BYTE;
        if (numBytes <= 2) return RegisterSize.WORD;
        if (numBytes <= 4) return RegisterSize.DWORD;
        if (numBytes <= 8) return RegisterSize.QWORD;
        return null;
    }

    public static RegisterSize? TypeToRegisterSize(IR.Type type)
    {
        if (type is IR.BooleanType)
        {
            return RegisterSize.BYTE;
        }
        else if (type is IR.IntType intType)
        {
            if (intType.BitWidth <= 8) return RegisterSize.BYTE;
            if (intType.BitWidth <= 16) return RegisterSize.WORD;
            if (intType.BitWidth <= 32) return RegisterSize.DWORD;
            if (intType.BitWidth <= 64) return RegisterSize.QWORD;
        }
        else if (type is IR.PointerType)
        {
            return RegisterSize.QWORD;
        }

        return null;
    }


    private static bool IsRegisterVolatile(Register register)
    {
        Register largestRegister = ToLargestRegister(register);
        switch (largestRegister)
        {
            case Register.RAX:
            case Register.RCX:
            case Register.RDX:
            case Register.R8:
            case Register.R9:
            case Register.R10:
            case Register.R11: return true;
        }
        return false;
    }

    private static bool IsRegisterCalleeSaved(Register register)
    {
        return !IsRegisterVolatile(register);
    }

    public MemoryLocation Allocate(IR.Value value)
    {
        if (valueLocations.ContainsKey(value))
        {
            throw new AlreadyAllocatedException(value);
        }

        int sizeOfType = value.Type.X64Size();
        RegisterSize? regSize = TypeToRegisterSize(value.Type);

        if (regSize != null)
        {
            Register[] sameSizedRegs = registers[regSize.Value];

            foreach (Register register in sameSizedRegs)
            {
                // We don't want to use AL, then allow RAX to be used.
                // The biggest register must be marked unavailable
                Register largestSizedReg = ToLargestRegister(register);

                if (usedRegisters.Contains(largestSizedReg))
                {
                    continue;
                }

                // Found a free reg, now we can claim it
                usedRegisters.Add(largestSizedReg);

                if (IsRegisterCalleeSaved(largestSizedReg))
                {
                    CalleeSavedRegisters.Add(largestSizedReg);
                }

                var registerLocation = new RegisterLocation(register.ToString());
                valueLocations[value] = registerLocation;
                return registerLocation;
            }
            // All the regs have been used. Fall to stack space.
        }

        StackSize += sizeOfType;
        var stackLocation = new StackLocation(StackSize);
        valueLocations[value] = stackLocation;
        return stackLocation;
    }

    public MemoryLocation GetMemoryLocation(IR.Value value)
    {
        if (valueLocations.TryGetValue(value, out MemoryLocation location))
        {
            return location;
        }
        else
        {
            throw new NotAllocatedException(value);
        }
    }

    public bool IsAllocated(IR.Value value)
    {
        return valueLocations.ContainsKey(value);
    }

    public MemoryLocation GetOrAllocate(IR.Value value)
    {
        if (valueLocations.TryGetValue(value, out MemoryLocation location))
        {
            return location;
        }
        else
        {
            return Allocate(value);
        }
    }

    public void Free(IR.Value value)
    {
        MemoryLocation location = GetMemoryLocation(value);
        valueLocations.Remove(value);

        if (location is RegisterLocation registerLocation)
        {
            if (!Enum.TryParse(registerLocation.Name, out Register register))
            {
                throw new InvalidRegisterException(registerLocation.Name);
            }

            Register largestRegister = ToLargestRegister(register);
            usedRegisters.Remove(largestRegister);
        }
    }
}