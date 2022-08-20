public class IllegalCharacterError : Error
{
    public IllegalCharacterError(Position positionStart, Position positionEnd, string details) : base(positionStart, positionEnd, "Illegal character", details)
    {

    }
}