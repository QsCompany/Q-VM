using System.Runtime.InteropServices;

namespace VM.Bases
{
    [StructLayout(LayoutKind.Sequential, Size = 40, Pack = 1)]
    
    public struct Structs
    {
        public string Name;
        public string Type;
        public int pointer;

        [AllowReversePInvokeCalls]
        public override string ToString()
        {
            return string.Format("{0} {1} : {2}", Type, Name, pointer);
        }
    }
}