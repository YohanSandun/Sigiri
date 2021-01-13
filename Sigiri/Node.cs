using System.Collections.Generic;

namespace Sigiri
{
    enum NodeType { 
        NUMBER,
        STRING,
        LIST,
        DICTIONARY,
        BINARY,
        UNARY,
        VAR_ACCESS,
        VAR_ASSIGN,
        SUBSCRIPT,
        SUBSCRIPT_ASSIGN,
        BLOCK,
        IF,
        FOR,
        FOR_EACH,
        WHILE,
        DO,
        WHEN,
        METHOD,
        CLASS,
        CALL,
        RETURN,
        BREAK,
        CONTINUE,
        ATTRIBUTE,
        LOAD
    }

    abstract class Node
    {
        public NodeType Type { get; set; }
        public override abstract string ToString();
        public Node(NodeType type)
        {
            this.Type = type;
        }
    }

    class NumberNode : Node {
        public Token Token { get; set; }
        public NumberNode(Token token) : base (NodeType.NUMBER)
        {
            this.Token = token;
        }
        public override string ToString()
        {
            return Token.Value.ToString();
        }
    }
    class StringNode : Node
    {
        public Token Token { get; set; }
        public StringNode(Token token) : base(NodeType.STRING)
        {
            this.Token = token;
        }
        public override string ToString()
        {
            return Token.Value.ToString();
        }
    }

    class BinaryNode : Node { 
        public Node LeftNode { get; set; }
        public Node RightNode { get; set; }
        public Token OpToken { get; set; }
        public BinaryNode(Node left, Token opTok, Node right) : base (NodeType.BINARY)
        {
            this.LeftNode = left;
            this.OpToken = opTok;
            this.RightNode = right;
        }
        public override string ToString()
        {
            return "(" + LeftNode + ", " + OpToken + ", " + RightNode.ToString() + "" + ")";
        }
    }

    class UnaryNode : Node { 
        public Node Node { get; set; }
        public Token Token { get; set; }
        public UnaryNode(Token token, Node node) : base(NodeType.UNARY)
        {
            this.Node = node;
            this.Token = token;
        }

        public override string ToString()
        {
            return "(" + Token + ", " + Node + ")";
        }
    }

    class VarAccessNode : Node { 
        public Token Token { get; set; }
        public VarAccessNode(Token token):base(NodeType.VAR_ACCESS)
        {
            this.Token = token;
        }
        public override string ToString()
        {
            return Token.ToString();
        }
    }

    class VarAssignNode : Node { 
        public Node Node { get; set; }
        public Token Token { get; set; }
        public VarAssignNode(Token token, Node node):base(NodeType.VAR_ASSIGN)
        {
            this.Node = node;
            this.Token = token;
        }

        public override string ToString()
        {
            return "(" + Token + "=" + Node + ")";
        }
    }

    class SubscriptAssignNode : Node
    {
        public Node Node { get; set; }
        public Node Target { get; set; }
        public SubscriptAssignNode(Node target, Node node) : base(NodeType.SUBSCRIPT_ASSIGN)
        {
            this.Node = node;
            this.Target = target;
        }

        public override string ToString()
        {
            return "(" + Target + "=" + Node + ")";
        }
    }

    class ListNode : Node { 
        public List<Node> ElementNodes { get; set; }
        public Position Position { get; set; }
        public ListNode(List<Node> elements) : base(NodeType.LIST)
        {
            this.ElementNodes = elements;
        }
        public override string ToString()
        {
            return "(LIST)"; //todo: list 
        }
        public Node SetPosition(Position position) {
            this.Position = position;
            return this;
        }
    }

    class SubscriptNode : Node { 
        public Node BaseNode { get; set; }
        public Node IndexNode { get; set; }
        public Position Position { get; set; }
        public SubscriptNode(Node baseNode, Node index) : base(NodeType.SUBSCRIPT)
        {
            this.BaseNode = baseNode;
            this.IndexNode = index;
        }

        public override string ToString()
        {
            return "(" + BaseNode + "[" + IndexNode + "])";
        }
        public Node SetPosition(Position position)
        {
            this.Position = position;
            return this;
        }
    }

    class BlockNode : Node { 
        public List<Node> Statements { get; set; }
        public Position Position { get; set; }
        public BlockNode(List<Node> statements) : base(NodeType.BLOCK)
        {
            this.Statements = statements;
        }

        public override string ToString()
        {
            string str = "BLOCK {\n";
            for (int i = 0; i < Statements.Count; i++)
            {
                str += " " + Statements[i] + "\n";
            }
            return str + "}";
        }
        public Node SetPosition(Position position)
        {
            this.Position = position;
            return this;
        }
    }

    class IfNode : Node
    {
        public List<(Node, Node)> Cases { get; set; }
        public Node ElseCase { get; set; }
        public Position Position { get; set; }
        public IfNode(List<(Node, Node)> cases, Node elseCase) : base(NodeType.IF)
        {
            this.Cases = cases;
            this.ElseCase = elseCase;
        }

        public override string ToString()
        {
            return "IF";
        }
        public Node SetPosition(Position position)
        {
            this.Position = position;
            return this;
        }
    }

    class ForNode : Node {
        public Token Token { get; set; }
        public Node Start { get; set; }
        public Node End { get; set; }
        public Node Step { get; set; }
        public Node Body { get; set; }
        public ForNode(Token token, Node start, Node end, Node step, Node body) : base(NodeType.FOR)
        {
            this.Token = token;
            this.Start = start;
            this.End = end;
            this.Step = step;
            this.Body = body;
        }
        public override string ToString()
        {
            return "FOR";
        }
    }

    class WhileNode : Node { 
        public Node Body { get; set; }
        public Node Condition { get; set; }
        public Position Position { get; set; }
        public WhileNode(Node condition, Node body) : base(NodeType.WHILE)
        {
            this.Body = body;
            this.Condition = condition;
        }

        public override string ToString()
        {
           return "WHILE";
        }
        public Node SetPosition(Position position)
        {
            this.Position = position;
            return this;
        }
    }
    class DoNode : Node
    {
        public Node Body { get; set; }
        public Node Condition { get; set; }
        public Position Position { get; set; }
        public DoNode(Node condition, Node body) : base(NodeType.DO)
        {
            this.Body = body;
            this.Condition = condition;
        }

        public override string ToString()
        {
            return "DO";
        }
        public Node SetPosition(Position position)
        {
            this.Position = position;
            return this;
        }
    }

    class WhenNode : Node { 
        public Token Token { get; set; }
        public List<(Node, Node)> Cases { get; set; }
        public WhenNode(Token token, List<(Node, Node)> cases) : base(NodeType.WHEN)
        {
            this.Token = token;
            this.Cases = cases;
        }
        public override string ToString()
        {
            return "WHEN";
        }
    }

    class MethodNode : Node {
        public Node Body { get; set; }
        public List<string> Parameters { get; set; }
        public Token Token { get; set; }

        public Dictionary<string, Node> DefaultValues { get; set; }
        public Position Position { get; set; }
        public MethodNode(Token token, List<string> parameters, Node body, Dictionary<string, Node> defaultValues) : base(NodeType.METHOD)
        {
            this.Token = token;
            this.Parameters = parameters;
            this.Body = body;
            this.DefaultValues = defaultValues;
        }

        public override string ToString()
        {
            return "METHOD";
        }
        public Node SetPosition(Position position) {
            this.Position = position;
            return this;
        }
    }

    class CallNode : Node { 
        public Node Node { get; set; }
        public List<(string, Node)> Arguments { get; set; }
        public Position Position { get; set; }
        public CallNode(Node node, List<(string, Node)> arguments) : base(NodeType.CALL)
        {
            this.Node = node;
            this.Arguments = arguments;
        }
        public Node SetPosition(Position position) {
            this.Position = position;
            return this;
        }
        public override string ToString()
        {
            return "CALL";
        }
    }

    class ReturnNode : Node { 
        public Node Node { get; set; }
        public Position Position { get; set; }
        public ReturnNode(Node node) : base(NodeType.RETURN)
        {
            this.Node = node;
        }
        public Node SetPosition(Position position)
        {
            this.Position = position;
            return this;
        }
        public override string ToString()
        {
            return "RETURN";
        }
    }

    class BreakNode : Node {
        public Position Position { get; set; }
        public Node SetPosition(Position position)
        {
            this.Position = position;
            return this;
        }
        public BreakNode() : base(NodeType.BREAK)
        {

        }

        public override string ToString()
        {
            return "BREAK";
        }
    }
    class ContinueNode : Node
    {
        public Position Position { get; set; }
        public Node SetPosition(Position position)
        {
            this.Position = position;
            return this;
        }
        public ContinueNode() : base(NodeType.CONTINUE)
        {

        }

        public override string ToString()
        {
            return "CONTINUE";
        }
    }

    class ClassNode : Node
    {
        public Node Body { get; set; }
        public Token Token { get; set; }
        public Token BaseClassToken { get; set; }
        public ClassNode(Token token, Node body, Token baseClass=null) : base(NodeType.CLASS)
        {
            this.Token = token;
            this.Body = body;
            this.BaseClassToken = baseClass;
        }

        public override string ToString()
        {
            return "CLASS";
        }
    }

    class AttributeNode : Node {
        public Node BaseNode { get; set; }
        public Node Node { get; set; }
        public AttributeNode(Node baseNode, Node node) : base(NodeType.ATTRIBUTE)
        {
            this.BaseNode = baseNode;
            this.Node = node;
        }

        public override string ToString()
        {
            return "ATTRIBUTE";
        }
    }

    class LoadNode : Node
    {
        public Token Token { get; set; }
        public Token ClassToken { get; set; }
        public LoadNode(Token token, Token classToken = null) : base(NodeType.LOAD)
        {
            this.Token = token;
            this.ClassToken = classToken;
        }

        public override string ToString()
        {
            return "LOAD " + Token.ToString();
        }
    }

    class ForEachNode : Node { 
        public Token Token { get; set; }
        public Token Iteratable { get; set; }
        public Node Body { get; set; }
        public ForEachNode(Token token, Token iteratable, Node body):base(NodeType.FOR_EACH)
        {
            this.Token = token;
            this.Iteratable = iteratable;
            this.Body = body;
        }

        public override string ToString()
        {
            return "FOR_EACH"; 
        }
    }

    class DictionaryNode : Node { 
        public List<(Node, Node)> KeyValuePairs { get; set; }
        public Position Position { get; set; }
        public DictionaryNode(List<(Node, Node)> pairs) : base(NodeType.DICTIONARY)
        {
            this.KeyValuePairs = pairs;
        }
        public Node SetPosition(Position position) {
            this.Position = position;
            return this;
        }
        public override string ToString()
        {
            return "DICTIONARY";
        }
    }
}
