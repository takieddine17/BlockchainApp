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
        public int targetBlockTime = 10;
        public DateTime lastBlockTimestamp;

        public Blockchain()
        {
            Blocks = new List<Block>() 
            {
                new Block()
            };

            lastBlockTimestamp = DateTime.Now;
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

        public List<Transaction> GetPendingTransactions(string preference, string userAddress)
        {
            List<Transaction> sortedPool = new List<Transaction>(transactionPool);

            // Sort transaction pool based on mining preference
            switch (preference)
            {
                case "Greedy":
                    sortedPool = sortedPool.OrderByDescending(tx => tx.fee).ToList();
                    break;

                case "Altruistic":
                    sortedPool = sortedPool.OrderBy(tx => tx.timestamp).ToList();
                    break;

                case "Random":
                    Random rng = new Random();
                    sortedPool = sortedPool.OrderBy(_ => rng.Next()).ToList();
                    break;

                case "Address Preference":
                    sortedPool = sortedPool
                        .OrderByDescending(tx => tx.senderAddress == userAddress || tx.recipientAddress == userAddress)
                        .ThenByDescending(tx => tx.fee)
                        .ToList();
                    break;
            }

            int count = Math.Min(transactionsPerBlock, sortedPool.Count);
            List<Transaction> selected = sortedPool.GetRange(0, count);

            transactionPool = transactionPool.Except(selected).ToList();

            DateTime now = DateTime.Now;
            double secondsTaken = (now - lastBlockTimestamp).TotalSeconds;
            lastBlockTimestamp = now;

            Block lastBlock = Blocks[Blocks.Count - 1];
            int targetBlockTime = 10; // seconds

            if (secondsTaken < targetBlockTime)
            {
                lastBlock.difficulty += 1;
            }
            else if (secondsTaken > targetBlockTime)
            {
                lastBlock.difficulty = Math.Max(1, lastBlock.difficulty - 1);
            }

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
