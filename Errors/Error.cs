public class Error
{
    public Position PositionStart { get; set; }
    public Position PositionEnd { get; set; }
    public string ErrorName { get; set; }
    public string Details { get; set; }

    public Error(Position positionStart, Position positionEnd, string errorName, string details)
    {
        PositionStart = positionStart;
        PositionEnd = positionEnd;
        ErrorName = errorName;
        Details = details;
    }

    public override string ToString()
    {
        return $"{ErrorName}: {Details}.\r\nLine {PositionStart.Line + 1}, column {PositionStart.Column + 1}.";
    }
}