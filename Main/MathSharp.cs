using System.Collections.Generic;
using System;

public class MathSharp
{
    /// <summary>
    /// Solve an algebric expression.
    /// </summary>
    /// <param name="expression">String expression to solve.</param>
    /// <returns>A decimal containing the result of the given expression.</returns>
    /// <exception cref="Exception"></exception>
    public static decimal ParseExpression(string expression)
    {
        // If the expression is equal to null, throw exception.
        if (expression == null)
        {
            throw new Exception("Expression cannot be null.");
        }

        // If the expression is empty, throw exception.
        if (expression.Replace(" ", "").Replace('\t'.ToString(), "") == "")
        {
            throw new Exception("Expression can not be empty.");
        }

        // Instatiate a new Lexer instance.
        // This will read the string to tokenize it into tokens.
        // A token contains a type and a (value, if necessary).
        Lexer lexer = new Lexer(expression);
        Tuple<List<Token>, Error> lexed = lexer.CreateTokens();

        // If the Lexer met something bad and gave error,
        // we are throwing the exception with that error.
        if (lexed.Item2 != null)
        {
            throw new Exception(lexed.Item2.ToString());
        }

        // If there is no error, we are going to put the tokens
        // into the Parser. The Parser will parse the token to make
        // the AST (Abstract Syntax Tree).
        List<Token> tokens = lexed.Item1;
        Parser parser = new Parser(tokens);
        ParserResult AST = parser.Parse();

        // If the Parser met something wrong and gave an error,
        // throw exception with that error.
        if (AST.Error != null)
        {
            throw new Exception(AST.Error.ToString());
        }

        // Instatiation of Interpreter. This will interpret the
        // AST made from the Parser to give the result.
        Interpreter interpreter = new Interpreter();
        RuntimeResult result = interpreter.Visit(AST.Node);

        // If the Interpreter is giving an error, then we throw
        // an exception with that error.
        if (result.Error != null)
        {
            throw new Exception(result.Error.ToString());
        }

        // If there are no errors, return the result of the expression
        // calculated by the Interpreter.
        return result.Value.Value;
    }
}