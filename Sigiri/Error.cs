using System;
using System.Collections.Generic;
using System.Text;

namespace Sigiri
{
    class Error
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public Position Position { get; set; }
        public Error(Position position, string name, string details)
        {
            this.Position = position;
            this.Name = name;
            this.Details = details;
        }

        public override string ToString()
        {
            return Name + ": " + Details + "\n"
                + " File " + Position.FileName + ", at line " + (Position.LineNum+1);
        }
    }

    class InvalidCharError : Error {
        public InvalidCharError(Position position, string details) : base(position, "InvalidCharacter", details)
        {
        }
    }

    class InvalidSyntaxError : Error {
        public InvalidSyntaxError(Position position, string details) : base(position, "InvalidSyntax", details)
        {

        }
    }

    class RuntimeError : Error {
        private Context Context { get; set; }
        public RuntimeError(Position position, string details, Context context) : base(position, "RuntimeError", details)
        {
            this.Context = context;
        }
        public override string ToString()
        {
            return Name + ": " + Details + "\n" + TraceBack();
        }

        private string TraceBack() {
            string result = "";
            Context ctx = Context;
            Position position = Position;
            while (ctx != null) {
                if (position != null) 
                    result += " File '" + position.FileName + "' at Line " + (position.LineNum + 1) + " in " + ctx.Name + result;
                else
                    result += " in " + ctx.Name + result;
                position = ctx.Position;
                ctx = ctx.Parent;
            }
            return "\nMost recent call last,\n" + result;
        }
    }
}
