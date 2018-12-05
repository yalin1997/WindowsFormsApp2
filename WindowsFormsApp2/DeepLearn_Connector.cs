using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class DeepLearn_Connector : IDisposable
    {
        TcpClient connector;
        Stream stream;
        public DeepLearn_Connector(String IP , Int32 port)
        {
            connector = new TcpClient(IP, port);
        }

        public void Dispose()
        {
            stream.Close();
            connector.Close();
        }

        public void setData(string BrainData)
        {
            Task t = Task.Run(()=>{
                Byte[] data = Encoding.Unicode.GetBytes(BrainData);
                stream = connector.GetStream();
                stream.Write(data, 0, data.Length);
            });
        }
    }
}
