namespace MathSharp
{
    public class BinaryOperationNode
    {
        public object LeftNode { get; set; }
        public Token OperatorToken { get; set; }
        public object RightNode { get; set; }
        public Position PositionStart { get; set; }
        public Position PositionEnd { get; set; }

        public BinaryOperationNode(object leftNode, Token operatorToken, object rightNode)
        {
            LeftNode = leftNode;
            OperatorToken = operatorToken;
            RightNode = rightNode;

            PositionStart = (Position)leftNode.GetType().GetProperty("PositionStart").GetValue(leftNode);
            PositionEnd = (Position)leftNode.GetType().GetProperty("PositionEnd").GetValue(leftNode);
        }

        public override string ToString()
        {
            return $"({LeftNode.ToString()}, {OperatorToken.ToString()}, {RightNode.ToString()})";
        }
    }
}