using VM.Component;
using VM.Parser;

namespace VM
{
    public static class Programm
    {
        public static void Main()
        {
            Exmp();
        }
        private static void Exmp()
        {
            const string c = "mov [ebp+0x0000000],12;mov [ebp+0x0000004],11;mov [ebp+0x0000008],1;mov [ebp+0x000000c],1;mov [ebp+0x0000010],2;mov [ebp+0x0000014],6;add [ebp+0x0000000],[ebp+0x0000004];mov [ebp+0x0000018],eax;mul [ebp+0x0000008],[ebp+0x000000c];mov [ebp+0x000001c],eax;add [ebp+0x0000018],[ebp+0x000001c];mov [ebp+0x0000018],eax;sub [ebp+0x0000018],2;mov [ebp+0x0000018],eax;mul [ebp+0x0000010],[ebp+0x0000014];mov [ebp+0x000001c],eax;add [ebp+0x0000018],[ebp+0x000001c];mov [ebp+0x0000018],eax;add 0,1;mov [ebp+0x0000020],eax;add [ebp+0x0000020],2;mov [ebp+0x0000020],eax;add [ebp+0x0000020],3;mov [ebp+0x0000020],eax;add [ebp+0x0000020],4;mov [ebp+0x0000020],eax;add [ebp+0x0000020],5;mov [ebp+0x0000020],eax;add [ebp+0x0000020],6;mov [ebp+0x0000020],eax;add [ebp+0x0000020],7;mov [ebp+0x0000020],eax;add [ebp+0x0000020],8;mov [ebp+0x0000020],eax;add [ebp+0x0000020],9;mov [ebp+0x0000020],eax;inf [ebp+0x0000020],0;mov eax,eax;jmp [ebp+0x0000024];neg [ebp+0x0000028];mov [ebp+0x000002c],eax;mov [ebp+0x0000030],[ebp+0x000002c]";
            var cc = c.Split(";\r\n".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);

            var MRT = new Component.Component(0xFFFFF);
            foreach (var d in cc)
                Instruct.Parse(d).Push(MRT.Cache.Stream);
            var e = 65635;
            MRT.Process.Execute(new VM.Global.InitialProcessData(0, e, e * 3, e * 2, e * 4, 0));
        }
    }
}