using BlockchainAssignment.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    class Block
    {
        public DateTime timestamp;
        public int index;          
        public string hash;         
        public string prevHash;
        public List<Transaction> transactionList;
        public long nonce = 0;
        public int difficulty = 4;



        public Block()
        {
            this.timestamp = DateTime.Now;
            this.index = 0;
            this.prevHash = String.Empty;
            this.hash = CreateHash();
        }

        public Block(int index, String hash)
        {
            this.timestamp = DateTime.Now;
            this.index = index + 1;
            this.prevHash = hash;
            this.hash = CreateHash();
        }

        public Block(Block prevBlock)
        {
            this.timestamp = DateTime.Now;
            this.index = prevBlock.index + 1;
            this.prevHash = prevBlock.hash;
            this.hash = CreateHash();
        }

        public Block(Block prevBlock, List<Transaction> transactions, string minerAddress)
        {
            this.timestamp = DateTime.Now;
            this.index = prevBlock.index + 1;
            this.prevHash = prevBlock.hash;
            this.difficulty = 4; 

            this.transactionList = new List<Transaction>(transactions);

            double fees = transactionList.Sum(t => t.fee);

            double reward = 25.0;

            Transaction rewardTx = new Transaction("Mine Rewards", minerAddress, reward + fees, 0, "");
            this.transactionList.Add(rewardTx);

            this.hash = Mine();
        }


        public string CreateHash()
        {
            SHA256 hasher = SHA256Managed.Create();
            string transactionsData = "";

            if (transactionList != null)
            {
                foreach (Transaction t in transactionList)
                {
                    transactionsData += t.ToString();
                }
            }

            string input = index.ToString() + timestamp.ToString() + prevHash + nonce + transactionsData;
            byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
            string hash = string.Empty;
            foreach (byte x in hashByte)
            {
                hash += string.Format("{0:x2}", x);
            }

            return hash;
        }

        public string Mine()
        {
            nonce = 0;
            string hash = CreateHash();
            string target = new string('0', difficulty);

            while (!hash.StartsWith(target))
            {
                nonce++;
                hash = CreateHash();
            }

            return hash;
        }

        public override string ToString()
        {
            string blockString = $"Index: {index}\n" +
                                 $"Timestamp: {timestamp}\n" +
                                 $"Hash: {hash}\n" +
                                 $"Previous Hash: {prevHash}\n" +
                                 $"Nonce: {nonce}\n" +
                                 $"Difficulty: {difficulty}\n" +
                                 $"Transactions:";

            if (transactionList != null)
            {
                foreach (Transaction t in transactionList)
                {
                    blockString += "\n" + t.ToString();
                }
            }
            return blockString;
        }
    }
}
