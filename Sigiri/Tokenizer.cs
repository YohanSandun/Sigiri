using System;
using System.Collections.Generic;
using System.Text;

namespace Sigiri
{
    class Tokenizer
    {
        private readonly string SkipChars = " \t";
        private readonly string Digits = "0123456789";
        private readonly string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_$";
        private readonly string[] Keywords = { "and", "or", "not", "in", 
                                               "if", "elif", "else",
                                               "for", "to", "step",
                                               "while", "do", "until",
                                               "when", "default",
                                               "method", "class",
                                               "return", "break", "continue" };
        private Dictionary<char, string> escapeChars = new Dictionary<char, string>()
        {
            { 'n', "\n"},
            { 'r', "\r"},
            { 'b', "\b"},
            { 't', "\t"}
        };
       
        private Position currentPosition;
        private char currentChar;
        private string code;
        public Tokenizer(string fileName, string code)
        {
            this.code = code;
            this.currentPosition = new Position(-1, fileName, 0, -1);
            Advance();
        }

        private void Advance(int step = 1) {
            for (int i = 0; i < step; i++)
                currentPosition.Advance(currentChar);
            if (currentPosition.Index < code.Length)
                currentChar = code[currentPosition.Index];
            else
                currentChar = '\0';
        }

        private char Peek(int step = 1) {
            if (currentPosition.Index + step < code.Length)
                return code[currentPosition.Index + step];
            return '\0';
        }

        public TokenizerResult GenerateTokens() {
            List<Token> tokens = new List<Token>();
            while (currentChar != '\0') {
                if (SkipChars.Contains(currentChar))
                    Advance();
                else if (Digits.Contains(currentChar))
                {
                    TokenizerResult result = MakeNumber();
                    if (result.HasError)
                        return result;
                    tokens.Add(result.SingleToken);
                }
                else if (Letters.Contains(currentChar))
                {
                    tokens.Add(MakeIdentifier());
                }
                else if (currentChar == '"') 
                {
                    tokens.Add(MakeString('"'));
                }
                else if (currentChar == '\'')
                {
                    tokens.Add(MakeString('\''));
                }
                else if (currentChar == '.')
                {
                    tokens.Add(new Token(TokenType.DOT).SetPosition(currentPosition.Clone()));
                    Advance();
                }
                else if (currentChar == ':')
                {
                    tokens.Add(new Token(TokenType.COLON).SetPosition(currentPosition.Clone()));
                    Advance();
                }
                else if (currentChar == '\n' || currentChar == ';')
                {
                    tokens.Add(new Token(TokenType.NEWLINE).SetPosition(currentPosition.Clone()));
                    Advance();
                }
                else if (currentChar == '+')
                {
                    if (Peek() == '=')
                    {
                        tokens.Add(new Token(TokenType.PLS_EQ).SetPosition(currentPosition.Clone()));
                        Advance(2);
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.PLUS).SetPosition(currentPosition.Clone()));
                        Advance();
                    }
                }
                else if (currentChar == '-')
                {
                    if (Peek() == '=')
                    {
                        tokens.Add(new Token(TokenType.MIN_EQ).SetPosition(currentPosition.Clone()));
                        Advance(2);
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.MINUS).SetPosition(currentPosition.Clone()));
                        Advance();
                    }
                }
                else if (currentChar == '*')
                {
                    if (Peek() == '*')
                    {
                        if (Peek(2) == '=')
                        {
                            tokens.Add(new Token(TokenType.EXP_EQ).SetPosition(currentPosition.Clone()));
                            Advance(3);
                        }
                        else
                        {
                            tokens.Add(new Token(TokenType.EXPONENT).SetPosition(currentPosition.Clone()));
                            Advance(2);
                        }
                    }
                    else if (Peek() == '=')
                    {
                        tokens.Add(new Token(TokenType.MUL_EQ).SetPosition(currentPosition.Clone()));
                        Advance(2);
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.MULTIPLY).SetPosition(currentPosition.Clone()));
                        Advance();
                    }
                }
                else if (currentChar == '/')
                {
                    if (Peek() == '=')
                    {
                        tokens.Add(new Token(TokenType.DIV_EQ).SetPosition(currentPosition.Clone()));
                        Advance(2);
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.DIVIDE).SetPosition(currentPosition.Clone()));
                        Advance();
                    }
                }
                else if (currentChar == '%')
                {
                    if (Peek() == '=')
                    {
                        tokens.Add(new Token(TokenType.MOD_EQ).SetPosition(currentPosition.Clone()));
                        Advance(2);
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.MODULUS).SetPosition(currentPosition.Clone()));
                        Advance();
                    }
                }
                else if (currentChar == '&')
                {
                    if (Peek() == '=')
                    {
                        tokens.Add(new Token(TokenType.AND_EQ).SetPosition(currentPosition.Clone()));
                        Advance(2);
                    }
                    else if (Peek() == '&')
                    {
                        tokens.Add(new Token(TokenType.BOOLEAN_AND).SetPosition(currentPosition.Clone()));
                        Advance(2);
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.BITWISE_AND).SetPosition(currentPosition.Clone()));
                        Advance();
                    }
                }
                else if (currentChar == '|')
                {
                    if (Peek() == '=')
                    {
                        tokens.Add(new Token(TokenType.OR_EQ).SetPosition(currentPosition.Clone()));
                        Advance(2);
                    }
                    else if (Peek() == '|')
                    {
                        tokens.Add(new Token(TokenType.BOOLEAN_OR).SetPosition(currentPosition.Clone()));
                        Advance(2);
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.BITWISE_OR).SetPosition(currentPosition.Clone()));
                        Advance();
                    }
                }
                else if (currentChar == '^')
                {
                    if (Peek() == '=')
                    {
                        tokens.Add(new Token(TokenType.XOR_EQ).SetPosition(currentPosition.Clone()));
                        Advance(2);
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.BITWISE_XOR).SetPosition(currentPosition.Clone()));
                        Advance();
                    }
                }
                else if (currentChar == '!')
                {
                    if (Peek() == '=')
                    {
                        tokens.Add(new Token(TokenType.NOT_EQUALS).SetPosition(currentPosition.Clone()));
                        Advance(2);
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.BOOLEAN_NOT).SetPosition(currentPosition.Clone()));
                        Advance();
                    }
                }
                else if (currentChar == '~')
                {
                    if (Peek() == '=')
                    {
                        tokens.Add(new Token(TokenType.COM_EQ).SetPosition(currentPosition.Clone()));
                        Advance(2);
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.COMPLEMENT).SetPosition(currentPosition.Clone()));
                        Advance();
                    }
                }
                else if (currentChar == '<')
                {
                    if (Peek() == '=')
                    {
                        tokens.Add(new Token(TokenType.LESS_TOE).SetPosition(currentPosition.Clone()));
                        Advance(2);
                    }
                    else if (Peek() == '<')
                    {
                        if (Peek(2) == '=')
                        {
                            tokens.Add(new Token(TokenType.LS_EQ).SetPosition(currentPosition.Clone()));
                            Advance(3);
                        }
                        else
                        {
                            tokens.Add(new Token(TokenType.LEFT_SHIFT).SetPosition(currentPosition.Clone()));
                            Advance(2);
                        }
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.LESS_THAN).SetPosition(currentPosition.Clone()));
                        Advance();
                    }
                }
                else if (currentChar == '>')
                {
                    if (Peek() == '=')
                    {
                        tokens.Add(new Token(TokenType.GREATER_TOE).SetPosition(currentPosition.Clone()));
                        Advance(2);
                    }
                    else if (Peek() == '>')
                    {
                        if (Peek(2) == '=')
                        {
                            tokens.Add(new Token(TokenType.RS_EQ).SetPosition(currentPosition.Clone()));
                            Advance(3);
                        }
                        else
                        {
                            tokens.Add(new Token(TokenType.RIGHT_SHIFT).SetPosition(currentPosition.Clone()));
                            Advance(2);
                        }
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.GREATER_THAN).SetPosition(currentPosition.Clone()));
                        Advance();
                    }
                }
                else if (currentChar == '=')
                {
                    if (Peek() == '=')
                    {
                        tokens.Add(new Token(TokenType.EQUALS_EQUALS).SetPosition(currentPosition.Clone()));
                        Advance(2);
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.EQUALS).SetPosition(currentPosition.Clone()));
                        Advance();
                    }
                }
                else if (currentChar == '(')
                {
                    tokens.Add(new Token(TokenType.LEFT_PAREN).SetPosition(currentPosition.Clone()));
                    Advance();
                }
                else if (currentChar == ')')
                {
                    tokens.Add(new Token(TokenType.RIGHT_PAREN).SetPosition(currentPosition.Clone()));
                    Advance();
                }
                else if (currentChar == '[')
                {
                    tokens.Add(new Token(TokenType.LEFT_SQB).SetPosition(currentPosition.Clone()));
                    Advance();
                }
                else if (currentChar == ']')
                {
                    tokens.Add(new Token(TokenType.RIGHT_SQB).SetPosition(currentPosition.Clone()));
                    Advance();
                }
                else if (currentChar == '{')
                {
                    tokens.Add(new Token(TokenType.LEFT_BRA).SetPosition(currentPosition.Clone()));
                    Advance();
                }
                else if (currentChar == '}')
                {
                    tokens.Add(new Token(TokenType.RIGHT_BRA).SetPosition(currentPosition.Clone()));
                    Advance();
                }
                else if (currentChar == ',')
                {
                    tokens.Add(new Token(TokenType.COMMA).SetPosition(currentPosition.Clone()));
                    Advance();
                }
                else
                    return new TokenizerResult(new InvalidCharError(currentPosition, currentChar.ToString()));
            }
            tokens.Add(new Token(TokenType.EOF).SetPosition(currentPosition.Clone()));
            return new TokenizerResult(tokens);
        }

        private Token MakeString(char strChar) {
            Advance();
            StringBuilder str = new StringBuilder();
            bool escapeChar = false;
            Position positionStart = currentPosition.Clone();

            while (currentChar != '\0' && (currentChar != strChar || escapeChar))
            {
                if (escapeChar)
                {
                    if (escapeChars.ContainsKey(currentChar))
                        str.Append(escapeChars[currentChar]);
                    else
                        str.Append(currentChar);
                    escapeChar = false;
                }
                else
                {
                    if (currentChar == '\\')
                        escapeChar = true;
                    else
                        str.Append(currentChar);
                }
                Advance();
            }
            Advance();
            return new Token(TokenType.STRING, str.ToString()).SetPosition(positionStart);
        }

        private TokenizerResult MakeNumber() {
            StringBuilder number = new StringBuilder();
            int dotCount = 0;
            Position positionStart = currentPosition.Clone();

            while (currentChar != '\0' && (Digits.Contains(currentChar) || currentChar == '.')) {
                if (currentChar == '.')
                    dotCount++;
                number.Append(currentChar);
                Advance();
            }

            if (dotCount == 0)
                return new TokenizerResult(new Token(TokenType.INTEGER, int.Parse(number.ToString())).SetPosition(positionStart));
            else if (dotCount == 1)
                return new TokenizerResult(new Token(TokenType.FLOAT, double.Parse(number.ToString())).SetPosition(positionStart));
            else
                return new TokenizerResult(new InvalidSyntaxError(positionStart, "More than one dot found in number literal"));
        }

        private Token MakeIdentifier() {
            StringBuilder name = new StringBuilder();
            Position positionStart = currentPosition.Clone();

            while (currentChar != '\0' && (Letters.Contains(currentChar) || Digits.Contains(currentChar))) {
                name.Append(currentChar);
                Advance();
            }

            if (IsKeyword(name.ToString()))
                return MakeKeyword(name.ToString(), positionStart);
            else
                return new Token(TokenType.IDENTIFIER, name).SetPosition(positionStart);
        }

        private bool IsKeyword(string name) {
            for (int i = 0; i < Keywords.Length; i++)
                if (name.Equals(Keywords[i]))
                    return true;
            return false;
        }

        private Token MakeKeyword(string name, Position position) {
            if (name.Equals("and"))
                return new Token(TokenType.BOOLEAN_AND).SetPosition(position);
            else if (name.Equals("or"))
                return new Token(TokenType.BOOLEAN_OR).SetPosition(position);
            else if (name.Equals("not"))
                return new Token(TokenType.BOOLEAN_NOT).SetPosition(position);
            else if (name.Equals("in"))
                return new Token(TokenType.IN).SetPosition(position);
            return new Token(TokenType.KEYWORD, name).SetPosition(position);
        }
    }

    class TokenizerResult {
        public List<Token> Tokens { get; set; }
        public Token SingleToken { get; set; }
        public Error Error { get; set; }
        public bool HasError { get { return Error != null; } }

        public TokenizerResult(List<Token> tokens)
        {
            this.Tokens = tokens;
        }

        public TokenizerResult(Token singleToken)
        {
            this.SingleToken = singleToken;
        }

        public TokenizerResult(Error error)
        {
            this.Error = error;
        }
    }
}
