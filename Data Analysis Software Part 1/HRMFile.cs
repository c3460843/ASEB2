using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Analysis_Software_Part_1
{
    public class HRMFile
    {
        public DateTime startDate;
        //public int counter = 0;
        public int checkCount = 0;
        public int timeSecs = 0;
        public int interval;
        public int userSetFTP = 0;
        public int userSetHeartRateMaximum = 0;
        public int tickBoxValue = 0;
        public int portionValue = 0;

        public double speedTotal = 0;
        public double speedMaximum = 0;
        public double altitudeTotal = 0;
        public double altitudeMaximum = 0;
        public int heartRateTotal = 0;
        public int heartRateMaximum = 0;
        public int heartRateMinimum = 0;
        public int powerTotal = 0;
        public int powerMaximum = 0;
        public decimal distanceTotal = 0;
        public bool distanceCalculationFlag = false;

        public double speedTotalRange = 0;
        public double speedMaximumRange = 0;
        public double altitudeTotalRange = 0;
        public double altitudeMaximumRange = 0;
        public int heartRateTotalRange = 0;
        public int heartRateMaximumRange = 0;
        public int heartRateMinimumRange = 0;
        public int powerTotalRange = 0;
        public int powerMaximumRange = 0;
        public decimal distanceTotalRange = 0;
        public bool distanceCalculationFlagRange = false;

        public int version;
        public bool sModeSpeed;
        public bool sModeCadence;
        public bool sModeAltitude;
        public bool sModePower;
        public bool sModePowerLeftRightBalance;
        public bool sModePowerPedallingIndex;
        public bool sModeHRCC;
        public bool sModeUnitStandard;
        public bool sModeAirPressure;

        public List<string> timeList = new List<string>();
        public List<double> speedList = new List<double>();
        public List<int> cadenceList = new List<int>();
        public List<double> altitudeList = new List<double>();
        public List<int> heartRateList = new List<int>();
        public List<int> powerList = new List<int>();
        public List<int> powerBalancePedallingIndexList = new List<int>();
        public List<int> powerLeftRightBalanceList = new List<int>();
        public List<int> powerPedallingIndexList = new List<int>();
        public List<decimal> intervalDistanceList = new List<decimal>();
        public List<decimal> cummalativeDistanceList = new List<decimal>();
        public List<double> heartRatePercentageList = new List<double>();
        public List<double> powerPercentageList = new List<double>();
    }
}
