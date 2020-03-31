using System;

namespace Shinobytes.Ravenfall.DataModels
{
    public class ExpGain
    {
        public decimal Amount { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime LastUpdate { get; set; }

        public decimal ExpPerHour
        {
            get
            {
                var elapsed = DateTime.UtcNow - StartTime;
                return Amount / (decimal)elapsed.TotalHours;
            }
        }

        public void AddExperience(decimal amount)
        {
            if (StartTime == DateTime.MinValue)
                StartTime = DateTime.UtcNow;
            Amount += amount;
            LastUpdate = DateTime.UtcNow;
        }
    }
}