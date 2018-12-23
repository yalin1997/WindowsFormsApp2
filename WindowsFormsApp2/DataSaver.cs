using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class DataSaver: IDisposable
    {
        Queue<String> dataQ ;
        FileStream fs;
        StreamWriter saver;
        bool closed=false;
        String nextString;
        public DataSaver(String fileName)
        {
            Task.Run(() =>
            {
                dataQ = new Queue<string>();
                using (fs = new FileStream(fileName,FileMode.Append)) {
                    using (saver = new StreamWriter(fs))
                    {
                        saver.AutoFlush=true;
                        while (!closed)
                        {
                            nextString = null;
                            lock (dataQ)
                            {
                                if (dataQ.Count == 0)
                                {
                                    Monitor.Wait(dataQ);
                                }
                            }
                            lock (dataQ)
                            {   
                                if (dataQ.Count != 0)
                                {
                                    nextString = dataQ.Dequeue();
                                }
                            }

                            if (nextString != null)
                            {
                                saver.WriteLine(nextString);
                            }
                        }
                    }
                }
            });
        }

        public void Dispose()
        {
            closed = true;
            lock (dataQ)
            {
                Monitor.Pulse(dataQ);
            }
        }

        public void AddData(String data) {
            lock (dataQ)
            {
                dataQ.Enqueue(data);
                Monitor.Pulse(dataQ);

            }
        }
    }
}
