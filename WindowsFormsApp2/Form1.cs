﻿using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using Jayrock.Json;
using Jayrock.Json.Conversion;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        Stream stream;
        TcpClient tcpClient;
        Boolean ready;
        //Task rawEgg;
        Pen bluePen = new Pen(Color.Blue);
        Graphics g;
        Graphics g2;
        Graphics g3;
        Graphics g4;
        Graphics g5;
        Graphics g6;
        Graphics g7;
        Graphics g8;
       
        private List<IDictionary> dataList = new List<IDictionary>();
        //Microsoft.Office.Interop.Excel.Application ExcelApp;
        private const string Path2 = @"..\";
        private int count=0;
        byte[] buffer = new byte[2048];
        int bytesRead;
        byte[] myWriteBuffer = Encoding.ASCII.GetBytes(@"{""enableRawOutput"": false, ""format"": ""Json""}");
        double delta=0;
        double theta=0;
        double lowAlpha=0;
        double highAlpha = 0;
        double lowBeta = 0;
        double highBeta = 0;
        double lowGamma = 0;
        double highGamma = 0;
        public Form1()
        {
            InitializeComponent();
        }
        public String  ParseJSON(string inputJson)
        {
            
            String strDelta;
            String strTheta;
            String strlowAlpha;
            String strhighAlpha;
            String strlowBeta;
            String strhighBeta;
            String strlowGamma;
            String strhighGamma;
            String attention;
            String medition;
            if (inputJson != "")
            {
                try
                {
                    //Console.WriteLine(inputJson);
                    IDictionary data = (IDictionary)JsonConvert.Import(typeof(IDictionary), inputJson);
                    //Console.WriteLine(data);


                    if (data.Contains("eegPower") && data.Contains("eSense"))
                    {
                        data.Add("Time", DateTime.Now.TimeOfDay);
                        int sig = Int32.Parse(data["poorSignalLevel"].ToString());
                        if (sig == 0)
                        {
                            if (button3.Enabled == false)
                            {
                                data.Add("Emotion", 0);
                            }
                            else if (button4.Enabled == false)
                            {
                                data.Add("Emotion", 1);
                            }
                            else if (button5.Enabled == false)
                            {
                                data.Add("Emotion", 2);
                            }
                            else if (button6.Enabled == false)
                            {
                                data.Add("Emotion", 3);
                            }
                            //dataList.Add(data);
                        }
                        int signal = Int32.Parse(data["poorSignalLevel"].ToString());
                        label1.InvokeIfRequired(() =>
                        {
                            label1.Text = "訊號強度:" + data["poorSignalLevel"].ToString();
                        });
                        if (signal < 200 && signal > 0)
                        {
                            pictureBox1.Image = Image.FromFile("D:\\專題\\connecting2_v2.png");
                        }
                        else if (signal >= 200)
                        {
                            pictureBox1.Image = Image.FromFile("D:\\專題\\nosignal_v2.png");
                        }
                        else
                        {
                            pictureBox1.Image = Image.FromFile("D:\\專題\\connected_v2.png");
                        }

                        IDictionary EegPower = (IDictionary)JsonConvert.Import(typeof(IDictionary), data["eegPower"].ToString());
                        IDictionary eSense = (IDictionary)JsonConvert.Import(typeof(IDictionary), data["eSense"].ToString());
                        strDelta = EegPower["delta"].ToString();
                        strTheta = EegPower["theta"].ToString();
                        strlowAlpha = EegPower["lowAlpha"].ToString();
                        strhighAlpha = EegPower["highAlpha"].ToString();
                        strlowBeta = EegPower["lowBeta"].ToString();
                        strhighBeta = EegPower["highBeta"].ToString();
                        strlowGamma = EegPower["lowGamma"].ToString();
                        strhighGamma = EegPower["highGamma"].ToString();

                        delta = Int32.Parse(strDelta);
                        theta = Int32.Parse(strTheta);
                        lowAlpha = Int32.Parse(strlowAlpha);
                        highAlpha = Int32.Parse(strhighAlpha);
                        lowBeta = Int32.Parse(strlowBeta);
                        highBeta = Int32.Parse(strhighBeta);
                        lowGamma = Int32.Parse(strlowGamma);
                        highGamma = Int32.Parse(strhighGamma);
                        attention = eSense["attention"].ToString();
                        medition = eSense["meditation"].ToString();

                        paneldelta.Invalidate();
                        panel1.Invalidate();
                        panel2.Invalidate();
                        panel3.Invalidate();
                        panel4.Invalidate();
                        panel5.Invalidate();
                        panel6.Invalidate();
                        panel7.Invalidate();

                        label10.InvokeIfRequired(() =>
                        {
                            label10.Text = strDelta;
                        });
                        label12.InvokeIfRequired(() =>
                        {
                            label12.Text = strTheta;
                        });
                        label18.InvokeIfRequired(() =>
                        {
                            label18.Text = strlowAlpha;
                        });
                        label13.InvokeIfRequired(() =>
                        {
                            label13.Text = strhighAlpha;
                        });
                        label14.InvokeIfRequired(() =>
                        {
                            label14.Text = strlowBeta;
                        });
                        label15.InvokeIfRequired(() =>
                        {
                            label15.Text = strhighBeta;
                        });
                        label16.InvokeIfRequired(() =>
                        {
                            label16.Text = strlowGamma;
                        });
                        label17.InvokeIfRequired(() =>
                       {
                           label17.Text = strhighGamma;
                       });
                        label20.InvokeIfRequired(() =>
                        {
                            label20.Text = eSense["attention"].ToString();
                        });
                        label22.InvokeIfRequired(() =>
                        {
                            label22.Text = eSense["meditation"].ToString();
                        });
                        String returnStr = String.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} ", strDelta, strTheta, strlowAlpha, strhighAlpha, strlowBeta, strhighBeta, strlowGamma, strhighGamma, attention, medition);
                        returnStr += String.Format("{0} {1} {2}", data["poorSignalLevel"], data["Time"], data["Emotion"]);
                        return returnStr;
                    }
                    else {
                        return "";
                    }
                }
                catch(Jayrock.Json.JsonException jj)
                {
                    Console.WriteLine("Exception:"+jj);
                    return "";
                }
            }
            else
            {
                //Console.WriteLine(" ------------------");
                return "";
            }
        }
  
        private async void button1_Click(object sender, EventArgs e)
        {
                button1.Enabled=false;
                count++;
                ready = true;
                int Times = Int32.Parse(textBox1.Text == "" ? "0" : textBox1.Text);
                 Boolean infinity = false;
                if (Times == 0)
                {
                    infinity = true;
                }
                Task t =Task.Run(() => { 
                    try
                    {
                        tcpClient = new TcpClient("127.0.0.1", 13854);
                        stream = tcpClient.GetStream();
                        // Sending configuration packet to TGC              
                        if (stream.CanWrite)
                        {
                            stream.Write(myWriteBuffer, 0, myWriteBuffer.Length);
                        }
                        //start Reading
                        if (stream.CanRead)
                        {
                            int i = 0;
                            String tempText;
                            using(DataSaver saver = new DataSaver(Path2 + "Data_for_learn_"+count+".txt"))
                            {
                                while (dataList.Count <= Times || infinity)
                                {
                                    if (!ready)
                                    {
                                        break;
                                    }
                                    // This should really be in it's own thread                   
                                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                                    string[] packets = Encoding.UTF8.GetString(buffer, 0, bytesRead).Split('\r');
                                    foreach (string json in packets)
                                    {
                                        tempText=ParseJSON(json.Trim());
                                        if (tempText != "") {
                                            Console.WriteLine(tempText);
                                            saver.AddData(tempText);
                                        }
                                    }
                                    i++;
                                }
                            }
                        }
                    }
                    catch (SocketException se) {
                        Console.WriteLine(se.ToString());
                    }
                });
            await t;
            if (tcpClient != null)
            {
                tcpClient.Close();
            }
            button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            ready=false;
            button1.Enabled = true;
           

        }
        
        /*private void outputExcel()
        {
            for(int i = 0; i < dataList.Count; i++)
            {
                IDictionary temp = dataList[i];
                IDictionary EegTemp = (IDictionary)JsonConvert.Import(typeof(IDictionary), temp["eegPower"].ToString());
                ExcelApp.Cells[i + 2, 1] = EegTemp["delta"];
                ExcelApp.Cells[i + 2, 2] = EegTemp["theta"];
                ExcelApp.Cells[i + 2, 3] = EegTemp["lowAlpha"];
                ExcelApp.Cells[i + 2, 4] = EegTemp["highAlpha"];
                ExcelApp.Cells[i + 2, 5] = EegTemp["lowBeta"];
                ExcelApp.Cells[i + 2, 6] = EegTemp["highBeta"];
                ExcelApp.Cells[i + 2, 7] = EegTemp["lowGamma"];
                ExcelApp.Cells[i + 2, 8] = EegTemp["highGamma"];
                ExcelApp.Cells[i + 2, 8] = temp["poorSignalLevel"];
            }

        }*/
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBoxRaw_Click(object sender, EventArgs e)
        {

        }

        private void pictureBoxlowBeta_Click(object sender, EventArgs e)
        {

        }

        private void paneldelta_Paint(object sender, PaintEventArgs e)
        {
            g = paneldelta.CreateGraphics();

            int xPoint = (paneldelta.Size.Width / 2) - 10;

            double yPoint = delta>= 1000000 ? paneldelta.Size.Height*(0.01): paneldelta.Size.Height * (1-(delta/ 1000000));
            if (yPoint > 93)
            {
                yPoint = 93;
            }

            g.DrawRectangle(bluePen, new System.Drawing.Rectangle(xPoint, (int)yPoint, paneldelta.Size.Width / 8, (int)(paneldelta.Size.Height-yPoint )));
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            g2 = panel1.CreateGraphics();

            int xPoint = (panel1.Size.Width / 2) - 10;
 
            double yPoint = theta > 1000000 ? panel1.Size.Height * (0.99) : panel1.Size.Height * (1 - (theta / 1000000));
            if (yPoint > 93)
            {
                yPoint = 93;
            }

            g2.DrawRectangle(bluePen, new System.Drawing.Rectangle(xPoint, (int)yPoint, panel1.Size.Width / 8, (int)(panel1.Size.Height - yPoint)));
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

            g3 = panel2.CreateGraphics();

            int xPoint = (panel2.Size.Width / 2) - 10;

            double yPoint = lowAlpha > 1000000 ? panel2.Size.Height * (0.99) : panel2.Size.Height * (1 - (lowAlpha / 1000000));
            if (yPoint > 93)
            {
                yPoint = 93;
            }

            g3.DrawRectangle(bluePen, new System.Drawing.Rectangle(xPoint, (int)yPoint, panel2.Size.Width / 8, (int)(panel2.Size.Height - yPoint)));
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

            g4 = panel3.CreateGraphics();

            int xPoint = (panel3.Size.Width / 2) - 10;

            double yPoint = highAlpha > 1000000 ? panel3.Size.Height * (0.99) : panel3.Size.Height * (1 - (highAlpha / 1000000));
            if (yPoint > 93)
            {
                yPoint = 93;
            }

            g4.DrawRectangle(bluePen, new System.Drawing.Rectangle(xPoint, (int)yPoint, panel3.Size.Width / 8, (int)(panel3.Size.Height - yPoint)));
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

            g5 = panel4.CreateGraphics();

            int xPoint = (panel4.Size.Width / 2) - 10;

            double yPoint = lowBeta > 1000000 ? panel4.Size.Height * (0.99) : panel4.Size.Height * (1 - (lowBeta / 1000000));
            if (yPoint > 93)
            {
                yPoint = 93;
            }

            g5.DrawRectangle(bluePen, new System.Drawing.Rectangle(xPoint, (int)yPoint, panel4.Size.Width / 8, (int)(panel4.Size.Height - yPoint)));
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

            g6 = panel5.CreateGraphics();

            int xPoint = (paneldelta.Size.Width / 2) - 10;

            double yPoint = highBeta > 1000000 ? panel5.Size.Height * (0.99) : panel5.Size.Height * (1 - (highBeta / 1000000));
            if (yPoint > 93)
            {
                yPoint = 93;
            }

            g6.DrawRectangle(bluePen, new System.Drawing.Rectangle(xPoint, (int)yPoint, panel5.Size.Width / 8, (int)(panel5.Size.Height - yPoint)));
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            g7 = panel6.CreateGraphics();

            int xPoint = (panel6.Size.Width / 2) - 10;

            double yPoint = lowGamma > 1000000 ? panel6.Size.Height * (0.99) : panel6.Size.Height * (1 - (lowGamma / 1000000));
            if (yPoint > 93)
            {
                yPoint = 93;
            }
 
            g7.DrawRectangle(bluePen, new System.Drawing.Rectangle(xPoint, (int)yPoint, panel6.Size.Width / 8, (int)(panel6.Size.Height - yPoint)));
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            g8 = panel7.CreateGraphics();

            int xPoint = (panel7.Size.Width / 2) - 10;

            double yPoint = highGamma > 1000000 ? panel7.Size.Height * (0.99) : panel7.Size.Height * (1 - (highGamma / 1000000));
            if (yPoint > 93)
            {
                yPoint = 93;
            }

            g8.DrawRectangle(bluePen, new System.Drawing.Rectangle(xPoint, (int)yPoint, panel7.Size.Width / 8, (int)(panel7.Size.Height - yPoint)));
        }
        /*private void ExcelOutPut()
        {
            ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            ExcelApp.DisplayAlerts = false;

            Workbook Excel_WB = ExcelApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            Worksheet WS1 = (Worksheet)Excel_WB.Worksheets[1];
            WS1.Name = "腦波資料";
            WS1.Activate();
            ExcelApp.Cells[1, 1] = "Delta";
            ExcelApp.Cells[1, 2] = "Theta";
            ExcelApp.Cells[1, 3] = "Low Alpha";
            ExcelApp.Cells[1, 4] = "High Alpha";
            ExcelApp.Cells[1, 5] = "Low Beta";
            ExcelApp.Cells[1, 6] = "High Beta";
            ExcelApp.Cells[1, 7] = "Low Gamma";
            ExcelApp.Cells[1, 8] = "High Gamma";
            ExcelApp.Cells[1, 9] = "訊號品質";
            outputExcel();
            Excel_WB.SaveAs("D:\\專題", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        }*/
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(button3.Enabled == true)
            {
                button3.Enabled = false;
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
            }
            else
            {
                button3.Enabled = true;
            }
   
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (button4.Enabled == true)
            {
                button4.Enabled = false;
                button3.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
            }
            else
            {
                button4.Enabled = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (button5.Enabled == true)
            {
                button5.Enabled = false;
                button4.Enabled = true;
                button3.Enabled = true;
                button6.Enabled = true;
            }
            else
            {
                button5.Enabled = true;
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (button6.Enabled == true)
            {
                button6.Enabled = false;
                button4.Enabled = true;
                button5.Enabled = true;
                button3.Enabled = true;
            }
            else
            {
                button6.Enabled = true;
            }
 
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button6.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button3.Enabled = true;
        }
    }
}
public static class Extension
{
    //非同步委派更新UI
    public static void InvokeIfRequired(this Control control, MethodInvoker action)
    {
        if (control.InvokeRequired)//在非當前執行緒內 使用委派
        {
            control.Invoke(action);
        }
        else
        {
            action();
        }
    }
}