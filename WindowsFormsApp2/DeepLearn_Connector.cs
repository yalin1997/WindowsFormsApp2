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

        public DeepLearn_Connector(String IP , Int32 port)
        {
            IPAddress ip = IPAddress.Parse(IP);
            IPEndPoint iPAddress = new IPEndPoint(ip, port);
            connector = new TcpClient(iPAddress);
        }

        public void Dispose()
        {
            stream.Close();
            connector.Close();
        }

        public void sendData(string BrainData)
        {
            Task sendT= Task.Run(()=>{
                Byte[] data = Encoding.Unicode.GetBytes(BrainData);
                stream = connector.GetStream();
                stream.Write(data, 0, data.Length);
            });
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
                setReader();
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
