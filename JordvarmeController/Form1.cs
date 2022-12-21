using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using EU.Iamia.Logging;
using JordvarmeController.Businesslayer;

namespace JordvarmeController
{
    public partial class Form1 : Form
    {
        //private static readonly ILog Logger = LogManager.GetLogger("eu.iamia.JordvarmeController.Form1");
        //private static readonly ILog BoosterLog = LogManager.GetLogger("Booster");

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
            //XuWeBrowser.Url = new Uri("https://cmi004096.cmi.ta.co.at/webi/schema.html#1");
        }

        private void XuSchema_Click(object sender, EventArgs e)
        {
            XuStop_Click(sender, e);
            //XuWeBrowser.Url = new Uri("https://cmi.ta.co.at/webi/CMI004096/schema.html#1");
            //XuWeBrowser.Url = new Uri("https://cmi004096.cmi.ta.co.at/webi/schema.html#1");
            XuWeBrowser.Url = new Uri("https://cmi004096.cmi.ta.co.at/webi/schematic_files/1.cgi?_=1671478826623");

            //XuWeBrowser.Refresh();
            //XuWeBrowser.Navigate(XuWeBrowser.Url);
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

                    //BoosterLog.Info(msg);

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

            //if (0 <= index && index <= 200 && BypassChanges-- <= 0)
            if (0 <= index && index <= 200)
            {
                NewIndex = (CurrentIndex != index) ? (int?)index : null;

                XuStop_Click(null, null);

                ////while (Measurements.Count > 0)
                ////{
                ////    Measurements.RemoveAt(0);
                ////}

                // https://cmi004096.cmi.ta.co.at/webi/INCLUDE/change.cgi?changeadr=01B51020C7&changeto=122&_=1671478826772
                // https://cmi004096.cmi.ta.co.at/webi/INCLUDE/change.cgi?changeadr=01B51020C7&changeto=80&_=1671478826789

                //var uri = String.Format(
                //    "https://cmi.ta.co.at/webi/CMI004096/change.cgi?changeadr=01B51020C7&changeto={0}"
                //    , index
                //);
                var uri = String.Format(
                    "https://cmi004096.cmi.ta.co.at/webi/INCLUDE/change.cgi?changeadr=01B51020C7&changeto={0}"
                    , index
                );
                XuWeBrowser.Url = new Uri(uri);
                //XuWeBrowser.Navigate(XuWeBrowser.Url);
                //XuWeBrowser.Refresh();

                MustRefresh = true;

                BypassChanges = 1;

                EnableAnalyze = false;
            }
        }

        private int MapSpeedToIdex(float value)
        {
            // 0,0  ->   0
            // 10,0 -> 200

            var index = 200 / 10.0f * value;

            return (int)index;
        }


        private int Increase(float pdif)
        {
            int amplification;
            amplification = int.TryParse(XuAmplificationUp.Text, out amplification) ? amplification : 100;

            var dif = (int)(-pdif * amplification) + 1;

            var newValue = Math.Min(200, CurrentIndex + dif);

            ChangeSpeed(newValue);

            XuLabelSpeedInfo.Text = String.Format("+Speed {0:F} -> {1:F}", CurrentIndex / 20.0f, newValue / 20.0f);
            XuLabelSpeedInfo.ForeColor = Color.Red;

            XuLogWindow.Text = $"\r\n{DateTime.Now:mm:ss} +Speed {CurrentIndex / 20.0f:F} -> {newValue / 20.0f:F}{XuLogWindow.Text}";

            return newValue;
        }

        private int Decrease(float pdif)
        {
            int amplification;
            amplification = int.TryParse(XuAmplificationDown.Text, out amplification) ? amplification : 100;

            var dif = (int)(-pdif * amplification) - 1;
            var newValue = Math.Max(20, CurrentIndex + dif);

            ChangeSpeed(newValue);

            XuLabelSpeedInfo.Text = String.Format("-Speed {0:F} -> {1:F}", CurrentIndex / 20.0f, newValue / 20.0f);
            XuLabelSpeedInfo.ForeColor = Color.Green;

            XuLogWindow.Text = $"\r\n{DateTime.Now:mm:ss} -Speed {CurrentIndex / 20.0f:F} -> {newValue / 20.0f:F}{XuLogWindow.Text}";

            return newValue;
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


        DateTime TimestampLastWrite { get; set; }


        private void Log(ExtractData value)
        {
            var fp = new CultureInfo("da");
            var now = DateTime.UtcNow;

            float offsett;

            //BoosterLog.InfoFormat(
            //    fp
            //    , "{0};{1,6:F};{19,6:F};{2,6:F};{3,6:F1};{4,6:F};{5,6:F};{6,6:F1};{7,6:F1};{8,6:F1};{9,6:F1};{10,6};{11,6:F2};{12,6:F2};{13,6:F2};{14,6:F2};{15,6:F2};{16,3};{17,3};{18}"
            //    , value.TimeStamp
            //    , value.BoosterSpeedIn
            //    , (value.PressureOut - value.PressureIn) * 10
            //    , value.CrossFlow / 10f
            //    , value.PressureIn
            //    , value.PressureOut
            //    , value.TempBrineIn
            //    , value.TempBrineOut
            //    , value.TempAirIndoor
            //    , value.TempAirOutdoor
            //    , value.CrossFlow
            //    , Math.Round(value.BoosterSpeedIn - Measurements[0].BoosterSpeedIn, 2)
            //    , value.Offset * 10
            //    , value.AmplificationUp / 100
            //    , value.AmplificationDown / 100
            //    , value.PressureOut - value.PressureIn - (float.TryParse(XuOffset.Text, out offsett) ? offsett : 0.07f) * 10
            //    , CurrentIndex
            //    , NewIndex
            //    , value.IsManualMode ? "MAN" : "AUTO"
            //    , value.BoosterSpeedOut
            //);
            TimestampLastWrite = now;
        }

        private void Analyze()
        {
            var fp = new CultureInfo("da");

            try
            {
                var iDoc = (mshtml.IHTMLDocument2)XuWeBrowser.Document.DomDocument;
                if (iDoc != null)
                {
                    var t1 = iDoc;
                    var t2 = iDoc.body;
                    var t3 = t2.outerHTML;


                    var ed = new ExtractData();
                    ed.OuterHtmlSource = t3;
                    if (ed.TryParse())
                    {
                        if (ed.TryAnalyzeXml())
                        {
                            if (ed.IsValid())
                            {
                                XuLogWindow.Text = $".{XuLogWindow.Text}";

                                ed.Offset = float.Parse(XuOffset.Text, CultureInfo.CurrentUICulture);
                                ed.AmplificationDown = float.Parse(XuAmplificationDown.Text, CultureInfo.CurrentUICulture);
                                ed.AmplificationUp = float.Parse(XuAmplificationUp.Text, CultureInfo.CurrentUICulture);

                                Measurements.Add(ed);

                                if (ed.PressureOut < 0.1f)
                                {
                                    // System low pressure
                                    if (ed.BoosterSpeedIn < 0.1) { return; }
                                    ChangeSpeed(0); // Full STOP
                                    //BoosterLog.DebugFormat(fp, "{0} FULL STOP, NO PRESSURE", ed.TimeStamp);
                                    Log(ed);
                                    return;
                                }

                                if (ed.PressureIn < 0.001f && ed.BoosterSpeedIn > 5.0f)
                                {
                                    // Overspeed
                                    ChangeSpeed(100); // ½ speed
                                    ed.BoosterSpeedOut = 5.0f;
                                    //BoosterLog.DebugFormat(fp, "{0} Overspeed, set to ½ speed", ed.TimeStamp);
                                    Log(ed);
                                    return;
                                }

                                if (Measurements.Count >= 2)
                                {
                                    //if (!(Measurements[0].Equals(Measurements[1])))
                                    {
                                        // Measurement changed

                                        var current = Measurements[1];


                                        // Date changed.
                                        if (current.TimeStamp < new TimeSpan(0, 0, 30))
                                        {
                                            //BoosterLog.Info("LocalTime;Speed;PDiff*10;CrossFl/10;PIn;POut;TBIn;TBOut;TAIns;TAOuts;CrossFlow;Change;Offset;AmplUp;AmplDwn;ControlValue;Current;New;Mode");
                                        }


                                        float offsett;
                                        var correction = float.TryParse(XuOffset.Text, out offsett) ? offsett : 0.07f;

                                        var pdif = current.PressureOut - current.PressureIn - correction;

                                        float threshold;
                                        threshold = float.TryParse(XuThreshold.Text, out threshold) ? threshold : 0.011f;

                                        var lowerThreshold = -threshold;
                                        var upperThreshold = threshold;

                                        CurrentIndex = MapSpeedToIdex(current.BoosterSpeedIn);

                                        if (ed.IsManualMode)
                                        {
                                            if (pdif < lowerThreshold)
                                            {
                                                // increase spead

                                                XuLogWindow.Text = $"\r\n{DateTime.Now:mm:ss} +Pdif: {pdif} = Pout: {current.PressureOut} - Pin:{current.PressureIn} - Correction: {correction} {XuLogWindow.Text}";

                                                //Log(current);
                                                current.BoosterSpeedOut = Increase(pdif) / 20.0f;
                                                //Log(current);
                                            }
                                            else if (pdif > upperThreshold)
                                            {
                                                // decrease speed
                                                XuLogWindow.Text = $"\r\n{DateTime.Now:mm:ss} -Pdif: {pdif} = Pout: {current.PressureOut} - Pin:{current.PressureIn} - Correction: {correction} {XuLogWindow.Text}";
                                                //Log(current);
                                                current.BoosterSpeedOut = Decrease(pdif) / 20.0f;
                                                //Log(current);
                                            }
                                            else
                                            {
                                                // keep speed.
                                                //Keep();
                                                current.BoosterSpeedOut = Decrease(0) / 20.0f;
                                            }
                                        }

                                        var now = DateTime.UtcNow;
                                        if (now.Subtract(TimestampLastWrite).TotalSeconds > 20)
                                        {
                                            Log(current);
                                        }

                                    }
                                    //else
                                    //{
                                    //    XuLabelSpeedInfo.Text = "";
                                    //}

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
                                //Logger.WarnFormat("Measuremen outside limits: {0}", ed);
                                //Logger.DebugFormat("HTML: {0}", iDoc.body.outerHTML);
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
                        //Logger.WarnFormat("Unable to parse Xml: \r\n{0}\r\n", iDoc.body.outerHTML);

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
                //Logger.Error("", ex);
            }
        }


        private void XuAnalyzeDom_Click(object sender, EventArgs e)
        {
            Analyze();
        }


        public bool EnableAnalyze { get; set; }

        private void XuTimer_Tick(object sender, EventArgs e)
        {
            XuSchema_Click(sender, e);
            //XuAnalyzeDom_Click(sender, e);
            EnableAnalyze = true;
            if (Restart)
            {
                //Logger.Error("\r\n\r\nRestarting\r\n\r\n");
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


        private void Window_Error(object sender, HtmlElementErrorEventArgs e)
        {
            // Ignore the error and suppress the error dialog box. 
            e.Handled = true;
        }

        private void XuWeBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            ((WebBrowser)sender).Document.Window.Error += new HtmlElementErrorEventHandler(Window_Error);

            var t1 = sender.GetType().Name;

            var browser = sender as WebBrowser;

            var ds = browser.DocumentStream;

            ds.Seek(0L, System.IO.SeekOrigin.Begin);

            var sr = new StreamReader(ds);

            var data = sr.ReadToEnd().TrimStart();

            XuLogWindow.Text = $"\r\n{DateTime.Now:mm:ss} {data.Substring(0, Math.Min(5, data.Length)).Replace("\r\n", "")} {XuLogWindow.Text}";
            XuLogWindow.Text = XuLogWindow.Text.Substring(0, Math.Min(XuLogWindow.Text.Length, 500));

            if (data.StartsWith("<div id=\"pos0\">") && EnableAnalyze)
            {
                Analyze();
                return;
            }

            if (data.StartsWith("<div class=\"alert alert-danger\">The interface"))
            {
                Thread.Sleep(5000);
                XuSchema_Click(sender, e);
            }

            //if (data.StartsWith("OK"))
            //{
            //    XuStop_Click(sender, e);
            //    //Thread.Sleep(10000);
            //}

            var tx = data;
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
