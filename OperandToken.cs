namespace RandomVariable
{
    public class OperandToken : TokenBase
    {
        private readonly double value;

        public double Value => value;
        
        public OperandToken(double value)
        {
            this.value = value;
        }
    }
}