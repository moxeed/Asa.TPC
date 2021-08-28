namespace Asa.Core.Domain
{
    public class Block
    {
        public int BlockId { get; private set; }
        public int Amount { get; private set; }

        public Block(int amount)
        {
            Amount = amount;
        }

        public void Release()
        {
            Amount = 0;
        }
    }
}
