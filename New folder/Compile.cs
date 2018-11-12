using System.Collections.Generic;
using System.IO;
using Compiler.Bases;
using Compiler.Global;
using Compiler.Grammair;
using Compiler.Grammair.Bases;

namespace Compiler
{
    using VM.Bases;

    public partial class Compile
    {
        private void Bloc(IEnumerable<Tree> trees)
        {
            Add("nop", "", null);
            foreach (var tree in trees)
                Compiler(tree);
            Add("rop", "", null);
        }

        private Tree Caller(IList<Tree> trees, Kind kind = Kind.Caller, string name = "")
        {
            var dname = string.IsNullOrEmpty(name);
            var T = new Assembler(dname ? "L" + ++_count : name);
            T.Set(trees[0].Content);

            for (var j = 0; j < trees[1].Count; j++)
                trees[1][j] =
                    Compiler(trees[1][j]);
            T.Param1 = trees[1];
            var h = new Tree(new Builder(T.Name), trees[0].Parent,
                Kind.Variable) {Start = 0, End = T.Name.Length - 1};

            foreach (var item in T.Param1)
                Add("push", "", new Tree(item.Content, Kind.Variable));
            Add("push", "", new Tree("IP", Kind.Register));
            if (kind == Kind.Caller)
                Add("call", "", new Tree(T.Fn, Kind.Variable));
            else
            {
                Add("push", "", new Tree(T.Fn, Kind.Variable));
                Add("call", "", new Tree("Get_Array", Kind.Variable));
            }
            Add("pop", "", new Tree(T.Name, Kind.Variable));

            if (dname) _count -= 1;
            return h;
        }

        private void Do(IList<Tree> trees)
        {
            var nl = ++_lb;
            Add("label", "B" + nl, new Tree("B" + nl, Kind.Variable));
            Compiler(trees[0]);
            var T = Compiler(trees[1]);
            Add("and", "", T, new Tree("true", Kind.Const));
            Add("jmp", "", new Tree("B" + nl, Kind.Variable));
        }

        private Tree EqAssigne(IList<Tree> trees)
        {
            var nc = trees[1].Count;
            var T = new Assembler(trees[0].Content)
            {
                Param1 = Compiler(trees[1], trees[0].Content)
            };
            if (nc < 2)
                Add("mov", "", new Tree(T.Name, Kind.Variable), T.Param1);
            trees.RemoveAt(1);
            return trees[0];
        }

        private void For(IList<Tree> trees)
        {
            Compiler(trees[0]);
            int nl = ++_lb, ml = ++_lb;
            Add("label", "B" + nl, new Tree("B" + nl, Kind.Variable));
            Add("ne", "", Compiler(trees[1]));
            Add("jmp", "", new Tree("B" + ml, Kind.Variable));
            Compiler(trees[3]);
            Compiler(trees[2]);
            Add("ne", "", new Tree("0", Kind.Numbre));
            Add("jmp", "", new Tree("B" + nl, Kind.Variable));
            Add("label", "", new Tree("B" + ml, Kind.Variable));
        }

        private void Goto(IList<Tree> trees)
        {
            var T = new Assembler();
            T.Set("goto");
            T.Param1 = trees[1];
            Add("jmp", "", new Tree(trees[1].Content, Kind.Label));
        }

        private void If(IList<Tree> trees)
        {
            var T = Compiler(trees[0]);
            int nl = ++_lb, ml = ++_lb;
            Add("ne", "", T);
            Add("jmp", "", new Tree("B" + nl, Kind.Variable));
            Compiler(trees[1]);
            if (trees.Count == 3)
                Add("jmp", "", new Tree("B" + ml, Kind.Variable));
            Add("label", "B" + nl, new Tree("B" + nl, Kind.Variable));
            if (trees.Count != 3) return;
            Compiler(trees[2]);
            Add("label", "B" + nl, new Tree("B" + ml, Kind.Variable));
        }

        private void TypeAssigne(IList<Tree> trees)
        {
            Add("push", "", trees[1]);
            Add("mem", "", trees[0]);
            trees.RemoveAt(0);
            if (trees.Count <= 1) return;
            trees[0].Parent.Kind = Kind.EqAssign;
            Compiler(trees[0].Parent);
        }

        private void While(IList<Tree> trees)
        {
            int nl = ++_lb, ml = ++_lb;
            Add("label", "B" + nl, new Tree("B" + nl, Kind.Variable));
            Add("ne", "", Compiler(trees[0]));
            Add("jmp", "", new Tree("B" + ml, Kind.Variable));
            Compiler(trees[1]);
            Add("jmp", "", new Tree("B" + nl, Kind.Variable));
            Add("label", "B" + nl, new Tree("B" + ml, Kind.Variable));
        }
    }

    public partial class Compile
    {
        public readonly Program String = new Program();
        private readonly List<Assembler> _structs = new List<Assembler>(15);
        private int _count;
        private int _lb;

        private void Class(IList<Tree> trees)
        {
            Add("class", trees[0].Content);
            foreach (var tree in trees)
                if (tree.Kind == Kind.Function)
                    Function(tree);
            Add(("end class"), trees[0].Content);
        }

        private Tree Compiler(Tree trees, string name = "")
        {
            if (trees == null)
                return null;
            if (trees.Count == 0)
                return trees;

            switch (trees.Kind)
            {
                case Kind.Const:
                case Kind.Numbre:
                case Kind.String:
                case Kind.Variable:
                    return trees;

                case Kind.Bloc:
                    Bloc(trees);
                    break;

                case Kind.Caller:
                    trees = Caller(trees, name: name);
                    break;

                case Kind.Array:
                    trees = Caller(trees, Kind.Array, name);
                    break;

                case Kind.Do:
                    Do(trees);
                    break;

                case Kind.While:
                    While(trees);
                    break;

                case Kind.For:
                    For(trees);
                    break;

                case Kind.EqAssign:
                    trees = EqAssigne(trees);
                    break;

                case Kind.TypeAssigne:
                    TypeAssigne(trees);
                    break;

                case Kind.Hyratachy:
                case Kind.Expression:
                case Kind.Parent:
                case Kind.Facteur:
                case Kind.Word:
                case Kind.Term:
                case Kind.Power:
                case Kind.Logic:
                    if (trees.Count > 2)
                        for (var j = 0; j < trees.Count & trees.Count > 1;) //j++
                            trees[j] = trees[0].Kind == Kind.Unair ? Unair(trees, j, name) : Operator(trees, j, name);

                        //j--;
                    else if (trees.Count == 2 & trees[0].Kind == Kind.Unair)
                        goto case Kind.Unair;
                    break;

                case Kind.Unair:
                    trees[0] = Unair(trees, 0, name);
                    break;

                case Kind.If:
                    If(trees);
                    break;

                case Kind.Goto:
                    Goto(trees);
                    break;

                case Kind.Function:
                    Function(trees);
                    break;

                case Kind.Class:
                    Class(trees);
                    break;

                case Kind.Space:
                    NameSpace(trees);
                    break;

                case Kind.Return:
                    Add("mov", "", Return, trees[1]);
                    trees.RemoveAt(0);
                    break;

                case Kind.Label:
                    Add("label", trees[0].Content);
                    break;

                case Kind.Param:
                    Compiler(trees[0]);
                    break;

                default:
                    String.Add(new Instruction("ERROR", trees.Kind.ToString()));
                    break;
            }

            if (trees.Count == 1) trees = trees[0];
            return trees;
        }

        private void Function(IList<Tree> trees)
        {
            Add("proc", trees[1].Content);
            Add("nop", "");
            Add("pop", "", new Tree("_IP", Kind.Variable));
            for (var j = trees[2].Count - 1; j >= 0; j--)
                Add("pop", "", trees[2][j][1]);
            Add("pop", "", new Tree("return", Kind.Variable));
            Compiler(trees[3]);
            Add("rop", "");
            Add("ret", "", new Tree("_IP", Kind.Variable));
            Add("end proc", trees[1].Content);
        }

        private void NameSpace(IList<Tree> trees)
        {
            Add("space", trees[0].Content);
            for (var i = 1; i < trees.Count; i++)
            {
                if (trees[i].Kind == Kind.Class)
                    Class(trees[i]);
                else NameSpace(trees[i]);
            }
            Add("end space", trees[0].Content);
        }

        private Tree Operator(List<Tree> trees, int i, string name = "")
        {
            var dname = string.IsNullOrEmpty(name);
            var T = new Assembler(dname ? "L" + ++_count : name);
            T.Set(trees[i + 1].Content);
            T.Param1 = Compiler(trees[i]);
            _count++;
            T.Param2 = Compiler(trees[i + 2]);
            var h = new Tree(new Builder(T.Name), trees[0].Parent, Kind.Variable) {Start = 0, End = T.Name.Length - 1};
            trees.RemoveRange(1, 2);
            var y = Consts.Operats.IndexOf(T.Fn[0]);
            Add(Consts.FOperats[y], "", T.Param1, T.Param2);
            Add("mov", "", new Tree(T.Name, Kind.Variable), new Tree("ax", Kind.Register));
            _count -= 1;
            if (dname) _count -= 1;
            return h;
        }

        private Tree Unair(IList<Tree> trees, int i, string name = "")
        {
            var dname = string.IsNullOrEmpty(name);
            var T = new Assembler(dname ? "L" + ++_count : name);
            T.Set(trees[i].Content);
            T.Param1 = Compiler(trees[1]);

            var h = new Tree(new Builder(T.Name), trees[0].Parent, Kind.Variable) {Start = 0, End = T.Name.Length - 1};
            trees.RemoveAt(1);

            var y = Consts.Unair.IndexOf(T.Fn[0]);
            Add(Consts.FUnair[y], "", T.Param1);
            Add("mov", "", new Tree(T.Name, Kind.Variable), new Tree("ax", Kind.Register));
            if (dname)
                _count -= 1;
            return h;
        }

        private void Add(string fn, string name, Tree p1 = null, Tree p2 = null)
        {
            var T = new Assembler(name);
            T.Set(fn);
            T.Param1 = Compiler(p1);
            T.Param2 = Compiler(p2);
            switch (fn)
            {
                case "pow":
                case "mul":
                case "div":
                case "add":
                case "sub":
                case "or":
                case "and":
                case "xor":
                case "inf":
                case "sup":
                case "jmp":
                case "ne":
                case "pop":
                case "push":
                case "ret":
                case "mem":
                case "call":
                case "upl":
                case "usu":
                case "mov":
                    String.Add(new Instruction(fn, T.Param1.Content, T.Param2 != null ? T.Param2.Content : ""));
                    break;

                case "proc":
                case "class":
                case "space":
                case "end proc":
                case "end class":
                case "end space":
                    String.Add(new Instruction(fn, name));
                    break;

                case "label":
                    String.Add(new Instruction(fn, name));
                    break;

                case "nop":
                    String.Add(new Instruction(fn));
                    break;

                default:
                    String.Add(new Instruction(fn, T.Param1 != null ? T.Param1.Content : name,
                        T.Param2 != null ? T.Param2.Content : ""));
                    break;
            }
            _structs.Add(T);
        }
    }

    public partial class Compile
    {
        public static readonly Tree Return = new Tree("return", Kind.Variable);

        public static bool Build(string sourceCode, out Program assemblerCode)
        {
            var b = new Builder(sourceCode);
            var g = new Complex(b);
            var parent = new Tree(b, null, Kind.Null);
            if (g.NameSpace(parent))
            {
                parent = parent[0];
                var r = new Compile();
                var e = r.Compiler(parent);
                assemblerCode = r.String;
                return true;
            }
            assemblerCode = null;
            return false;
        }

        public static bool Build(string sourceCode, out string assemblerCode)
        {
            Program prog;
            var t=Build(sourceCode, out prog);
            assemblerCode = prog.ToString();
            return t; 
        }

        public static bool Build(string fileIn, string fileOut)
        {
            var sourceCode = File.ReadAllText(fileIn);
            Program prog;
            var t = Build(sourceCode, out prog);
            if (t)
            {
                var s = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                s.Flush();
                var c = System.Text.Encoding.ASCII.GetBytes(prog.ToString());
                s.Write(c, 0, c.Length);
                s.Close();
            }
            return t;
        }
        
    }
}