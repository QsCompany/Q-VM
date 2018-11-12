using VM.Global;
using VM.Parser;
namespace VM.Component
{
    public class ObjectsMap
    {
        public uint address;
        public ushort size;
        public uint isDeleted;
        public byte[] GetBytes()
        {
            return Serialize(address, size, (byte)isDeleted);
        }
        public unsafe static ObjectsMap ToObjectsMap(byte[] bytes)
        {
            long l = 0;
            fixed (byte* numRef = &(bytes[0]))
                l = *(((long*)numRef));
            ObjectsMap j = new ObjectsMap()
            {
                address = (uint)l & 0xfffffff,
                size = (ushort)(l >> 32),
                isDeleted = (uint)(l >> 48),
            };
            return j;
        }

        public unsafe static long ToLong(uint address, ushort size, byte isDeleted)
        {
            return address + (size << 32) + ((isDeleted & 0x1) << 48);
        }
        public unsafe static byte[] Serialize(uint address, ushort size, ushort isDeleted)
        {
            byte[] r = new byte[8];
            long c = address + (size << 32) + (isDeleted << 48);
            fixed (byte* numRef = r)
                *((long*)numRef) = c;
            return r;
        }
    }
    
    public class Process
    {
        public Component MRT;
        
        public Process(Component mrt)
        {
            MRT = mrt;
        }

        public void Execute(InitialProcessData iPD)
        {
            iPD.Upload(MRT.Registers);
            do
            {
                int c = MRT.Registers[12];
                MRT.Cache.Stream.Seek(c);
                b = Instruct.Pop(MRT.Cache.Stream);
                c += b.NCLength();
                MRT.Registers[12] = c;
                UAL.BasicInstructions[b.Function](b.Desdestination, b.Source);
            } while (b.Function != 0);
        }
        Instruct b;
        private int Source { get { return MRT.Umc.GetValue(b.Source); } }
        private int Dest { get { return MRT.Umc.GetValue(b.Desdestination); } }
        internal void initHeapMemory(uint @startHeapMemory)
        {
            var mem = MRT.Ram.Stream;
            var c = MRT.Registers["esi"];
            mem.Stream.Seek(c);
            mem.Stream.push(ObjectsMap.Serialize(@startHeapMemory, ushort.MaxValue, ushort.MaxValue), 64);
        }
        internal uint newVar(ushort size)
        {
            var mem = MRT.Ram.Stream;
            var c = MRT.Registers["esi"];
            mem.Seek(c - 8);
            uint addr = (uint)mem.read(32) + (uint)mem.read(16);
            mem.Stream.Seek(c);
            mem.Stream.push(ObjectsMap.Serialize(addr, size, 0), 64);
            MRT.Registers["esi"] += 8;
            var q = @"{
                    
                }";
            return addr;
        }
        internal void destVar()
        {
            var mem =MRT.Ram.Stream;
            var c = MRT.Registers["esi"] -= 8;
            mem.Stream.Seek(c);
            mem.Stream.push(ObjectsMap.Serialize(0, 0, 0), 64);
        }
        internal void destVar(uint varAddress)
        {
            var mem = MRT.Ram.Stream;
            var c = MRT.Registers["esi"] - 8;
            ushort sz, isd;
            do
            {
                mem.Seek(c);
                uint addr = (uint)mem.read(32);
                sz = (ushort)mem.read(16);
                isd = (ushort)mem.read(16);
                if (addr == varAddress)
                {
                    mem.Stream.Seek((int)addr);
                    mem.Stream.push(new byte[sz], sz * 8);
                    mem.Stream.Seek(c);
                    mem.Stream.push(new byte[8], 64);
                    return;
                }
            } while (sz == ushort.MaxValue && isd == ushort.MaxValue);
        }
    }
}