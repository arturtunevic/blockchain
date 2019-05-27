using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace Blockchain
{
    public class Blockchain
    {
        public List<Block> chain { get; set; }

        public BlockChain()
        {
            InitializeChain();
            AddGenesisBlock();
        }

        public void InitializeChain()
        {
            chain = new List<Block>();
        }

        public Block CreateGenesisBlock()
        {
            return new Block(DateTime.Now, null, "{}");
        }

        public void AddGenesisBlock()
        {
            chain.Add(CreateGenesisBlock());
        }

        public Block GetLatestBlock()
        {
            return chain[chain.Count - 1];
        }

        public void AddBlock(Block block)
        {
            Block latestBlock = GetLatestBlock();
            block.SetIndex(latestBlock.GetIndex() + 1);
            block.SetPreviousHash(latestBlock.GetHash());
            block.SetHash(block.CalculateHash());
            chain.Add(block);
        }

        public List<Block> GetChain()
        {
            return chain;
        }

        public void SetChain(List<Block> chain)
        {
            this.chain = chain;
        }

        public bool IsValid()
        {
            for (int i = 1; i < chain.Count; i++)
            {
                Block currentBlock = chain[i];
                Block previousBlock = chain[i - 1];

                if (currentBlock.GetHash() != currentBlock.CalculateHash())
                {
                    return false;
                }
                else if (currentBlock.GetPreviousHash() != previousBlock.GetHash())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
