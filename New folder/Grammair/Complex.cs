using Compiler.Decompiler.Bases;
using Compiler.Global;
using Compiler.Grammair.Bases;
using Microsoft.Win32;
using System.Collections.Generic;

namespace Compiler.Grammair
{
    public partial class Complex : Basic
    {
        internal readonly List<string> Errors;
        public Complex(Builder builder)
            : base(builder)
        {
            var hKey = Registry.LocalMachine;
            Errors = new List<string>(5);
        }

        public override Tree Parse(Builder builder)
        {
            var tree = new Tree(builder, null, Kind.Null);
            return NameSpace(tree) ? tree[0] : null;
        }

        internal bool Assigne(Tree parent)
        {
            var vB = EqAssigne(parent);
            if (!vB) vB = TypeAssigne(parent);
            return vB;
        }

        internal bool Bloc(Tree parent)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, Kind.Bloc);
            if (ESpace() & !KeyWord(null, "{"))
                return Builder.Leave(false);
            while (Instruction(T)) ;
            return T.Set(KeyWord(null, "}"));
        }

        internal bool Boucle(Tree parent)
        {
            Builder.Save();
            var vB = If(parent);
            if (!vB) vB = For(parent);
            if (!vB) vB = While(parent);
            if (!vB) vB = Do(parent);
            if (!vB) vB = Bloc(parent);
            return Builder.Leave(vB);
        }

        internal bool Caller(Tree parent, bool cA = true)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, cA ? Kind.Caller : Kind.Array);
            var vB = Heratachy(T);
            if (vB) vB = Parametre(T, cA);
            return T.Set(vB);
        }

        internal bool Class(Tree parent)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, Kind.Class);
            var vB = KeyWord(null, "class");
            if (vB)
                vB = Variable(T);
            if (vB)
                vB = KeyWord(T, ":") ? Heratachy(T) & KeyWord(null, "{") : KeyWord(null, "{");
            var end = vB;
            while (vB)
            {
                vB = Function(T);
                if (!vB) vB = TypeAssigne(T) & KeyWord(null, ";");
            }
            if (end) end = KeyWord(null, "}");
            return T.Set(end);
        }

        internal bool CWord(Tree parent)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, Kind.Hyratachy);
            bool vB1 = Word(T), vB;
            if (vB1)
                do
                {
                    vB = KeyWord(null, ".");
                    if (vB) vB = Word(T);
                } while (vB);
            if (T.Count == 1)
            {
                T = T[0];
                T.Parent = parent;
            }
            return T.Set(vB1);
        }

        internal bool DeclaredParam(Tree parent)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, Kind.DeclareParam);
            var vB = false;
            if (Heratachy(T))
                if (Variable(T))
                    vB = true;
            return T.Set(vB);
        }

        internal bool DeclaredParams(Tree parent)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, Kind.DeclareParams);
            if (!KeyWord(null, "(")) return Builder.Leave(false);
            var vB = DeclaredParam(T);
            while (vB)
            {
                vB = KeyWord(null, ",");
                if (vB) vB = DeclaredParam(T);
            }
            return T.Set(KeyWord(null, ")"));
        }

        internal bool EqAssigne(Tree parent)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, Kind.EqAssign);
            var vB = Heratachy(T);
            if (vB) vB = KeyWord(null, "=");
            if (vB)
            {
                if (Caller(T, false)) ;
                else if (Caller(T)) ;
                else vB = Expression(T);
            }
            return T.Set(vB);
        }

        internal bool Expression(Tree parent, int i = 0)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, (Kind)(50 + i));
            bool vB;
            var vB1 = i < 4 ? Expression(T, ++i) : CWord(T);
            if (vB1)
                do
                {
                    vB = Contain(Consts.Operators[i - 1].ToCharArray(), T);
                    if (vB) vB = Expression(T, i);
                } while (vB);
            if (T.Count == 1)
            {
                T[0].Parent = parent;
                T = T[0];
            }
            return T.Set(vB1);
        }

        internal bool Function(Tree parent)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, Kind.Function);
            var vB = false;
            if (KeyWord(null, "function"))
            {
                vB = Heratachy(T);
                if (vB) Variable(T);
                if (vB) vB = DeclaredParams(T);
                if (vB) vB = Bloc(T) & ESpace();
            }
            return T.Set(vB);
        }

        internal bool Heratachy(Tree parent)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, Kind.Hyratachy);
            bool vB1 = Variable(T), vB;
            if (vB1)
                do
                {
                    vB = KeyWord(null, ".");
                    if (vB) vB = Variable(T);
                } while (vB);
            if (T.Count == 1)
            {
                T = T[0];
                T.Parent = parent;
                parent.Add(T);
                return Builder.Leave(vB1);
            }
            return T.Set(vB1);
        }

        internal bool If(Tree parent)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, Kind.If);
            var isif = Ifs(T);
            if (isif)
                if (KeyWord(null, "else"))

                    isif = Instruction(T);
            return T.Set(isif);
        }

        internal bool Ifs(Tree parent)
        {
            Builder.Save();
            var vB = KeyWord(null, "if");
            if (vB) vB = KeyWord(null, "(");
            if (vB) vB = Expression(parent);
            if (vB) vB = KeyWord(null, ")");
            if (vB) vB = Instruction(parent);
            return Builder.Leave(vB);
        }

        internal bool Instruction(Tree parent, bool wC = true)
        {
            Builder.Save();
            if (Boucle(parent)) return Builder.Leave(true);
            var vB = Caller(parent);
            if (!vB) vB = Goto(parent);
            if (!vB)
            {
                vB = Label(parent);
                if (vB) wC = false;
            }
            if (!vB) vB = Return(parent);
            if (!vB) vB = Assigne(parent);
            if (!vB) vB = Expression(parent);
            if (wC & vB) vB = KeyWord(null, ";");
            return Builder.Leave(vB);
        }

        internal bool Library(Tree parent)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, Kind.Link);
            var vB = KeyWord(null, "^");
            if (vB) vB = Heratachy(T);
            if (vB) vB = KeyWord(null, ";");
            return T.Set(vB);
        }

        private bool Derective(Tree parent)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, Kind.Derective);
            var vB = KeyWord(null, "^");
            if (vB) vB &= Heratachy(T) & ESpace() & KeyWord(null, ";");
            return T.Set(vB);
        }

        private bool Derectives(Tree parent)
        {
            var T = new Tree(Builder, parent, Kind.Derectives);
            while (Derective(T)) ;
            return true;
        }

        public bool File(Tree parent)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, Kind.File);
            Derectives(T);
            while (NameSpace(T)) ;
            return T.Set(!Builder.Open);
        }

        public bool NameSpace(Tree parent)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, Kind.Space);
            var vB = KeyWord(null, "space");
            if (vB) vB = Heratachy(T);
            if (vB) vB = KeyWord(null, "{");
            var end = vB;
            while (vB)
            {
                vB = Class(T);
                if (!vB) vB = NameSpace(T);
            }
            if (end)
                end = KeyWord(null, "}");
            if (!end)
                SetError("space");
            return T.Set(end);
        }

        internal bool Parametre(Tree parent, bool pB = true)
        {
            Builder.Save();
            string o = pB ? "(" : "[", c = pB ? ")" : "]";
            var T = new Tree(Builder, parent, Kind.Param);
            if (!KeyWord(null, o)) return Builder.Leave(false);
            var vB = Expression(T);
            while (vB)
            {
                vB = KeyWord(null, ",") & Expression(T);
            }
            return T.Set(KeyWord(null, c));
        }

        internal bool Parent(Tree parent)
        {
            Builder.Save();
            if (!KeyWord(null, "(")) return Builder.Leave(false);
            bool vB = Expression(parent), vB1 = KeyWord(null, ")");
            if (vB1 & !vB)
                Errors.Add("Fattal Error From :  <" + Builder.PilePos[Builder.PilePos.Count - 1] + "> To :  <" +
                           Builder.CurrentPos + ">");
            return Builder.Leave(vB1);
        }

        internal bool Program(Tree parent)
        {
            var vB = true;
            do
            {
                vB = Library(parent);
                vB |= NameSpace(parent);
                vB |= Class(parent);
            } while (vB);
            return false;
        }

        internal bool Return(Tree parent)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, Kind.Return);
            return !KeyWord(T, "return") ? Builder.Leave(false) : T.Set(Expression(T));
        }

        internal void SetError(string message)
        {
            Errors.Add(message + "in :" + Builder.CurrentPos);
        }

        internal bool TypeAssigne(Tree parent)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, Kind.TypeAssigne);
            var vB = false;
            if (Heratachy(T))
            {
                if (Variable(T))
                {
                    vB = true;
                    if (KeyWord(null, "="))
                        vB = Expression(T);
                }
            }
            return T.Set(vB);
        }
    }

    public partial class Complex
    {
        internal bool Do(Tree parent)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, Kind.Do);
            var vB = false;
            if (KeyWord(null, "do"))
                if (Instruction(T) & KeyWord(null, "while") &
                    Parametre(T))
                    vB = true;
            return T.Set(vB);
        }

        internal bool For(Tree parent)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, Kind.For);
            var vB = false;
            if (KeyWord(null, "for"))
                if (KeyWord(null, "(") & Assigne(T) &
                    KeyWord(null, ";") & Expression(T) & KeyWord(null, ";") &
                    Instruction(T, false) & KeyWord(null, ")"))
                    if (Instruction(T))
                        vB = true;
            return T.Set(vB);
        }

        internal bool While(Tree parent)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, Kind.While);
            var vB = false;
            if (KeyWord(null, "while"))
                if (KeyWord(null, "(") & Expression(T) & KeyWord(null, ")"))
                    if (Instruction(T))
                        vB = true;
            return T.Set(vB);
        }

        internal bool Word(Tree parent)
        {
            Builder.Save();
            bool
                un = Unaire(parent),
                vB = Caller(parent);
            if (!vB) vB = Caller(parent, false);
            if (!vB) vB = Heratachy(parent);
            if (!vB) vB = Numbre(parent);
            if (!vB) vB = String(parent);
            if (!vB) vB = Parent(parent);
            if (!vB & un)
                parent.RemoveAt(parent.Count - 1);
            return Builder.Leave(vB);
        }
    }
}