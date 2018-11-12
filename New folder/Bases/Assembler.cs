using Compiler.Annotations;
using Compiler.Global;

namespace Compiler.Bases
{
    public struct Assembler
    {
        public string Name;
        public Tree Param1;
        public Tree Param2;

        public Assembler(string name)
            : this()
        {
            Name = name;
        }

        public string Fn { get; set; }

        public byte Function { [UsedImplicitly] get; set; }

        public void Set(string func)
        {
            Fn = func;
            switch (func)
            {
                case "+":
                    Function = 1;
                    break;

                case "-":
                    Function = 2;
                    break;

                case "*":
                    Function = 3;
                    break;

                case "/":
                    Function = 4;
                    break;

                case "%":
                    Function = 5;
                    break;

                case "\\":
                    Function = 6;
                    break;

                case "^":
                    Function = 7;
                    break;

                case "&":
                    Function = 8;
                    break;

                case "!":
                    Function = 9;
                    break;

                case "#":
                    Function = 10;
                    break;

                case ">":
                    Function = 11;
                    break;

                case "<":
                    Function = 12;
                    break;

                case "~":
                    Function = 13;
                    break;

                case "goto":
                    Function = 14;
                    break;
            }
        }
    }
}