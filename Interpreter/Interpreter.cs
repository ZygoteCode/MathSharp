using System;

public class Interpreter
{
    /// <summary>
    /// Visit a generic node.
    /// </summary>
    /// <param name="node">The generic node.</param>
    /// <returns>A runtime result with number & error, visiting all the AST.</returns>
    public RuntimeResult Visit(object node)
    {
        // Visit the first node.
        // Visiting the first node, we'll visit all the nodes as this is a Syntax Tree.
        return (RuntimeResult)GetType().GetMethod("Visit" + node.GetType().Name).Invoke(this, new object[] { node });
    }

    /// <summary>
    /// Visit a number node.
    /// </summary>
    /// <param name="node">Number node.</param>
    /// <returns>A runtime result with a number.</returns>
    private RuntimeResult VisitNumberNode(NumberNode node)
    {
        // Visit a number node.
        // This returns a number in the RuntimeResult.
        return new RuntimeResult().Success(new Number((decimal)node.Token.Value).SetPosition(node.PositionStart, node.PositionEnd));
    }

    /// <summary>
    /// Visit a binary operation node.
    /// </summary>
    /// <param name="node">Binary operation node.</param>
    /// <returns>A runtime result with a number as result of the operation.</returns>
    private RuntimeResult VisitBinaryOperationNode(BinaryOperationNode node)
    {
        // Visit a Binary Operation Node, which is an operation between two operands
        // with an operator in the mid.
        RuntimeResult result = new RuntimeResult();
        Number left = result.Register(Visit(node.LeftNode));

        if (result.Error != null)
        {
            return result;
        }

        Number right = result.Register(Visit(node.RightNode));
        Tuple<Number, Error> theResult = new Tuple<Number, Error>(null, null);

        // Support for all the basic operations:
        // + => PLUS
        // - => MINUS
        // * => MUL
        // / => DIV
        // ^ => POW

        if (node.OperatorToken.Type.Equals(TokenType.PLUS))
        {
            theResult = left.AddedTo(right);
        }
        else if (node.OperatorToken.Type.Equals(TokenType.MINUS))
        {
            theResult = left.SubbedBy(right);
        }
        else if (node.OperatorToken.Type.Equals(TokenType.MUL))
        {
            theResult = left.MultedBy(right);
        }
        else if (node.OperatorToken.Type.Equals(TokenType.DIV))
        {
            theResult = left.DivedBy(right);
        }
        else if (node.OperatorToken.Type.Equals(TokenType.POW))
        {
            theResult = left.PowedBy(right);
        }

        if (theResult.Item2 != null)
        {
            return result.Failure(theResult.Item2);
        }
        else
        {
            return result.Success(theResult.Item1.SetPosition(node.PositionStart, node.PositionEnd));
        }
    }

    /// <summary>
    /// Visit a unary operation node.
    /// </summary>
    /// <param name="node">Unary operation node.</param>
    /// <returns>A runtime result with the result number of the operation.</returns>
    private RuntimeResult VisitUnaryOperationNode(UnaryOperationNode node)
    {
        // Visit a Unary Operation Node. This is a node type that is created
        // for algebric operations. For example, -5 is a unary operation, so 
        // we have first an operator and then a number.
        RuntimeResult result = new RuntimeResult();
        Number number = result.Register(Visit(node.Node));

        if (result.Error != null)
        {
            return result;
        }

        Tuple<Number, Error> theResult = new Tuple<Number, Error>(null, null);

        if (node.OperatorToken.Type.Equals(TokenType.MINUS))
        {
            theResult = number.MultedBy(new Number(-1));
        }

        if (theResult.Item2 != null)
        {
            return result.Failure(theResult.Item2);
        }
        else
        {
            return result.Success(theResult.Item1.SetPosition(node.PositionStart, node.PositionEnd));
        }
    }
}