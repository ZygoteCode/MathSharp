public class UnaryOperationNode
{
    public Token OperatorToken { get; set; }
    public object Node { get; set; }
    public Position PositionStart { get; set; }
    public Position PositionEnd { get; set; }

    public UnaryOperationNode(Token operatorToken, object node)
    {
        OperatorToken = operatorToken;
        Node = node;

        PositionStart = operatorToken.PositionStart;
        PositionEnd = (Position)node.GetType().GetProperty("PositionEnd").GetValue(node);
    }

    public override string ToString()
    {
        return $"({OperatorToken.ToString()}, {Node.ToString()})";
    }
}