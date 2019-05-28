using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;  
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Blockchain
{
    public class Block
    {
        [JsonProperty("@index")]
        private int index;
        [JsonProperty("@timeStamp")]
        private DateTime timeStamp;
        [JsonProperty("@previousHash")]
        private string previousHash;
        [JsonProperty("@hash")]
        private string hash;
        [JsonProperty("@data")]
        private string data;

        public Block(DateTime timeStamp, string previousHash, string data)
        {
            index = 0;
            this.timeStamp = timeStamp;
            this.previousHash = previousHash;
            this.data = data;
            hash = CalculateHash();
        }

        public string CalculateHash()
        {
            SHA256 sha256 = SHA256.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(timeStamp + previousHash + data);
            byte[] outputBytes = sha256.ComputeHash(inputBytes);
            return Convert.ToBase64String(outputBytes);

        }

        public int GetIndex() {
            return index;
        }

        public void SetIndex(int index)
        {
            this.index = index;
        }

        public DateTime GetTimeStamp()
        {
            return timeStamp;
        }

        public string GetPreviousHash()
        {
            return previousHash;
        }

        public void SetPreviousHash(string previousHash)
        {
            this.previousHash = previousHash;
        }

        public string GetHash()
        {
            return hash;
        }

        public void SetHash(string hash)
        {
            this.hash = hash;
        }

        public string GetData()
        {
            return data;
        }

        public void SetData(string data)
        {
            this.data = data;
        }
    }
}
