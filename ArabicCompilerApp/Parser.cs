using System;
using System.Collections.Generic;

public class Parser
{
    private readonly List<(string TokenType, string Value)> Tokens;
    private int CurrentToken;
    private int RuleNumber;

    public Parser(List<(string TokenType, string Value)> tokens)
    {
        Tokens = tokens;
        CurrentToken = 0;
        RuleNumber = 1;
    }

    private (string TokenType, string Value) Current()
    {
        return CurrentToken < Tokens.Count ? Tokens[CurrentToken] : ("EOF", "");
    }

    private void Eat(string expectedTokenType)
    {
        if (Current().TokenType == expectedTokenType)
        {
            CurrentToken++;
        }
        else
        {
            throw new Exception($"Syntax Error: Expected {expectedTokenType}, but found {Current().TokenType} ('{Current().Value}') at token {CurrentToken + 1}");
        }
    }

    public void ParseProgram()
    {
        Console.WriteLine($"{RuleNumber}. <program>→<statement_list>");
        RuleNumber++;
        ParseStatementList();
    }

    private void ParseStatementList()
    {
        Console.WriteLine($"{RuleNumber}. <statement_list> → <statement> <statement_list>");
        RuleNumber++;

        while (Current().TokenType != "EOF")
        {
            if (Current().TokenType == "KEYWORD" && Current().Value == "متغير")
            {
                ParseDeclaration();
            }
            else if (Current().TokenType == "KEYWORD" && Current().Value == "اذا")
            {
                ParseIfStatement();
            }
            else if (Current().TokenType == "KEYWORD" && Current().Value == "طالما")
            {
                ParseWhileStatement();
            }
            else if (Current().TokenType == "IDENT")
            {
                ParseAssignment();
            }
            else
            {
                throw new Exception($"Syntax Error: Unexpected token '{Current().Value}' at token {CurrentToken + 1}");
            }
        }
    }
    private void ParseDeclaration()
    {
        Console.Write("        <declaration>→ ");
        Eat("KEYWORD");
        Console.Write($"{Tokens[CurrentToken - 1].Value} ");
        Eat("IDENT");
        Console.Write($"{Tokens[CurrentToken - 1].Value} = ");
        Eat("ASSIGN");
        ParseExpression();
        Eat("SEMICOLON");
        Console.WriteLine(";");
    }

   
    private void ParseIfStatement()
    {
        Console.WriteLine($"{RuleNumber}. <statement list>→ <if statement>");
        RuleNumber++;
        Console.Write("        <if_statement> اذا(");
        Eat("KEYWORD");
        Eat("LPAREN");
        ParseCondition();
        Eat("RPAREN");
        Console.Write($") {{ ");
        Eat("LBRACE");

        
        while (Current().TokenType != "RBRACE" && Current().TokenType != "EOF")
        {
            if (Current().TokenType == "KEYWORD" && Current().Value == "طالما")
            {
                ParseWhileStatement();
            }
            else if (Current().TokenType == "IDENT")
            {
                ParseAssignment();
            }
            else if (Current().TokenType == "KEYWORD" && Current().Value == "اذا")
            {
                ParseIfStatement(); 
            }
            else
            {
                throw new Exception($"Syntax Error: Unexpected token '{Current().Value}' inside if statement at token {CurrentToken + 1}");
            }
        }

        Eat("RBRACE");
        Console.WriteLine("}");
    }

    private void ParseWhileStatement()
    {
        Console.WriteLine($"{RuleNumber}. <statement list>→ <while_statement>");
        RuleNumber++;
        Console.Write("        <while_statement> طالما(");
        Eat("KEYWORD");
        Eat("LPAREN");
        ParseCondition();
        Eat("RPAREN");
        Console.Write($") {{ ");
        Eat("LBRACE");

        
        while (Current().TokenType != "RBRACE" && Current().TokenType != "EOF")
        {
            if (Current().TokenType == "KEYWORD" && Current().Value == "اذا")
            {
                ParseIfStatement();
            }
            else if (Current().TokenType == "IDENT")
            {
                ParseAssignment();
            }
            else if (Current().TokenType == "KEYWORD" && Current().Value == "طالما")
            {
                ParseWhileStatement(); 
            }
            else
            {
                throw new Exception($"Syntax Error: Unexpected token '{Current().Value}' inside while statement at token {CurrentToken + 1}");
            }
        }

        Eat("RBRACE");
        Console.WriteLine("}");
    }

    private void ParseCondition()
    {
        ParseExpression();
        if (Current().TokenType != "OPERATOR")
        {
            throw new Exception($"Syntax Error: Expected a relational operator, but found '{Current().Value}' at token {CurrentToken + 1}");
        }
        Eat("OPERATOR");
        Console.Write($" {Tokens[CurrentToken - 1].Value} ");
        ParseExpression();
    }

    private void ParseExpression()
    {
        ParseTerm();
        while (Current().TokenType == "OPERATOR" && "+-*/%^&!$".Contains(Current().Value))
        {
            Eat("OPERATOR");
            Console.Write($" {Tokens[CurrentToken - 1].Value} ");
            ParseTerm();
        }
    }

    private void ParseTerm()
    {
        if (Current().TokenType == "IDENT")
        {
            Eat("IDENT");
            Console.Write($"{Tokens[CurrentToken - 1].Value}");
        }
        else if (Current().TokenType == "NUM")
        {
            Eat("NUM");
            Console.Write($"{Tokens[CurrentToken - 1].Value}");
        }
        else
        {
            throw new Exception($"Syntax Error: Unexpected term '{Current().Value}' at token {CurrentToken + 1}");
        }
    }

    private void ParseAssignment()
    {
        Eat("IDENT");
        Console.Write($"{Tokens[CurrentToken - 1].Value} = ");
        Eat("ASSIGN");
        ParseExpression();
        Eat("SEMICOLON");
        Console.Write(";");
    }
}





