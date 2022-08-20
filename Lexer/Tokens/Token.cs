public class Token
{
    public TokenType Type { get; set; }
    public object Value { get; set; }
    public Position PositionStart { get; set; }
    public Position PositionEnd { get; set; }

    public Token(TokenType type, object value = null, Position positionStart = null, Position positionEnd = null)
    {
        Type = type;
        Value = value;

        if (positionStart != null)
        {
            PositionStart = positionStart.Clone();
            PositionEnd = positionStart.Clone().Next();
        }

        if (positionEnd != null)
        {
            PositionEnd = positionEnd;
        }
    }

    public override string ToString()
    {
        if (Value == null)
        {
            return Type.ToString();
        }

        return $"{Type.ToString()}:{Value.ToString()}";
    }
}