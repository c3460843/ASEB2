using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using ZedGraph;


/// <author>
/// c3460843
/// </author>

namespace Data_Analysis_Software_Part_2
{
    public partial class Form1 : Form
    {
        Stream myStream = null;
        DateTime startDate;
        int counter = 0;
        int checkCount = 0;
        int timeSecs = 0;
        int interval;
        int userSetFTP = 0;
        int userSetHeartRateMaximum = 0;
        int tickBoxValue = 0;
        int portionValue = 0;

        double speedTotal = 0;
        double speedMaximum = 0;
        double altitudeTotal = 0;
        double altitudeMaximum = 0;
        int heartRateTotal = 0;
        int heartRateMaximum = 0;
        int heartRateMinimum = 0;
        int powerTotal = 0;
        int powerMaximum = 0;
        decimal distanceTotal = 0;
        bool distanceCalculationFlag = false;

        double speedTotalRange = 0;
        double speedMaximumRange = 0;
        double altitudeTotalRange = 0;
        double altitudeMaximumRange = 0;
        int heartRateTotalRange = 0;
        int heartRateMaximumRange = 0;
        int heartRateMinimumRange = 0;
        int powerTotalRange = 0;
        int powerMaximumRange = 0;
        decimal distanceTotalRange = 0;
        bool distanceCalculationFlagRange = false;

        int version;
        bool sModeSpeed;
        bool sModeCadence;
        bool sModeAltitude;
        bool sModePower;
        bool sModePowerLeftRightBalance;
        bool sModePowerPedallingIndex;
        bool sModeHRCC;
        bool sModeUnitStandard;
        bool sModeAirPressure;

        List<string> timeList = new List<string>();
        List<double> speedList = new List<double>();
        List<int> cadenceList = new List<int>();
        List<double> altitudeList = new List<double>();
        List<int> heartRateList = new List<int>();
        List<int> powerList = new List<int>();
        List<int> powerBalancePedallingIndexList = new List<int>();
        List<int> powerLeftRightBalanceList = new List<int>();
        List<int> powerPedallingIndexList = new List<int>();
        List<decimal> distanceList = new List<decimal>();
        List<double> heartRatePercentageList = new List<double>();
        List<double> powerPercentageList = new List<double>();

        public Form1()
        {
            InitializeComponent();
            checkBoxSPAll.Checked = true;
            buttonAddSP2.Enabled = false;
            buttonAddSP3.Enabled = false;
            buttonAddSP4.Enabled = false;
            buttonRemoveSP1.Enabled = false;
            buttonRemoveSP2.Enabled = false;
            buttonRemoveSP3.Enabled = false;
            buttonRemoveSP4.Enabled = false;
            checkBoxSP1.Enabled = false;
            checkBoxSP2.Enabled = false;
            checkBoxSP3.Enabled = false;
            checkBoxSP4.Enabled = false;
            textBoxSPStartH2.Enabled = false;
            textBoxSPStartH3.Enabled = false;
            textBoxSPStartH4.Enabled = false;
            textBoxSPStartM2.Enabled = false;
            textBoxSPStartM3.Enabled = false;
            textBoxSPStartM4.Enabled = false;
            textBoxSPStartS2.Enabled = false;
            textBoxSPStartS3.Enabled = false;
            textBoxSPStartS4.Enabled = false;
            textBoxSPEndH2.Enabled = false;
            textBoxSPEndH3.Enabled = false;
            textBoxSPEndH4.Enabled = false;
            textBoxSPEndM2.Enabled = false;
            textBoxSPEndM3.Enabled = false;
            textBoxSPEndM4.Enabled = false;
            textBoxSPEndS2.Enabled = false;
            textBoxSPEndS3.Enabled = false;
            textBoxSPEndS4.Enabled = false;
        }

        /// <summary>
        /// Reads the file then splits line by line into lists of data.
        /// </summary>
        public void Read()
        {
            string line;
            bool data = false;
            StreamReader file = new StreamReader(myStream);
            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains("Version"))
                {
                    String[] lineArray = line.Split('=');
                    version = Int32.Parse(lineArray[1]);
                }

                if (line.Contains("SMode"))
                {
                    String[] lineArray = line.Split('=');
                    char[] sModeChars = lineArray[1].ToCharArray();
                    int i = 0;
                    if (version >= 106)
                    {
                        if (sModeChars[i] == '0') { sModeSpeed = false; i++; }                  else if (sModeChars[i] == '1') { sModeSpeed = true; i++; }
                        if (sModeChars[i] == '0') { sModeCadence = false; i++; }                else if (sModeChars[i] == '1') { sModeCadence = true; i++; }
                        if (sModeChars[i] == '0') { sModeAltitude = false; i++; }               else if (sModeChars[i] == '1') { sModeAltitude = true; i++; }
                        if (sModeChars[i] == '0') { sModePower = false; i++; }                  else if (sModeChars[i] == '1') { sModePower = true; i++; }
                        if (sModeChars[i] == '0') { sModePowerLeftRightBalance = false; i++; }  else if (sModeChars[i] == '1') { sModePowerLeftRightBalance = true; i++; }
                        if (sModeChars[i] == '0') { sModePowerPedallingIndex = false; i++; }    else if (sModeChars[i] == '1') { sModePowerPedallingIndex = true; i++; }
                        if (sModeChars[i] == '0') { sModeHRCC = false; i++; }                   else if (sModeChars[i] == '1') { sModeHRCC = true; i++; }
                        if (sModeChars[i] == '0') { sModeUnitStandard = false; i++; }           else if (sModeChars[i] == '1') { sModeUnitStandard = true; i++; }
                    }
                    if (version >= 107)
                    {
                        if (sModeChars[i] == '0') { sModeAirPressure = false; i++; }            else if (sModeChars[8] == '1') { sModeAirPressure = true; i++; }
                    }
                    Console.WriteLine("Version="+version);
                    Console.WriteLine("SMode="+sModeChars[0].ToString() + sModeChars[1].ToString() + sModeChars[2].ToString() +
                                               sModeChars[3].ToString() + sModeChars[4].ToString() + sModeChars[5].ToString() +
                                               sModeChars[6].ToString() + sModeChars[7].ToString());
                }

                if (line.Contains("Date"))
                {
                    String[] lineArray = line.Split('=');
                    DateTime d = DateTime.ParseExact(lineArray[1], "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    startDate = d;
                    dateLabel.Text = d.ToString("dd/MM/yyyy");
                }

                if (line.Contains("StartTime"))
                {
                    String[] lineArray = line.Split('=');
                    startTimeLabel.Text = lineArray[1];
                }

                if (line.Contains("Interval"))
                {
                    String[] lineArray = line.Split('=');
                    intervalLabel.Text = lineArray[1] + "s";
                    interval = Int32.Parse(lineArray[1]);
                }

                if (line.Contains("HRData"))
                {
                    data = true;
                }

                if (data == true)
                {
                    TimeSpan timeSpan = TimeSpan.FromSeconds(timeSecs);
                    if (!line.Contains("HRData"))
                    {
                        timeSecs+=interval;
                        string timeString = timeSpan.ToString(@"hh\:mm\:ss");
                        String[] lineArray = line.Split();
                        timeList.Add(timeString);

                        heartRateList.Add(Int32.Parse(lineArray[0]));

                        if (sModeSpeed == true)
                        {
                            speedList.Add(Int32.Parse(lineArray[1]));
                            distanceList.Add(Decimal.Parse(lineArray[1]) / 3600);
                        }
                        if (sModeCadence == true)               { cadenceList.Add(Int32.Parse(lineArray[2])); }
                        if (sModeAltitude == true)              { altitudeList.Add(Int32.Parse(lineArray[3])); }
                        if (sModePower == true)                 { powerList.Add(Int32.Parse(lineArray[4])); }

                        if (sModePowerLeftRightBalance == true || sModePowerPedallingIndex == true)
                        { powerBalancePedallingIndexList.Add(Int32.Parse(lineArray[5])); }

                        if (sModePowerLeftRightBalance == true) { powerLeftRightBalanceList.Add(1); }
                        if (sModePowerPedallingIndex == true)   { powerPedallingIndexList.Add(1); }
                    }
                }
                counter++;
            }
            file.Close();
            AddRows();
            Calculate();
            heartRateTrackBar.Value = 0;
            powerTrackBar.Value = 0;

        }


        /// <summary>
        /// Adds rows of data from lists to a data grid.       
        /// </summary>
        private void AddRows()
        {
            for (int i = 0; i < timeList.Count; i++)
            {
                string speedValue="";
                string cadenceValue="";
                string altitudeValue="";
                string powerValue="";
                string heartRateValue="";
                if (sModeSpeed==true)    { speedValue = (speedList[i]/10).ToString(); }
                if (sModeCadence==true)  { cadenceValue = cadenceList[i].ToString(); }
                if (sModeAltitude==true) { altitudeValue = altitudeList[i].ToString(); }
                if (sModePower == true)
                {
                    if (powerTrackBar.Value == 0)      { powerValue = powerList[i].ToString();  }
                    else if (powerTrackBar.Value == 1) { powerValue = Math.Round(powerPercentageList[i], 2).ToString();  }
                }
                if (sModeHRCC == true)
                {
                    if (heartRateTrackBar.Value == 0)      { heartRateValue = heartRateList[i].ToString(); }
                    else if (heartRateTrackBar.Value == 1) { heartRateValue = Math.Round(heartRatePercentageList[i], 2).ToString();  }
                }
                dataGridView.Rows.Add(timeList[i], heartRateValue, speedValue, cadenceValue, altitudeValue, powerValue, powerBalancePedallingIndexList[i]);
            }
            Console.WriteLine("AddRows()");
        }

        /// <summary>
        /// Calculates all the averages, minimums and maximums from the lists.
        /// </summary>
        private void Calculate()
        {
            speedTotal = 0;
            speedMaximum = 0;
            altitudeTotal = 0;
            heartRateTotal = 0;
            powerTotal = 0;

            try
            {
                foreach (int heartRateInt in heartRateList) { heartRateTotal += heartRateInt; }
                double heartRateAverage = heartRateTotal / heartRateList.Count;
                heartRateAverageLabel.Text = heartRateAverage.ToString() + "bpm";

                foreach (double speedDouble in speedList) { speedTotal += speedDouble; }
                double speedAverage = speedTotal / speedList.Count;
                speedAverage = Math.Round(speedAverage, 1);
                if (measurementTrackBar.Value == 0) { speedAverageLabel.Text = (speedAverage / 10).ToString() + "km/h"; }
                else if (measurementTrackBar.Value == 1) { speedAverageLabel.Text = (speedAverage / 10).ToString() + "mph"; }

                foreach (double altitudeDouble in altitudeList) { altitudeTotal += altitudeDouble; }
                double altitudeAverage = altitudeTotal / altitudeList.Count;
                altitudeAverage = Math.Round(altitudeAverage, 2);
                if (measurementTrackBar.Value == 0) { altitudeAverageLabel.Text = altitudeAverage.ToString() + "m"; }
                else if (measurementTrackBar.Value == 1) { altitudeAverageLabel.Text = altitudeAverage.ToString() + "ft"; }

                foreach (int powerInt in powerList) { powerTotal += powerInt; }
                double powerAverage = powerTotal / powerList.Count;
                powerAverageLabel.Text = powerAverage.ToString() + "W";

                foreach (double speedInt in speedList) { if (speedInt > speedMaximum) { speedMaximum = speedInt; } }
                if (measurementTrackBar.Value == 0) { speedMaximumLabel.Text = (speedMaximum / 10) + "km/h"; }
                else if (measurementTrackBar.Value == 1) { speedMaximumLabel.Text = (speedMaximum / 10) + "mph"; }

                if (distanceCalculationFlag == false)
                {
                    foreach (decimal distanceDecimal in distanceList) { distanceTotal += distanceDecimal; }
                    distanceTotal = Math.Round(distanceTotal, 1);
                    distanceCalculationFlag = true;
                }
                if (measurementTrackBar.Value == 0) { totalDistanceLabel.Text = (distanceTotal / 10) + "km"; }
                else if (measurementTrackBar.Value == 1) { totalDistanceLabel.Text = (distanceTotal / 10) + "miles"; }

                foreach (int heartRateInt in heartRateList) { if (heartRateInt > heartRateMaximum) { heartRateMaximum = heartRateInt; } }
                heartRateMaximumLabel.Text = heartRateMaximum + "bpm";

                heartRateMinimum = heartRateList[0];
                foreach (int heartRateInt in heartRateList) { if (heartRateInt < heartRateMinimum) { heartRateMinimum = heartRateInt; } }
                heartRateMinimumLabel.Text = heartRateMinimum + "bpm";

                foreach (int powerInt in powerList) { if (powerInt > powerMaximum) { powerMaximum = powerInt; } }
                powerMaximumLabel.Text = powerMaximum + "W";

                foreach (double altitudeInt in altitudeList) { if (altitudeInt > altitudeMaximum) { altitudeMaximum = altitudeInt; } }
                if (measurementTrackBar.Value == 0) { altitudeMaximumLabel.Text = altitudeMaximum + "m"; }
                else if (measurementTrackBar.Value == 1) { altitudeMaximumLabel.Text = altitudeMaximum + "ft"; }
            }
            catch (System.DivideByZeroException)
            {
                //No user dialogue needed.
            }
            Console.WriteLine("Calculate()");
        }


        /// <summary>
        /// Calculates all the averages from a user specified portion.
        /// </summary>
        private void CalculatePortion(int min, int max)
        {

            Console.WriteLine("min = " + min);
            Console.WriteLine("max = " + max);

            speedTotalRange = 0;
            speedMaximumRange = 0;
            altitudeTotalRange = 0;
            heartRateTotalRange = 0;
            powerTotalRange = 0;

            try
            {
                List<int> heartTateListRange = heartRateList.GetRange(min, (max - min) + 1);
                List<double> speedListRange = speedList.GetRange(min, (max - min) + 1);
                List<double> altitudeListRange = altitudeList.GetRange(min, (max - min) + 1);
                List<int> powerListRange = powerList.GetRange(min, (max - min) + 1);
                List<decimal> distanceListRange = distanceList.GetRange(min, (max - min) + 1);

                foreach (int heartRateInt in heartTateListRange)
                {
                    Console.WriteLine(heartRateInt);
                    heartRateTotalRange += heartRateInt;
                }
                double heartRateAverage = heartRateTotalRange / heartTateListRange.Count;
                heartRateAverageRangeLabel.Text = heartRateAverage.ToString() + "bpm";

                foreach (double speedDouble in speedListRange) { speedTotalRange += speedDouble; }
                double speedAverage = speedTotalRange / speedListRange.Count;
                speedAverage = Math.Round(speedAverage, 1);
                if (measurementTrackBar.Value == 0) { speedAverageRangeLabel.Text = (speedAverage / 10).ToString() + "km/h"; }
                else if (measurementTrackBar.Value == 1) { speedAverageRangeLabel.Text = (speedAverage / 10).ToString() + "mph"; }

                foreach (double altitudeDouble in altitudeListRange) { altitudeTotalRange += altitudeDouble; }
                double altitudeAverage = altitudeTotalRange / altitudeListRange.Count;
                altitudeAverage = Math.Round(altitudeAverage, 2);
                if (measurementTrackBar.Value == 0) { altitudeAverageRangeLabel.Text = altitudeAverage.ToString() + "m"; }
                else if (measurementTrackBar.Value == 1) { altitudeAverageRangeLabel.Text = altitudeAverage.ToString() + "ft"; }

                foreach (int powerInt in powerListRange) { powerTotalRange += powerInt; }
                double powerAverage = powerTotalRange / powerListRange.Count;
                powerAverageRangeLabel.Text = powerAverage.ToString() + "W";

                foreach (double speedInt in speedListRange) { if (speedInt > speedMaximumRange) { speedMaximumRange = speedInt; } }
                if (measurementTrackBar.Value == 0) { speedMaximumRangeLabel.Text = (speedMaximumRange / 10) + "km/h"; }
                else if (measurementTrackBar.Value == 1) { speedMaximumRangeLabel.Text = (speedMaximumRange / 10) + "mph"; }

                if (distanceCalculationFlagRange == false)
                {
                    foreach (decimal distanceDecimal in distanceListRange) { distanceTotalRange += distanceDecimal; }
                    distanceTotalRange = Math.Round(distanceTotalRange, 1);
                    distanceCalculationFlagRange = true;
                }
                if (measurementTrackBar.Value == 0) { totalDistanceRangeLabel.Text = (distanceTotalRange / 10) + "km"; }
                else if (measurementTrackBar.Value == 1) { totalDistanceRangeLabel.Text = (distanceTotalRange / 10) + "miles"; }

                foreach (int heartRateInt in heartTateListRange) { if (heartRateInt > heartRateMaximumRange) { heartRateMaximumRange = heartRateInt; } }
                heartRateMaximumRangeLabel.Text = heartRateMaximumRange + "bpm";

                heartRateMinimumRange = heartTateListRange[0];
                foreach (int heartRateInt in heartTateListRange) { if (heartRateInt < heartRateMinimumRange) { heartRateMinimumRange = heartRateInt; } }
                heartRateMinimumRangeLabel.Text = heartRateMinimumRange + "bpm";

                foreach (int powerInt in powerListRange) { if (powerInt > powerMaximumRange) { powerMaximumRange = powerInt; } }
                powerMaximumRangeLabel.Text = powerMaximumRange + "W";

                foreach (double altitudeInt in altitudeListRange) { if (altitudeInt > altitudeMaximumRange) { altitudeMaximumRange = altitudeInt; } }
                if (measurementTrackBar.Value == 0) { altitudeMaximumRangeLabel.Text = altitudeMaximumRange + "m"; }
                else if (measurementTrackBar.Value == 1) { altitudeMaximumRangeLabel.Text = altitudeMaximumRange + "ft"; }
            }
            catch (System.DivideByZeroException)
            {
                //No user dialogue needed.
            }
            speedTotalRange = 0;
            speedMaximumRange = 0;
            altitudeTotalRange = 0;
            altitudeMaximumRange = 0;
            heartRateTotalRange = 0;
            heartRateMaximumRange = 0;
            heartRateMinimumRange = 0;
            powerTotalRange = 0;
            powerMaximumRange = 0;
            distanceTotalRange = 0;
            distanceCalculationFlagRange = false;
            Console.WriteLine("CalculatePortion()");
        }


        /// <summary>
        /// Plots the zedGraph for numerical data.
        /// </summary>
        private void PlotGraph1()
        {
            Console.WriteLine(1);
            
            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl1.GraphPane.GraphObjList.Clear();
            GraphPane myPane1 = zedGraphControl1.GraphPane;
            myPane1.YAxisList.Clear();


            Console.WriteLine(2);
            PointPairList speedPairList = new PointPairList();
            PointPairList cadencePairList = new PointPairList();
            PointPairList altitudePairList = new PointPairList();
            PointPairList heartRatePairList = new PointPairList();
            PointPairList powerPairList = new PointPairList();

            DateTime dtStart = startDate;
            XDate xDateStart = new XDate(dtStart);
            XDate xDateEnd = new XDate(dtStart.AddSeconds(timeSecs));
            myPane1.XAxis.Scale.Min = xDateStart;
            myPane1.XAxis.Scale.Max = xDateEnd;

            myPane1.Title.Text = "";
            myPane1.XAxis.Type = AxisType.Date;
            myPane1.XAxis.Title.Text = "Time (HH:MM:SS)";
            myPane1.XAxis.Scale.Format = "HH:mm:ss";
            myPane1.XAxis.Scale.MajorUnit = DateUnit.Minute;
            myPane1.XAxis.Scale.MinorUnit = DateUnit.Minute;
         

            var y1 = myPane1.AddYAxis("KM/H");
            var y2 = myPane1.AddYAxis("RPM");
            var y3 = myPane1.AddYAxis("M");
            var y4 = myPane1.AddYAxis("BPM");
            var y5 = myPane1.AddYAxis("W");


            Console.WriteLine(3);
            for (int i = 0; i < timeSecs; i++)
            {
                DateTime dtLoop = startDate.AddSeconds(i);
                XDate xDateLoop = new XDate(dtLoop);
                speedPairList.Add(xDateLoop, (speedList[i] / 10));
                cadencePairList.Add(xDateLoop, cadenceList[i]);
                altitudePairList.Add(xDateLoop, altitudeList[i]);
                heartRatePairList.Add(xDateLoop, heartRateList[i]);
                powerPairList.Add(xDateLoop, powerList[i]);
            }

            Console.WriteLine(4);
            LineItem speedCurve = myPane1.AddCurve("Speed (KM/H)",
                   speedPairList, Color.Red, SymbolType.None);

            speedCurve.YAxisIndex = y1;
            myPane1.YAxis.Color = Color.Gray;
            //myPane1.YAxis.Title.FontSpec.Size = 6;

            LineItem cadenceCurve = myPane1.AddCurve("Cadence (RPM)",
                  cadencePairList, Color.Green, SymbolType.None);

            cadenceCurve.YAxisIndex = y2;

            LineItem altitudeCurve = myPane1.AddCurve("Altitude (M)",
                  altitudePairList, Color.Gray, SymbolType.None);

            altitudeCurve.YAxisIndex = y3;

            LineItem heartRateCurve = myPane1.AddCurve("Heart Rate (BPM)",
                  heartRatePairList, Color.Purple, SymbolType.None);

            heartRateCurve.YAxisIndex = y4;

            LineItem powerCurve = myPane1.AddCurve("Power (W)",
                  powerPairList, Color.Orange, SymbolType.None);

            powerCurve.YAxisIndex = y5;

           

            zedGraphControl1.AxisChange();
            zedGraphControl1.Refresh();

            Console.WriteLine("PlotGraph1()");

            //Console.WriteLine(myPane1.XAxis.Min);
            //Console.WriteLine(myPane1.XAxis.Min);
        }


        /// <summary>
        /// Plots the zedGraph for percentage data.
        /// </summary>
        private void PlotGraph2()
        {
            try
            {
                zedGraphControl2.GraphPane.CurveList.Clear();
                zedGraphControl2.GraphPane.GraphObjList.Clear();
                GraphPane myPane2 = zedGraphControl2.GraphPane;

                myPane2.Title.Text = "";
                myPane2.XAxis.Title.Text = "Time (Seconds)";
                myPane2.YAxis.Title.Text = "%";
                myPane2.XAxis.Scale.Max = timeSecs;
                PointPairList heartRatePercentagePairList = new PointPairList();
                PointPairList powerPercentagePairList = new PointPairList();

                for (int i = 0; i < timeSecs; i++)
                {
                    heartRatePercentagePairList.Add(i, heartRatePercentageList[i]);
                    powerPercentagePairList.Add(i, powerPercentageList[i]);
                }

                LineItem heartRatePercentageCurve = myPane2.AddCurve("Heart Rate",
                      heartRatePercentagePairList, Color.Purple, SymbolType.None);

                LineItem powerPercentageCurve = myPane2.AddCurve("Power",
                      powerPercentagePairList, Color.Orange, SymbolType.None);

                zedGraphControl2.AxisChange();
                zedGraphControl2.Refresh();

                Console.WriteLine("PlotGraph2()");
            }
            catch (Exception)
            {

            }
        }

        private void SetSize()
        {
            zedGraphControl1.Location = new Point(0, 0);
            zedGraphControl1.IsShowPointValues = true;
            zedGraphControl1.Size = new Size(this.ClientRectangle.Width - 20, this.ClientRectangle.Height - 50);
            zedGraphControl2.Location = new Point(0, 0);
            zedGraphControl2.IsShowPointValues = true;
            zedGraphControl2.Size = new Size(this.ClientRectangle.Width - 20, this.ClientRectangle.Height - 50);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            SetSize();
        }

        /// <summary>
        /// Listens for File...Open click, and creates open file dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "text files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if(sModeUnitStandard == false)      { measurementTrackBar.Value = 0; }
            else if (sModeUnitStandard == true) { measurementTrackBar.Value = 1; }
            
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        dataGridView.Rows.Clear();
                        Read();
                        PlotGraph1();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Listens for change in Metric/Imperial trackbar value change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void measurementTrackBar_ValueChanged(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();
            if (measurementTrackBar.Value == 0)
            {
                Metric();
            }
            else if(measurementTrackBar.Value == 1)
            {
                Imperial();
            }
            AddRows();
            Thread.Sleep(50);
            Calculate();

            int min=0;
            int max=0;
            string minTimeString;
            string maxTimeString;
            switch (tickBoxValue)
            {
                case 0:
                    min = 0;
                    max = 0;
                    break;
                case 1:
                    minTimeString = textBoxSPStartH1.Text + ":" + textBoxSPStartM1.Text + ":" + textBoxSPStartS1.Text;
                    min = timeList.IndexOf(minTimeString);
                    maxTimeString = textBoxSPEndH1.Text + ":" + textBoxSPEndM1.Text + ":" + textBoxSPEndS1.Text;
                    max = timeList.IndexOf(maxTimeString);
                    break;
                case 2:
                    minTimeString = textBoxSPStartH2.Text + ":" + textBoxSPStartM2.Text + ":" + textBoxSPStartS2.Text;
                    min = timeList.IndexOf(minTimeString);
                    maxTimeString = textBoxSPEndH2.Text + ":" + textBoxSPEndM2.Text + ":" + textBoxSPEndS2.Text;
                    max = timeList.IndexOf(maxTimeString);
                    break;
                case 3:
                    minTimeString = textBoxSPStartH3.Text + ":" + textBoxSPStartM3.Text + ":" + textBoxSPStartS3.Text;
                    min = timeList.IndexOf(minTimeString);
                    maxTimeString = textBoxSPEndH3.Text + ":" + textBoxSPEndM3.Text + ":" + textBoxSPEndS3.Text;
                    max = timeList.IndexOf(maxTimeString);
                    break;
                case 4:
                    minTimeString = textBoxSPStartH4.Text + ":" + textBoxSPStartM4.Text + ":" + textBoxSPStartS4.Text;
                    min = timeList.IndexOf(minTimeString);
                    maxTimeString = textBoxSPEndH4.Text + ":" + textBoxSPEndM4.Text + ":" + textBoxSPEndS4.Text;
                    max = timeList.IndexOf(maxTimeString);
                    break;
            }
            if (min != 0 && max != 0) { CalculatePortion(min, max); }

            PlotGraph1();
            Console.WriteLine("measurementTrackBar_ValueChanged()");
        }

        /// <summary>
        /// Maths for converting metric into imperial.
        /// </summary>
        private void Imperial()
        {
            for (int i = 0; i < speedList.Count; i++)
            {
                speedList[i] = Math.Round(speedList[i] * 0.6214);
            }

            for (int i = 0; i < altitudeList.Count; i++)
            {
                altitudeList[i] = Math.Round(altitudeList[i] * 3.2808);
            }

            distanceTotal = Math.Round(distanceTotal * (Convert.ToDecimal(0.6214)), 1);
            distanceTotalRange = Math.Round(distanceTotalRange * (Convert.ToDecimal(0.6214)), 1);
            altitudeMaximum = Math.Round(altitudeMaximum * 3.2808);
            altitudeMaximumRange = Math.Round(altitudeMaximumRange * 3.2808);
            dataGridView.Columns[2].HeaderText = "Speed (mph)";
            dataGridView.Columns[4].HeaderText = "Altitude (ft)";
        }

        /// <summary>
        /// Maths for converting imperial into metric.
        /// </summary>
        private void Metric()
        {
            for (int i = 0; i < speedList.Count; i++)
            {
                speedList[i] = Math.Round(speedList[i] * 1.6093);
            }

            for (int i = 0; i < altitudeList.Count; i++)
            {
                altitudeList[i] = Math.Round(altitudeList[i] * 0.3048);
            }

            distanceTotal = Math.Round(distanceTotal * (Convert.ToDecimal(1.6093)), 1);
            distanceTotalRange = Math.Round(distanceTotalRange * (Convert.ToDecimal(1.6093)), 1);
            altitudeMaximum = Math.Round(altitudeMaximum * 0.3048);
            altitudeMaximumRange = Math.Round(altitudeMaximumRange * 0.3048);
            dataGridView.Columns[2].HeaderText = "Speed (km/h)";
            dataGridView.Columns[4].HeaderText = "Altitude (m)";
        }

        /// <summary>
        /// Listens for change in numerical/percentage heart rate trackbar value change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeartRateTrackBar_ValueChanged(object sender, EventArgs e)
        {
            if (userSetHeartRateMaximum != 0)
            {
                Console.WriteLine("HeartRateTrackBar_ValueChanged()");
                dataGridView.Rows.Clear();
                AddRows();
            }
            if (heartRateTrackBar.Value == 0)      { dataGridView.Columns[1].HeaderText = "Heart Rate (bpm)"; }
            else if (heartRateTrackBar.Value == 1) { dataGridView.Columns[1].HeaderText = "Heart Rate (%ofMax)"; }
        }
        
        /// <summary>
        /// Listens for change in numerical/percentage power trackbar value change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PowerTrackBar_ValueChanged(object sender, EventArgs e)
        {
            if (userSetFTP != 0)
            {
                Console.WriteLine("PowerTrackBar_ValueChanged()");
                dataGridView.Rows.Clear();
                AddRows();
            }
            if (powerTrackBar.Value == 0) { dataGridView.Columns[5].HeaderText = "Power (Watts)"; }
            else if (powerTrackBar.Value == 1) { dataGridView.Columns[5].HeaderText = "Power (%ofFTP)"; }
        }

        /// <summary>
        /// Button for setting user defined heart rate in beats per minute.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setBPMButton_Click(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();
            try
            {
                userSetHeartRateMaximum = Int32.Parse(heartRateMaxTextBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Enter valid heart rate(bpm).");
                heartRateMaxTextBox.Text = heartRateMaximum.ToString();
            }
            heartRatePercentageList.Clear();
            foreach (double heartRateInt in heartRateList) { heartRatePercentageList.Add((heartRateInt / userSetHeartRateMaximum) * 100); }
            AddRows();
            PlotGraph2();
        }

        /// <summary>
        /// Button for setting user defined power in watts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setFTPButton_Click(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();
            try
            {
                userSetFTP = Int32.Parse(fTPTextBox.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Enter valid functional threshold power(W).");
                fTPTextBox.Text = powerMaximum.ToString();
            }
            powerPercentageList.Clear();
            foreach (double powerInt in powerList) { powerPercentageList.Add((powerInt / userSetFTP) * 100); }
            AddRows();
            PlotGraph2();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by c3460843 using ZedGraph.");
        }

        private void checkBoxSPAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSPAll.Checked == true)
            {
                checkBoxSPAll.Enabled = false;
                if (checkCount >= 1) { checkBoxSP1.Enabled = true; }
                if (checkCount >= 2) { checkBoxSP2.Enabled = true; }
                if (checkCount >= 3) { checkBoxSP3.Enabled = true; }
                if (checkCount >= 4) { checkBoxSP4.Enabled = true; }
                checkBoxSP1.Checked = false;
                checkBoxSP2.Checked = false;
                checkBoxSP3.Checked = false;
                checkBoxSP4.Checked = false;
                tickBoxValue = 0;

                string minTimeString = textBoxSPStartH1.Text + ":" + textBoxSPStartM1.Text + ":" + textBoxSPStartS1.Text;
                string maxTimeString = "";

                if (portionValue == 1) { maxTimeString = textBoxSPEndH1.Text + ":" + textBoxSPEndM1.Text + ":" + textBoxSPEndS1.Text; }
                if (portionValue == 2) { maxTimeString = textBoxSPEndH2.Text + ":" + textBoxSPEndM2.Text + ":" + textBoxSPEndS2.Text; }
                if (portionValue == 3) { maxTimeString = textBoxSPEndH3.Text + ":" + textBoxSPEndM3.Text + ":" + textBoxSPEndS3.Text; }
                if (portionValue == 4) { maxTimeString = textBoxSPEndH4.Text + ":" + textBoxSPEndM4.Text + ":" + textBoxSPEndS4.Text; }

                if (portionValue != 0) { CalculatePortion(timeList.IndexOf(minTimeString), timeList.IndexOf(maxTimeString)); }
            }
        }

        private void checkBoxSP1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSP1.Checked == true)
            {
                checkBoxSPAll.Enabled = true;
                checkBoxSP1.Enabled = false;
                if (checkCount >= 2) { checkBoxSP2.Enabled = true; }
                if (checkCount >= 3) { checkBoxSP3.Enabled = true; }
                if (checkCount >= 4) { checkBoxSP4.Enabled = true; }
                checkBoxSPAll.Checked = false;
                checkBoxSP2.Checked = false;
                checkBoxSP3.Checked = false;
                checkBoxSP4.Checked = false;
                tickBoxValue = 1;

                string minTimeString = textBoxSPStartH1.Text + ":" + textBoxSPStartM1.Text + ":" + textBoxSPStartS1.Text;
                string maxTimeString = textBoxSPEndH1.Text + ":" + textBoxSPEndM1.Text + ":" + textBoxSPEndS1.Text;
                CalculatePortion(timeList.IndexOf(minTimeString), timeList.IndexOf(maxTimeString));

            }
        }

        private void checkBoxSP2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSP2.Checked == true)
            {
                checkBoxSPAll.Enabled = true;
                checkBoxSP1.Enabled = true;
                checkBoxSP2.Enabled = false;
                if (checkCount >= 3) { checkBoxSP3.Enabled = true; }
                if (checkCount >= 4) { checkBoxSP4.Enabled = true; }
                checkBoxSPAll.Checked = false;
                checkBoxSP1.Checked = false;
                checkBoxSP3.Checked = false;
                checkBoxSP4.Checked = false;
                tickBoxValue = 2;

                string minTimeString = textBoxSPStartH2.Text + ":" + textBoxSPStartM2.Text + ":" + textBoxSPStartS2.Text;
                string maxTimeString = textBoxSPEndH2.Text + ":" + textBoxSPEndM2.Text + ":" + textBoxSPEndS2.Text;
                CalculatePortion(timeList.IndexOf(minTimeString), timeList.IndexOf(maxTimeString));
            }
        }

        private void checkBoxSP3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSP3.Checked == true)
            {
                checkBoxSPAll.Enabled = true;
                checkBoxSP1.Enabled = true;
                checkBoxSP2.Enabled = true;
                checkBoxSP3.Enabled = false;
                if (checkCount >= 4) { checkBoxSP4.Enabled = true; }
                checkBoxSPAll.Checked = false;
                checkBoxSP1.Checked = false;
                checkBoxSP2.Checked = false;
                checkBoxSP4.Checked = false;
                tickBoxValue = 3;

                string minTimeString = textBoxSPStartH3.Text + ":" + textBoxSPStartM3.Text + ":" + textBoxSPStartS3.Text;
                string maxTimeString = textBoxSPEndH3.Text + ":" + textBoxSPEndM3.Text + ":" + textBoxSPEndS3.Text;
                CalculatePortion(timeList.IndexOf(minTimeString), timeList.IndexOf(maxTimeString));
            }
        }

        private void checkBoxSP4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSP4.Checked == true)
            {
                checkBoxSPAll.Enabled = true;
                checkBoxSP1.Enabled = true;
                checkBoxSP2.Enabled = true;
                checkBoxSP3.Enabled = true;
                checkBoxSP4.Enabled = false;
                checkBoxSPAll.Checked = false;
                checkBoxSP1.Checked = false;
                checkBoxSP2.Checked = false;
                checkBoxSP3.Checked = false;
                tickBoxValue = 4;

                string minTimeString = textBoxSPStartH4.Text + ":" + textBoxSPStartM4.Text + ":" + textBoxSPStartS4.Text;
                string maxTimeString = textBoxSPEndH4.Text + ":" + textBoxSPEndM4.Text + ":" + textBoxSPEndS4.Text;
                CalculatePortion(timeList.IndexOf(minTimeString), timeList.IndexOf(maxTimeString));
            }
        }

        private void buttonAddSP1_Click(object sender, EventArgs e)
        {
            buttonAddSP1.Enabled = false;
            buttonRemoveSP1.Enabled = true;
            checkBoxSP1.Enabled = true;
            buttonAddSP2.Enabled = true;
            textBoxSPStartH1.Enabled = false;
            textBoxSPStartM1.Enabled = false;
            textBoxSPStartS1.Enabled = false;
            textBoxSPEndH1.Enabled = false;
            textBoxSPEndM1.Enabled = false;
            textBoxSPEndS1.Enabled = false;
            textBoxSPStartH2.Enabled = true;
            textBoxSPStartM2.Enabled = true;
            textBoxSPStartS2.Enabled = true;
            textBoxSPEndH2.Enabled = true;
            textBoxSPEndM2.Enabled = true;
            textBoxSPEndS2.Enabled = true;
            checkCount++;
            portionValue = 1;

            textBoxSPStartHAll.Text = textBoxSPStartH1.Text;
            textBoxSPStartMAll.Text = textBoxSPStartM1.Text;
            textBoxSPStartSAll.Text = textBoxSPStartS1.Text;

            textBoxSPEndHAll.Text = textBoxSPEndH1.Text;
            textBoxSPEndMAll.Text = textBoxSPEndM1.Text;
            textBoxSPEndSAll.Text = textBoxSPEndS1.Text;
        }

        private void buttonAddSP2_Click(object sender, EventArgs e)
        {
            buttonAddSP2.Enabled = false;
            buttonRemoveSP1.Enabled = false;
            buttonRemoveSP2.Enabled = true;
            checkBoxSP2.Enabled = true;
            buttonAddSP3.Enabled = true;
            textBoxSPStartH2.Enabled = false;
            textBoxSPStartM2.Enabled = false;
            textBoxSPStartS2.Enabled = false;
            textBoxSPEndH2.Enabled = false;
            textBoxSPEndM2.Enabled = false;
            textBoxSPEndS2.Enabled = false;
            textBoxSPStartH3.Enabled = true;
            textBoxSPStartM3.Enabled = true;
            textBoxSPStartS3.Enabled = true;
            textBoxSPEndH3.Enabled = true;
            textBoxSPEndM3.Enabled = true;
            textBoxSPEndS3.Enabled = true;
            checkCount++;
            portionValue = 2;

            textBoxSPEndHAll.Text = textBoxSPEndH2.Text;
            textBoxSPEndMAll.Text = textBoxSPEndM2.Text;
            textBoxSPEndSAll.Text = textBoxSPEndS2.Text;
        }

        private void buttonAddSP3_Click(object sender, EventArgs e)
        {
            buttonAddSP3.Enabled = false;
            buttonRemoveSP2.Enabled = false;
            buttonRemoveSP3.Enabled = true;
            checkBoxSP3.Enabled = true;
            buttonAddSP4.Enabled = true;
            textBoxSPStartH3.Enabled = false;
            textBoxSPStartM3.Enabled = false;
            textBoxSPStartS3.Enabled = false;
            textBoxSPEndH3.Enabled = false;
            textBoxSPEndM3.Enabled = false;
            textBoxSPEndS3.Enabled = false;
            textBoxSPStartH4.Enabled = true;
            textBoxSPStartM4.Enabled = true;
            textBoxSPStartS4.Enabled = true;
            textBoxSPEndH4.Enabled = true;
            textBoxSPEndM4.Enabled = true;
            textBoxSPEndS4.Enabled = true;
            checkCount++;
            portionValue = 3;

            textBoxSPEndHAll.Text = textBoxSPEndH3.Text;
            textBoxSPEndMAll.Text = textBoxSPEndM3.Text;
            textBoxSPEndSAll.Text = textBoxSPEndS3.Text;
        }

        private void buttonAddSP4_Click(object sender, EventArgs e)
        {
            buttonAddSP4.Enabled = false;
            buttonRemoveSP3.Enabled = false;
            buttonRemoveSP4.Enabled = true;
            checkBoxSP4.Enabled = true;
            textBoxSPStartH4.Enabled = false;
            textBoxSPStartM4.Enabled = false;
            textBoxSPStartS4.Enabled = false;
            textBoxSPEndH4.Enabled = false;
            textBoxSPEndM4.Enabled = false;
            textBoxSPEndS4.Enabled = false;
            checkCount++;
            portionValue = 4;

            textBoxSPEndHAll.Text = textBoxSPEndH4.Text;
            textBoxSPEndMAll.Text = textBoxSPEndM4.Text;
            textBoxSPEndSAll.Text = textBoxSPEndS4.Text;
        }

        private void buttonRemoveSP1_Click(object sender, EventArgs e)
        {
            buttonAddSP1.Enabled = true;
            buttonRemoveSP1.Enabled = false;
            buttonAddSP2.Enabled = false;
            checkBoxSP1.Enabled = false;
            textBoxSPStartH2.Enabled = false;
            textBoxSPStartM2.Enabled = false;
            textBoxSPStartS2.Enabled = false;
            textBoxSPEndH2.Enabled = false;
            textBoxSPEndM2.Enabled = false;
            textBoxSPEndS2.Enabled = false;
            textBoxSPStartH1.Enabled = true;
            textBoxSPStartM1.Enabled = true;
            textBoxSPStartS1.Enabled = true;
            textBoxSPEndH1.Enabled = true;
            textBoxSPEndM1.Enabled = true;
            textBoxSPEndS1.Enabled = true;
            checkCount--;
            portionValue = 0;

            textBoxSPStartHAll.Text = "00";
            textBoxSPStartMAll.Text = "00";
            textBoxSPStartSAll.Text = "00";

            textBoxSPEndHAll.Text = "00";
            textBoxSPEndMAll.Text = "00";
            textBoxSPEndSAll.Text = "00";
        }

        private void buttonRemoveSP2_Click(object sender, EventArgs e)
        {
            buttonAddSP2.Enabled = true;
            buttonRemoveSP1.Enabled = true;
            buttonRemoveSP2.Enabled = false;
            buttonAddSP3.Enabled = false;
            checkBoxSP2.Enabled = false;
            textBoxSPStartH3.Enabled = false;
            textBoxSPStartM3.Enabled = false;
            textBoxSPStartS3.Enabled = false;
            textBoxSPEndH3.Enabled = false;
            textBoxSPEndM3.Enabled = false;
            textBoxSPEndS3.Enabled = false;
            textBoxSPStartH2.Enabled = true;
            textBoxSPStartM2.Enabled = true;
            textBoxSPStartS2.Enabled = true;
            textBoxSPEndH2.Enabled = true;
            textBoxSPEndM2.Enabled = true;
            textBoxSPEndS2.Enabled = true;
            checkCount--;
            portionValue = 1;

            textBoxSPEndHAll.Text = textBoxSPEndH1.Text;
            textBoxSPEndMAll.Text = textBoxSPEndM1.Text;
            textBoxSPEndSAll.Text = textBoxSPEndS1.Text;
        }

        private void buttonRemoveSP3_Click(object sender, EventArgs e)
        {
            buttonAddSP3.Enabled = true;
            buttonRemoveSP2.Enabled = true;
            buttonRemoveSP3.Enabled = false;
            buttonAddSP4.Enabled = false;
            checkBoxSP3.Enabled = false;
            textBoxSPStartH4.Enabled = false;
            textBoxSPStartM4.Enabled = false;
            textBoxSPStartS4.Enabled = false;
            textBoxSPEndH4.Enabled = false;
            textBoxSPEndM4.Enabled = false;
            textBoxSPEndS4.Enabled = false;
            textBoxSPStartH3.Enabled = true;
            textBoxSPStartM3.Enabled = true;
            textBoxSPStartS3.Enabled = true;
            textBoxSPEndH3.Enabled = true;
            textBoxSPEndM3.Enabled = true;
            textBoxSPEndS3.Enabled = true;
            checkCount--;
            portionValue = 2;

            textBoxSPEndHAll.Text = textBoxSPEndH2.Text;
            textBoxSPEndMAll.Text = textBoxSPEndM2.Text;
            textBoxSPEndSAll.Text = textBoxSPEndS2.Text;
        }

        private void buttonRemoveSP4_Click(object sender, EventArgs e)
        {
            buttonRemoveSP3.Enabled = true;
            buttonRemoveSP4.Enabled = false;
            buttonAddSP4.Enabled = true;
            checkBoxSP4.Enabled = false;
            textBoxSPStartH4.Enabled = true;
            textBoxSPStartM4.Enabled = true;
            textBoxSPStartS4.Enabled = true;
            textBoxSPEndH4.Enabled = true;
            textBoxSPEndM4.Enabled = true;
            textBoxSPEndS4.Enabled = true;
            checkCount--;
            portionValue = 3;
            
            textBoxSPEndHAll.Text = textBoxSPEndH3.Text;
            textBoxSPEndMAll.Text = textBoxSPEndM3.Text;
            textBoxSPEndSAll.Text = textBoxSPEndS3.Text;
        }
    }
}
