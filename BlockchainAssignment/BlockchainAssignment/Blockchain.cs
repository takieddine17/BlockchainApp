using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    class Blockchain
    {
        public List<Block> Blocks;
        public List<Transaction> transactionPool = new List<Transaction>();
        private int transactionsPerBlock = 5;

        public Blockchain()
        {
            Blocks = new List<Block>() 
            {
                new Block()
            };
        }

        public String getBlockAsString(int index)
        {
            if (index >= 0 && index < Blocks.Count)
                return Blocks[index].ToString();
            return "Block does not exist at index: " + index.ToString();
        }

        public Block GetLastBlock()
        {
            return Blocks[Blocks.Count - 1];
        }

        public List<Transaction> GetPendingTransactions()
        {
            int count = Math.Min(transactionsPerBlock, transactionPool.Count);
            List<Transaction> selected = transactionPool.GetRange(0, count);
            transactionPool = transactionPool.Except(selected).ToList();
            return selected;
        }

        public string ValidateBlockchain()
        {
            for (int i = 1; i < Blocks.Count; i++)
            {
                Block current = Blocks[i];
                Block previous = Blocks[i - 1];

                if (current.prevHash != previous.hash)
                    return $"Invalid chain at block {i}: previous hash mismatch.";

                if (current.CreateHash() != current.hash)
                    return $"Invalid hash at block {i}: hash does not match recalculated hash.";
            }

            return "Blockchain is valid.";
        }

        public string GetBalance(string address)
        {
            double balance = 0;
            string history = "";

            foreach (Block block in Blocks)
            {
                foreach (Transaction tx in block.transactionList)
                {
                    if (tx.recipientAddress == address)
                    {
                        balance += tx.amount;
                        history += $"\n+ {tx.amount} from {tx.senderAddress}";
                    }

                    if (tx.senderAddress == address)
                    {
                        balance -= (tx.amount + tx.fee);
                        history += $"\n- {tx.amount + tx.fee} to {tx.recipientAddress}";
                    }
                }
            }

            return $"Address: {address}\nBalance: {balance}\n\nTransaction History:{history}";
        }

    }
}
