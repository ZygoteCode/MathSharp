namespace MathSharp
{
    public class Position
    {
        public int Index { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }

        public Position(int index, int line, int column)
        {
            Index = index;
            Line = line;
            Column = column;
        }

        public Position Next(char currentCharacter = default(char))
        {
            Index++;
            Column++;

            if (currentCharacter == '\n')
            {
                Line++;
                Column = 0;
            }

            return this;
        }

        public Position Clone()
        {
            return new Position(Index, Line, Column);
        }
    }
}