using System.Collections.Generic;
using VM.Bases;
using VM.Global;
using VM.Parser;

namespace VM.Component
{

    public class Memory
    {
        public readonly StreamWriter Cache;
        public readonly StreamReader Stream;

        public Memory(int size)
        {
            Cache = new StreamWriter(false, 0, size);
            Stream = new StreamReader(Cache);
        }
    }

    public class Component
    {
        public readonly Process Process;
        public readonly Memory Ram;
        public readonly UMC Umc;
        public readonly UAL Ual;
        public readonly Memory Cache = new Memory(65000*5);
        public readonly Registers Registers = new Registers();

        public Component(int memSize)
        {
            Ram = new Memory(memSize);
            Ual = new UAL(this);
            Umc = new UMC(this);
            this.Process=new Process(this);
            this.Cache=new Memory(0x3fffc);
        }
    }
    public partial class UAL
    {
        public static readonly List<BasicInstruction> BasicInstructions = new List<BasicInstruction>(30);
        public static readonly List<string> NameInstructions = new List<string>(30);

        static UAL()
        {
            if (!NameInstructions.Contains("halt"))
            {
                BasicInstructions.Add(Halt);
                NameInstructions.Add("halt");
            }
        }

        private static void Halt(Operand v1, Operand v2)
        {

        }

    }
    public partial class UAL
    {
        public readonly Component MRT;

        public UAL(Component mtr)
        {
            MRT = mtr;
            Help.Import(GetType(), this);
        }

        [BasicInstruction]
        public void @goto(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            MRT.Registers[12] = MRT.Umc.GetValue(v1);
        }

        [BasicInstruction]
        public void @in(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {

        }

        [BasicInstruction(OperandType.imm)]
        public void @int(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {

        }

        [BasicInstruction]
        public void @lock(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void @out(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void aaa(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void aad(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void aam(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void aas(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void adc(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction(OperandType.Reg, OperandType.Reg)]
        [BasicInstruction(OperandType.Reg, OperandType.Mem)]
        [BasicInstruction(OperandType.Reg, OperandType.imm)]
        [BasicInstruction(OperandType.Mem, OperandType.Reg)]
        [BasicInstruction(OperandType.Mem, OperandType.Mem)]
        [BasicInstruction(OperandType.Mem, OperandType.imm)]
        public void add(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            if (v1.OperandType == OperandType.Reg)
                MRT.Umc.SetValue(v1, MRT.Umc.GetValue(v1) + MRT.Umc.GetValue(v2));
            else
                MRT.Registers[0] = MRT.Umc.GetValue(v1) + MRT.Umc.GetValue(v2);
        }

        [BasicInstruction(OperandType.Reg, OperandType.Reg)]
        [BasicInstruction(OperandType.Reg, OperandType.Mem)]
        [BasicInstruction(OperandType.Reg, OperandType.imm)]
        [BasicInstruction(OperandType.Mem, OperandType.Reg)]
        [BasicInstruction(OperandType.Mem, OperandType.Mem)]
        [BasicInstruction(OperandType.Mem, OperandType.imm)]
        public void and(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            MRT.Registers[0] = MRT.Umc.GetValue(v1) & MRT.Umc.GetValue(v2);
        }

        [BasicInstruction]
        public void bsf(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void bsr(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void bswap(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void bt(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void btc(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void btr(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void bts(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }
        
        /// call
        

        [BasicInstruction]
        public void cbw(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {

        }

        [BasicInstruction]
        public void cdq(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void clc(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void cld(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void cli(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void cmc(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void cmp(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            var a = MRT.Umc.GetValue(v1);
            var b = MRT.Umc.GetValue(v2);
            MRT.Registers.SetDraps("zf", a == b);
            MRT.Registers.SetDraps("cf", a < b);
            MRT.Registers.SetDraps("of", a < 0);
        }

        [BasicInstruction]
        public void cmps(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void cmpsb(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void cmpsd(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void cmpsw(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void cmpxchg(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void cwd(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void daa(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void das(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        public void dec(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            MRT.Umc.SetValue(v1, MRT.Umc.GetValue(v1) - 1);
        }

        [BasicInstruction(OperandType.Reg, OperandType.Reg)]
        [BasicInstruction(OperandType.Reg, OperandType.Mem)]
        [BasicInstruction(OperandType.Reg, OperandType.imm)]
        [BasicInstruction(OperandType.Mem, OperandType.Reg)]
        [BasicInstruction(OperandType.Mem, OperandType.Mem)]
        [BasicInstruction(OperandType.Mem, OperandType.imm)]
        public void div(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            MRT.Registers[0] = MRT.Umc.GetValue(v1) / MRT.Umc.GetValue(v2);
        }

        [BasicInstruction]
        public void enter(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fabs(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fadd(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void faddp(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fbldfbstp(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fchs(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fclex(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fcmov(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fcom(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fcomi(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fcos(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fdecstp(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fdiv(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fdivp(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fdivr(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fdivrp(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {

        }

        [BasicInstruction]
        public void ffree(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fiadd(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void ficom(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fidiv(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fidivr(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fild(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction(OperandType.Reg, OperandType.Reg)]
        [BasicInstruction(OperandType.Reg, OperandType.Mem)]
        [BasicInstruction(OperandType.Reg, OperandType.imm)]
        [BasicInstruction(OperandType.Mem, OperandType.Reg)]
        [BasicInstruction(OperandType.Mem, OperandType.Mem)]
        [BasicInstruction(OperandType.Mem, OperandType.imm)]
        public void fimul(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fincstp(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void finit(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fist(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fisttp(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction(OperandType.Reg, OperandType.Reg)]
        [BasicInstruction(OperandType.Reg, OperandType.Mem)]
        [BasicInstruction(OperandType.Reg, OperandType.imm)]
        [BasicInstruction(OperandType.Mem, OperandType.Reg)]
        [BasicInstruction(OperandType.Mem, OperandType.Mem)]
        [BasicInstruction(OperandType.Mem, OperandType.imm)]
        public void fisub(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction(OperandType.Reg, OperandType.Reg)]
        [BasicInstruction(OperandType.Reg, OperandType.Mem)]
        [BasicInstruction(OperandType.Reg, OperandType.imm)]
        [BasicInstruction(OperandType.Mem, OperandType.Reg)]
        [BasicInstruction(OperandType.Mem, OperandType.Mem)]
        [BasicInstruction(OperandType.Mem, OperandType.imm)]
        public void fisubr(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fld(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fld1(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fldCW(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fldENV(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fldL2E(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fldL2T(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fldLG2(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fldLN2(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fldPI(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fldZ(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction(OperandType.Reg, OperandType.Reg)]
        [BasicInstruction(OperandType.Reg, OperandType.Mem)]
        [BasicInstruction(OperandType.Reg, OperandType.imm)]
        [BasicInstruction(OperandType.Mem, OperandType.Reg)]
        [BasicInstruction(OperandType.Mem, OperandType.Mem)]
        [BasicInstruction(OperandType.Mem, OperandType.imm)]
        public void fmul(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fmulp(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fnop(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fpatan(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fprem(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fptan(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void frndint(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void frstor(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fsave(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fscale(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fsin(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fsincos(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fsqrt(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fst(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fstcw(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fstenv(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fstsw(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction(OperandType.Reg, OperandType.Reg)]
        [BasicInstruction(OperandType.Reg, OperandType.Mem)]
        [BasicInstruction(OperandType.Reg, OperandType.imm)]
        [BasicInstruction(OperandType.Mem, OperandType.Reg)]
        [BasicInstruction(OperandType.Mem, OperandType.Mem)]
        [BasicInstruction(OperandType.Mem, OperandType.imm)]
        public void fsub(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction(OperandType.Reg, OperandType.Reg)]
        [BasicInstruction(OperandType.Reg, OperandType.Mem)]
        [BasicInstruction(OperandType.Reg, OperandType.imm)]
        [BasicInstruction(OperandType.Mem, OperandType.Reg)]
        [BasicInstruction(OperandType.Mem, OperandType.Mem)]
        [BasicInstruction(OperandType.Mem, OperandType.imm)]
        public void fsubp(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction(OperandType.Reg, OperandType.Reg)]
        [BasicInstruction(OperandType.Reg, OperandType.Mem)]
        [BasicInstruction(OperandType.Reg, OperandType.imm)]
        [BasicInstruction(OperandType.Mem, OperandType.Reg)]
        [BasicInstruction(OperandType.Mem, OperandType.Mem)]
        [BasicInstruction(OperandType.Mem, OperandType.imm)]
        public void fsubr(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction(OperandType.Reg, OperandType.Reg)]
        [BasicInstruction(OperandType.Reg, OperandType.Mem)]
        [BasicInstruction(OperandType.Reg, OperandType.imm)]
        [BasicInstruction(OperandType.Mem, OperandType.Reg)]
        [BasicInstruction(OperandType.Mem, OperandType.Mem)]
        [BasicInstruction(OperandType.Mem, OperandType.imm)]
        public void fsubrp(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void ftst(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fwait(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fxam(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fxch(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fxsave(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fxstor(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fxtract(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fy2lx(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void fyl2xp1(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void hlt(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void idiv(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction(OperandType.Reg, OperandType.Reg)]
        [BasicInstruction(OperandType.Reg, OperandType.Mem)]
        [BasicInstruction(OperandType.Reg, OperandType.imm)]
        [BasicInstruction(OperandType.Mem, OperandType.Reg)]
        [BasicInstruction(OperandType.Mem, OperandType.Mem)]
        [BasicInstruction(OperandType.Mem, OperandType.imm)]
        public void imul(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        public void inc(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void ins(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void insb(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void insd(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void insw(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void into(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void iret(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void ja(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            if ((MRT.Registers.GetDraps("cf") || MRT.Registers.GetDraps("zf")) == false) @goto(v1);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jae(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            if (MRT.Registers.GetDraps("cf") == false) @goto(v1);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jb(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            if (MRT.Registers.GetDraps("cf")) @goto(v1);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jbe(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            if (MRT.Registers.GetDraps("cf") || MRT.Registers.GetDraps("zf")) @goto(v1);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jc(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            if (MRT.Registers.GetDraps("cf")) @goto(v1);

        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jcxz(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void je(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            if (MRT.Registers.GetDraps("zf")) @goto(v1);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jecxz(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jg(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            if (!((MRT.Registers.GetDraps("sf") ^ MRT.Registers.GetDraps("of")) | MRT.Registers.GetDraps("zf"))) @goto(v1);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jge(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            if (!(MRT.Registers.GetDraps("sf") ^ MRT.Registers.GetDraps("of"))) @goto(v1);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jl(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            if (MRT.Registers.GetDraps("sf") ^ MRT.Registers.GetDraps("of")) @goto(v1);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jle(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            if ((MRT.Registers.GetDraps("sf") ^ MRT.Registers.GetDraps("of")) | MRT.Registers.GetDraps("zf")) @goto(v1);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jmp(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            if (MRT.Registers[0] == 0) return;
            MRT.Registers[12] += MRT.Umc.GetValue(v1);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jna(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            jbe(v1);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jnae(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            jb(v1, v2);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jnb(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            jae(v1, v2);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jnbe(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            ja(v1, v2);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jnc(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            if (!MRT.Registers.GetDraps("cf")) @goto(v1);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jne(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            if (!MRT.Registers.GetDraps("zf")) @goto(v1);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jng(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            jle(v1, v2);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jnge(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            jl(v1, v2);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jnl(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            jge(v1, v2);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jnle(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            jg(v1, v2);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jno(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            if (!MRT.Registers.GetDraps("of")) @goto(v1);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jnp(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            if (!MRT.Registers.GetDraps("pf")) @goto(v1);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jns(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            if (!MRT.Registers.GetDraps("sf")) @goto(v1);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jnz(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            jne(v1, v2);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jo(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            if (MRT.Registers.GetDraps("of")) @goto(v1);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jp(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            if (MRT.Registers.GetDraps("pf")) @goto(v1);

        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jpe(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            if (MRT.Registers.GetDraps("pf")) @goto(v1);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jpo(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            jnp(v1, v2);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void js(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            if (MRT.Registers.GetDraps("sf")) @goto(v1);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.imm)]
        public void jz(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            je(v1, v2);
        }

        [BasicInstruction]
        public void lahf(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void lds(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction(OperandType.Reg)]
        [BasicInstruction(OperandType.Mem)]
        public void lea(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void leave(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void les(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void lfs(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void lgs(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void lods(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void lodsb(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void lodsd(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void lodsw(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void loop(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void loopd(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void loope(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void loopne(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void loopnz(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void loopw(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void loopz(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void lss(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {

            /*
             * push ret
             * push [ebp+0xFF0000]
             * call 0x00000
             * 
             * 
             * mov eax,ebp
             * 
             * pop ret
             * pop a1
             * pop 
             * push ebp
             * 
             * push esi
             * push esp
             * 
             */
        }
        [BasicInstruction]
        public void mov(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            MRT.Umc.SetValue(v1, MRT.Umc.GetValue(v2));
        }

        [BasicInstruction]
        public void movs(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void movsb(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void movsd(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void movsw(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void movsx(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void movzx(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction(OperandType.Reg, OperandType.Reg)]
        [BasicInstruction(OperandType.Reg, OperandType.Mem)]
        [BasicInstruction(OperandType.Reg, OperandType.imm)]
        [BasicInstruction(OperandType.Mem, OperandType.Reg)]
        [BasicInstruction(OperandType.Mem, OperandType.Mem)]
        [BasicInstruction(OperandType.Mem, OperandType.imm)]
        public void mul(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            MRT.Registers[0] = MRT.Umc.GetValue(v1) * MRT.Umc.GetValue(v2);
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        public void neg(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            MRT.Registers[0] = -MRT.Umc.GetValue(v1);
        }

        [BasicInstruction]
        public void nop(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }
        [BasicInstruction]
        public void rop(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.Reg)]
        public void not(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            MRT.Registers[0] = MRT.Umc.GetValue(v1) != 0 ? 0 : 1;
        }

        [BasicInstruction]
        public void or(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            MRT.Registers[0] = MRT.Umc.GetValue(v1) | MRT.Umc.GetValue(v2);
        }

        [BasicInstruction]
        public void outb(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void outd(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void outs(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void outw(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void pop(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            var i1 = MRT.Registers["ss"] - 4;
            if (i1 < 0) 
                return;
            var i = MRT.Cache.Stream.read(31, i1);
            MRT.Registers["ss"] = i1;
            MRT.Umc.SetValue(v1, i);
        }

        [BasicInstruction]
        public void popa(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {

        }

        [BasicInstruction]
        public void popad(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void popf(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void popfd(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void pow(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {

        }

        private void Push (int val)
        {
            var i1 = MRT.Registers["ss"];
            MRT.Cache.Cache.push(Bit.Coder(val), 32, i1);
            MRT.Registers["ss"] = i1 + 1;
        }

        private int Pop ()
        {
            var i1 = MRT.Registers["ss"] - 1;
            if (i1 < 0) return int.MaxValue;
            var val = MRT.Cache.Stream.read(32, i1);
            MRT.Registers["ss"] = i1;
            return val;
        }

        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.imm)]
        [BasicInstruction(OperandType.Reg)]
        public void push(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            var i1 = MRT.Registers["ss"] ;
            var d = MRT.Umc.GetValue(v1);
            MRT.Cache.Cache.push(Bit.Coder(d), 32, i1);
            MRT.Registers["ss"] = i1 + 4;
        }

        [BasicInstruction]
        public void pusha(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void pushad(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void pushd(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void pushf(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void pushfd(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void pushw(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void rcl(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void rcr(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void rep(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }


        [BasicInstruction(OperandType.Mem)]
        [BasicInstruction(OperandType.imm)]
        [BasicInstruction(OperandType.Reg)]
        public void call(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            var ss = MRT.Registers["sc"];
            var ip = MRT.Registers[12];
            MRT.Cache.Cache.push(Bit.Coder(ip), 32,  ss);
            MRT.Registers["sc"] = ss + 4;
            MRT.Registers[12] += MRT.Umc.GetValue(v1);
        }
        [BasicInstruction]
        public void ret(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            var ss = MRT.Registers["sc"] - 4;
            if (ss<0)return;
            var i = MRT.Cache.Stream.read(32, ss);
            MRT.Registers["sc"] = ss;
            MRT.Registers[12] = i;
        }

        [BasicInstruction]
        public void retf(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void retn(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void rol(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void ror(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void sahf(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void sal(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void sar(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void sbb(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void scas(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void scasb(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void scasd(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void scasw(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void shl(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void shld(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void shr(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void shrd(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void std(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void sti(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void stos(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void stosb(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void stosd(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void stosw(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void sts(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction(OperandType.Reg, OperandType.Reg)]
        [BasicInstruction(OperandType.Reg, OperandType.Mem)]
        [BasicInstruction(OperandType.Reg, OperandType.imm)]
        [BasicInstruction(OperandType.Mem, OperandType.Reg)]
        [BasicInstruction(OperandType.Mem, OperandType.Mem)]
        [BasicInstruction(OperandType.Mem, OperandType.imm)]
        public void sub(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            if (v1.OperandType == OperandType.Reg)
                MRT.Umc.SetValue(v1, MRT.Umc.GetValue(v1) - MRT.Umc.GetValue(v2));
            else
                MRT.Registers[0] = MRT.Umc.GetValue(v1) - MRT.Umc.GetValue(v2);
        }

        [BasicInstruction]
        public void test(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void wait(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void xadd(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void xand(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void xchg(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void xlat(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction]
        public void xlatb(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
        }

        [BasicInstruction(OperandType.Reg, OperandType.Reg)]
        [BasicInstruction(OperandType.Reg, OperandType.Mem)]
        [BasicInstruction(OperandType.Reg, OperandType.imm)]
        [BasicInstruction(OperandType.Mem, OperandType.Reg)]
        [BasicInstruction(OperandType.Mem, OperandType.Mem)]
        [BasicInstruction(OperandType.Mem, OperandType.imm)]
        public void xor(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            MRT.Registers[0] = MRT.Umc.GetValue(v1) ^ MRT.Umc.GetValue(v2);
        }
        
        public void inf(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            MRT.Registers[0] = MRT.Umc.GetValue(v1) < MRT.Umc.GetValue(v2) ? 1 : 0;
        }
        public void sup(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            MRT.Registers[0] = MRT.Umc.GetValue(v1) > MRT.Umc.GetValue(v2) ? 1 : 0;
        }
        public void eq(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            MRT.Registers[0] = MRT.Umc.GetValue(v1) == MRT.Umc.GetValue(v2) ? 1 : 0;
        }


        public void leq(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            MRT.Registers[0] = MRT.Umc.GetValue(v1) <= MRT.Umc.GetValue(v2) ? 1 : 0;
        }
        public void gte(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            MRT.Registers[0] = MRT.Umc.GetValue(v1) >= MRT.Umc.GetValue(v2) ? 1 : 0;
        }
        public void znew(Operand v1 = default(Operand), Operand v2 = default (Operand))
        {
            var z = (int)MRT.Process.newVar((ushort)MRT.Umc.GetValue(v1));
            push(new Operand() { DataType = DataType.DWord, OperandType = OperandType.imm, Value = z });
        }
        public static void Initialize () { }
    }
}