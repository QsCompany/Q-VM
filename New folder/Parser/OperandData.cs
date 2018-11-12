using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Bases;
using VM.Component;
using VM.Global;

namespace VM.Parser
{
    
    
    public class OperandData
    {
        public static Operand GetImmediat(int value, DataType type)
        {
            return new Operand() { Value = value, DataType = type };
        }
        public static Operand GetImmediat(int value)
        {
            return new Operand() { Value = value, DataType = (DataType)MathHelp.length(value) };
        }

        public static Operand GetRegister(Regs reg)
        {
            return new Operand() { OperandType = OperandType.Reg, Value = (int)reg };
        }

        public static Operand GetMemory(int value)
        {
            return new Operand() { OperandType = OperandType.Mem, Value = new Nbit(4, -1) << 28 | new Nbit(28, value).Value };
        }
        public static Operand GetMemory(Regs reg, int value)
        {
            return new Operand() { OperandType = OperandType.Mem, Value = new Nbit(4, (int)reg) << 28 | new Nbit(28, value).Value };
        }
    }
}
