using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blockchain
{
    class Client
    {
        public static List<int> ports = new List<int>();

        public static bool IsBusy(int port)
        {
            IPGlobalProperties ipGP = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] endpoints = ipGP.GetActiveTcpListeners();
            if (endpoints == null || endpoints.Length == 0) return false;
            for (int i = 0; i < endpoints.Length; i++)
                if (endpoints[i].Port == port)
                    return true;
            return false;
        }

        public static void AskForBlockChain(int currentPort)
        {
            int port = 0;
            for (int i = 7700; i < 7950; i++)
            {
                if (IsBusy(i) == true && i != currentPort)
                {
                    port = i;
                    break;
                }
            }
            if (port != 0 && port != currentPort)
            {
                try
                {
                    IPAddress ip = IPAddress.Parse("192.168.0.147");
                    IPEndPoint end = new IPEndPoint(ip, port);
                    Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                    byte[] type = new byte[5];
                    type = Encoding.ASCII.GetBytes("b" + currentPort.ToString());
                    byte[] dataToSend = new byte[5];
                    type.CopyTo(dataToSend, 0);
                    sock.Connect(end);
                    sock.Send(dataToSend);
                    sock.Close();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }

        }

        public static void SendBlockChain(int port, BlockChain blockChain)
        {
            try
            {
                IPAddress ip = IPAddress.Parse("192.168.0.147");
                IPEndPoint end = new IPEndPoint(ip, port);
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                string jsonObject = JsonConvert.SerializeObject(blockChain.GetChain());
                byte[] type = new byte[1];
                type = Encoding.ASCII.GetBytes("c");
                byte[] jsonInBytes = Encoding.ASCII.GetBytes(jsonObject);
                int dataLength = jsonInBytes.Length;
                byte[] dataToSend = new byte[1 + dataLength];
                type.CopyTo(dataToSend, 0);
                jsonInBytes.CopyTo(dataToSend, 1);

                sock.Connect(end);
                sock.Send(dataToSend);
                sock.Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
        public static void Vote(Block block)
        {
            ports.Clear();
            for (int i = 7700; i < 7950; i++)
            {
                if (IsBusy(i) == true)
                {
                    ports.Add(i);
                }
            }
            foreach (int port in ports)
            {
                try
                {
                    IPAddress ip = IPAddress.Parse("192.168.0.147");
                    IPEndPoint end = new IPEndPoint(ip, port);
                    Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                    string jsonObject = JsonConvert.SerializeObject(block);
                    byte[] type = new byte[1];
                    type = Encoding.ASCII.GetBytes("a");
                    byte[] jsonInBytes = Encoding.ASCII.GetBytes(jsonObject);
                    int dataLength = jsonInBytes.Length;
                    byte[] dataToSend = new byte[1 + dataLength];
                    type.CopyTo(dataToSend, 0);
                    jsonInBytes.CopyTo(dataToSend, 1);

                    sock.Connect(end);
                    sock.Send(dataToSend);
                    sock.Close();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message);
                }
            }
        }
    }
}
