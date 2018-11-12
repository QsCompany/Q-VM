using System.Collections.Generic;

namespace Compiler.Grammair.Bases
{
    public static class Consts
    {
        public const string Eps = " \0\n\t\r\v";
        public const string Operats = "+-*/%\\^&~#<>|@_°¨";
        public const string Unair = "!+-";
        public static readonly List<string> Operators = new List<string> { "+-", "*/%\\", "^", "&~#<>|@_°¨" };
        public static string[] FUnair = { "ne", "upl", "usu" };

        internal static readonly List<string> FOperats = new List<string>
        {
            "add",
            "sub",
            "mul",
            "div",
            "rst",
            "idiv",
            "pow",
            "and",
            "eq",
            "neq",
            "inf",
            "sup",
            "or",
            "arb",
            "tir",
            "cmp",
            "sqr"
        };
    }
}