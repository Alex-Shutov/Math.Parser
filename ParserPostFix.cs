using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;

namespace RandomVariable
{
    public class ParserPostFix
    {
        private readonly Stack<OperandToken> operandTokenStack;
        
        public ParserPostFix(Stack<OperandToken> operandTokenStack)
        {
            this.operandTokenStack = operandTokenStack;
        }
        public OperandToken Calculate(IEnumerable<TokenBase> tokens)
        {
            foreach (var token in tokens)
            {
                CalculateToken(token);
            }
            return GetResult();
        }

        private OperandToken GetResult()
        {
            return operandTokenStack.Pop();
        }

        private void CalculateToken(TokenBase token)
        {
            switch (token)
            {
                case OperandToken operandToken:
                    operandTokenStack.Push(operandToken);
                    break;
                case OperatorToken operatorToken:
                    ApplyOperator(operatorToken);
                    break;
            }
        }

        private void ApplyOperator(OperatorToken operatorToken)
        {
            switch (operatorToken.OperatorType)
            {
                case OperatorType.Addition:
                    ApplyOperator((x,y)=>x+y);
                    break;
                case OperatorType.Subtraction:
                    ApplyOperator((x,y)=>x-y);
                    break;
                case OperatorType.Multiplication:
                    ApplyOperator((x,y)=>x*y);
                    break;
                case OperatorType.Division:
                    ApplyOperator((x,y)=>x/y);
                    break;
                case OperatorType.Random:
                    ApplyRandomDicreteOperator();
                    break;
            }
        }

        private void ApplyRandomDicreteOperator()
        {
            throw new NotImplementedException();
        }

        private void ApplyOperator(Func<double,double,double> func)
        {
            var operands = GetArguments();
            var result = new OperandToken(func(operands.Item1.Value,operands.Item2.Value));
            operandTokenStack.Push(result);
        }

        private (OperandToken, OperandToken) GetArguments()
        {
            var right = operandTokenStack.Pop();
            var left = operandTokenStack.Pop();
            return (left, right);
        }
    }
}