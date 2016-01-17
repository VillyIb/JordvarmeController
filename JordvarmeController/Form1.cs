using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using EU.Iamia.Logging;
using JordvarmeController.Businesslayer;

namespace JordvarmeController
{
    public partial class Form1 : Form
    {
        private static readonly ILog Logger = LogManager.GetLogger("eu.iamia.JordvarmeController.Form1");
        private static readonly ILog BoosterLog = LogManager.GetLogger("Booster");

        private int RetryCount { get; set; }

        /// <summary>
        /// Methods can flip this and the browser shoud be closed and reopened.
        /// </summary>
        public bool Restart { get; protected set; }

        private bool MustRefresh { get; set; }

        private ControllerSettings Settings { get; set; }
        public Form1()
        {
            InitializeComponent();
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("da-DK");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Measurements = new List<ExtractData>(2);
            Settings = new ControllerSettings();
            XuOffset.Text = Settings.Offset.ToString(CultureInfo.CurrentUICulture);
            XuThreshold.Text = Settings.Threshold.ToString(CultureInfo.CurrentUICulture);
            XuAmplificationUp.Text = Settings.AmplificationUp.ToString(CultureInfo.CurrentUICulture);
            XuAmplificationDown.Text = Settings.AmplificatinDown.ToString(CultureInfo.CurrentUICulture);
            Restart = false;
        }

        private void XuLogin_Click(object sender, EventArgs e)
        {
            XuStop_Click(sender, e);
            XuWeBrowser.Url = new Uri("https://cmi.ta.co.at/portal/ta/loginformular/");
        }

        private void XuSchema_Click(object sender, EventArgs e)
        {
            XuStop_Click(sender, e);
            XuWeBrowser.Url = new Uri("https://cmi.ta.co.at/webi/CMI004096/schema.html#1");
            //XuWeBrowser.Refresh();
        }

        #region unused
        private void oldAnalyze()
        {
            //XuAnalyzeDom.Enabled = false;
            try
            {
                var t1 = XuWeBrowser.Document;
                var t2 = XuWeBrowser.Document.DomDocument;
                var t3 = XuWeBrowser.DocumentText;
                //var t5 = XuWeBrowser.DocumentCompleted

                mshtml.IHTMLDocument2 iDoc = (mshtml.IHTMLDocument2)XuWeBrowser.Document.DomDocument;

                if (iDoc != null)
                {
                    var t14 = iDoc.body.outerHTML;

                    var t21 = new XmlDocument();
                    var t22 = $"<?xml version=\"1.0\" encoding=\"utf - 8\" ?> {t14}";
                    var t23 = t22.Replace("<br>", "<br />").Replace("png\">", "png\" />");
                    t21.LoadXml(t23);


                    // <div id="pos8"> 1,14 bar  </div>
                    var xp1 = "//div[@id='pos8']";
                    var t33 = t21.DocumentElement.SelectSingleNode(xp1);

                    float pressureIn;
                    var t53 = t33.InnerText.Substring(2).Replace("bar", "");
                    float.TryParse(t53, out pressureIn);

                    // <div id="pos9"> 1,26 bar  </div>
                    var xp2 = "//div[@id='pos9']";
                    var t34 = t21.DocumentElement.SelectSingleNode(xp2);

                    float pressureOut;
                    var t54 = t34.InnerText.Substring(2).Replace("bar", "");
                    float.TryParse(t54, out pressureOut);

                    // <div id="pos10"><a href="javascript:loadChanger('11020C50180');">AUTO<br /> 3,90 V  </a>  </div>
                    var xp3 = "//div[@id='pos10']/a";
                    var t35 = t21.DocumentElement.SelectSingleNode(xp3);

                    float speed;
                    var t55 = t35.InnerXml.Substring(11).Replace("V", "");
                    float.TryParse(t55, out speed);

                    // <div id="pos5">~    0 l/h  </div>
                    float crossFlow;
                    {
                        const string xPath = "//div[@id='pos5']";
                        var xmlNode = t21.DocumentElement.SelectSingleNode(xPath);

                        var value = xmlNode.InnerText.Substring(4).Replace("l/h", "");
                        float.TryParse(value, out crossFlow);
                    }

                    //   <div id="pos2">Inde:   9,5  °C  </div>
                    float tempAirOutdor;
                    {
                        const string xPath = "//div[@id='pos2']";
                        var xmlNode = t21.DocumentElement.SelectSingleNode(xPath);

                        var value = xmlNode.InnerText.Substring(7).Replace("°C", "");
                        float.TryParse(value, out tempAirOutdor);
                    }

                    //   <div id="pos4">Ude:   7,6  °C  </div>
                    float tempAirIndor;
                    {
                        const string xPath = "//div[@id='pos4']";
                        var xmlNode = t21.DocumentElement.SelectSingleNode(xPath);

                        var value = xmlNode.InnerText.Substring(7).Replace("°C", "");
                        float.TryParse(value, out tempAirIndor);
                    }

                    // <div id="pos6">  9,1  °C  </div>
                    float tempBrineIn;
                    {
                        const string xPath = "//div[@id='pos6']";
                        var xmlNode = t21.DocumentElement.SelectSingleNode(xPath);

                        var value = xmlNode.InnerText.Substring(2).Replace("°C", "");
                        float.TryParse(value, out tempBrineIn);
                    }

                    //   <div id="pos7">  6,8  °C  </div>
                    float tempBrineOut;
                    {
                        const string xPath = "//div[@id='pos7']";
                        var xmlNode = t21.DocumentElement.SelectSingleNode(xPath);

                        var value = xmlNode.InnerText.Substring(2).Replace("°C", "");
                        float.TryParse(value, out tempBrineOut);
                    }

                    var msg =
                        $"{DateTime.UtcNow:HH:mm:ss};{speed:F};{(pressureOut - pressureIn) * 10:F};{Math.Min(99f, crossFlow) / 10f:F1};{pressureIn:F};{pressureOut:F};{tempBrineIn:F1};{tempBrineOut:f1};{tempAirIndor:F1};{tempAirOutdor:F1};{crossFlow}";

                    BoosterLog.Info(msg);

                    var t19 = t14;
                }



                var t4 = t1;
            }
            catch (NullReferenceException)
            { }

            XuAnalyzeDom.Enabled = false;
            System.Threading.Thread.Sleep(500);
            XuAnalyzeDom.Enabled = true;
        }
        #endregion



        private List<ExtractData> Measurements { get; set; }

        private int CountKeep { get; set; }

        private int CurrentIndex { get; set; }

        private int? NewIndex { get; set; }

        /// <summary>
        /// Bypass a number of changes.
        /// </summary>
        public int BypassChanges { get; set; }

        private void ChangeSpeed(int index)
        {
            NewIndex = null;

            if (0 <= index && index <= 200 && BypassChanges-- <= 0)
            {
                NewIndex = (CurrentIndex != index) ? (int?)index : null;

                XuStop_Click(null, null);

                ////while (Measurements.Count > 0)
                ////{
                ////    Measurements.RemoveAt(0);
                ////}

                var uri = String.Format(
                        "https://cmi.ta.co.at/webi/CMI004096/change.cgi?changeadr=01B51020C7&changeto={0}"
                        , index
                    );
                XuWeBrowser.Url = new Uri(uri);
                //XuWeBrowser.Refresh();

                MustRefresh = true;

                BypassChanges = 1;
            }
        }

        private int MapSpeedToIdex(float value)
        {
            // 0,0  ->   0
            // 10,0 -> 200

            var index = 200 / 10.0f * value;

            return (int)index;
        }


        private void Increase(float pdif)
        {
            int amplification;
            amplification = int.TryParse(XuAmplificationUp.Text, out amplification) ? amplification : 100;

            var dif = (int)(-pdif * amplification) + 1;

            ChangeSpeed(Math.Min(200, CurrentIndex + dif));
        }

        private void Decrease(float pdif)
        {
            int amplification;
            amplification = int.TryParse(XuAmplificationDown.Text, out amplification) ? amplification : 100;

            var dif = (int)(-pdif * amplification) - 1;
            ChangeSpeed(Math.Max(20, CurrentIndex + dif));
        }

        private void Keep()
        {
            NewIndex = null;

            if (CountKeep-- <= 0)
            {
                ChangeSpeed(CurrentIndex - 1);
                CountKeep = 10; // 5 min.
            }
        }



        private void Analyze()
        {
            try
            {
                var iDoc = (mshtml.IHTMLDocument2)XuWeBrowser.Document.DomDocument;
                if (iDoc != null)
                {
                    var ed = new ExtractData { OuterHtmlSource = iDoc.body.outerHTML };
                    if (ed.TryParse())
                    {
                        if (ed.TryAnalyzeXml())
                        {
                            if (ed.IsValid())
                            {
                                ed.Offset = float.Parse( XuOffset.Text, CultureInfo.CurrentUICulture);
                                ed.AmplificationDown = float.Parse(XuAmplificationDown.Text, CultureInfo.CurrentUICulture);
                                ed.AmplificationUp = float.Parse(XuAmplificationUp.Text, CultureInfo.CurrentUICulture);

                                Measurements.Add(ed);

                                if (Measurements.Count >= 2)
                                {
                                    if (!(Measurements[0].Equals(Measurements[1])))
                                    {
                                        // Measurement changed

                                        var current = Measurements[1];

                                        var fp = new CultureInfo("da");

                                        // Date changed.
                                        if (current.TimeStamp < new TimeSpan(0, 0, 30))
                                        {
                                            BoosterLog.Info("LocalTime;Speed;PDiff*10;CrossFl/10;PIn;POut;TBIn;TBOut;TAIns;TAOuts;CrossFlow;Change;Offset;AmplUp;AmplDwn;ControlValue;Current;New;");
                                        }


                                        float offsett;
                                        
                                        var pdif = current.PressureOut - current.PressureIn - (float.TryParse(XuOffset.Text, out offsett) ? offsett : 0.07f);

                                        float threshold;
                                        threshold = float.TryParse(XuThreshold.Text, out threshold) ? threshold : 0.011f;

                                        var lowerThreshold = -threshold;
                                        var upperThreshold = threshold;

                                        CurrentIndex = MapSpeedToIdex(current.BoosterSpeed);

                                        if (pdif < lowerThreshold)
                                        {
                                            // increase spead
                                            Increase(pdif);
                                        }
                                        else if (pdif > upperThreshold)
                                        {
                                            // decrease speed
                                            Decrease(pdif);
                                        }
                                        else
                                        {
                                            // keep speed.
                                            Keep();
                                        }

                                        BoosterLog.InfoFormat(
                                            fp
                                            , "{0};{1,6:F};{2,6:F};{3,6:F1};{4,6:F};{5,6:F};{6,6:F1};{7,6:F1};{8,6:F1};{9,6:F1};{10,6};{11,6:F2};{12,6:F2};{13,6:F2};{14,6:F2};{15,6:F2};{16,3};{17,3}"
                                            , current.TimeStamp
                                            , current.BoosterSpeed
                                            , (current.PressureOut - current.PressureIn) * 10
                                            , current.CrossFlow / 10f
                                            , current.PressureIn
                                            , current.PressureOut
                                            , current.TempBrineIn
                                            , current.TempBrineOut
                                            , current.TempAirIndoor
                                            , current.TempAirOutdoor
                                            , current.CrossFlow
                                            , Math.Round(current.BoosterSpeed - Measurements[0].BoosterSpeed, 2)
                                            , current.Offset * 10
                                            , current.AmplificationUp / 100
                                            , current.AmplificationDown / 100
                                            , pdif * 10
                                            , CurrentIndex
                                            , NewIndex
                                        );

                                    }

                                    // strip off older measurements
                                    while (Measurements.Count > 1)
                                    {
                                        Measurements.RemoveAt(0);
                                    }
                                }
                            }
                            else
                            {
                                // ranges outside limits
                                Logger.WarnFormat("Measuremen outside limits: {0}", ed);
                                Logger.DebugFormat("HTML: {0}", iDoc.body.outerHTML);
                            }
                        }
                        else
                        {
                            // unable to analyze Xml
                            // aleady logged by exception.
                        }
                    }
                    else
                    {
                        // unable to parse 
                        Logger.WarnFormat("Unable to parse Xml: \r\n{0}\r\n", iDoc.body.outerHTML);

                        // Try reload page.
                        if (RetryCount-- > 0)
                        {
                            XuSchema_Click(null, null);
                        }
                        else
                        {
                            Restart = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);
            }
        }


        private void XuAnalyzeDom_Click(object sender, EventArgs e)
        {
            Analyze();
        }
        

        private void XuTimer_Tick(object sender, EventArgs e)
        {
            XuAnalyzeDom_Click(sender, e);
            if (Restart)
            {
                Logger.Error("\r\n\r\nRestarting\r\n\r\n");
                Close();
            }
        }


        private void XuTimer1_Click(object sender, EventArgs e)
        {
            XuDelay.Start();
        }


        private void XuStop_Click(object sender, EventArgs e)
        {

            XuStop.Enabled = false;
            XuTimer1.Enabled = true;
            XuTimer.Stop();

        }


        private void XuDelay_Tick(object sender, EventArgs e)
        {
            XuDelay.Stop();
            XuStop.Enabled = true;
            XuTimer1.Enabled = false;
            XuTimer.Start();
        }


        private void XuSave_Click(object sender, EventArgs e)
        {
            float t1;

            Settings.Offset = float.TryParse(XuOffset.Text, out t1) ? t1 : Settings.Offset;
            Settings.Threshold = float.TryParse(XuThreshold.Text, out t1) ? t1 : Settings.Threshold;
            Settings.AmplificationUp = float.TryParse(XuAmplificationUp.Text, out t1) ? t1 : Settings.AmplificationUp;
            Settings.AmplificatinDown = float.TryParse(XuAmplificationDown.Text, out t1) ? t1 : Settings.AmplificatinDown;
            Settings.Save();
        }


        private void XuWeBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //var t1 = sender.GetType().Name;

            //var browser = sender as WebBrowser;

            //var ds = browser.DocumentStream;

            //ds.Seek(0L, System.IO.SeekOrigin.Begin);

            //var sr = new StreamReader(ds);

            //var data = sr.ReadToEnd();

            //var tx = data;
            if (MustRefresh)
            {
                XuSchema_Click(sender, e);
                MustRefresh = false;
            }
            else
            {
                RetryCount = 0;
                XuTimer1_Click(sender, e);
            }
        }



    }
}
