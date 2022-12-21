using System;
using System.Diagnostics;
using System.Globalization;
using System.Xml;
using EU.Iamia.Logging;

namespace JordvarmeController.Businesslayer
{
    public class ExtractData : IEquatable<ExtractData>
    {
        //public static readonly ILog Logger = LogManager.GetLogger();

        public String OuterHtmlSource { get; set; }

        private XmlDocument Document { get; set; }

        public bool TryParse()
        {
            return true;
        }

        /// <summary>
        /// Convert OuterHtml to XmlDocument.
        /// A return value indcates the succes of the operation.
        /// </summary>
        /// <returns></returns>
        public bool TryParsex()
        {

            // Convert OuterhHtml to an Xml document.
            var t1 = $"<?xml version=\"1.0\" encoding=\"utf-8\" ?> {OuterHtmlSource}";

            // Fix known defects in document.

            // Non terminated newlind
            var t2 = t1.Replace("<br>", "<br />").Replace("<BR>", "<BR />");

            // Non terminated img
            var t3 = t2.Replace("png\">", "png\" />");

            // Non terminated imput
            var t4 = t3.Replace("</select>", "</select></input>");

            var t5 = t4.Replace("nbsp;", ""); // HTML not supported by XML

            var t6 = t5.Replace("\"();", "();\"");

            try
            {
                Document = new XmlDocument();
                Document.LoadXml(t6); // XmlException

                // try reading first dataset 
                if (Document.DocumentElement != null)
                {
                    const string xPath = "//div[@id='pos10']/a";
                    var t9 = Document.DocumentElement.SelectNodes(xPath);
                    if (t9 == null
                        || t9.Count < 1)
                    {
                        throw new ArgumentException(xPath);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                var msg = $"OuterHtmlSource: '{OuterHtmlSource}'";
                //Logger.Error(msg, ex);
                return false;
            }
        }

        public float BoosterSpeedIn { get; set; }


        public float BoosterSpeedOut { get; set; }


        public int CrossFlow { get; set; }


        public bool IsManualMode { get; set; }


        public float PressureIn { get; set; }


        public float PressureOut { get; set; }


        public float TempAirIndoor { get; set; }


        public float TempAirOutdoor { get; set; }


        public float TempBrineIn { get; set; }


        public float TempBrineOut { get; set; }


        public TimeSpan TimeStamp { get; set; }

        public float Offset { get; set; }

        public float AmplificationUp { get; set; }

        public float AmplificationDown { get; set; }

        private int IgnoreError { get; set; }

        private string Extract(string lookFor, string until)
        {
            var foundPosition = OuterHtmlSource.IndexOf(lookFor, StringComparison.OrdinalIgnoreCase);
            if (foundPosition < 0) return null;

            var startPosition = foundPosition + lookFor.Length;
            var endPosition = OuterHtmlSource.IndexOf(until, startPosition, StringComparison.InvariantCultureIgnoreCase);

            var length = endPosition - startPosition;

            var result =  OuterHtmlSource.Substring(startPosition, length);
            return result;
        }

        public bool TryAnalyzeXml()
        {
            var provider = CultureInfo.GetCultureInfo("da-DK");

            this.BoosterSpeedIn = float.Parse(Extract("<DIV id=pos10><A href=\"javascript:loadChanger('11020C50180');\">MAN</A><BR><A href=\"javascript:loadChanger('01020C701');\">", "V </A></DIV>"),provider);
            this.CrossFlow= int.Parse(Extract("<DIV id=pos5>~ ", " l/h </DIV>"));
            this.PressureIn = float.Parse(Extract("<DIV id=pos8>", " bar </DIV>"), provider);
            this.PressureOut = float.Parse(Extract("<DIV id=pos9>", " bar </DIV>"), provider);
            this.TempAirIndoor = float.Parse(Extract("<DIV id=pos2>Inde:", "°C </DIV>"), provider);
            this.TempAirOutdoor = float.Parse(Extract("<DIV id=pos4>Ude:", "°C </DIV>"), provider);
            this.TempBrineIn = float.Parse(Extract("<DIV id=pos7>", "°C </DIV>"), provider);
            this.TempBrineOut = float.Parse(Extract("<DIV id=pos6>", "°C </DIV>"), provider);
            this.IsManualMode = true;

            return true;
        }

        public bool TryAnalyzeXmlX()
        {
            Debug.Assert(Document != null, "Document != null");
            Debug.Assert(Document.DocumentElement != null, "Document.DocumentElement!=null");

            // If document is temporary unavailable then only log the error once.
            if (Document.DocumentElement.SelectSingleNode("//div") == null)
            {
                if (IgnoreError-- <= 0)
                {
                    //Logger.DebugFormat("Unable to parse document {0}", Document.DocumentElement.InnerXml);
                    IgnoreError = 14; // Number of retry withn one GUI refresh cycle (30 sec / 2sec) -1.
                }
                return false;
            }

            IgnoreError = 0;

            try
            {
                float f1;

                // Booster speed
                {
                    // <div id="pos10"><a href="javascript:loadChanger('11020C50180');">AUTO<br /> 3,90 V  </a>  </div>
                    const string xPath = "//div[@id='pos10']/a";
                    var t2 = Document.DocumentElement.SelectNodes(xPath);
                    if (t2 == null) { throw new ArgumentException(xPath); }

                    if (t2.Count == 1)
                    {
                        // Auto mode
                        var t3 = t2[0].LastChild.Value.Substring(0).Replace("V", "");
                        BoosterSpeedIn = float.TryParse(t3, out f1) ? f1 : -1f;
                        IsManualMode = false;
                        BoosterSpeedOut = BoosterSpeedIn;
                    }
                    else
                    {
                        // Manual mode
                        var t3 = t2[1].InnerXml.Substring(0).Replace("V", "");
                        BoosterSpeedIn = float.TryParse(t3, out f1) ? f1 : -1f;
                        IsManualMode = true;
                        BoosterSpeedOut = BoosterSpeedIn;
                    }
                }

                // Cross flow
                {
                    // <div id="pos5">~    0 l/h  </div>
                    const string xPath = "//div[@id='pos5']";
                    var t2 = Document.DocumentElement.SelectSingleNode(xPath);
                    if (t2 == null) { throw new ArgumentException(xPath); }

                    var t3 = t2.InnerXml.Substring(4).Replace("l/h", "");
                    CrossFlow = (int)(float.TryParse(t3, out f1) ? f1 : -1f);
                }

                // Pressure In
                {
                    // <div id="pos8"> 1,14 bar  </div>
                    // <div id="pos8">-99,99 bar  </div>
                    const string xPath = "//div[@id='pos8']";
                    var t2 = Document.DocumentElement.SelectSingleNode(xPath);
                    if (t2 == null) { throw new ArgumentException(xPath); }

                    var t3 = t2.InnerXml.Substring(1).Replace("bar", "");
                    PressureIn = float.TryParse(t3, out f1) ? f1 : -1f;
                }

                // Pressure Out
                {
                    // <div id="pos9"> 1,26 bar  </div>
                    const string xPath = "//div[@id='pos9']";
                    var t2 = Document.DocumentElement.SelectSingleNode(xPath);
                    if (t2 == null) { throw new ArgumentException(xPath); }

                    var t3 = t2.InnerXml.Substring(1).Replace("bar", "");
                    PressureOut = float.TryParse(t3, out f1) ? f1 : -1f;
                }

                // TempAirIndoor
                {
                    // <div id="pos2">Inde:   9,5  °C  </div>
                    const string xPath = "//div[@id='pos2']";
                    var t2 = Document.DocumentElement.SelectSingleNode(xPath);
                    if (t2 == null) { throw new ArgumentException(xPath); }

                    var t3 = t2.InnerXml.Substring(7).Replace("°C", "").Replace("- ", "-");
                    TempAirIndoor = float.TryParse(t3, out f1) ? f1 : -1f;
                }

                // TempAirOutdoor
                {
                    // <div id="pos4">Ude:   7,6  °C  </div>
                    const string xPath = "//div[@id='pos4']";
                    var t2 = Document.DocumentElement.SelectSingleNode(xPath);
                    if (t2 == null) { throw new ArgumentException(xPath); }

                    var t3 = t2.InnerXml.Substring(7).Replace("°C", "").Replace("- ", "-");
                    TempAirOutdoor = float.TryParse(t3, out f1) ? f1 : -1f;
                }

                // TempBineIn
                {
                    // <div id="pos7">  6,8  °C  </div>
                    const string xPath = "//div[@id='pos7']";
                    var t2 = Document.DocumentElement.SelectSingleNode(xPath);
                    if (t2 == null) { throw new ArgumentException(xPath); }

                    var t3 = t2.InnerXml.Substring(2).Replace("°C", "").Replace("- ", "-"); ;
                    TempBrineIn = float.TryParse(t3, out f1) ? f1 : -1f;
                }

                // TempBrineOut
                {
                    // <div id="pos6">  9,1  °C  </div>
                    const string xPath = "//div[@id='pos6']";
                    var t2 = Document.DocumentElement.SelectSingleNode(xPath);
                    if (t2 == null) { throw new ArgumentException(xPath); }

                    var t3 = t2.InnerXml.Substring(2).Replace("°C", "").Replace("- ", "-"); ;
                    TempBrineOut = float.TryParse(t3, out f1) ? f1 : -1f;
                }

                // Timestamp
                {
                    // <div id="pos0">21:44:48  </div>
                    const string xPath = "//div[@id='pos0']";
                    var t2 = Document.DocumentElement.SelectSingleNode(xPath);
                    if (t2 == null) { throw new ArgumentException(xPath); }

                    var t3 = t2.InnerXml;
                    DateTime d1;
                    TimeStamp = (DateTime.TryParse(t3, out d1) ? d1 : DateTime.Now).TimeOfDay;
                }

                return true;
            }
            catch (Exception ex)
            {
                //Logger.Error("", ex);
                //Logger.WarnFormat("XML: \r\n{0}\r\n", Document.DocumentElement.InnerXml);
                return false;
            }
        }

        private static readonly TimeSpan MinTimeValue = new TimeSpan(0, 0, 1);
        private static readonly TimeSpan MaxTimeValue = new TimeSpan(23, 59, 59);


        /// <summary>
        /// Returns true if all measure values lies within acceptable range.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return
                BoosterSpeedIn.IsValid(-0.01f, 10.01f)
                && CrossFlow.IsValid(0, 200)
                && PressureIn.IsValid(-100.00f, 2.1f)
                && PressureOut.IsValid(0.00f, 2.1f)
            //&& TempAirIndoor.IsValid(-30f, 40f)
            //&& TempAirOutdoor.IsValid(-30f, 40f)
            //&& TempBrineIn.IsValid(-20f, 25f)
            //&& TempBrineOut.IsValid(-20f, 25f)
            //&& TimeStamp.IsValid(MinTimeValue, MaxTimeValue)
            ;
        }


        public bool Equals(ExtractData other)
        {
            return
                other.BoosterSpeedIn.IsEqualTo(BoosterSpeedIn, 0.001f)
                 && other.CrossFlow == CrossFlow
                 && other.PressureIn.IsEqualTo(PressureIn, 0.001f)
                 && other.PressureOut.IsEqualTo(PressureOut, 0.001f)
                 //&& other.TempAirIndoor.IsEqualTo(TempAirIndoor, 0.001f)
                 //&& other.TempAirOutdoor.IsEqualTo(TempAirOutdoor, 0.001f)
                 //&& other.TempBrineIn.IsEqualTo(TempBrineIn, 0.001f)
                 //&& other.TempBrineOut.IsEqualTo(TempBrineOut, 0.001f)
                 && other.TimeStamp.Equals(TimeStamp)
            ;
        }

        public override string ToString()
        {
            return String.Format(
                "Speed:{0:F}, {9:F}, CrossFlow: {1}, Pressue in: {2:F}, -out: {3:F}, TempAir indoor: {4:F1}, -outdoor: {5:F1}, TempBrine in: {6:F1}, - out: {7:F1}, Time: {8:c}"
                , BoosterSpeedIn
                , CrossFlow
                , PressureIn
                , PressureOut
                , TempAirIndoor
                , TempAirOutdoor
                , TempBrineIn
                , TempBrineOut
                , TimeStamp
                , BoosterSpeedOut
                );
        }
    }
}
