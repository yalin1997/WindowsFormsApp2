using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using Jayrock.Json.Conversion;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using System.Diagnostics;
using System.Threading;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        Stream stream;
        TcpClient tcpClient;
        DeepLearn_Connector DC ;
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
        int tickUnit;
        private List<IDictionary> dataList = new List<IDictionary>();

        private string Path2 = @"..\" ;
        private int count=0;
        byte[] buffer = new byte[2048];
        Class1 deltaChart = new Class1();
        Class1 thetaChart = new Class1();
        Class1 lowAlphaChart = new Class1();
        Class1 highAlphaChart = new Class1();
        Class1 lowBetaChart = new Class1();
        Class1 highBetaChart = new Class1();
        Class1 lowGammaChart = new Class1();
        Class1 highGammaChart = new Class1();
        int bytesRead;
        byte[] myWriteBuffer = Encoding.ASCII.GetBytes(@"{""enableRawOutput"": false, ""format"": ""Json""}");
        filter<string> SendQueue = new filter<string>();
        double delta=0;
        double theta=0;
        double lowAlpha=0;
        double highAlpha = 0;
        double lowBeta = 0;
        double highBeta = 0;
        double lowGamma = 0;
        double highGamma = 0;
        private string name;
        private string year;

        public Form1(string name ,string year)
        {
            InitializeComponent();
            this.name = name;
            this.year = year;
            tickUnit = paneldelta.Width / 10;
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
                    IDictionary data = (IDictionary)JsonConvert.Import(typeof(IDictionary), inputJson);


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
                        }
                        int signal = Int32.Parse(data["poorSignalLevel"].ToString());
                        label1.InvokeIfRequired(() =>
                        {
                            label1.Text = "訊號強度:" + data["poorSignalLevel"].ToString();
                        });
                        if (signal < 200 && signal > 0)
                        {
                            pictureBox1.Image = Image.FromFile(Path2 + "connecting2_v2.png");
                        }
                        else if (signal >= 200)
                        {
                            pictureBox1.Image = Image.FromFile(Path2 +"nosignal_v2.png");
                        }
                        else
                        {
                            pictureBox1.Image = Image.FromFile(Path2+"connected_v2.png");
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
                        deltaChart.addQueue(delta,10);
                        thetaChart.addQueue(theta, 10);
                        lowAlphaChart.addQueue(lowAlpha, 10);
                        highAlphaChart.addQueue(highAlpha, 10);
                        lowBetaChart.addQueue(lowBeta, 10);
                        highBetaChart.addQueue(highBeta, 10);
                        lowGammaChart.addQueue(lowGamma, 10);
                        highGammaChart.addQueue(highGamma, 10);

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
                        label24.InvokeIfRequired(() =>
                        {
                            label24.Text=deltaChart.getAverage().ToString();
                        });
                        label25.InvokeIfRequired(() =>
                        {
                            label25.Text = thetaChart.getAverage().ToString();
                        });
                        label26.InvokeIfRequired(() =>
                        {
                            label26.Text = lowAlphaChart.getAverage().ToString();
                        });
                        label27.InvokeIfRequired(() =>
                        {
                            label27.Text = highAlphaChart.getAverage().ToString();
                        });
                        label28.InvokeIfRequired(() =>
                        {
                            label28.Text = lowBetaChart.getAverage().ToString();
                        });
                        label29.InvokeIfRequired(() =>
                        {
                            label29.Text = highBetaChart.getAverage().ToString();
                        });
                        label30.InvokeIfRequired(() =>
                        {
                            label30.Text = lowGammaChart.getAverage().ToString();
                        });
                        label31.InvokeIfRequired(() =>
                        {
                            label31.Text = highGammaChart.getAverage().ToString();
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
                return "";
            }
        }
 
        public static void WriteLog(string message)
        {
            string DIRNAME = AppDomain.CurrentDomain.BaseDirectory + @"\Log\";
            string FILENAME = DIRNAME + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            if (!Directory.Exists(DIRNAME))
                Directory.CreateDirectory(DIRNAME);

            if (!File.Exists(FILENAME))
            {
                // The File.Create method creates the file and opens a FileStream on the file. You neeed to close it.
                File.Create(FILENAME).Close();
            }
            using (StreamWriter sw = File.AppendText(FILENAME))
            {
                Log(message, sw);
            }
        }

        private static void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }
        private Boolean openConnector()
        {
            Boolean createNew;
            using (Mutex mutex = new Mutex(true, "ThinkGear Connector", out createNew))
            {
                if (createNew)
                {
                    Process Connector = new Process();
                    Connector.StartInfo.FileName = System.AppDomain.CurrentDomain.BaseDirectory + @"ThinkGear Connector\win32\ThinkGear Connector.exe";
                    return Connector.Start();
                }
                else
                {
                    Thread.Sleep(1000);
                    return true;
                }
            }

        }
        private async void button1_Click(object sender, EventArgs e)
        {
                WriteLog("start connect!");
                button1.Enabled=false;
                count++;
                ready = true;
                bool isConncted = false;
                int Times = Int32.Parse(textBox1.Text == "" ? "0" : textBox1.Text);
                 Boolean infinity = false;
                if (Times == 0)
                {
                    infinity = true;
                }
                Task t =Task.Run(() => { 
                    try
                    {
                        if (openConnector())
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
                                using (DataSaver saver = new DataSaver(Path2 + name + year + "_"+count + ".txt"))
                                {
                                    if (checkBox1.CheckState == CheckState.Checked && !isConncted)
                                    {
                                        DC = new DeepLearn_Connector();
                                        isConncted = DC.Start_Connect("192.168.10.166", 8888);
                                    }
                                    while (dataList.Count <= Times || infinity)
                                     {
                                            if (!ready)
                                            {
                                                if (tcpClient != null)
                                                {
                                                    tcpClient.Close();
                                                }
                                                DC.Dispose();
                                                break;
                                            }                 
                                            bytesRead = stream.Read(buffer, 0, buffer.Length);
                                            string[] packets = Encoding.UTF8.GetString(buffer, 0, bytesRead).Split('\r');
                                            foreach (string json in packets)
                                            {
                                                tempText = ParseJSON(json.Trim());

                                                if (tempText != "")
                                                {
                                                    saver.AddData(tempText);

                                                    SendQueue.addQueue(tempText + "\n",5);
                                                    if (SendQueue.Count >= 5)
                                                    {
                                                        string brainData;
                                                        brainData = SendQueue.Dequeue();
                                                        if (isConncted)
                                                        {
                                                            DC.sendData(brainData);
                                                            Console.WriteLine("TCP Message sended:" + brainData);
                                                            Task receiver = Task.Run(() =>
                                                            {
                                                                Console.WriteLine("Task start!");
                                                                while (isConncted)
                                                                {
                                                                            DC.setReader();
                                                                            var temp = DC.receiveResult();
                                                                            Console.WriteLine("ANS:" + temp.ToString());
                                                                            switch (temp.ToString())
                                                                            {
                                                                                case "0":
                                                                                    pictureBox2.InvokeIfRequired(() =>
                                                                                    {
                                                                                        pictureBox2.Image = Image.FromFile(Path2 + "靈魂畫師.png");
                                                                                    });
                                                                                    break;
                                                                                case "1":
                                                                                    pictureBox2.InvokeIfRequired(() =>
                                                                                    {
                                                                                        pictureBox2.Image = Image.FromFile(Path2 + "靈魂畫師平靜.png");
                                                                                    });
                                                                                    break;
                                                                                case "2":
                                                                                    pictureBox2.InvokeIfRequired(() =>
                                                                                    {
                                                                                        pictureBox2.Image = Image.FromFile(Path2 + "哀.png");
                                                                                    });
                                                                                    break;
                                                                                case "3":
                                                                                    pictureBox2.InvokeIfRequired(() =>
                                                                                    {
                                                                                        pictureBox2.Image = Image.FromFile(Path2 + "怒.png");
                                                                                    });
                                                                                break;
                                                                             }
                                                                    
                                                                }
                                                            });
                                                        }
                                                    }
                                                }
                                            }
                                            i++;
                                     }
                                }
                            }
                        }
                    }
                    catch (SocketException se) {
                        WriteLog(se.ToString());
                        Console.WriteLine(se.ToString());
                        checkBox1.InvokeIfRequired(() =>
                        {
                            checkBox1.CheckState = CheckState.Unchecked;
                            label1.Text = "已經取消連接Server，請重新連線";
                        });
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
            label1.Text = "停止";
            ready=false;
            button1.Enabled = true;
        }
        
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
            if(button8.Text == "改為折線圖顯示") {
                g = paneldelta.CreateGraphics();

                int xPoint = (paneldelta.Size.Width / 2) - 10;

                double yPoint = delta >= 1000000 ? paneldelta.Size.Height * (0.01) : paneldelta.Size.Height * (1 - (delta / 1000000));
                if (yPoint > 93)
                {
                    yPoint = 93;
                }
                g.DrawRectangle(bluePen, new System.Drawing.Rectangle(xPoint, (int)yPoint, paneldelta.Size.Width / 8, (int)(paneldelta.Size.Height - yPoint)));
            }
            else
            {
                try
                {
                    int lastPoint = 0;
                    int xPoint = 0;
                    g = paneldelta.CreateGraphics();
                    List<double> tempArr = deltaChart.getList();
                    tempArr.ForEach(action: x =>
                    {
                        int yPoint = (int)(Math.Log10(x) >= 7 ? paneldelta.Size.Height * (0.01) : paneldelta.Size.Height * (1 - (Math.Log10(x) / 7)));
                        if (yPoint > 93)
                        {
                            yPoint = 93;
                        }
                        g.DrawLine(bluePen, new System.Drawing.Point((xPoint - tickUnit), lastPoint), new System.Drawing.Point(xPoint, yPoint));
                        lastPoint = yPoint;
                        xPoint += tickUnit;
                    });
                }
                catch  (InvalidOperationException ex)
                {
                    WriteLog(ex.ToString());
                }
            }
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (button8.Text == "改為折線圖顯示")
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
            else
            {
                try
                {
                    int lastPoint = 0;
                    int xPoint = 0;
                    g = panel1.CreateGraphics();
                    thetaChart.getList().ForEach(action: x =>
                    {
                        int yPoint = (int)(Math.Log10(x) >= 7 ? paneldelta.Size.Height * (0.01) : paneldelta.Size.Height * (1 - (Math.Log10(x) / 7)));
                        if (yPoint > 93)
                        {
                            yPoint = 93;
                        }
                        g.DrawLine(bluePen, new System.Drawing.Point((xPoint - tickUnit), lastPoint), new System.Drawing.Point(xPoint, yPoint));
                        lastPoint = yPoint;
                        xPoint += tickUnit;
                    });
                }
                catch (InvalidOperationException ex)
                {
                    WriteLog(ex.ToString());
                }
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            if (button8.Text == "改為折線圖顯示")
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
            else
            {
                try
                {
                    int lastPoint = 0;
                    int xPoint = 0;
                    g = panel2.CreateGraphics();
                    lowAlphaChart.getList().ForEach(action: x =>
                    {
                        int yPoint = (int)(Math.Log10(x) >= 7 ? paneldelta.Size.Height * (0.01) : paneldelta.Size.Height * (1 - (Math.Log10(x) / 7)));
                        if (yPoint > 93)
                        {
                            yPoint = 93;
                        }
                        g.DrawLine(bluePen, new System.Drawing.Point((xPoint - tickUnit), lastPoint), new System.Drawing.Point(xPoint, yPoint));
                        lastPoint = yPoint;
                        xPoint += tickUnit;
                    });
                }
                catch (InvalidOperationException ex)
                {
                    WriteLog(ex.ToString());
                }
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            if (button8.Text == "改為折線圖顯示")
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
            else
            {
                try
                {
                    int lastPoint = 0;
                    int xPoint = 0;
                    g = panel3.CreateGraphics();
                    highAlphaChart.getList().ForEach(action: x =>
                    {
                        int yPoint = (int)(Math.Log10(x) >= 7 ? paneldelta.Size.Height * (0.01) : paneldelta.Size.Height * (1 - (Math.Log10(x) / 7)));
                        if (yPoint > 93)
                        {
                            yPoint = 93;
                        }
                        g.DrawLine(bluePen, new System.Drawing.Point((xPoint - tickUnit), lastPoint), new System.Drawing.Point(xPoint, yPoint));
                        lastPoint = yPoint;
                        xPoint += tickUnit;
                    });
                }
                catch (InvalidOperationException ex)
                {
                    WriteLog(ex.ToString());
                }
               
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            if (button8.Text == "改為折線圖顯示")
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
            else
            {
                try
                {
                    int lastPoint = 0;
                    int xPoint = 0;
                    g = panel4.CreateGraphics();
                    lowBetaChart.getList().ForEach(action: x =>
                    {
                        int yPoint = (int)(Math.Log10(x) >= 7 ? paneldelta.Size.Height * (0.01) : paneldelta.Size.Height * (1 - (Math.Log10(x) / 7)));
                        if (yPoint > 93)
                        {
                            yPoint = 93;
                        }
                        g.DrawLine(bluePen, new System.Drawing.Point((xPoint - tickUnit), lastPoint), new System.Drawing.Point(xPoint, yPoint));
                        lastPoint = yPoint;
                        xPoint += tickUnit;
                    });
                }
                catch (InvalidOperationException ex)
                {
                    WriteLog(ex.ToString());
                }
               
            }
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            if (button8.Text == "改為折線圖顯示")
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
            else
            {
                try
                {
                    int lastPoint = 0;
                    int xPoint = 0;
                    g = panel5.CreateGraphics();
                    highBetaChart.getList().ForEach(action: x =>
                    {
                        int yPoint = (int)(Math.Log10(x) >= 7 ? paneldelta.Size.Height * (0.01) : paneldelta.Size.Height * (1 - (Math.Log10(x) / 7)));
                        if (yPoint > 93)
                        {
                            yPoint = 93;
                        }
                        g.DrawLine(bluePen, new System.Drawing.Point((xPoint - tickUnit), lastPoint), new System.Drawing.Point(xPoint, yPoint));
                        lastPoint = yPoint;
                        xPoint += tickUnit;
                    });
                }
                catch (InvalidOperationException ex)
                {
                    WriteLog(ex.ToString());
                }
                
            }
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            if (button8.Text == "改為折線圖顯示")
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
            else
            {
                try
                {
                    int lastPoint = 0;
                    int xPoint = 0;
                    g = panel6.CreateGraphics();
                    lowGammaChart.getList().ForEach(action: x =>
                    {
                        int yPoint = (int)(Math.Log10(x) >= 7 ? paneldelta.Size.Height * (0.01) : paneldelta.Size.Height * (1 - (Math.Log10(x) / 7)));
                        if (yPoint > 93)
                        {
                            yPoint = 93;
                        }
                        g.DrawLine(bluePen, new System.Drawing.Point((xPoint - tickUnit), lastPoint), new System.Drawing.Point(xPoint, yPoint));
                        lastPoint = yPoint;
                        xPoint += tickUnit;
                    });
                }
                catch (InvalidOperationException ex)
                {
                    WriteLog(ex.ToString());
                }
              
            }
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            if (button8.Text == "改為折線圖顯示")
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
            else
            {
                try
                {
                    int lastPoint = 0;
                    int xPoint = 0;
                    g = panel7.CreateGraphics();
                    highGammaChart.getList().ForEach(action: x =>
                    {
                    int yPoint = (int)(Math.Log10(x) >= 7 ? paneldelta.Size.Height * (0.01) : paneldelta.Size.Height * (1 - (Math.Log10(x) / 7)));
                    if (yPoint > 93)
                    {
                        yPoint = 93;
                    }
                        Point point1 = new Point((xPoint - tickUnit),lastPoint);
                        Point point2 = new Point(xPoint, yPoint);
                        g.DrawLine(bluePen, point1, point2);
                        
                        lastPoint = yPoint;
                        xPoint += tickUnit;
                        xPoint = xPoint >= panel7.Size.Width ? panel7.Size.Width : xPoint;
                    });
                }
                catch (InvalidOperationException ex)
                {
                    WriteLog(ex.ToString());
                }
                catch (OverflowException over)
                {
                    WriteLog(over.ToString());
                    Console.WriteLine(over.ToString());
                }
               
            }
        }
      
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

        private void button8_Click(object sender, EventArgs e)
        {
            if (button8.Text != "改為長條圖顯示")
            {
                button8.Text = "改為長條圖顯示";
                label1.Text = "已變更折線圖顯示";
            }
            else
            {
                button8.Text = "改為折線圖顯示";
                label1.Text = "已變更長條圖顯示";
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            switch (checkBox1.CheckState)
            {
                case CheckState.Checked:
                    Console.WriteLine("click ="+checkBox1.CheckState.ToString());
                    checkBox1.CheckState=CheckState.Checked;
                    break;
                case CheckState.Unchecked:
                    Console.WriteLine("click =" + checkBox1.CheckState.ToString());
                    checkBox1.CheckState = CheckState.Unchecked;
                    break;
                case CheckState.Indeterminate:
                    
                    break;
            }
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
