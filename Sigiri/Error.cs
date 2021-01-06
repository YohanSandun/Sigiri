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
}
