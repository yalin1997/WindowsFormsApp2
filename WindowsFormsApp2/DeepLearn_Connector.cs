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
            if (stream != null)
            {
                stream.Close();
            }
            connector.Close();
        }

        public void sendData(string BrainData)
        {
            if (stream.CanWrite)
            {
                byte[] data = Encoding.UTF8.GetBytes(BrainData);
                stream.Write(data, 0, data.Length);
            }
        }
        public void setReader()
        {
            if (stream.CanRead)
            {
                Console.WriteLine("can start");
            }
            else
            {
                stream = connector.GetStream();
                setReader();
            }
        }
        public string receiveResult()
        {
            byte[] receive =new byte[1];
            try
                {
                    stream.Read(receive,0,receive.Length);
                    string pack=Encoding.UTF8.GetString(receive);
                     return pack;
                }
                catch
                {
                    return "接收失敗";
                }
        }
    }
}
