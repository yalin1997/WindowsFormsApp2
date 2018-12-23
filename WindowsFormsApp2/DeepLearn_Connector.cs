using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class DeepLearn_Connector : IDisposable
    {
        TcpClient connector;
        NetworkStream stream;
        BinaryReader br;

        public DeepLearn_Connector()
        {
            connector = new TcpClient();
        }
        public Boolean Start_Connect(string IP, int port)
        {
            IPAddress ip = IPAddress.Parse(IP);
            IPEndPoint iPAddress = new IPEndPoint(ip, port);
            connector.Connect(iPAddress);
            if (connector.Connected)
            {
                stream = connector.GetStream();
            }
            return connector.Connected;
        }
        public void Dispose()
        {
            stream.Close();
            connector.Close();
        }

        public void sendData(string BrainData)
        {
            if (stream.CanWrite)
            {
                byte[] data = Encoding.Unicode.GetBytes(BrainData);
                stream.Write(data, 0, data.Length);
            }
        }
        public void setReader()
        {
            if (stream.CanRead)
            {
                br = new BinaryReader(stream);
            }
            else
            {
                stream = connector.GetStream();
               // this.setReader();
            }
        }
        public string receiveResult()
        {
            string receive = null;
            try
                {
                    receive = br.ReadString();
                     return receive;
                }
                catch
                {
                receive = "接收失敗";
                return receive;
            }
        }
    }
}
