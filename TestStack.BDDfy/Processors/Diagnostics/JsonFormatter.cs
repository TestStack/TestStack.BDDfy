using System.Text;

namespace TestStack.BDDfy.Processors.Diagnostics
{
    // http://www.limilabs.com/blog/json-net-formatter
    public class JsonFormatter
    {
        StringWalker _walker;
        IndentWriter _writer = new IndentWriter();
        StringBuilder _currentLine = new StringBuilder();
        bool _quoted;

        public JsonFormatter(string json)
        {
            _walker = new StringWalker(json);
            ResetLine();
        }

        public void ResetLine()
        {
            _currentLine.Length = 0;
        }

        public string Format()
        {
            while (MoveNextChar())
            {
                if (this._quoted == false && this.IsOpenBracket())
                {
                    this.WriteCurrentLine();
                    this.AddCharToLine();
                    this.WriteCurrentLine();
                    _writer.Indent();
                }
                else if (this._quoted == false && this.IsCloseBracket())
                {
                    this.WriteCurrentLine();
                    _writer.UnIndent();
                    this.AddCharToLine();
                }
                else if (this._quoted == false && this.IsColon())
                {
                    this.AddCharToLine();
                    this.WriteCurrentLine();
                }
                else
                {
                    AddCharToLine();
                }
            }
            this.WriteCurrentLine();
            return _writer.ToString();
        }

        private bool MoveNextChar()
        {
            bool success = _walker.MoveNext();
            if (this.IsApostrophe())
            {
                this._quoted = !_quoted;
            }
            return success;
        }

        public bool IsApostrophe()
        {
            return this._walker.CharAtIndex() == '"';
        }

        public bool IsOpenBracket()
        {
            return this._walker.CharAtIndex() == '{'
                || this._walker.CharAtIndex() == '[';
        }

        public bool IsCloseBracket()
        {
            return this._walker.CharAtIndex() == '}'
                || this._walker.CharAtIndex() == ']';
        }

        public bool IsColon()
        {
            return this._walker.CharAtIndex() == ',';
        }

        private void AddCharToLine()
        {
            this._currentLine.Append(_walker.CharAtIndex());
        }

        private void WriteCurrentLine()
        {
            string line = this._currentLine.ToString().Trim();
            if (line.Length > 0)
            {
                _writer.WriteLine(line);
            }
            this.ResetLine();
        }
    }

    public class IndentWriter
    {
        StringBuilder _sb = new StringBuilder();
        int _indent;

        public void Indent()
        {
            _indent++;
        }

        public void UnIndent()
        {
            if (_indent > 0)
                _indent--;
        }

        public void WriteLine(string line)
        {
            _sb.AppendLine(CreateIndent() + line);
        }

        private string CreateIndent()
        {
            StringBuilder indentString = new StringBuilder();
            for (int i = 0; i < _indent; i++)
                indentString.Append("    ");
            return indentString.ToString();
        }

        public override string ToString()
        {
            return _sb.ToString();
        }
    }

    public class StringWalker
    {
        string _s;
        public int Index { get; set; }

        public StringWalker(string s)
        {
            _s = s;
            Index = -1;
        }

        public bool MoveNext()
        {
            if (Index == _s.Length - 1)
                return false;
            Index++;
            return true;
        }

        public char CharAtIndex()
        {
            return _s[Index];
        }
    };
}
