using System;
using System.Collections.Generic;
using System.Text;

namespace Sigiri
{
    class Tokenizer
    {
        private readonly string SkipChars = " \t";
        private readonly string Digits = "0123456789_";
        private readonly string HexDigits = "0123456789ABCDEFabcdef_";
        private readonly string OctDigits = "01234567_";
        private readonly string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz_$";
        private readonly string[] Keywords = { "and", "or", "not", "in", 
                                               "if", "elif", "else",
                                               "for", "to", "step", "each",
                                               "while", "do", "until",
                                               "when", "default",
                                               "method", "class",
                                               "return", "break", "continue", "load",
                                               "int", "long", "big" };
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
        private char GetCharAt(int index)
        {
            if (index < code.Length)
                return code[index];
            return '\0';
        }

        public TokenizerResult GenerateTokens() {
            List<Token> tokens = new List<Token>();
            while (currentChar != '\0') {
                if (SkipChars.Contains(currentChar))
                    Advance();
                else if (currentChar == '0') {
                    if (Peek() == 'x' || Peek() == 'X')
                    {
                        TokenizerResult result = MakeHexNumber();
                        if (result.HasError) return result;
                        tokens.Add(result.SingleToken);
                    }
                    else if (Peek() == 'o' || Peek() == 'O')
                    {
                        TokenizerResult result = MakeOctNumber();
                        if (result.HasError) return result;
                        tokens.Add(result.SingleToken);
                    }
                    else if (Peek() == 'b' || Peek() == 'B')
                    {
                        TokenizerResult result = MakeBinNumber();
                        if (result.HasError) return result;
                        tokens.Add(result.SingleToken);
                    }
                    else {
                        TokenizerResult result = MakeNumber();
                        if (result.HasError)
                            return result;
                        tokens.Add(result.SingleToken);
                    }
                }
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
                    else if (Peek() == '/')
                    {
                        while (currentChar != '\n')
                        {
                            if (currentChar == '\0')
                                break;
                            Advance();
                        }
                        Advance();
                        continue;
                    }
                    else if (Peek() == '*')
                    {
                        SkipMultilineComment();
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

        private TokenizerResult MakeBinNumber()
        {
            Advance(2); //Advance 0b
            StringBuilder number = new StringBuilder("0"); // provide 0 for bigints otherwise it will return a negative number
            Position positionStart = currentPosition.Clone();

            while (currentChar != '\0' && (currentChar == '0'|| currentChar=='1'||currentChar=='_'))
            {
                if (currentChar == '_')
                {
                    Advance();
                    continue;
                }
                number.Append(currentChar);
                Advance();
            }
            try
            {
                int tryInt = Convert.ToInt32(number.ToString(), 2);
                return new TokenizerResult(new Token(TokenType.INTEGER, tryInt.ToString()).SetPosition(positionStart));
            }
            catch
            {
                System.Numerics.BigInteger bigInt = 0;
                long exp = 0;
                for (int i = number.Length - 1; i >= 0; i--)
                    bigInt += (number[i] - 48) * Util.BigIntPow(2, exp++);
                return new TokenizerResult(new Token(TokenType.BIGINTEGER, bigInt.ToString()).SetPosition(positionStart));
            }
        }

        private TokenizerResult MakeOctNumber()
        {
            Advance(2); //Advance 0o
            StringBuilder number = new StringBuilder("0"); // provide 0 for bigints otherwise it will return a negative number
            Position positionStart = currentPosition.Clone();

            while (currentChar != '\0' && OctDigits.Contains(currentChar))
            {
                if (currentChar == '_')
                {
                    Advance();
                    continue;
                }
                number.Append(currentChar);
                Advance();
            }
            try
            {

                int tryInt = Convert.ToInt32(number.ToString(), 8);
                return new TokenizerResult(new Token(TokenType.INTEGER, tryInt.ToString()).SetPosition(positionStart));
            }
            catch
            {
                System.Numerics.BigInteger bigInt = 0;
                long exp = 0;
                for (int i = number.Length - 1; i >= 0; i--)
                    bigInt += (number[i] - 48) * Util.BigIntPow(8, exp++);
                Console.WriteLine(bigInt);
                return new TokenizerResult(new Token(TokenType.BIGINTEGER, bigInt.ToString()).SetPosition(positionStart));
            }
        }

        private TokenizerResult MakeHexNumber() {
            Advance(2); //Advance 0x
            StringBuilder number = new StringBuilder("0"); // provide 0 for bigints otherwise it will return a negative number
            Position positionStart = currentPosition.Clone();

            while (currentChar != '\0' && HexDigits.Contains(currentChar))
            {
                if (currentChar == '_')
                {
                    Advance();
                    continue;
                }
                number.Append(currentChar);
                Advance();
            }
            try {
                int tryInt = Convert.ToInt32(number.ToString(), 16);
                return new TokenizerResult(new Token(TokenType.INTEGER, tryInt.ToString()).SetPosition(positionStart));
            } catch {
                System.Numerics.BigInteger tryBigInt = System.Numerics.BigInteger.Parse(number.ToString(), System.Globalization.NumberStyles.AllowHexSpecifier);
                return new TokenizerResult(new Token(TokenType.BIGINTEGER, tryBigInt.ToString()).SetPosition(positionStart));
            }
        }

        private void SkipMultilineComment()
        {
            int subCmtCount = 0;
            while (currentChar != '\0')
            {
                if (currentChar == '*' && Peek() == '/')
                {
                    subCmtCount -= 1;
                    Advance();
                    Advance();
                }
                else if (currentChar == '/' && Peek() == '*')
                {
                    subCmtCount += 1;
                    Advance();
                    Advance();
                }
                else
                    Advance();
                if (subCmtCount == 0)
                    return;
            }
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

        private Token PeekNumberUntil(char chr) {
            int index = currentPosition.Index + 1; // +1 for skip leading char (maybe + sign)
            StringBuilder number = new StringBuilder();
            char cChar = GetCharAt(index);
            int skipCount = 0;

            while (cChar != '\0' && (Digits.Contains(cChar) || cChar == '.' || cChar == '-')) {
                skipCount++;
                number.Append(cChar);
                cChar = GetCharAt(++index);
            }
            if (GetCharAt(index) == chr && !Letters.Contains(GetCharAt(index+1))) {
                Advance(skipCount + 2);
                return new Token(TokenType.FLOAT, number.ToString()).SetPosition(currentPosition);
            }
            return null;
            
        }

        private TokenizerResult MakeNumber() {
            StringBuilder number = new StringBuilder();
            int dotCount = 0;
            Position positionStart = currentPosition.Clone();

            while (currentChar != '\0' && (Digits.Contains(currentChar) || currentChar == '.' || currentChar == '+')) {
                if (currentChar == '_')
                {
                    Advance();
                    continue;
                }
                if (currentChar == '+') {
                    if (Peek() == 'i')
                        break;
                    if (Peek() == '-' && Peek(2) == 'i')
                        break;
                    Token token = PeekNumberUntil('i');
                    if (token == null)
                        break;
                    else
                        return new TokenizerResult(new Token(TokenType.COMPLEX, number.ToString(), token.Value.ToString()).SetPosition(positionStart));
                }
                if (currentChar == '.')
                    dotCount++;
                number.Append(currentChar);
                Advance();
            }

            if (dotCount == 0)
            {
                //todo try parse into long
                int tryInt;
                if (int.TryParse(number.ToString(), out tryInt))
                    return new TokenizerResult(new Token(TokenType.INTEGER, number.ToString()).SetPosition(positionStart));
                else
                    return new TokenizerResult(new Token(TokenType.BIGINTEGER, number.ToString()).SetPosition(positionStart));
            }
            else if (dotCount == 1)
                return new TokenizerResult(new Token(TokenType.FLOAT, number.ToString()).SetPosition(positionStart));
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
