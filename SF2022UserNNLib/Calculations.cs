using System;
using System.Collections.Generic;

namespace SF2022UserNNLib
{
    public class Calculations
    {
        public static string[] AvailablePeriods(
            TimeSpan[] startTimes,
            int[] durations,
            TimeSpan beginWorkingTime,
            TimeSpan endWorkingTime,
            int consultationTime)
        {
            try
            {
                List<string> availablePeriods = new List<string>();
                TimeSpan currentTime = beginWorkingTime;

                if (startTimes.Length != durations.Length)
                {
                    throw new InvalidOperationException(
                        "Количество консультаций не равно количеству продолжительностей консультаций");
                }


                TimeSpan timeWork = endWorkingTime - beginWorkingTime;
                for (int i = 0; i < durations.Length; i++)
                {
                    if (durations[i] > timeWork.TotalMinutes)
                    {
                        throw new InvalidOperationException(
                            "Продолжительность консультации превышает рабочее время");
                    }
                }
                
                
                for (int i = 0; i < startTimes.Length; i++)
                {
                    
                    while (currentTime < startTimes[i])
                    {
                        TimeSpan endTime = currentTime.Add(TimeSpan.FromMinutes(consultationTime));
                        if (endTime <= endWorkingTime)
                        {
                            availablePeriods.Add($"{currentTime:hh\\:mm}-{endTime:hh\\:mm}");
                        }
                        currentTime = currentTime.Add(TimeSpan.FromMinutes(consultationTime));
                    }
                    currentTime = startTimes[i].Add(TimeSpan.FromMinutes(durations[i]));
                }
            
                while (currentTime < endWorkingTime)
                {
                    TimeSpan endTime = currentTime.Add(TimeSpan.FromMinutes(consultationTime));
                    if (endTime <= endWorkingTime)
                    {
                        availablePeriods.Add($"{currentTime:hh\\:mm}-{endTime:hh\\:mm}");
                    }
                    currentTime = currentTime.Add(TimeSpan.FromMinutes(consultationTime));
                }
                return availablePeriods.ToArray();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Eroor {ex}");
                return [];
            }
        }
    }
}
