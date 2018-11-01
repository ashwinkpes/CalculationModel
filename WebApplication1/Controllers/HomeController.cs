using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using Z.EntityFramework.Plus;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly AbbRelCareContext _abbRelCareContext;
        private readonly CalculationEngine _calculationEngine;

        public HomeController(AbbRelCareContext abbRelCareContext, CalculationEngine calculationEngine)
        {
            _abbRelCareContext = abbRelCareContext;
            _calculationEngine = calculationEngine;
        }

        public async Task<IActionResult> Index()
        {
            //await _calculationEngine.PerformConditionCalculation(new Telemetry
            //{
            //    CharacteristicId = new Guid("64EC13F1-3F0A-4090-821D-FA41C81F86F7"),
            //    AssetId = new Guid("54EC13F1-3F0A-4090-821D-FA41C81F86F7"),
            //    DataRecordedOn = new DateTime(2017, 10, 10, 10, 10, 10),
            //    Value = 0.7,
            //}).ConfigureAwait(false);

            //await _calculationEngine.PerformConditionCalculation(new Telemetry
            //{
            //    CharacteristicId = new Guid("74EC13F1-3F0A-4090-821D-FA41C81F86F7"),
            //    AssetId = new Guid("54EC13F1-3F0A-4090-821D-FA41C81F86F7"),
            //    DataRecordedOn = new DateTime(2018, 10, 10, 10, 10, 10),
            //    Value = 75,
            //}).ConfigureAwait(false);

            //await _calculationEngine.PerformConditionCalculation(new Telemetry
            //{
            //    CharacteristicId = new Guid("84EC13F1-3F0A-4090-821D-FA41C81F86F7"),
            //    AssetId = new Guid("54EC13F1-3F0A-4090-821D-FA41C81F86F7"),
            //    DataRecordedOn = new DateTime(2018, 11, 2, 00, 52, 10),
            //    Value = 30,
            //}).ConfigureAwait(false);

            await _calculationEngine.CalculateSubSystemHealthScore(new Guid("54EC13F1-3F0A-4090-821D-FA41C81F86F8")).ConfigureAwait(false);

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
