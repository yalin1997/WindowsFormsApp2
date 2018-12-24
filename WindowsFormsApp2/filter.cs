using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class filter<T>:IDisposable
    {
        private List<T> outputQueue = new List<T>();
        public int Count {
            get => outputQueue.Count;
        }
        public filter()
        {

        }
        public void addQueue(T input, int range)
        {
            Task.Run(() =>
            {
                lock (outputQueue)
                {
                    if (outputQueue.Count == range)
                    {
                        outputQueue.RemoveAt(0);
                        outputQueue.Add(input);
                    }
                    else
                    {
                        outputQueue.Add(input);
                    }
                }
            });
        }
        public void Dispose()
        {
            outputQueue = null;
        }
        public List<T> getList()
        {
            lock (outputQueue) { return outputQueue; }
        }
        public string Dequeue()
        {
            string temp = "";
            var tempArr = outputQueue.ToArray();
            for(int i = 0; i < Count; i++)
            {
                temp += tempArr[i];
            }
            return temp;
         }
        }
    }


