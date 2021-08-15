namespace Asa.TPC.Domain
{
    class Decision
    {
        public Decision(int totalAmount)
        {
            TotalAmount = totalAmount;
        }

        public int DecisionId { get; private set; }
        public int BlockId { get; private set; }
        public int TotalAmount { get; private set; }

        internal void SaveBlockId(int blockId)
        {
            BlockId = blockId;
        }

    }
}
