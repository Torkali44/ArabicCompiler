using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Scanner
{
    private static readonly List<(string TokenName, string Pattern)> Tokens = new List<(string, string)>
    {
        ("KEYWORD", @"\b(اذا|طالما|متغير|ابدأ|انهاء|حساب|int)\b"),
        ("IDENT", @"\b[a-zA-Z_]\w*\b"),
        ("NUM", @"\b\d+\b"),
        ("OPERATOR", @"(\+|\-|\*|\/|==|!=|>=|<=|<|>)"),
        ("ASSIGN", @"="),
        ("LPAREN", @"\("),
        ("RPAREN", @"\)"),
        ("LBRACE", @"\{"),
        ("RBRACE", @"\}"),
        ("SEMICOLON", @";"),
        ("WHITESPACE", @"\s+")
    };

    private static readonly HashSet<string> ReservedKeywords = new HashSet<string>
    {
        "اذا", "طالما", "متغير", "ابدأ", "انهاء", "حساب", "int"
    };

    public List<(string TokenType, string Value)> Scan(string input)
    {
        var results = new List<(string TokenType, string Value)>();
        while (!string.IsNullOrEmpty(input))
        {
            bool matchFound = false;

            foreach (var (tokenName, pattern) in Tokens)
            {
                var regex = new Regex($"^{pattern}");
                var match = regex.Match(input);

                if (match.Success)
                {
                    if (tokenName == "IDENT" && ReservedKeywords.Contains(match.Value))
                    {
                        results.Add(("KEYWORD", match.Value));
                    }
                    else if (tokenName != "WHITESPACE")
                    {
                        results.Add((tokenName, match.Value));
                    }

                    input = input.Substring(match.Length);
                    matchFound = true;
                    break;
                }
            }

            if (!matchFound)
            {
                throw new Exception($"Unexpected character: {input[0]}");
            }
        }
        return results;
    }
}