using System;
using System.Collections.Generic;
using System.Text;

namespace Sigiri
{
    class Parser
    {
        private List<Token> tokens;
        private Token currentToken;
        private int currentTokenIndex;
        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            currentTokenIndex = -1;
            Advance();
        }
        private void Advance(int step = 1) {
            currentTokenIndex += step;
            if (currentTokenIndex < tokens.Count)
                currentToken = tokens[currentTokenIndex];
        }

        private Token Peek(int step = 1) {
            if (currentTokenIndex + 1 < tokens.Count)
                return tokens[currentTokenIndex + 1];
            return currentToken;
        }

        private void SkipNewLines() {
            while (currentToken.Type == TokenType.NEWLINE)
                Advance();
        }

        public ParserResult Parse() {
            ParserResult result = Block(TokenType.EOF);
            if (!result.HasError && currentToken.Type != TokenType.EOF)
                return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Unknown error"));
            return result;
        }

        private ParserResult ListExpr() {
            Advance();
            List<Node> elements = new List<Node>();
            if (currentToken.Type == TokenType.RIGHT_SQB)
            {
                Advance();
                return new ParserResult(new ListNode(elements).SetPosition(currentToken.Position));
            }
            ParserResult exprResult = Expr();
            if (exprResult.HasError) return exprResult;
            elements.Add(exprResult.Node);

            while (currentToken.Type == TokenType.COMMA) {
                Advance();
                exprResult = Expr();
                if (exprResult.HasError) return exprResult;
                elements.Add(exprResult.Node);
            }

            if (currentToken.Type != TokenType.RIGHT_SQB)
                return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Expected ']' or ','"));
            Advance();
            if (currentToken.Type == TokenType.LEFT_SQB)
                return Subscript(new ListNode(elements).SetPosition(currentToken.Position));
            return new ParserResult(new ListNode(elements).SetPosition(currentToken.Position));
        }

        private ParserResult Subscript(Node baseNode) {
            Advance();
            ParserResult exprResult = Expr();
            if (exprResult.HasError) return exprResult;
            
            if (currentToken.Type != TokenType.RIGHT_SQB)
                return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Expected ']'"));
            Advance();
            if (currentToken.Type == TokenType.LEFT_SQB)
                return Subscript(new SubscriptNode(baseNode, exprResult.Node).SetPosition(currentToken.Position));
            if (currentToken.Type == TokenType.EQUALS) {
                Advance();
                ParserResult parserResult = Expr();
                if (parserResult.HasError) return parserResult;
                return new ParserResult(new SubscriptAssignNode(new SubscriptNode(baseNode, exprResult.Node).SetPosition(currentToken.Position), parserResult.Node));
            }
            return new ParserResult(new SubscriptNode(baseNode, exprResult.Node).SetPosition(currentToken.Position));
        }

        private ParserResult Attribute(Node baseNode) {
            Advance();
            ParserResult nodeResult = Expr();
            if (nodeResult.HasError) return nodeResult;
            if (currentToken.Type == TokenType.DOT)
                return Attribute(new AttributeNode(baseNode, nodeResult.Node));
            return new ParserResult(new AttributeNode(baseNode, nodeResult.Node));
        }

        private ParserResult IfStmt() {
            Advance();

            List<(Node, Node)> cases = new List<(Node, Node)>();
            Node elseCase = null;

            ParserResult condResult = Expr();
            if (condResult.HasError) return condResult;

            if (currentToken.Type == TokenType.COLON)
            {
                Advance();
                SkipNewLines();
                ParserResult caseResult = Expr();
                if (caseResult.HasError) return caseResult;
                cases.Add((condResult.Node, caseResult.Node));

                SkipNewLines();
                while (currentToken.CheckKeyword("elif")) {
                    Advance();
                    condResult = Expr();
                    if (condResult.HasError) return condResult;
                    if (currentToken.Type == TokenType.COLON)
                    {
                        Advance();
                        SkipNewLines();
                        caseResult = Expr();
                        if (caseResult.HasError) return caseResult;
                        cases.Add((condResult.Node, caseResult.Node));
                        SkipNewLines();
                    }
                    else {
                        SkipNewLines();
                        if (currentToken.Type == TokenType.LEFT_BRA)
                        {
                            Advance();
                            SkipNewLines();
                            caseResult = Block();
                            if (caseResult.HasError) return caseResult;
                            cases.Add((condResult.Node, caseResult.Node));
                            Advance();
                            SkipNewLines();
                        }
                        else
                            return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Expected ':' or '{'"));
                    }
                }

                SkipNewLines();
                if (currentToken.CheckKeyword("else")) {
                    Advance();
                    if (currentToken.Type == TokenType.COLON)
                    {
                        Advance();
                        SkipNewLines();
                        ParserResult elseResult = Expr();
                        if (elseResult.HasError) return elseResult;
                        elseCase = elseResult.Node;
                    }
                    else {
                        SkipNewLines();
                        if (currentToken.Type == TokenType.LEFT_BRA)
                        {
                            Advance();
                            SkipNewLines();
                            ParserResult elseResult = Block();
                            if (elseResult.HasError) return elseResult;
                            elseCase = elseResult.Node;
                            Advance();
                            SkipNewLines();
                        }
                        else
                            return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Expected ':' or '{'"));
                    }
                }
            }
            else {
                SkipNewLines();
                if (currentToken.Type == TokenType.LEFT_BRA) {
                    Advance();
                    ParserResult caseResult = Block();
                    if (caseResult.HasError) return caseResult;
                    cases.Add((condResult.Node, caseResult.Node));
                    Advance();
                    SkipNewLines();
                    while (currentToken.CheckKeyword("elif")) {
                        Advance();
                        condResult = Expr();
                        if (condResult.HasError) return condResult;
                        if (currentToken.Type == TokenType.COLON)
                        {
                            Advance();
                            SkipNewLines();
                            caseResult = Expr();
                            if (caseResult.HasError) return caseResult;
                            cases.Add((condResult.Node, caseResult.Node));
                            SkipNewLines();
                        }
                        else
                        {
                            SkipNewLines();
                            if (currentToken.Type == TokenType.LEFT_BRA)
                            {
                                Advance();
                                SkipNewLines();
                                caseResult = Block();
                                if (caseResult.HasError) return caseResult;
                                cases.Add((condResult.Node, caseResult.Node));
                                Advance();
                                SkipNewLines();
                            }
                            else
                                return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Expected ':' or '{'"));
                        }
                    }

                    SkipNewLines();
                    if (currentToken.CheckKeyword("else")) {
                        Advance();
                        if (currentToken.Type == TokenType.COLON)
                        {
                            Advance();
                            SkipNewLines();
                            ParserResult elseResult = Expr();
                            if (elseResult.HasError) return elseResult;
                            elseCase = elseResult.Node;
                        }
                        else
                        {
                            SkipNewLines();
                            if (currentToken.Type == TokenType.LEFT_BRA) {
                                Advance();
                                SkipNewLines();
                                ParserResult elseResult = Block();
                                if (elseResult.HasError) return elseResult;
                                elseCase = elseResult.Node;
                                Advance();
                                SkipNewLines();
                            }else
                                return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Expected ':' or '{'"));
                        }
                    }
                }else
                    return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Expected ':' or '{'"));
            }
            return new ParserResult(new IfNode(cases, elseCase).SetPosition(currentToken.Position));
        }

        private ParserResult ForStmt() {
            Node start, end;
            Node step = null;
            Advance();
            Token token = currentToken;
            if (token.Type != TokenType.IDENTIFIER)
                return new ParserResult(new InvalidSyntaxError(token.Position, "Expected an identifier"));
            Advance();
            if (currentToken.Type != TokenType.EQUALS)
                return new ParserResult(new InvalidSyntaxError(token.Position, "Expected '='"));
            Advance();
            ParserResult startResult = BitwiseOr();
            if (startResult.HasError) return startResult;
            start = startResult.Node;
            if (!currentToken.CheckKeyword("to"))
                return new ParserResult(new InvalidSyntaxError(token.Position, "Expected keyword 'to'"));
            Advance();
            ParserResult endResult = BitwiseOr();
            if (endResult.HasError) return endResult;
            end = endResult.Node;
            if (currentToken.CheckKeyword("step")) {
                Advance();
                ParserResult stepResult = BitwiseOr();
                if (stepResult.HasError) return stepResult;
                step = stepResult.Node;
            }
            if (currentToken.Type == TokenType.COLON)
            {
                Advance();
                SkipNewLines();
                ParserResult parserResult = Expr();
                if (parserResult.HasError) return parserResult;
                return new ParserResult(new ForNode(token, start, end, step, parserResult.Node));
            }
            else {
                SkipNewLines();
                if (currentToken.Type == TokenType.LEFT_BRA) {
                    Advance();
                    SkipNewLines();
                    ParserResult parserResult = Block();
                    if (parserResult.HasError) return parserResult;
                    Advance();
                    return new ParserResult(new ForNode(token, start, end, step, parserResult.Node));
                }else
                    return new ParserResult(new InvalidSyntaxError(token.Position, "Expected ':' or '{'"));
            }
        }

        private ParserResult ForEachStmt() {
            Advance(2);
            Token token = currentToken;
            if (currentToken.Type != TokenType.IDENTIFIER)
                return new ParserResult(new InvalidSyntaxError(token.Position, "Expected an identifier"));
            Advance();
            if (currentToken.Type != TokenType.IN)
                return new ParserResult(new InvalidSyntaxError(token.Position, "Expected 'in'"));
            Advance();
            ParserResult iteratable = Expr();
            if (iteratable.HasError) return iteratable;
            if (currentToken.Type == TokenType.COLON)
            {
                Advance();
                SkipNewLines();
                ParserResult parserResult = Expr();
                if (parserResult.HasError) return parserResult;
                return new ParserResult(new ForEachNode(token, iteratable.Node, parserResult.Node));
            }
            else
            {
                SkipNewLines();
                if (currentToken.Type == TokenType.LEFT_BRA)
                {
                    Advance();
                    SkipNewLines();
                    ParserResult parserResult = Block();
                    if (parserResult.HasError) return parserResult;
                    Advance();
                    return new ParserResult(new ForEachNode(token, iteratable.Node, parserResult.Node));
                }
                else
                    return new ParserResult(new InvalidSyntaxError(token.Position, "Expected ':' or '{'"));
            }
        }

        private ParserResult WhileStmt() {
            Advance();
            ParserResult condResult = Expr();
            if (condResult.HasError) return condResult;
            if (currentToken.Type == TokenType.COLON)
            {
                Advance();
                SkipNewLines();
                ParserResult bodyResult = Expr();
                if (bodyResult.HasError) return bodyResult;
                return new ParserResult(new WhileNode(condResult.Node, bodyResult.Node).SetPosition(currentToken.Position));
            }
            else {
                SkipNewLines();
                if (currentToken.Type == TokenType.LEFT_BRA)
                {
                    Advance();
                    SkipNewLines();
                    ParserResult bodyResult = Block();
                    if (bodyResult.HasError) return bodyResult;
                    Advance();
                    return new ParserResult(new WhileNode(condResult.Node, bodyResult.Node).SetPosition(currentToken.Position));
                }
                else
                    return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Expected ':' or '{'"));
            }
        }

        private ParserResult DoStmt() {
            Advance();
            if (currentToken.Type == TokenType.COLON)
            {
                Advance();
                SkipNewLines();
                ParserResult exprResult = Expr();
                if (exprResult.HasError) return exprResult;
                if (!currentToken.CheckKeyword("until"))
                    return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Expected keyword 'until'"));
                Advance();
                ParserResult condResutl = Expr();
                if (condResutl.HasError) return condResutl;
                return new ParserResult(new DoNode(condResutl.Node, exprResult.Node).SetPosition(currentToken.Position));
            }
            else {
                SkipNewLines();
                if (currentToken.Type == TokenType.LEFT_BRA) {
                    Advance();
                    SkipNewLines();
                    ParserResult bodyResult = Block();
                    if (bodyResult.HasError) return bodyResult;
                    Advance();
                    ParserResult condResutl = Expr();
                    if (condResutl.HasError) return condResutl;
                    return new ParserResult(new DoNode(condResutl.Node, bodyResult.Node).SetPosition(currentToken.Position));
                }else
                    return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Expected ':' or '{'"));
            }
        }

        private ParserResult WhenStmt() {
            //TODO complete
            Advance();
            Token token = currentToken;
            if (token.Type != TokenType.IDENTIFIER)
                return new ParserResult(new InvalidSyntaxError(token.Position, "Expected an identifier"));
            Advance();
            SkipNewLines();
            if (currentToken.Type != TokenType.LEFT_BRA)
                return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Expected '{'"));
            Advance();
            List<(Node, Node)> cases = new List<(Node, Node)>();
            while (currentToken.Type != TokenType.RIGHT_BRA) {
                SkipNewLines();
                ParserResult parserResult = Expr();
                if (parserResult.HasError) return parserResult;
                if (currentToken.Type != TokenType.COLON)
                    return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Expected ':'"));
                Advance();
                SkipNewLines();
                if (currentToken.Type == TokenType.LEFT_BRA)
                {
                    Advance();
                    SkipNewLines();
                    ParserResult bodyResult = Block();
                    if (bodyResult.HasError) return bodyResult;
                    Advance();
                    cases.Add((parserResult.Node, bodyResult.Node));
                    SkipNewLines();
                }
                else
                {
                    ParserResult exprResult = Expr();
                    if (exprResult.HasError) return exprResult;
                    cases.Add((parserResult.Node, exprResult.Node));
                    SkipNewLines();
                }
            }
            Advance();
            return new ParserResult(new WhenNode(token, cases));
        }

        private ParserResult Method() {
            //todo avoid use true,false,null as param names
            Advance();
            Token token = null;
            List<string> parameters = new List<string>();
            Dictionary<string, Node> defaUltValues = new Dictionary<string, Node>();

            if (currentToken.Type == TokenType.IDENTIFIER)
            {
                token = currentToken;
                Advance();
            }
            if (currentToken.Type != TokenType.LEFT_PAREN)
                return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Expected '('"));
            Advance();
            if (currentToken.Type == TokenType.RIGHT_PAREN)
                Advance();
            else { 
                if (currentToken.Type != TokenType.IDENTIFIER)
                    return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Expected an identifer"));
                string paramName = currentToken.Value.ToString();
                parameters.Add(paramName);
                Advance();
                if (currentToken.Type == TokenType.EQUALS) {
                    Advance();
                    ParserResult result = BitwiseOr();
                    if (result.HasError) return result;
                    defaUltValues.Add(paramName, result.Node);
                }
                while (currentToken.Type == TokenType.COMMA) {
                    Advance();
                    if (currentToken.Type != TokenType.IDENTIFIER)
                        return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Expected an identifer or ')'"));
                    paramName = currentToken.Value.ToString();
                    parameters.Add(paramName);
                    Advance();
                    if (currentToken.Type == TokenType.EQUALS)
                    {
                        Advance();
                        ParserResult result = BitwiseOr();
                        if (result.HasError) return result;
                        defaUltValues.Add(paramName, result.Node);
                    }
                }
                if (currentToken.Type != TokenType.RIGHT_PAREN)
                    return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Expected ')'"));
                Advance();
            }
            if (currentToken.Type == TokenType.COLON)
            {
                Advance();
                SkipNewLines();
                ParserResult parserResult = Expr();
                if (parserResult.HasError) return parserResult;
                return new ParserResult(new MethodNode(token, parameters, parserResult.Node, defaUltValues).SetPosition(currentToken.Position));
            }
            else {
                SkipNewLines();
                if (currentToken.Type == TokenType.LEFT_BRA) {
                    Advance();
                    SkipNewLines();
                    ParserResult parserResult = Block();
                    if (parserResult.HasError) return parserResult;
                    Advance();
                    return new ParserResult(new MethodNode(token, parameters, parserResult.Node, defaUltValues).SetPosition(currentToken.Position));
                }else
                    return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Expected '{' or ':'"));
            }
        }

        private ParserResult ReturnStmt() {
            Advance();
            if (currentToken.Type == TokenType.NEWLINE || currentToken.Type == TokenType.RIGHT_BRA)
                return new ParserResult(new ReturnNode(null).SetPosition(currentToken.Position));
            ParserResult exprResult = Expr();
            if (exprResult.HasError) return exprResult;
            return new ParserResult(new ReturnNode(exprResult.Node).SetPosition(currentToken.Position));
        }

        private ParserResult Class() {
            Advance();
            Token token = currentToken;
            if (token.Type != TokenType.IDENTIFIER)
                return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Expected an identifier"));
            Advance();
            Token baseClassToken = null;
            if (currentToken.Type == TokenType.COLON) {
                Advance();
                if (currentToken.Type != TokenType.IDENTIFIER)
                    return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Expected an identifier"));
                baseClassToken = currentToken;
                Advance();
            }
            SkipNewLines();
            if (currentToken.Type != TokenType.LEFT_BRA)
                return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Expected an '{'"));
            Advance();
            ParserResult bodyResult = Block();
            if (bodyResult.HasError) return bodyResult;
            Advance();
            return new ParserResult(new ClassNode(token, bodyResult.Node, baseClassToken));
        }

        private ParserResult LoadStmt() {
            Advance();
            Token token = currentToken;
            if (token.Type == TokenType.IDENTIFIER)
            {
                Advance();
                if (currentToken.Type == TokenType.DOT) {
                    Advance();
                    Token clsTok = currentToken;
                    if (clsTok.Type == TokenType.IDENTIFIER) {
                        Advance();
                        return new ParserResult(new LoadNode(token, clsTok));
                    }
                }
                else
                    return new ParserResult(new LoadNode(token));
            }
            return new ParserResult(new InvalidSyntaxError(token.Position, "Expected identifier or string"));
        }

        private ParserResult DictionaryExpr() {
            Advance();

            ParserResult keyExpr = Expr();
            if (keyExpr.HasError) return keyExpr;

            if (currentToken.Type != TokenType.COLON)
                return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Expected ':'"));
            Advance();

            ParserResult valueExpr = Expr(); 
            if (valueExpr.HasError) return valueExpr;

            List<(Node, Node)> pairs = new List<(Node, Node)>();
            pairs.Add((keyExpr.Node, valueExpr.Node));

            while (currentToken.Type == TokenType.COMMA) {
                Advance();
                keyExpr = Expr();
                if (keyExpr.HasError) return keyExpr;
                if (currentToken.Type != TokenType.COLON)
                    return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Expected ':'"));
                Advance();
                valueExpr = Expr();
                if (valueExpr.HasError) return valueExpr;
                pairs.Add((keyExpr.Node, valueExpr.Node));
            }
            if (currentToken.Type != TokenType.RIGHT_BRA)
                return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Expected ',' or '}'"));
            Advance();
            return new ParserResult(new DictionaryNode(pairs).SetPosition(currentToken.Position));
        }

        private ParserResult Atom() {
            Token token = currentToken;
            if (token.Type == TokenType.INTEGER || token.Type == TokenType.FLOAT)
            {
                Advance();
                return new ParserResult(new NumberNode(token));
            }
            else if (token.Type == TokenType.STRING)
            {
                Advance();
                if (currentToken.Type == TokenType.LEFT_SQB)
                    return Subscript(new StringNode(token));
                return new ParserResult(new StringNode(token));
            }
            else if (token.Type == TokenType.IDENTIFIER)
            {
                Advance();
                if (currentToken.Type == TokenType.LEFT_SQB)
                    return Subscript(new VarAccessNode(token));
                if (currentToken.Type == TokenType.DOT)
                    return Attribute(new VarAccessNode(token));
                return new ParserResult(new VarAccessNode(token));
            }
            else if (token.Type == TokenType.LEFT_PAREN)
            {
                Advance();
                ParserResult exprResult = Expr();
                if (exprResult.HasError) return exprResult;
                if (currentToken.Type != TokenType.RIGHT_PAREN)
                    return new ParserResult(new InvalidSyntaxError(token.Position, "Expected ')'"));
                Advance();
                return exprResult;
            }
            else if (token.Type == TokenType.LEFT_SQB)
                return ListExpr();
            else if (token.Type == TokenType.LEFT_BRA)
                return DictionaryExpr();
            else if (token.CheckKeyword("if"))
                return IfStmt();
            else if (token.CheckKeyword("for"))
            {
                if (Peek().CheckKeyword("each"))
                    return ForEachStmt();
                return ForStmt();
            }
            else if (token.CheckKeyword("while"))
                return WhileStmt();
            else if (token.CheckKeyword("do"))
                return DoStmt();
            else if (token.CheckKeyword("when"))
                return WhenStmt();
            else if (token.CheckKeyword("method"))
                return Method();
            else if (token.CheckKeyword("class"))
                return Class();
            else if (token.CheckKeyword("load"))
                return LoadStmt();
            else if (token.CheckKeyword("return"))
                return ReturnStmt();
            else if (token.CheckKeyword("break"))
            {
                Advance();
                return new ParserResult(new BreakNode().SetPosition(currentToken.Position));
            }
            else if (token.CheckKeyword("continue"))
            {
                Advance();
                return new ParserResult(new ContinueNode().SetPosition(currentToken.Position));
            }
            return new ParserResult(new InvalidSyntaxError(token.Position, "Expected int or float"));
        }

        private ParserResult Call() {
            ParserResult atomResult = Atom();
            if (atomResult.HasError) return atomResult;
            if (currentToken.Type == TokenType.LEFT_PAREN)
            {
                Advance();
                List<(string, Node)> arguments = new List<(string, Node)>();
                if (currentToken.Type == TokenType.RIGHT_PAREN)
                    Advance();
                else
                {
                    string paramName = "";
                    if (currentToken.Type == TokenType.IDENTIFIER && Peek().Type == TokenType.COLON) {
                        paramName = currentToken.Value.ToString();
                        Advance(2);

                    }
                    ParserResult exprResult = Expr();
                    if (exprResult.HasError) return exprResult;
                    arguments.Add((paramName, exprResult.Node));
                    while (currentToken.Type == TokenType.COMMA)
                    {
                        paramName = "";
                        Advance();
                        if (currentToken.Type == TokenType.IDENTIFIER && Peek().Type == TokenType.COLON)
                        {
                            paramName = currentToken.Value.ToString();
                            Advance(2);
                        }
                        exprResult = Expr();
                        if (exprResult.HasError) return exprResult;
                        arguments.Add((paramName, exprResult.Node));
                    }
                    if (currentToken.Type != TokenType.RIGHT_PAREN)
                        return new ParserResult(new InvalidSyntaxError(currentToken.Position, "Expected ')' or ','"));
                    Advance();
                }
                if (currentToken.Type == TokenType.LEFT_SQB)
                    return Subscript(new CallNode(atomResult.Node, arguments).SetPosition(currentToken.Position));
                else
                    return new ParserResult(new CallNode(atomResult.Node, arguments).SetPosition(currentToken.Position));
            }
            return atomResult;
        }

        private ParserResult Complement() {
            Token token = currentToken;
            if (token.Type == TokenType.COMPLEMENT) {
                Advance();
                ParserResult factorResult = Factor();
                if (factorResult.HasError) return factorResult;
                return new ParserResult(new UnaryNode(token, factorResult.Node));
            }
            return Call();
        }

        private ParserResult Exponent()
        {
            ParserResult leftResult = Complement();
            if (leftResult.HasError) return leftResult;
            while (currentToken.Type == TokenType.EXPONENT)
            {
                Token token = currentToken;
                Advance();
                ParserResult rightResult = Factor();
                if (rightResult.HasError) return rightResult;
                leftResult = new ParserResult(new BinaryNode(leftResult.Node, token, rightResult.Node));
            }
            return leftResult;
        }

        private ParserResult Factor() {
            Token token = currentToken;
            if (token.Type == TokenType.PLUS || token.Type == TokenType.MINUS) {
                Advance();
                ParserResult factorResult = Factor();
                if (factorResult.HasError) return factorResult;
                return new ParserResult(new UnaryNode(token, factorResult.Node));
            }
            return Exponent();
        }

        private ParserResult Term() {
            ParserResult leftResult = Factor();
            if (leftResult.HasError) return leftResult;
            while (currentToken.Type == TokenType.MULTIPLY || currentToken.Type == TokenType.DIVIDE || currentToken.Type == TokenType.MODULUS) {
                Token token = currentToken;
                Advance();
                ParserResult rightResult = Factor();
                if (rightResult.HasError) return rightResult;
                leftResult = new ParserResult(new BinaryNode(leftResult.Node, token, rightResult.Node));
            }
            return leftResult;
        }

        private ParserResult ArithExpr()
        {
            ParserResult leftResult = Term();
            if (leftResult.HasError) return leftResult;
            while (currentToken.Type == TokenType.PLUS || currentToken.Type == TokenType.MINUS)
            {
                Token token = currentToken;
                Advance();
                ParserResult rightResult = Term();
                if (rightResult.HasError) return rightResult;
                leftResult = new ParserResult(new BinaryNode(leftResult.Node, token, rightResult.Node));
            }
            return leftResult;
        }

        private ParserResult Shift() {
            ParserResult leftResult = ArithExpr();
            if (leftResult.HasError) return leftResult;
            while (currentToken.Type == TokenType.LEFT_SHIFT || currentToken.Type == TokenType.RIGHT_SHIFT)
            {
                Token token = currentToken;
                Advance();
                ParserResult rightResult = ArithExpr();
                if (rightResult.HasError) return rightResult;
                leftResult = new ParserResult(new BinaryNode(leftResult.Node, token, rightResult.Node));
            }
            return leftResult;
        }

        private ParserResult BitwiseAnd()
        {
            ParserResult leftResult = Shift();
            if (leftResult.HasError) return leftResult;
            while (currentToken.Type == TokenType.BITWISE_AND)
            {
                Token token = currentToken;
                Advance();
                ParserResult rightResult = Shift();
                if (rightResult.HasError) return rightResult;
                leftResult = new ParserResult(new BinaryNode(leftResult.Node, token, rightResult.Node));
            }
            return leftResult;
        }

        private ParserResult BitwiseXor()
        {
            ParserResult leftResult = BitwiseAnd();
            if (leftResult.HasError) return leftResult;
            while (currentToken.Type == TokenType.BITWISE_XOR)
            {
                Token token = currentToken;
                Advance();
                ParserResult rightResult = BitwiseAnd();
                if (rightResult.HasError) return rightResult;
                leftResult = new ParserResult(new BinaryNode(leftResult.Node, token, rightResult.Node));
            }
            return leftResult;
        }

        private ParserResult BitwiseOr()
        {
            ParserResult leftResult = BitwiseXor();
            if (leftResult.HasError) return leftResult;
            while (currentToken.Type == TokenType.BITWISE_OR)
            {
                Token token = currentToken;
                Advance();
                ParserResult rightResult = BitwiseXor();
                if (rightResult.HasError) return rightResult;
                leftResult = new ParserResult(new BinaryNode(leftResult.Node, token, rightResult.Node));
            }
            return leftResult;
        }

        private ParserResult CompareExpr()
        {
            if (currentToken.Type == TokenType.BOOLEAN_NOT)
            {
                Token token = currentToken;
                Advance();
                ParserResult exprResult = CompareExpr();
                if (exprResult.HasError) return exprResult;
                return new ParserResult(new UnaryNode(token, exprResult.Node));
            }

            ParserResult leftResult = BitwiseOr();
            if (leftResult.HasError) return leftResult;
            while (currentToken.Type == TokenType.EQUALS_EQUALS || currentToken.Type == TokenType.NOT_EQUALS ||
                   currentToken.Type == TokenType.LESS_THAN || currentToken.Type == TokenType.GREATER_THAN ||
                   currentToken.Type == TokenType.LESS_TOE || currentToken.Type == TokenType.GREATER_TOE)
            {
                Token token = currentToken;
                Advance();
                ParserResult rightResult = BitwiseOr();
                if (rightResult.HasError) return rightResult;
                leftResult = new ParserResult(new BinaryNode(leftResult.Node, token, rightResult.Node));
            }
            return leftResult;
        }

        private ParserResult Expr()
        {
            if (currentToken.Type == TokenType.IDENTIFIER && Peek().Type == TokenType.EQUALS) {
                Token token = currentToken;
                Advance(2);
                ParserResult exprResult = Expr();
                if (exprResult.HasError) return exprResult;
                return new ParserResult(new VarAssignNode(token, exprResult.Node));
            }
            ParserResult leftResult = CompareExpr();
            if (leftResult.HasError) return leftResult;
            while (currentToken.Type == TokenType.BOOLEAN_AND || currentToken.Type == TokenType.BOOLEAN_OR || currentToken.Type == TokenType.IN)
            {
                Token token = currentToken;
                Advance();
                ParserResult rightResult = CompareExpr();
                if (rightResult.HasError) return rightResult;
                leftResult = new ParserResult(new BinaryNode(leftResult.Node, token, rightResult.Node));
            }
            return leftResult;
        }

        private ParserResult Block(TokenType terminator = TokenType.RIGHT_BRA) {
            List<Node> statements = new List<Node>();
            while (true) {
                SkipNewLines();
                if (currentToken.Type == terminator)
                    break;
                ParserResult parserResult = Expr();
                if (parserResult.HasError) return parserResult;
                statements.Add(parserResult.Node);
            }
            return new ParserResult(new BlockNode(statements).SetPosition(currentToken.Position));
        }
    }

    class ParserResult { 
        public Node Node { get; set; }
        public Error Error { get; set; }
        public bool HasError { get { return Error != null; } }
        public ParserResult(Node node)
        {
            this.Node = node;
        }
        public ParserResult(Error error)
        {
            this.Error = error;
        }
    }
}
