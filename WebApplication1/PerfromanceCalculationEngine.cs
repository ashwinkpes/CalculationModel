using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static WebApplication1.Utility;

namespace WebApplication1
{
    public class CalculationEngine
    {
        private readonly AbbRelCareContext _abbRelCareContext;
        private readonly ILogger<CalculationEngine> _logger;

        public CalculationEngine(AbbRelCareContext abbRelCareContext, ILogger<CalculationEngine> logger)
        {
            _abbRelCareContext = abbRelCareContext;
            _logger = logger;
        }

        public async Task PerformConditionCalculation(Telemetry telemetryData)
        {
          await GetCharcteristicHealthScoreQuality(telemetryData);           
        }

        public async Task GetCharcteristicHealthScoreQuality(Telemetry telemetryData)
        {
          var charcteristicFromDb =  await _abbRelCareContext.Characteristics.SingleOrDefaultAsync(s => s.CharacteristicId == telemetryData.CharacteristicId).ConfigureAwait(false);
          if (charcteristicFromDb == null)
                _logger.LogError($"No charcteristic with CharacteristicId {telemetryData.CharacteristicId} and Assetid {telemetryData.AssetId} is found in system");
            else
                if (charcteristicFromDb.UsedInCalculation == 1)
                await PerformHsqCalculations(telemetryData, charcteristicFromDb).ConfigureAwait(false);          
        }

        public async Task PerformHsqCalculations(Telemetry telemetryData, Characteristic charcteristicFromDb)
        {
            var result = await GetCharcteristicHealthScoreQuality(telemetryData, charcteristicFromDb).ConfigureAwait(false);
            if (result.performAssetCalculation)
            {
                charcteristicFromDb.HealthScoreQuality = result.healthScoreQuality;
                charcteristicFromDb.HealthScore = await GetCharcteristicHealthScore(telemetryData, charcteristicFromDb);
                _abbRelCareContext.Entry(charcteristicFromDb).State = EntityState.Modified;
                await _abbRelCareContext.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<(int? healthScoreQuality, bool performAssetCalculation)> GetCharcteristicHealthScoreQuality(Telemetry telemetryData, Characteristic characteristic)
        {
            int? healthScoreQuality = null;
            bool performAssetCalculation = true;
            bool sanityCheckFailed = false;

            if (!telemetryData.Value.HasValue)
            {
                if (characteristic.IsMandatory)
                {
                  var charcFromDb =  await _abbRelCareContext.Characteristics.Include(s => s.SubSystem).ThenInclude(y => y.Asset).SingleOrDefaultAsync(s => s.CharacteristicId == characteristic.CharacteristicId).ConfigureAwait(false);
                  charcFromDb.SubSystem.Asset.HealthScoreQuality = 100;
                  _abbRelCareContext.Entry(charcFromDb.SubSystem.Asset).State = EntityState.Modified;
                  await _abbRelCareContext.SaveChangesAsync().ConfigureAwait(false);
                    performAssetCalculation = false;
                }
                else
                {
                    healthScoreQuality = 100;
                    sanityCheckFailed = true;
                }
            }


            if (!sanityCheckFailed && telemetryData.Value < characteristic.Min)
            {
                _logger.LogWarning($"charcteristic with id {telemetryData.CharacteristicId} and subsytem id  {characteristic.SubSystemId} has value less than Min value {characteristic.Min} , timestamp {telemetryData.DataRecordedOn}");
                sanityCheckFailed = true;
                performAssetCalculation = false;
            }

            if (!sanityCheckFailed && telemetryData.Value > characteristic.Max)
            {
                _logger.LogWarning($"charcteristic with id {telemetryData.CharacteristicId} and subsytem id  {characteristic.SubSystemId} has value greater than Max value {characteristic.Max} , timestamp {telemetryData.DataRecordedOn}");
                sanityCheckFailed = true;
                performAssetCalculation = false;
            }

            if (!sanityCheckFailed)
            {
                switch(characteristic.DataAgeScale)
                {
                    case 1:
                        DateTime zeroTime = new DateTime(1, 1, 1);
                        TimeSpan timeSpan = DateTime.Now.ToUniversalTime().Subtract(telemetryData.DataRecordedOn.ToUniversalTime());
                        int years = (zeroTime + timeSpan).Year - 1;

                        if (years < (characteristic.DataAgeScale * characteristic.DataAgeMax))
                        healthScoreQuality = 0;
                        else
                            healthScoreQuality = 100;
                        break;

                    case 2:
                        double months = DateTime.UtcNow.Subtract(telemetryData.DataRecordedOn.ToUniversalTime()).Days / (365.25 / 12);
                        if(!(months < (characteristic.DataAgeScale * characteristic.DataAgeMax)))
                            healthScoreQuality = 100;
                        else
                            healthScoreQuality = 0;
                        break;

                    case 3:
                        if (!(DateTime.UtcNow.Subtract(telemetryData.DataRecordedOn.ToUniversalTime()).TotalDays < (characteristic.DataAgeScale * characteristic.DataAgeMax)))
                            healthScoreQuality = 100;
                        else
                            healthScoreQuality = 0;
                        break;

                    case 4:
                        if (!(DateTime.UtcNow.Subtract(telemetryData.DataRecordedOn.ToUniversalTime()).TotalHours < (characteristic.DataAgeScale * characteristic.DataAgeMax)))
                            healthScoreQuality = 100;
                        else
                            healthScoreQuality = 0;
                        break;

                    case 5:
                        if (!(DateTime.UtcNow.Subtract(telemetryData.DataRecordedOn.ToUniversalTime()).TotalMinutes < (characteristic.DataAgeScale * characteristic.DataAgeMax)))
                            healthScoreQuality = 100;
                        else
                            healthScoreQuality = 0;
                        break;

                    default: break;
                }
            }

            return (healthScoreQuality, performAssetCalculation);
        }

        public Task<int?> GetCharcteristicHealthScore(Telemetry telemetryData, Characteristic characteristic)
        {
            int? healthScore = null;
            Task<int?> taskHealthScore = default(Task<int?>);

            var calculationResult = new CalculationResult
            {
                Value = telemetryData.Value.Value,
                TextValue = telemetryData.TextValue,
                Value1 = characteristic.Value1,
                Value2 = characteristic.Value2,
                Value3 = characteristic.Value3,
                Value4 = characteristic.Value4,
                RelativeValue = characteristic.RelativeBaseValue,
            };

            switch (characteristic.CalculationType)
            {
                case CalculationType.Type1:
                    taskHealthScore = Type1Calculation(telemetryData, calculationResult);                    
                    break;
                case CalculationType.Type2:
                    taskHealthScore = Type2Calculation(telemetryData, calculationResult);
                    break;
                case CalculationType.Type3:
                    taskHealthScore = Type3Calculation(telemetryData, calculationResult);
                    break;
                case CalculationType.Type4:
                    taskHealthScore = Type4Calculation(telemetryData, calculationResult);
                    break;
                case CalculationType.Type5:
                    taskHealthScore = Type5Calculation(telemetryData, calculationResult);
                    break;
                case CalculationType.Type6:
                    taskHealthScore = Type6Calculation(telemetryData, calculationResult);
                    break;
                case CalculationType.Type7:
                    taskHealthScore = Type7Calculation(telemetryData, calculationResult);
                    break;
                case CalculationType.Type8:
                    //taskHealthScore = Type8Calculation(telemetryData, calculationResult);
                    break;
                case CalculationType.Type9:
                   // taskHealthScore = Type9Calculation(telemetryData, calculationResult);
                    break;
                case CalculationType.Type10:
                    taskHealthScore = Type10Calculation(telemetryData, calculationResult);
                    break;
                case CalculationType.Type11:
                    taskHealthScore = Type11Calculation(telemetryData, calculationResult);
                    break;
                case CalculationType.Type12:
                    taskHealthScore = Type12Calculation(telemetryData, calculationResult);
                    break;
                case CalculationType.Type13:
                    taskHealthScore = Type13Calculation(telemetryData, calculationResult);
                    break;
            }
            
            healthScore = taskHealthScore.GetAwaiter().GetResult();
            return Task.FromResult<int?>(healthScore);
        }

        public Task<int?> Type1Calculation(Telemetry telemetryData, CalculationResult characteristic)
        {
            int? healthScore = null;
            double value = telemetryData.Value.Value;

            if (value >= characteristic.Min && value <= characteristic.Value1)
            {
                healthScore = 0;
            }else if (value > characteristic.Value1 && value <= characteristic.Value2)
            {
                healthScore = 25;
            }
            else if (value > characteristic.Value2 && value <= characteristic.Value3)
            {
                healthScore = 50;
            }
            else if (value > characteristic.Value3 && value <= characteristic.Value4)
            {
                healthScore = 75;
            }
            else if (value > characteristic.Value4 && value <= characteristic.Max)
            {
                healthScore = 100;
            }

            return Task.FromResult<int?>(healthScore);
        }

        public Task<int?> Type2Calculation(Telemetry telemetryData, CalculationResult characteristic)
        {
            int? healthScore = null;
            double value = telemetryData.Value.Value;

            if (value >= characteristic.Min && value <= characteristic.Value1)
            {
                healthScore = 100;
            }
            else if (value > characteristic.Value1 && value <= characteristic.Value2)
            {
                healthScore = 75;
            }
            else if (value > characteristic.Value2 && value <= characteristic.Value3)
            {
                healthScore = 50;
            }
            else if (value > characteristic.Value3 && value <= characteristic.Value4)
            {
                healthScore = 25;
            }
            else if (value > characteristic.Value4 && value <= characteristic.Max)
            {
                healthScore = 0;
            }

            return Task.FromResult<int?>(healthScore);
        }

        public Task<int?> Type3Calculation(Telemetry telemetryData, CalculationResult characteristic)
        {
            int? healthScore = null;
            double value = telemetryData.Value.Value;

            characteristic.Value2 = characteristic.Value2 * characteristic.RelativeValue;
            characteristic.Value3 = characteristic.Value3 * characteristic.RelativeValue;
            characteristic.Value4 = characteristic.Value4 * characteristic.RelativeValue;

            if (value >= characteristic.Min && value <= characteristic.Value1)
            {
                healthScore = 0;
            }
            else if (value > characteristic.Value1 && value <= characteristic.Value2)
            {
                healthScore = 25;
            }
            else if (value > characteristic.Value2 && value <= characteristic.Value3)
            {
                healthScore = 50;
            }
            else if (value > characteristic.Value3 && value <= characteristic.Value4)
            {
                healthScore = 75;
            }
            else if (value > characteristic.Value4 && value <= characteristic.Max)
            {
                healthScore = 100;
            }

            return Task.FromResult<int?>(healthScore);
        }

        public Task<int?> Type4Calculation(Telemetry telemetryData, CalculationResult characteristic)
        {
            int? healthScore = null;
            double value = telemetryData.Value.Value;

            characteristic.Value2 = characteristic.Value2 * characteristic.RelativeValue;
            characteristic.Value3 = characteristic.Value3 * characteristic.RelativeValue;
            characteristic.Value4 = characteristic.Value4 * characteristic.RelativeValue;

            if (value >= characteristic.Min && value <= characteristic.Value1)
            {
                healthScore = 100;
            }
            else if (value > characteristic.Value1 && value <= characteristic.Value2)
            {
                healthScore = 75;
            }
            else if (value > characteristic.Value2 && value <= characteristic.Value3)
            {
                healthScore = 50;
            }
            else if (value > characteristic.Value3 && value <= characteristic.Value4)
            {
                healthScore = 25;
            }
            else if (value > characteristic.Value4 && value <= characteristic.Max)
            {
                healthScore = 0;
            }

            return Task.FromResult<int?>(healthScore);
        }

        public Task<int?> Type5Calculation(Telemetry telemetryData, CalculationResult characteristic)
        {
            int? healthScore = null;
            string value = telemetryData.TextValue;

            if (!string.IsNullOrEmpty(value))
            {
                if (value == "Very good" || value == "None")
                {
                    healthScore = 0;
                }
                else if (value == "Good" || value == "Light")
                {
                    healthScore = 25;
                }
                else if (value == "Fair" || value == "Moderate")
                {
                    healthScore = 50;
                }
                else if (value == "Poor" || value == "High")
                {
                    healthScore = 75;
                }
                else if (value == "Very poor" || value == "Very high")
                {
                    healthScore = 100;
                }
            }

            return Task.FromResult<int?>(healthScore);
        }

        public Task<int?> Type6Calculation(Telemetry telemetryData, CalculationResult characteristic)
        {
            int? healthScore = null;
            string value = telemetryData.TextValue;

            if (!string.IsNullOrEmpty(value))
            {
                if (value == "True" || value == "Yes" || value == "OK")
                {
                    healthScore = 100;
                }
                else if (value == "False" || value == "No" || value == "NOK")
                {
                    healthScore = 0;
                }
            }

            return Task.FromResult<int?>(healthScore);
        }

        public Task<int?> Type7Calculation(Telemetry telemetryData, CalculationResult characteristic)
        {
            int? healthScore = null;
            string value = telemetryData.TextValue;

            if (!string.IsNullOrEmpty(value))
            {
                if (value == "True" || value == "Yes" || value == "OK")
                {
                    healthScore = 0;
                }
                else if (value == "False" || value == "No" || value == "NOK")
                {
                    healthScore = 100;
                }
            }

            return Task.FromResult<int?>(healthScore);
        }

        public Task<int?> Type10Calculation(Telemetry telemetryData, CalculationResult characteristic)
        {
            int? healthScore = null;
            double value = telemetryData.Value.Value;

            if (value >= characteristic.Min && value <= characteristic.Value1)
            {
                healthScore = 100;
            }
            else if (value > characteristic.Value1 && value <= characteristic.Value4)
            {
                healthScore = 0;
            }      
            else if (value > characteristic.Value4 && value <= characteristic.Max)
            {
                healthScore = 100;
            }

            return Task.FromResult<int?>(healthScore);
        }

        public Task<int?> Type11Calculation(Telemetry telemetryData, CalculationResult characteristic)
        {
            int? healthScore = null;
            double value = telemetryData.Value.Value;

            if (value >= characteristic.Min && value <= characteristic.Value1)
            {
                healthScore = 0;
            }
            else if (value > characteristic.Value1 && value <= characteristic.Value4)
            {
                healthScore = 100;
            }
            else if (value > characteristic.Value4 && value <= characteristic.Max)
            {
                healthScore = 0;
            }

            return Task.FromResult<int?>(healthScore);
        }

        public Task<int?> Type12Calculation(Telemetry telemetryData, CalculationResult characteristic)
        {
            int? healthScore = null;
            double value = telemetryData.Value.Value;

            if (value >= characteristic.Min && value <= characteristic.Value1)
            {
                healthScore = 0;
            }
            else if (value > characteristic.Value1 && value <= characteristic.Max)
            {
                healthScore = 100;
            }      

            return Task.FromResult<int?>(healthScore);
        }

        public Task<int?> Type13Calculation(Telemetry telemetryData, CalculationResult characteristic)
        {
            int? healthScore = null;
            double value = telemetryData.Value.Value;

            if (value >= characteristic.Min && value <= characteristic.Value1)
            {
                healthScore = 100;
            }
            else if (value > characteristic.Value1 && value <= characteristic.Max)
            {
                healthScore = 0;
            }

            return Task.FromResult<int?>(healthScore);
        }

        public async Task<double?> CalculateSubSystemHealthScore(Guid subSystemId)
        {            
            double? subsystemHealthScore = null;
            var allCharcteristics = await _abbRelCareContext.Characteristics.Where(s => s.SubSystemId == subSystemId && s.UsedInCalculation == 1).ToListAsync().ConfigureAwait(false);
            allCharcteristics = allCharcteristics.OrderBy(c => c.HealthScore).ThenBy(n => n.Weight).ToList();
            var lstCharcteristicsDto = allCharcteristics.Select(r => new CharcteristicDto
                                       {
                                         CharacteristicId = r.CharacteristicId,
                                         HealthScore = r.HealthScore.Value,
                                         Weight = r.Weight
                                       }).ToList();

            var counter = 1;
            foreach (var x in lstCharcteristicsDto)
            {
                x.OrderNo = counter++;
            }
            foreach (var x in lstCharcteristicsDto)
            {
                x.NewWeight = x.Weight * (int)(Math.Pow(x.OrderNo, 2));
            }
            foreach (var x in lstCharcteristicsDto)
            {
                x.WeightedHealthScore = x.HealthScore * x.NewWeight;
            }
          
            var TotalWeightedHealthScore = lstCharcteristicsDto.Sum(item => item.WeightedHealthScore);
            var TotalNewWeight = lstCharcteristicsDto.Sum(item => item.NewWeight);


            subsystemHealthScore = TotalWeightedHealthScore/ TotalNewWeight;

            return subsystemHealthScore;
        }
      
    }
}
