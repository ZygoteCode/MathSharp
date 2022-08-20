using System.Collections.Generic;
using System;

public class Lexer
{
    public string Text { get; set; }
    public Position Position { get; set; }
    public char CurrentCharacter { get; set; }
    public const string Digits = "0123456789";

    // Main constructor.
    public Lexer(string text)
    {
        Text = text;
        Position = new Position(-1, 0, -1);
        CurrentCharacter = default(char);

        NextCharacter();
    }

    /// <summary>
    /// Skip a character in the text.
    /// If the string is finished, return default(char).
    /// </summary>
    private void NextCharacter()
    {
        Position.Next(CurrentCharacter);
        CurrentCharacter = Position.Index < Text.Length ? Text[Position.Index] : default(char);
    }

    /// <summary>
    /// Create the list of tokens from the string.
    /// If there's an error, throw that error.
    /// </summary>
    /// <returns></returns>
    public Tuple<List<Token>, Error> CreateTokens()
    {
        List<Token> tokens = new List<Token>();

        // We create tokens until the current character does not become default(char)
        // as the NextCharacter() function will do that in case we reached the end of the string.
        while (CurrentCharacter != default(char))
        {
            switch (CurrentCharacter)
            {
                case ' ':
                    NextCharacter();
                    break;
                case '\t':
                    NextCharacter();
                    break;
                case '+':
                    tokens.Add(new Token(TokenType.PLUS, positionStart: Position));
                    NextCharacter();
                    break;
                case '-':
                    tokens.Add(new Token(TokenType.MINUS, positionStart: Position));
                    NextCharacter();
                    break;
                case '*':
                    tokens.Add(new Token(TokenType.MUL, positionStart: Position));
                    NextCharacter();
                    break;
                case '/':
                    tokens.Add(new Token(TokenType.DIV, positionStart: Position));
                    NextCharacter();
                    break;
                case '^':
                    tokens.Add(new Token(TokenType.POW, positionStart: Position));
                    NextCharacter();
                    break;
                case '(':
                    tokens.Add(new Token(TokenType.LPAREN, positionStart: Position));
                    NextCharacter();
                    break;
                case ')':
                    tokens.Add(new Token(TokenType.RPAREN, positionStart: Position));
                    NextCharacter();
                    break;
                default:
                    if (Digits.Contains(CurrentCharacter.ToString()))
                    {
                        tokens.Add(GetNumber());
                    }
                    else
                    {
                        Position positionStart = Position.Clone();
                        char theChar = CurrentCharacter;
                        NextCharacter();
                        return new Tuple<List<Token>, Error>(null, new IllegalCharacterError(positionStart, Position, "'" + theChar + "'"));
                    }
                    break;
            }
        }

        // EOF token is important to understand when we reached the end.
        tokens.Add(new Token(TokenType.EOF, positionStart: Position));
        return new Tuple<List<Token>, Error>(tokens, null);
    }

    /// <summary>
    /// Get a number token when digits are met in the string.
    /// </summary>
    /// <returns></returns>
    private Token GetNumber()
    {
        string numberStr = "";
        int dots = 0;
        Position positionStart = Position.Clone();

        while (CurrentCharacter != default(char) && (Digits + ".").Contains(CurrentCharacter.ToString()))
        {
            if (CurrentCharacter == '.')
            {
                if (dots == 1)
                {
                    break;
                }

                dots++;
                numberStr += '.';
            }
            else
            {
                numberStr += CurrentCharacter;
            }

            NextCharacter();
        }

        return new Token(TokenType.NUMBER, decimal.Parse(numberStr), positionStart, Position);
    }
}