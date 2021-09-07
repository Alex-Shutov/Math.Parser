using System;
using System.Collections.Generic;
using System.Linq;

namespace RandomVariable
{
    public class SortYard
    {
        private Stack<OperatorToken> operatorsStack;
        private readonly List<TokenBase> postFixNotationTokens;

        public SortYard()
        {
            operatorsStack = new Stack<OperatorToken>();
            postFixNotationTokens = new List<TokenBase>();
        }

        public IEnumerable<TokenBase> TakingTokenFromCollection(IEnumerable<TokenBase> infixTokens)
        {
            foreach (var token in infixTokens)
            {
                ProcessInfixToken(token);
            }

            return GetPostFixNotation();
        }

        private List<TokenBase> GetPostFixNotation()
        {
            while (operatorsStack.Count > 0)
            {
                var token = operatorsStack.Pop();
                postFixNotationTokens.Add(token);
            }

            return postFixNotationTokens;
        }

        private void ProcessInfixToken(TokenBase token)
        {
            switch (token)
            {
                case OperandToken operandToken:
                    SaveOperand(operandToken);
                    break;
                case OperatorToken operatorToken:
                    ProcessOperator(operatorToken);
                    break;
            }
        }

        private void SaveOperand(OperandToken operandToken) => postFixNotationTokens.Add(operandToken);

        private void ProcessOperator(OperatorToken token)
        {
            switch (token.OperatorType)
            {
                case OperatorType.OpeningBracket:
                    PushOpeningBracket(token);
                    break;
                case OperatorType.ClosingBracket:
                    PushClosingBracket(token);
                    break;
                default:
                    PushOperator(token);
                    break;
            }
        }

        private void PushClosingBracket(OperatorToken token)
        {
            var openingBracketFound = false;

            while (operatorsStack.Count > 0 || openingBracketFound)
            {
                var stackOperatorToken = operatorsStack.Pop();
                if (stackOperatorToken.OperatorType == OperatorType.OpeningBracket)
                    openingBracketFound = true;
                else
                    postFixNotationTokens.Add(stackOperatorToken);
            }
            
        }

        private void PushOpeningBracket(OperatorToken token) => operatorsStack.Push(token);

        private void PushOperator(OperatorToken token)
        {
            var operatorPriority = GetOperatorPriority(token);
            while (operatorsStack.Count > 0)
            {
                var stackOperator = operatorsStack.Peek();
                if (stackOperator.OperatorType == OperatorType.OpeningBracket)
                    break;
                var stackOperatorPriority = GetOperatorPriority(stackOperator);
                if (stackOperatorPriority < operatorPriority)
                    break;
                postFixNotationTokens.Add(operatorsStack.Pop());
            }
            operatorsStack.Push(token);
        }

        private Priority GetOperatorPriority(OperatorToken token)
        {
            switch (token.OperatorType)
            {
                case OperatorType.Addition:
                case OperatorType.Subtraction:
                    return Priority.LowPriority;
                case OperatorType.Multiplication:
                case OperatorType.Division:
                    return Priority.HighPriority;
                case OperatorType.Random:
                    return Priority.HighPriority;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}