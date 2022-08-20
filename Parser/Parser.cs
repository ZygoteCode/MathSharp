using System.Collections.Generic;

public class Parser
{
    public List<Token> Tokens { get; set; }
    public int TokenIndex { get; set; }
    public Token CurrentToken { get; set; }

    public Parser(List<Token> tokens)
    {
        Tokens = tokens;
        TokenIndex = -1;

        NextToken();
    }

    /// <summary>
    /// Go to the next token.
    /// </summary>
    /// <returns>Current token.</returns>
    private Token NextToken()
    {
        TokenIndex++;

        if (TokenIndex < Tokens.Count)
        {
            CurrentToken = Tokens[TokenIndex];
        }

        return CurrentToken;
    }

    /// <summary>
    /// Parse the given tokens list from the Lexer.
    /// </summary>
    /// <returns>A result given from the Parser operations.</returns>
    public ParserResult Parse()
    {
        ParserResult result = GetExpression();

        if (result.Error == null && !CurrentToken.Type.Equals(TokenType.EOF))
        {
            return result.Failure(new InvalidSyntaxError(CurrentToken.PositionEnd, CurrentToken.PositionEnd, "Expected '+', '-', '*' or '/'"));
        }

        return result;
    }

    /// <summary>
    /// Parse a power operation.
    /// </summary>
    /// <returns></returns>
    private ParserResult GetPower()
    {
        return GetBinaryOperation("atom", new List<TokenType>() { TokenType.POW });
    }

    /// <summary>
    /// Parse a atom operation (numbers, parens).
    /// </summary>
    /// <returns></returns>
    private ParserResult GetAtom()
    {
        ParserResult result = new ParserResult();
        Token token = CurrentToken;

        if (token.Type.Equals(TokenType.NUMBER))
        {
            result.Register(NextToken());
            return result.Success(new NumberNode(token));
        }
        else if (token.Type.Equals(TokenType.LPAREN))
        {
            result.Register(NextToken());
            object expression = result.Register(GetExpression());

            if (result.Error != null)
            {
                return result;
            }

            if (CurrentToken.Type.Equals(TokenType.RPAREN))
            {
                result.Register(NextToken());
                return result.Success(expression);
            }
            else
            {
                return result.Failure(new InvalidSyntaxError(CurrentToken.PositionStart, CurrentToken.PositionEnd, "Expected ')'"));
            }
        }

        return result.Failure(new InvalidSyntaxError(CurrentToken.PositionEnd, CurrentToken.PositionEnd, "Expected number, '+', '-', '*' or '/'"));
    }

    /// <summary>
    /// Parse a single factor, or return power.
    /// </summary>
    /// <returns></returns>
    private ParserResult GetFactor()
    {
        ParserResult result = new ParserResult();
        Token token = CurrentToken;

        if (token.Type.Equals(TokenType.PLUS) || token.Type.Equals(TokenType.MINUS))
        {
            result.Register(NextToken());
            object anotherFactor = result.Register(GetFactor());

            if (result.Error != null)
            {
                return result;
            }

            return result.Success(new UnaryOperationNode(token, anotherFactor));
        }

        return GetPower();
    }

    /// <summary>
    /// Parse a single term (two factors, between MUL or DIV).
    /// </summary>
    /// <returns></returns>
    private ParserResult GetTerm()
    {
        return GetBinaryOperation("factor", new List<TokenType> { TokenType.MUL, TokenType.DIV });
    }

    /// <summary>
    /// Parse a single expression (two terms, between PLUS and MINUS).
    /// </summary>
    /// <returns></returns>
    private ParserResult GetExpression()
    {
        return GetBinaryOperation("term", new List<TokenType>() { TokenType.PLUS, TokenType.MINUS });
    }

    /// <summary>
    /// Parse a binary operation (between two factors, terms or atoms).
    /// </summary>
    /// <param name="function">Function to invoke</param>
    /// <param name="operatorTokens">Operators involved in the operation</param>
    /// <returns>A complete Parser Result with the result of parsing.</returns>
    private ParserResult GetBinaryOperation(string function, List<TokenType> operatorTokens)
    {
        ParserResult result = new ParserResult();
        object leftNode = null;

        if (function.Equals("factor"))
        {
            leftNode = result.Register(GetFactor());
        }
        else if (function.Equals("term"))
        {
            leftNode = result.Register(GetTerm());
        }
        else if (function.Equals("atom"))
        {
            leftNode = result.Register(GetAtom());
        }

        if (result.Error != null)
        {
            return result;
        }

        while (true)
        {
            bool exists = false;

            foreach (TokenType tokenType in operatorTokens)
            {
                if (tokenType.Equals(CurrentToken.Type))
                {
                    Token operatorToken = CurrentToken;
                    result.Register(NextToken());
                    object rightNode = null;

                    if (function.Equals("factor"))
                    {
                        rightNode = result.Register(GetFactor());
                    }
                    else if (function.Equals("term"))
                    {
                        rightNode = result.Register(GetTerm());
                    }
                    else if (function.Equals("atom"))
                    {
                        rightNode = result.Register(GetAtom());
                    }

                    if (result.Error != null)
                    {
                        return result;
                    }

                    leftNode = new BinaryOperationNode(leftNode, operatorToken, rightNode);
                    exists = true;
                }
            }

            if (!exists)
            {
                break;
            }
        }

        return result.Success(leftNode);
    }
}