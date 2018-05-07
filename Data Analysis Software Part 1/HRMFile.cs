using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Analysis_Software_Part_1
{
    /// <summary>
    /// Class stores all values output from Form1 class for corresponding HRMFile thats loaded.
    /// </summary>
    public class HRMFile
    {
        /// <summary>
        /// The start date from the file.
        /// </summary>
        public DateTime startDate;

        /// <summary>
        /// The total amount of seconds the HRMData has data stored for.
        /// </summary>
        public int timeSecs = 0;

        /// <summary>
        /// The interval between rows of HRMData (seconds).
        /// </summary>
        public int interval;

        /// <summary>
        /// A total count for all the speed values for each row of HRMData.
        /// </summary>
        public double speedTotal = 0;

        /// <summary>
        /// The highest speed value from the data.
        /// </summary>
        public double speedMaximum = 0;

        /// <summary>
        /// A total count for all the altitude values for each row of HRMData.
        /// </summary>
        public double altitudeTotal = 0;

        /// <summary>
        /// The highest altitude value from the data.
        /// </summary>
        public double altitudeMaximum = 0;

        /// <summary>
        /// A total count for all the heart rate values for each row of HRMData.
        /// </summary>
        public int heartRateTotal = 0;

        /// <summary>
        /// The highest heart rate value from the data.
        /// </summary>
        public int heartRateMaximum = 0;

        /// <summary>
        /// The lowest heart rate value from the data.
        /// </summary>
        public int heartRateMinimum = 0;

        /// <summary>
        /// A total count for all the power values for each row of HRMData.
        /// </summary>
        public int powerTotal = 0;

        /// <summary>
        /// The highest power value from the data.
        /// </summary>
        public int powerMaximum = 0;

        /// <summary>
        /// A total count for all the distance values calculated from speeds and times.
        /// </summary>
        public decimal distanceTotal = 0;

        /// <summary>
        /// A flag to show when the distance calculation has been done.
        /// </summary>
        public bool distanceCalculationFlag = false;

        /// <summary>
        /// A total count for all the speed values for each row of HRMData from a spefic sector.
        /// </summary>
        public double speedTotalRange = 0;

        /// <summary>
        /// The highest speed value from the data from a spefic sector.
        /// </summary>
        public double speedMaximumRange = 0;

        /// <summary>
        /// A total count for all the altitude values for each row of HRMData from a spefic sector.
        /// </summary>
        public double altitudeTotalRange = 0;

        /// <summary>
        /// The highest altitude value from the data from a spefic sector.
        /// </summary>
        public double altitudeMaximumRange = 0;

        /// <summary>
        /// A total count for all the heart rate values for each row of HRMData from a spefic sector.
        /// </summary>
        public int heartRateTotalRange = 0;

        /// <summary>
        /// The highest heart rate value from the data from a spefic sector.
        /// </summary>
        public int heartRateMaximumRange = 0;

        /// <summary>
        /// The lowest heart rate value from the data from a spefic sector.
        /// </summary>
        public int heartRateMinimumRange = 0;

        /// <summary>
        /// A total count for all the power values for each row of HRMData from a spefic sector.
        /// </summary>
        public int powerTotalRange = 0;

        /// <summary>
        /// The highest power value from the data from a spefic sector.
        /// </summary>
        public int powerMaximumRange = 0;

        /// <summary>
        /// A total count for all the distance values calculated from speeds and times from a spefic sector.
        /// </summary>
        public decimal distanceTotalRange = 0;

        /// <summary>
        /// A flag to show when the distance calculation has been done from a spefic sector.
        /// </summary>
        public bool distanceCalculationFlagRange = false;

        /// <summary>
        /// Stores the version of the HRMFile.
        /// </summary>
        public int version;

        /// <summary>
        /// Inditcates whether HRMFile's speed field was enabled.
        /// </summary>
        public bool sModeSpeed;

        /// <summary>
        /// Inditcates whether HRMFile's cadence field was enabled.
        /// </summary>
        public bool sModeCadence;

        /// <summary>
        /// Inditcates whether HRMFile's altitude field was enabled.
        /// </summary>
        public bool sModeAltitude;

        /// <summary>
        /// Inditcates whether HRMFile's power field was enabled.
        /// </summary>
        public bool sModePower;

        /// <summary>
        /// Inditcates whether HRMFile's power left/right balance field was enabled.
        /// </summary>
        public bool sModePowerLeftRightBalance;

        /// <summary>
        /// Inditcates whether HRMFile's speed power pedalling index was enabled.
        /// </summary>
        public bool sModePowerPedallingIndex;

        /// <summary>
        /// Inditcates whether HRMFile's HRCC field was enabled.
        /// </summary>
        public bool sModeHRCC;

        /// <summary>
        /// Inditcates whether HRMFile's unit standard field was metric or imperial.
        /// </summary>
        public bool sModeUnitStandard;

        /// <summary>
        /// Inditcates whether HRMFile's air pressure field was enabled.
        /// </summary>
        public bool sModeAirPressure;

        /// <summary>
        /// Array list that stores the time stamp from each row of HRMdata.
        /// </summary>
        public List<string> timeList = new List<string>();

        /// <summary>
        /// Array list that stores the speed from each row of HRMdata.
        /// </summary>
        public List<double> speedList = new List<double>();

        /// <summary>
        /// Array list that stores the cadence from each row of HRMdata.
        /// </summary>
        public List<int> cadenceList = new List<int>();

        /// <summary>
        /// Array list that stores the altitude from each row of HRMdata.
        /// </summary>
        public List<double> altitudeList = new List<double>();

        /// <summary>
        /// Array list that stores the heart rate from each row of HRMdata.
        /// </summary>
        public List<int> heartRateList = new List<int>();

        /// <summary>
        /// Array list that stores the power from each row of HRMdata.
        /// </summary>
        public List<int> powerList = new List<int>();

        /// <summary>
        /// Array list that stores the normalised power calculated from power list.
        /// </summary>
        public List<double> normalisedPowerList = new List<double>();

        /// <summary>
        /// Array list that stores the balance and pedalling index combined from each row of HRMdata.
        /// </summary>
        public List<int> powerBalancePedallingIndexList = new List<int>();

        /// <summary>
        /// Array list that stores the left right balance from each row of combined balance and pedalling index.
        /// </summary>
        public List<int> powerLeftRightBalanceList = new List<int>();

        /// <summary>
        /// Array list that stores the power pedalling index from each row of combined balance and pedalling index.
        /// </summary>
        public List<int> powerPedallingIndexList = new List<int>();

        /// <summary>
        /// Array list that stores distance done at each interval calculated from the speeds and time.
        /// </summary>
        public List<decimal> intervalDistanceList = new List<decimal>();

        /// <summary>
        /// Array list that stores distance done after each interval calculated by adding the running total of interval distances..
        /// </summary>
        public List<decimal> cummalativeDistanceList = new List<decimal>();

        /// <summary>
        /// Array list that stores heart rate as a percentage of the value set by the user agaisnt the value stored in the heart rate array list.
        /// </summary>
        public List<double> heartRatePercentageList = new List<double>();

        /// <summary>
        /// Array list that stores power as a percentage of the value set by the user agaisnt the value stored in the power array list.
        /// </summary>
        public List<double> powerPercentageList = new List<double>();
    }
}
