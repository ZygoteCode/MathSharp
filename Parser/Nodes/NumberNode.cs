namespace MathSharp
{
    public class NumberNode
    {
        public Token Token { get; set; }
        public Position PositionStart { get; set; }
        public Position PositionEnd { get; set; }

        public NumberNode(Token token)
        {
            Token = token;
            PositionStart = token.PositionStart;
            PositionEnd = token.PositionEnd;
        }

        public override string ToString()
        {
            return Token.ToString();
        }
    }
}