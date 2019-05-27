using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace BlockChain
{
    class Server
    {
        IPEndPoint endPoint;
        Socket socket;
        BlockChain blockChain;
        Form1 form;
        bool valid;
        BlockChain copyBlockChain;
        int port;

        public void SetBlockChain(BlockChain blockChain, Form1 form)
        {
            this.blockChain = blockChain;
            this.form = form;
        }

        public int GetPort()
        {
            return port;
        }

        public Server()
        {
            port = 0;
            bool free = false;
            while(free != true)
            {
                Random random = new Random();
                port = random.Next(7700, 7950);
                if (Client.IsBusy(port) == false)
                    free = true;
            }
            endPoint = new IPEndPoint(IPAddress.Any, port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            socket.Bind(endPoint);
        }

        public static string path;

        public void StartServer()
        {
                try
                {
                    while(true)
                    {
                        socket.Listen(100);

                        Socket clientSocket = socket.Accept();
                        byte[] clientData = new byte[1024 * 5000];
                        int receivedByteLen = clientSocket.Receive(clientData);
                        string jsonObject = Encoding.ASCII.GetString(clientData, 0, receivedByteLen);
                        char first = jsonObject.ToCharArray()[0];
                        if (first == 'a')
                        {
                            string jsonObjectBlock = jsonObject.Substring(1, jsonObject.Length - 1);
                            Block block = JsonConvert.DeserializeObject<Block>(jsonObjectBlock);
                            copyBlockChain = DeepCopy(blockChain);
                            //if (blockChain.GetChain().Count > 1)
                            //{
                            //    blockChain.GetChain()[1].SetData("Saulius Skvernelis");
                            //}
                            valid = blockChain.IsValid();
                            if (valid != true)
                            {
                                blockChain = copyBlockChain;
                                form.SetRinkimaiInvoker(blockChain);
                                form.RecalculateInvoker();
                            }
                            else
                            {
                                blockChain.AddBlock(block);
                                form.RecalculateInvoker();
                            }

                            clientSocket.Close();
                        }
                        else if(first == 'b')
                        {
                            string portNumber = jsonObject.Substring(1, jsonObject.Length - 1);
                            form.SendBlockChainInvoker(Convert.ToInt32(portNumber), blockChain);
                        }
                        else if(first == 'c')
                        {
                            string jsonObjectBlockChain = jsonObject.Substring(1, jsonObject.Length - 1);
                            List<Block> blockChainList = JsonConvert.DeserializeObject<List<Block>>(jsonObjectBlockChain);
                            blockChain.SetChain(blockChainList);
                            form.SetRinkimaiInvoker(blockChain);
                            form.RecalculateInvoker();
                        }
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
        }

        public BlockChain DeepCopy(BlockChain chain)
        {
            BlockChain temp = new BlockChain();
            foreach(Block block in chain.GetChain().Skip(1))
            {
                Block tempBlock = new Block(block.GetTimeStamp(), block.GetPreviousHash(), block.GetData());
                temp.AddBlock(tempBlock);
            }
            return temp;
        }
    }
}
