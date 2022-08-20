namespace MathSharp
{
    public class ParserResult
    {
        public object Node { get; set; }
        public Error Error { get; set; }

        public object Register(object result)
        {
            if (result.GetType() == typeof(ParserResult))
            {
                ParserResult newResult = (ParserResult)result;

                if (newResult.Error != null)
                {
                    Error = newResult.Error;
                }

                return newResult.Node;
            }

            return result;
        }

        public ParserResult Success(object node)
        {
            this.Node = node;
            return this;
        }

        public ParserResult Failure(Error error)
        {
            this.Error = error;
            return this;
        }
    }
}