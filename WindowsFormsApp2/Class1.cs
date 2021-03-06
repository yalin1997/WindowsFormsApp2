﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    class Class1 : IDisposable
    {
        private List<double> outputQueue = new List<double>();
        public Class1()
        {

        }
        public void addQueue(double input,int range)
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
        public int getAverage()
        {
            return (int)outputQueue.Average();
        }
        public void Dispose()
        {
            outputQueue = null;
        }
        public List<double> getList()
        {
            lock (outputQueue) { return outputQueue; }
        }
    }
}
