using Compiler.Decompiler.Bases;
using Compiler.Global;
using Compiler.Grammair.Bases;
using System.Collections.Generic;
using System.Data;

namespace Compiler.Grammair
{
    public abstract partial class Basic
    {
        protected readonly Builder Builder;

        protected Basic(Builder builder)
        {
            Builder = builder;
        }

        public static bool Contain(string chainChars, char charact)
        {
            for (var i = 0; i < chainChars.Length; i++)
                if (chainChars[i] == charact) return true;
            return false;
        }

        public bool Chiffre(bool Int = false)
        {
            Builder.Save();
            PMun();
            bool dot = false,
                start = true
                ;
            while (Builder.Open)
            {
                if (char.IsDigit(Builder.Current))
                {
                }
                else if (Builder.Current == '.' & !Int)
                    if (dot) throw new InvalidExpressionException();
                    else dot = true;
                else
                    break;
                start = false;
                Builder.Next();
            }
            return Builder.Leave(!start);
        }

        public bool Constant(Tree parent)
        {
            Builder.Save();
            var vB = String(parent);
            if (!vB)
                vB = Numbre(parent);
            return Builder.Leave(vB);
        }

        public bool Operator(Tree parent)
        {
            Builder.Save();
            var vB = false;
            if (Builder.Open)
            {
                vB = Contain(Consts.Operats, Builder.Current);
                if (vB)
                    Builder.Next();
            }
            return Builder.Leave(vB);
        }

        public abstract Tree Parse(Builder builder);

        public bool String(Tree parent)
        {
            Builder.Save();
            if (Builder.Close) return Builder.Leave(false);
            var sC = Builder.Current;
            var T = new Tree(Builder, parent, Kind.String);
            if (KeyWord(T, "\"") | KeyWord(T, "\'"))
            {
                while (Builder.Open)
                {
                    if (Builder.Current == sC)
                    {
                        Builder.Next();
                        return T.Set();
                    }
                    Builder.Next();
                }
            }
            return Builder.Leave(false);
        }

        public bool Unaire(Tree parent)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, Kind.Unair);
            var vB = false;
            if (Builder.Open)
            {
                if (Contain(Consts.Unair, Builder.Current))
                {
                    vB = true;
                    Builder.Next();
                }
            }
            return T.Set(vB);
        }

        public bool Variable(Tree parent)
        {
            Builder.Save();
            var start = ESpace();
            var T = new Tree(Builder, parent, Kind.Variable);
            while (Builder.Open)
            {
                if (char.IsDigit(Builder.Current))
                {
                    if (start)
                        return Builder.Leave(false);
                }
                else if (char.IsLetter(Builder.Current))
                {
                }
                else
                    break;

                start = false;
                Builder.Next();
            }
            return T.Set(!start) & ESpace();
        }

        protected bool Contain(IList<char> list, Tree parent)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, Kind.Operator);
            if (Builder.Open)
                if (list.Contains(Builder.Current))
                {
                    Builder.Next();
                    return T.Set();
                }
            return Builder.Leave(false);
        }

        protected bool ESpace()
        {
            while (Builder.Open)
            {
                if (!Contain(Consts.Eps, Builder.Current))
                    break;
                Builder.Next();
            }
            return true;
        }

        protected bool KeyWord(Tree parent, string keyWord)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, Kind.KeyWord);
            foreach (var item in keyWord)
            {
                if (Builder.Open)
                    if (item == Builder.Current)
                    {
                        Builder.Next();
                        continue;
                    }

                return Builder.Leave(false);
            }
            return T.Set() & ESpace();
        }

        protected bool Numbre(Tree parent)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, Kind.Numbre);
            var n1 = ESpace() & Chiffre();
            if (n1)
                if (KeyWord(null, "e") || KeyWord(null, "E"))
                    n1 = Chiffre();
            return T.Set(n1) & ESpace();
        }

        protected void PMun()
        {
            Builder.Save();
            var vB = false;
            while (Builder.Open)
            {
                if (!Contain(Consts.Operators[0], Builder.Current))
                    break;
                vB = true;
                Builder.Next();
            }
            Builder.Leave(vB);
        }
    }

    public abstract partial class Basic
    {
        public bool Goto(Tree parent)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, Kind.Goto);
            return !KeyWord(T, "goto") ? Builder.Leave(false) : T.Set(Variable(T));
        }

        public bool Label(Tree parent)
        {
            Builder.Save();
            var T = new Tree(Builder, parent, Kind.Label);
            var vB = Variable(T);
            if (vB) vB = KeyWord(null, ":");
            return T.Set(vB);
        }
    }
}