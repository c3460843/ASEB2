using Data_Analysis_Software_Part_1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Analysis_Software_Part_2
{
    public class TestClass
    {
        /// <summary>
        /// Test data.
        /// </summary>
        public string[] testArray = { "Version=106", "SMode=111111100", "Date=20120801", "StartTime=18:01:28.0", "Interval=1", "[HRData]", "95	171	50	147	208	4662", "91	183	56	147	216	5173", "92	196	61	147	215	5428", "94	203	65	147	207	5940", "95	215	69	147	201	5941" };

        /// <summary>
        /// Reads test data as normal data would be read and inserts it into (test) HRMFile.
        /// </summary>
        /// <param name="hRMFile"></param>
        public void Read(HRMFile hRMFile)
        {
            string line = null;
            decimal runningTotalDistance = 0;
            bool data = false;
            int counter = 0;

            while (counter < 11)
            {
                line = testArray[counter];

                if (line.Contains("Version"))
                {
                    String[] lineArray = line.Split('=');
                    hRMFile.version = Int32.Parse(lineArray[1]);
                }

                if (line.Contains("SMode"))
                {
                    String[] lineArray = line.Split('=');
                    char[] sModeChars = lineArray[1].ToCharArray();
                    int i = 0;
                    if (hRMFile.version >= 106)
                    {
                        if (sModeChars[i] == '0') { hRMFile.sModeSpeed = false; i++; } else if (sModeChars[i] == '1') { hRMFile.sModeSpeed = true; i++; }
                        if (sModeChars[i] == '0') { hRMFile.sModeCadence = false; i++; } else if (sModeChars[i] == '1') { hRMFile.sModeCadence = true; i++; }
                        if (sModeChars[i] == '0') { hRMFile.sModeAltitude = false; i++; } else if (sModeChars[i] == '1') { hRMFile.sModeAltitude = true; i++; }
                        if (sModeChars[i] == '0') { hRMFile.sModePower = false; i++; } else if (sModeChars[i] == '1') { hRMFile.sModePower = true; i++; }
                        if (sModeChars[i] == '0') { hRMFile.sModePowerLeftRightBalance = false; i++; } else if (sModeChars[i] == '1') { hRMFile.sModePowerLeftRightBalance = true; i++; }
                        if (sModeChars[i] == '0') { hRMFile.sModePowerPedallingIndex = false; i++; } else if (sModeChars[i] == '1') { hRMFile.sModePowerPedallingIndex = true; i++; }
                        if (sModeChars[i] == '0') { hRMFile.sModeHRCC = false; i++; } else if (sModeChars[i] == '1') { hRMFile.sModeHRCC = true; i++; }
                        if (sModeChars[i] == '0') { hRMFile.sModeUnitStandard = false; i++; } else if (sModeChars[i] == '1') { hRMFile.sModeUnitStandard = true; i++; }
                    }
                    if (hRMFile.version >= 107)
                    {
                        if (sModeChars[i] == '0') { hRMFile.sModeAirPressure = false; i++; } else if (sModeChars[i] == '1') { hRMFile.sModeAirPressure = true; i++; }
                    }
                }

                if (line.Contains("Date"))
                {
                    String[] lineArray = line.Split('=');
                    DateTime d = DateTime.ParseExact(lineArray[1], "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    hRMFile.startDate = d;
                }


                if (line.Contains("Interval"))
                {
                    String[] lineArray = line.Split('=');
                    hRMFile.interval = Int32.Parse(lineArray[1]);
                }

                if (line.Contains("HRData"))
                {
                    data = true;
                }

                if (data == true)
                {
                    TimeSpan timeSpan = TimeSpan.FromSeconds(hRMFile.timeSecs);
                    if (!line.Contains("HRData"))
                    {
                        hRMFile.timeSecs += hRMFile.interval;
                        string timeString = timeSpan.ToString(@"hh\:mm\:ss");
                        String[] lineArray = line.Split();
                        hRMFile.timeList.Add(timeString);

                        hRMFile.heartRateList.Add(Int32.Parse(lineArray[0]));


                        if (hRMFile.sModeSpeed == true)
                        {
                            hRMFile.speedList.Add(Int32.Parse(lineArray[1]));

                            hRMFile.intervalDistanceList.Add(Decimal.Parse(lineArray[1]) / 3600);

                            runningTotalDistance += (Decimal.Parse(lineArray[1]) / 3600);

                            hRMFile.cummalativeDistanceList.Add(runningTotalDistance * 100);


                        }
                        if (hRMFile.sModeCadence == true) { hRMFile.cadenceList.Add(Int32.Parse(lineArray[2])); }
                        if (hRMFile.sModeAltitude == true) { hRMFile.altitudeList.Add(Int32.Parse(lineArray[3])); }
                        if (hRMFile.sModePower == true) { hRMFile.powerList.Add(Int32.Parse(lineArray[4])); }

                        try
                        {
                            if (hRMFile.sModePowerLeftRightBalance == true && hRMFile.sModePowerPedallingIndex == true)
                            { hRMFile.powerBalancePedallingIndexList.Add(Int32.Parse(lineArray[5])); }

                            if (hRMFile.sModePowerLeftRightBalance == true) { hRMFile.powerLeftRightBalanceList.Add(1); }
                            if (hRMFile.sModePowerPedallingIndex == true) { hRMFile.powerPedallingIndexList.Add(1); }
                        }
                        catch (IndexOutOfRangeException)
                        {
                        }
                    }
                }
                counter++;
            }
        }
    }    
}
