using System;
using System.Collections.Generic;
using System.Text;

namespace Sigiri
{
    class Position
    {
        public int Index { get; set; }
        public string FileName { get; set; }
        public int LineNum { get; set; }
        public int ColumnNum { get; set; }


        public Position(int index, string fname, int line, int column)
        {
            this.Index = index;
            this.FileName = fname;
            this.LineNum = line;
            this.ColumnNum = column;
        }

        public void Advance(char chr) {
            Index++;
            ColumnNum++;
            if (chr == '\n') {
                ColumnNum = 0;
                LineNum++;
            }
        }


        public override string ToString()
        {
            return FileName + " - StartLine:" + LineNum + ", Index:" + Index + ", Col:" + ColumnNum;
        }

        public Position Clone() {
            return new Position(Index, FileName, LineNum, ColumnNum);
        }
    }
}
