using System;
using System.Globalization;
using VM.Bases;
using VM.Component;
using VM.Global;

namespace VM.Parser
{
    public struct Operand
    {
        public DataType DataType;
        public OperandType OperandType;
        public int Value;

        
        /// <summary>
        ///  This Operant Type Is imediate : reg
        /// </summary>
        public Operand(Regs reg):this()
        {
            OperandType = OperandType.Reg; Value = (int)reg;
        }
     
        /// <summary>
        ///  This Operant Type Is Memory : [reg+value]
        /// </summary>
        public Operand(Regs reg, int value)
            : this()
        {
            OperandType = OperandType.Mem; Value = new Nbit(4, (int)reg) << 28 | new Nbit(28, value).Value;
        }

        public Operand(bool asImmediat_Memory, int value, DataType datatype = DataType.None)
            : this()
        {
            if (asImmediat_Memory)
            {
                OperandType = Bases.OperandType.imm;
                Value = value;
                DataType = datatype == Bases.DataType.None ? (DataType)MathHelp.length(value) : DataType = datatype;
            }
            else
            {
                OperandType = OperandType.Mem;
                Value = new Nbit(4, -1) << 28 | new Nbit(28, value).Value;
            }
        }

        public bool Equals(Operand obj)
        {
            if (OperandType == obj.OperandType)
                switch (OperandType)
                {
                    case OperandType.none:
                        return true;
                    case OperandType.Reg:
                        return (Value & 0x3f) == (obj.Value & 0x3f);
                    case OperandType.Mem:
                        return Value == obj.Value;
                    case OperandType.imm:
                        if (DataType == obj.DataType)
                            switch (DataType)
                            {
                                case DataType.Hex:
                                    return (Value & 0xf) == (obj.Value & 0xf);
                                case DataType.Byte:
                                    return (Value & 0xff) == (obj.Value & 0xff);
                                case DataType.Word:
                                    return (Value & 0xffff) == (obj.Value & 0xffff);
                                case DataType.DWord:
                                    return Value == obj.Value;
                            }
                        break;
                }
            return false;
        }

        public int Length
        {
            get
            {
                switch (OperandType)
                {
                    case  OperandType.none:
                        return 2;
                    case  OperandType.Reg:
                        return 8;
                    case  OperandType.Mem:
                        return 34;
                    case  OperandType.imm:
                        switch (DataType)
                        {
                            case DataType.Hex:
                                return 8;
                            case DataType.Byte:
                                return 12;
                            case DataType.Word:
                                return 20;
                            case DataType.DWord:
                                return 36;
                            default:
                                throw new Exception();
                        }
                }
                return 0;
            }
        }

        public void Push(StreamWriter stream)
        {
            stream.push((byte)OperandType, (int)AsmDataType.TBits);
            switch (OperandType)
            {
                case  OperandType.Reg:
                    stream.push((byte)Value, (int)AsmDataType.RBits);
                    break;

                case  OperandType.Mem:
                    stream.push(Bit.Coder(Value), (int)AsmDataType.DWord);
                    break;
                case  OperandType.imm:
                    stream.push((byte)DataType, (int)AsmDataType.TBits);
                    switch (DataType)
                    {
                        case DataType.Hex:
                            stream.push((byte)Value, (int)AsmDataType.Hex);
                            break;

                        case DataType.Byte:
                            stream.push((byte)Value, (int)AsmDataType.Byte);
                            break;

                        case DataType.Word:
                            stream.push(Bit.Coder((Int16)Value), (int)AsmDataType.Word);
                            break;

                        case DataType.DWord:
                            stream.push(Bit.Coder(Value), (int)AsmDataType.DWord);
                            break;
                    }
                    break;
            }
        }

        public static Operand Pop(StreamReader stream)
        {
            var o = new Operand();
            o.OperandType = (OperandType) stream.read((int) AsmDataType.TBits);
            switch (o.OperandType)
            {
                case OperandType.Reg:
                    o.Value = stream.read((int) AsmDataType.RBits);
                    break;

                case OperandType.Mem:
                    o.Value = stream.read((int) AsmDataType.DWord);
                    break;

                case OperandType.imm:
                    DataType d;
                    o.Value = GetValue(stream, out d);
                    o.DataType = d;
                    break;
            }
            return o;
        }

        private static int GetValue(StreamReader stream,out DataType dataType)
        {
            dataType = (DataType) stream.read((int) AsmDataType.TBits);
            switch (dataType)
            {
                case DataType.Hex:
                    return stream.read((int) AsmDataType.Hex);
                case DataType.Byte:
                    return stream.read((int) AsmDataType.Byte);
                case DataType.Word:
                    return stream.read((int) AsmDataType.Word);
                case DataType.DWord:
                    return stream.read((int) AsmDataType.DWord);
                default:
                    throw new Exception("Bad Error");
            }
        }
        
        public static Operand Parse(string s)
        {
            var o = new Operand
            {
                Value = Registers.GetHasheCode(s)
            };
            int r = 0;
            o.OperandType = Parser.Parse.IsNumbre(s, out r)
                ? OperandType.imm
                : (s[0] == '[' | o.Value == -1 ? OperandType.Mem : OperandType.Reg);

           if (o.OperandType == OperandType.Mem)
                o.Value = Parser.Parse.MemOperandType(s.Substring(1, s.Length - 2));
           else if (o.OperandType == OperandType.imm)
           {
               o.Value = r;
               o.DataType = (DataType)(s.Length > 2 && s[1] == 'x' ? MathHelp.SRound(Math.Log(s.Length - 2, 2)) : MathHelp.length(r));
           }
            return o;
        }
        
        public override string ToString()
        {
            switch (OperandType)
            {
                case OperandType.Reg:
                    return Registers.GetHashName(Value);
                case OperandType.Mem:
                    Nbit a = new Nbit(4, Value >> 28), b = new Nbit(28, Value);
                    string c = "[";
                    if (a.Value != 15) c += Registers.GetHashName(a.Value) + (b.Value != 0 ? "+" : "");
                    c += a.Value != 15 && b.Value == 0 ? "]" : "0x" + b.Value.ToString("x7") + "]";
                    return c;
                case OperandType.imm:
                    c = "0x";
                    switch (DataType)
                    {
                        case DataType.Hex:
                            return c + ((byte) (Value & 0xf)).ToString("x1", CultureInfo.InvariantCulture);
                        case DataType.Byte:
                            return c + ((byte)Value).ToString("x2", CultureInfo.InvariantCulture);
                        case DataType.Word:
                            return c + ((short)Value).ToString("x4", CultureInfo.InvariantCulture);
                        case DataType.DWord:
                            return c + Value.ToString("x8", CultureInfo.InvariantCulture);
                    }
                    break;
            }
            return "";
        }

        
    }
    public static class MathHelp
    {
        public static int SRound(double x)
        {
            double d = x % 1;
            return (int)(x + (d == 0 ? 0 : (x > 0 ? 1 : 0) - d));
        }
        public static int LogRound(int x, int n = 2, int i = 0)
        {
            int m = x / n;
            if (m < 1) return i;
            return LogRound(m, n, ++i);
        }
        public static int length(int i)
        {
            var e = LogRound(i, 16);
            return SRound(Math.Log(e, 2));
        }
    }
    public static class Parse
    {        
        public static int MemOperandType(string s)
        {
            int r = 0;
            Nbit a = new Nbit(4, -1), b = new Nbit(28, 0);
            var d = s.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < d.Length; i++)
                if (IsNumbre(d[i], out r))
                    b.Value = r;
                else
                    a.Value = Registers.GetHasheCode(d[i]);
            return a.Value << 28 | b.Value;
        }
        public static bool IsNumbre(string s, out int i)
        {

            if (s.StartsWith("0x"))
                return int.TryParse(s.Substring(2), NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out i);
            return int.TryParse(s, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out i);
        }        
    }
    
}