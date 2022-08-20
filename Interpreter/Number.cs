using System;

namespace MathSharp
{
    public class Number
    {
        public decimal Value { get; set; }
        public Position PositionStart { get; set; }
        public Position PositionEnd { get; set; }

        public Number(decimal value)
        {
            Value = value;
            SetPosition();
        }

        public Number SetPosition(Position positionStart = null, Position positionEnd = null)
        {
            PositionStart = positionStart;
            PositionEnd = positionEnd;
            return this;
        }

        public Tuple<Number, Error> AddedTo(Number other)
        {
            return new Tuple<Number, Error>(new Number(Value + other.Value), null);
        }

        public Tuple<Number, Error> SubbedBy(Number other)
        {
            return new Tuple<Number, Error>(new Number(Value - other.Value), null);
        }

        public Tuple<Number, Error> MultedBy(Number other)
        {
            return new Tuple<Number, Error>(new Number(Value * other.Value), null);
        }

        public Tuple<Number, Error> DivedBy(Number other)
        {
            if (other.Value == 0)
            {
                return new Tuple<Number, Error>(null, new RuntimeError(other.PositionStart, other.PositionEnd, "Division by zero"));
            }

            return new Tuple<Number, Error>(new Number(Value / other.Value), null);
        }

        public Tuple<Number, Error> PowedBy(Number other)
        {
            return new Tuple<Number, Error>(new Number((decimal)Math.Pow((double)Value, (double)other.Value)), null);
        }

        public override string ToString()
        {
            return Value.ToString().Replace(",", ".");
        }
    }
}