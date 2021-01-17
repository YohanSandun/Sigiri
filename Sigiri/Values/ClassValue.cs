using System;

namespace Sigiri.Values
{
    class ClassValue : Value
    {
        public Node Body { get; set; }
        public string Name { get; set; }
        public Value BaseClass { get; set; } = null;
        public string BaseName { get; set; } = "";
        public ClassValue(string name, Node body, Value baseClass, string baseName) : base(ValueType.CLASS)
        {
            this.Body = body;
            this.Name = name;
            this.BaseClass = baseClass;
            this.BaseName = baseName;
        }
        public override string ToString()
        {
            return "<class-" + Name + ">";
        }

        public ClassValue Clone() {
            return new ClassValue(Name, Body, BaseClass, BaseName);
        }

        public RuntimeResult InitializeBaseClass(Context ctx, Interpreter interpreter) {
            if (BaseClass != null) {
                ClassValue classValue = ((ClassValue)BaseClass).Clone();
                //classValue.Type = ValueType.OBJECT;
                if (classValue.BaseClass != null)
                {
                    RuntimeResult baseResult = classValue.InitializeBaseClass(ctx, interpreter);
                    if (baseResult.HasError) return baseResult;

                    Context newContext = new Context(BaseName, baseResult.Value.Context);
                    classValue.SetPositionAndContext(Position, newContext);
                    newContext.AddSymbol("this", classValue);
                    newContext.AddSymbol("base", baseResult.Value);
                    return interpreter.Visit(classValue.Body, newContext);
                }
                else {
                    Context newContext = new Context(BaseName, ctx);
                    classValue.SetPositionAndContext(Position, newContext);
                    newContext.AddSymbol("this", classValue);
                    return interpreter.Visit(classValue.Body, newContext);
                }
            }
            return new RuntimeResult(new RuntimeError(Position, "Base class '" + BaseName + "' not found!", ctx));
        }
    }
}
