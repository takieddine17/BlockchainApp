using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockchainAssignment
{
    public partial class BlockchainApp : Form
    {
        Blockchain blockchain;
        public BlockchainApp()
        {
            InitializeComponent();
            blockchain = new Blockchain();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (Int32.TryParse(blockIndex.Text, out int index))
            {
                richTextBox1.Text = blockchain.getBlockAsString(index);
            }
                
            else
            {
                richTextBox1.Text = "Please enter a valid number";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String privKey;
            Wallet.Wallet myNewWallet = new Wallet.Wallet(out privKey);
            publicKey.Text = myNewWallet.publicID;
            this.privateKey.Text = privKey;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Wallet.Wallet.ValidatePrivateKey(privateKey.Text, publicKey.Text))
            {
                richTextBox1.Text = "Keys are valid";
            }
            else
            {
                richTextBox1.Text = "Keys are invalid";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Transaction newTransaction = new Transaction(publicKey.Text, receiver.Text, double.Parse(amount.Text), double.Parse(fee.Text), privateKey.Text);

            blockchain.transactionPool.Add(newTransaction);
            richTextBox1.Text = newTransaction.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string preference = comboBox1.SelectedItem.ToString();
            List<Transaction> pendingTx = blockchain.GetPendingTransactions(preference, publicKey.Text);
            int currentDifficulty = blockchain.GetLastBlock().difficulty;
            Block newBlock = new Block(blockchain.GetLastBlock(), pendingTx, publicKey.Text, currentDifficulty);

            blockchain.Blocks.Add(newBlock);
            richTextBox1.Text = newBlock.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string result = "BLOCKCHAIN:\n";

            foreach (Block block in blockchain.Blocks)
            {
                result += block.ToString() + "\n\n";
            }

            richTextBox1.Text = result;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (blockchain.transactionPool.Count == 0)
            {
                richTextBox1.Text = "Transaction pool is empty.";
                return;
            }

            string result = "PENDING TRANSACTIONS:\n";

            foreach (Transaction tx in blockchain.transactionPool)
            {
                result += tx.ToString() + "\n\n";
            }

            richTextBox1.Text = result;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string address = publicKey.Text;
            richTextBox1.Text = blockchain.GetBalance(address);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = blockchain.ValidateBlockchain();
        }
    }
}
