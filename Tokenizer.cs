using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Text;

namespace RandomVariable
{
    public class Tokenizer
    {
        private readonly StringBuilder tokenBuilder;
        private readonly List<TokenBase> infixNotationTokens;
        private readonly Dictionary<char,OperatorToken> dictionaryOfOperatorTokens;

        public Tokenizer()
        {
            dictionaryOfOperatorTokens = new Dictionary<char, OperatorToken>()
            {
                {'(', new OperatorToken(OperatorType.OpeningBracket)},
                {')', new OperatorToken(OperatorType.ClosingBracket)},
                {'+', new OperatorToken(OperatorType.Addition)},
                {'-', new OperatorToken(OperatorType.Subtraction)},
                {'/', new OperatorToken(OperatorType.Division)},
                {'*', new OperatorToken(OperatorType.Multiplication)},
                {'d', new OperatorToken(OperatorType.Random)}
            };
            tokenBuilder = new StringBuilder();
            infixNotationTokens = new List<TokenBase>();
        }

        public IEnumerable<TokenBase> Parse(string expression)
        {
            foreach (var character in expression)
            {
                ProcessCharacter(character);
            }

            return GetCollectionTokens();
        }

        private void ProcessCharacter(char character)
        {
            if (char.IsWhiteSpace(character))
            {
                AddingInfixTokens();
            }
            else if (character.IsMathOperator())
            {
                AddingInfixTokens();
                var operatorToken = dictionaryOfOperatorTokens.TryGetValue(character, out var token)
                    ? token
                    : throw new ArgumentException();
                infixNotationTokens.Add(operatorToken);
            }
            else 
            {
                tokenBuilder.Append(character);   
            }
        }
        

        private void AddingInfixTokens()
        {
            if (tokenBuilder.Length > 0)
            {
                var token = CreateOperandTokenFromCharacter(tokenBuilder.ToString());
                tokenBuilder.Clear();
                infixNotationTokens.Add(token);
            }
        }

        private IEnumerable<TokenBase> GetCollectionTokens()
        {
            if (tokenBuilder.Length > 0)
            {
                var token = CreateOperandTokenFromCharacter(tokenBuilder.ToString());
                tokenBuilder.Clear();
                infixNotationTokens.Add(token);
            }

            return infixNotationTokens;
        }
        
        private TokenBase CreateOperandTokenFromCharacter(string notProcessed)
        {
            if (double.TryParse(notProcessed, out var result))
            {
                return new OperandToken(result);
            }
            throw new SyntaxErrorException();
        }
    }
}