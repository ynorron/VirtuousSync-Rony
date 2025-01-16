namespace Sync
{
    public class IsStateCondition : Condition
    {
        public IsStateCondition( string stateAbbreviation)
        {
            Parameter = "State";
            Operator = "Is";
            Value = stateAbbreviation;
        }
    }

}
