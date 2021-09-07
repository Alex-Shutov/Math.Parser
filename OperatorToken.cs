namespace RandomVariable
{
    public class OperatorToken: TokenBase
    {
        public OperatorType OperatorType { get; }

        public OperatorToken(OperatorType operatorType)
        {
            this.OperatorType = operatorType;
        }

        
        
    }
}