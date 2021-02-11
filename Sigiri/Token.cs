namespace Sigiri
{
    enum TokenType { 
        INTEGER,
        LONG,
        FLOAT,
        STRING,
        BIGINTEGER,
        COMPLEX,
        BYTE_ARRAY,

        IDENTIFIER,
        KEYWORD,

        PLUS,
        MINUS,
        MULTIPLY,
        DIVIDE,
        MODULUS,
        EXPONENT,

        COMPLEMENT,
        BITWISE_AND,
        BITWISE_OR,
        BITWISE_XOR,
        LEFT_SHIFT,
        RIGHT_SHIFT,

        BOOLEAN_AND,
        BOOLEAN_OR,
        BOOLEAN_NOT,
        IN,

        EQUALS_EQUALS,
        NOT_EQUALS,
        LESS_THAN,
        GREATER_THAN,
        LESS_TOE,       //Less than or equal
        GREATER_TOE,    //Greater than or equal

        EQUALS,
        PLS_EQ,
        MIN_EQ,
        MUL_EQ,
        DIV_EQ,
        MOD_EQ,
        EXP_EQ,
        AND_EQ,
        XOR_EQ,
        COM_EQ,
        LS_EQ,
        RS_EQ,
        OR_EQ,

        COLON,
        COMMA,
        DOT,

        LEFT_PAREN,     //Left parentheses
        RIGHT_PAREN,    //Right parentheses
        LEFT_SQB,       //Left square bracket
        RIGHT_SQB,      //Right square bracket
        LEFT_BRA,       //Left brace
        RIGHT_BRA,      //Right brace
        LEFT_DIA,       //Left diamond/angle bracket
        RIGHT_DIA,      //Right diamond/angle bracket

        NEWLINE,
        EOF
    }

    class Token
    {
        public TokenType Type { get; set; }
        public object Value { get; set; }
        public object SecondaryValue { get; set; }
        public Position Position { get; set; }
        public Token(TokenType type, object value = null, object secondVal = null)
        {
            this.Type = type;
            this.Value = value;
            this.SecondaryValue = secondVal;
        }

        public Token SetPosition (Position position) {
            this.Position = position;
            return this;
        }

        public override string ToString()
        {
            if (Value != null)
                return Type + ":" + Value;
            return Type.ToString();
        }

        public bool CheckKeyword(string id) {
            if (Type == TokenType.KEYWORD && Value.ToString().Equals(id))
                return true;
            return false;
        }
    }
}
