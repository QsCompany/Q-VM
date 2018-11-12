using System;
using VM.Parser;

namespace VM.Bases
{
    public delegate void BasicInstruction(Operand v1 = default(Operand), Operand v2 = default (Operand));

    public enum DataType : byte
    {
        Hex = 0x0,
        Byte = 0x1,
        Word = 0x2,
        DWord = 0x3,
        None = 0xFF,
    }
    public enum OperandType : byte
    {
        none = 0x0,
        Reg = 0x1,
        Mem = 0x2,
        imm = 0x3,
    }
    public enum Regs
    {
        eax = 0, ebx = 1, ecx = 2, edx = 3, esi = 4, edi = 5, esp = 6, ebp = 7, bs = 8, cs = 9, ds = 10, es = 11, ip = 12, dip = 13, sip = 14, dd = 15, al = 16, bl = 17, cl = 18, dl = 19, ah = 32, bh = 33, ch = 34, dh = 35, ax = 48, bx = 49, cx = 50, dx = 51, si = 52, di = 53, sp = 54, bp = 55, sc = 64
    }
    public enum AsmDataType : byte
    {
        OBit = 1,
        TBits = 2,
        Hex = 4,
        RBits = 6,
        Byte = 8,
        Word = 16,
        DWord = 32,
    }
    public enum Kind:byte
    {
        Null,
        Unair,
        Numbre,
        Variable,
        String,
        Expression,
        Return,
        Caller,
        Assigne,
        Hyratachy,
        For,
        If,
        ElseIf,
        While,
        Do,
        Bloc,
        Instruction,
        Parent,
        Ifs,
        Param,
        TypeAssigne,
        EqAssign,
        Space,
        Class,
        Const,
        DeclareParams,
        Function,
        DeclareParam,
        KeyWord,
        Operator,
        Program,
        Term = 50,
        Facteur = 51,
        Power = 52,
        Logic = 53,
        Word = 54,
        Array,
        Goto,
        Label,
        Register,
        Link,
        Derective,
        Derectives,
        File
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    internal sealed class BasicInstructionAttribute : Attribute
    {
        

        public BasicInstructionAttribute(OperandType dType = OperandType.none, OperandType sType = OperandType.none)
        {
            DType = dType;
            SType = sType;
        }

        public OperandType SType { get; private set; }
        public OperandType DType { get; private set; }
    }
}