namespace Asa.Core.Domain
{
    public enum DecisionState : byte 
    { 
        Pending,
        Rejected, 
        Compeleted
    }

    public class Decision
    {
        public Decision(int totalAmount)
        {
            TotalAmount = totalAmount;
        }

        public int DecisionId { get; private set; }
        public int BlockId { get; private set; }
        public int TotalAmount { get; private set; }
        public DecisionState State { get; private set; } = DecisionState.Pending;

        public void SaveBlockId(int blockId)
        {
            BlockId = blockId;
        }

        public void Reject() => State = DecisionState.Rejected;
        public void Close() => State = DecisionState.Compeleted;
    }
}
