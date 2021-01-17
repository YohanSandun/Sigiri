using System;
using System.Collections.Generic;
using System.Text;

namespace Sigiri
{
    class Context
    {
        public string Name { get; set; }
        public Context Parent { get; set; }
        public bool Break { get; set; }
        public bool Continue { get; set; }
        public bool Return { get; set; }

        private Dictionary<string, Values.Value> symbolTable;
        public Position Position { get; set; }
        public Context(string name, Context parent = null)
        {
            this.Parent = parent;
            this.Name = name;
            this.symbolTable = new Dictionary<string, Values.Value>();
        }

        public void AddSymbol(string name, Values.Value value) {
            if (symbolTable.ContainsKey(name))
                symbolTable[name] = value;
            else
                symbolTable.Add(name, value);
        }

        public Values.Value GetSymbol(string name) {
            if (symbolTable.ContainsKey(name))
                return symbolTable[name];
            if (Parent != null)
                return Parent.GetSymbol(name);
            return null;
        }

        
    }
}
