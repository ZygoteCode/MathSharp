namespace MathSharp
{
    public class RuntimeError : Error
    {
        public RuntimeError(Position positionStart, Position positionEnd, string details) : base(positionStart, positionEnd, "Runtime error", details)
        {

        }
    }
}