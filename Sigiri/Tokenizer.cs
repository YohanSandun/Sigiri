using System;
using System.Collections.Generic;
using System.Text;

namespace Sigiri
{
    class Tokenizer
    {
        private readonly string SkipChars = " \t";
        private readonly string Digits = "0123456789";

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
                else if (Digits.Contains(currentChar)) {
                    TokenizerResult result = MakeNumber();
                    if (result.HasError)
                        return result;
                    tokens.Add(result.SingleToken);
                }
                else if (currentChar == '+')
                {
                    tokens.Add(new Token(TokenType.PLUS).SetPosition(currentPosition.Clone()));
                    Advance();
                }
                else if (currentChar == '-')
                {
                    tokens.Add(new Token(TokenType.MINUS).SetPosition(currentPosition.Clone()));
                    Advance();
                }
                else if (currentChar == '*')
                {
                    if (Peek() == '*')
                    {
                        tokens.Add(new Token(TokenType.EXPONENT).SetPosition(currentPosition.Clone()));
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
                    tokens.Add(new Token(TokenType.DIVIDE).SetPosition(currentPosition.Clone()));
                    Advance();
                }
                else if (currentChar == '%')
                {
                    tokens.Add(new Token(TokenType.MODULUS).SetPosition(currentPosition.Clone()));
                    Advance();
                }
                else
                    return new TokenizerResult(new InvalidCharError(currentPosition, currentChar.ToString()));
            }
            return new TokenizerResult(tokens);
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
                return new TokenizerResult(new Token(TokenType.INTEGER, int.Parse(number.ToString())).SetPosition(currentPosition.Clone()));
            else if (dotCount == 1)
                return new TokenizerResult(new Token(TokenType.FLOAT, double.Parse(number.ToString())).SetPosition(currentPosition.Clone()));
            else
                return new TokenizerResult(new InvalidSyntaxError(positionStart, "More than one dot found in number literal"));
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
