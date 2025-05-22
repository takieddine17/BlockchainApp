using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    class Transaction
    {
        DateTime timestamp;
        public string senderAddress, recipientAddress;
        public double amount, fee;
        public string hash, signature;

        public Transaction(string from, string to, double amount, double fee, string privateKey)
        {
            this.timestamp = DateTime.Now;

            this.senderAddress = from;
            this.recipientAddress = to;

            this.amount = amount;
            this.fee = fee;

            this.hash = CreateHash();
            this.signature = Wallet.Wallet.CreateSignature(from, privateKey, hash);
        }

        public string CreateHash()
        {
            string hash = string.Empty;
            SHA256 hasher = SHA256Managed.Create();

            string input = timestamp + senderAddress + recipientAddress + amount + fee;
            byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));

            foreach (byte x in hashByte)
                hash += string.Format("{0:x2}", x);

            return hash;
        }

        public override string ToString()
        {
            return "\n  Timestamp: " + timestamp.ToString() +
                   "\n  Hash: " + hash +
                   "\n  Signature: " + signature +
                   "\n  Transferred: " + amount.ToString() + " Assignment Coin" +
                   "\t  Fee: " + fee.ToString() + " Assignment Coin" +
                   "\n  Sender: " + senderAddress +
                   "\n  Receiver: " + recipientAddress;
        }
    }
}
