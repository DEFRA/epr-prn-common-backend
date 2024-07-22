using EPR.PRN.Backend.Obligation.Services;
using Microsoft.AspNetCore.Mvc;
using ObligationCalculationDemoUI.Models;
using System.Diagnostics;

namespace ObligationCalculationDemoUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;      

        private static List<RecyclingTarget> _entries = new List<RecyclingTarget>
            {
                new RecyclingTarget { Year = 2025, Material = ObligationCalculationMaterial.Paper, Number = 0.75 },
                new RecyclingTarget { Year = 2025, Material = ObligationCalculationMaterial.Glass, Number = 0.74, GlassRemelt = 0.75},
                new RecyclingTarget { Year = 2025, Material = ObligationCalculationMaterial.Aluminium, Number = 0.61 },
                new RecyclingTarget { Year = 2025, Material = ObligationCalculationMaterial.Steel, Number = 0.8 },
                new RecyclingTarget { Year = 2025, Material = ObligationCalculationMaterial.Plastic, Number = 0.55 },
                new RecyclingTarget { Year = 2025, Material = ObligationCalculationMaterial.Wood, Number = 0.45 },

                new RecyclingTarget { Year = 2026, Material = ObligationCalculationMaterial.Paper, Number = 0.77 },
                new RecyclingTarget { Year = 2026, Material = ObligationCalculationMaterial.Glass, Number = 0.76, GlassRemelt = 0.76 },
                new RecyclingTarget { Year = 2026, Material = ObligationCalculationMaterial.Aluminium, Number = 0.62 },
                new RecyclingTarget { Year = 2026, Material = ObligationCalculationMaterial.Steel, Number = 0.81 },
                new RecyclingTarget { Year = 2026, Material = ObligationCalculationMaterial.Plastic, Number = 0.57 },
                new RecyclingTarget { Year = 2026, Material = ObligationCalculationMaterial.Wood, Number = 0.46 },

                new RecyclingTarget { Year = 2027, Material = ObligationCalculationMaterial.Paper, Number = 0.79 },
                new RecyclingTarget { Year = 2027, Material = ObligationCalculationMaterial.Glass, Number = 0.78, GlassRemelt = 0.77 },
                new RecyclingTarget { Year = 2027, Material = ObligationCalculationMaterial.Aluminium, Number = 0.63 },
                new RecyclingTarget { Year = 2027, Material = ObligationCalculationMaterial.Steel, Number = 0.82 },
                new RecyclingTarget { Year = 2027, Material = ObligationCalculationMaterial.Plastic, Number = 0.59 },
                new RecyclingTarget { Year = 2027, Material = ObligationCalculationMaterial.Wood, Number = 0.47 },

                new RecyclingTarget { Year = 2028, Material = ObligationCalculationMaterial.Paper, Number = 0.81 },
                new RecyclingTarget { Year = 2028, Material = ObligationCalculationMaterial.Glass, Number = 0.8, GlassRemelt = 0.78 },
                new RecyclingTarget { Year = 2028, Material = ObligationCalculationMaterial.Aluminium, Number = 0.64 },
                new RecyclingTarget { Year = 2028, Material = ObligationCalculationMaterial.Steel, Number = 0.83 },
                new RecyclingTarget { Year = 2028, Material = ObligationCalculationMaterial.Plastic, Number = 0.61 },
                new RecyclingTarget { Year = 2028, Material = ObligationCalculationMaterial.Wood, Number = 0.48 },

                new RecyclingTarget { Year = 2029, Material = ObligationCalculationMaterial.Paper, Number = 0.83 },
                new RecyclingTarget { Year = 2029, Material = ObligationCalculationMaterial.Glass, Number = 0.82, GlassRemelt = 0.79 },
                new RecyclingTarget { Year = 2029, Material = ObligationCalculationMaterial.Aluminium, Number = 0.65 },
                new RecyclingTarget { Year = 2029, Material = ObligationCalculationMaterial.Steel, Number = 0.84 },
                new RecyclingTarget { Year = 2029, Material = ObligationCalculationMaterial.Plastic, Number = 0.63 },
                new RecyclingTarget { Year = 2029, Material = ObligationCalculationMaterial.Wood, Number = 0.49 },
                
                new RecyclingTarget { Year = 2030, Material = ObligationCalculationMaterial.Paper, Number = 0.85 },
                new RecyclingTarget { Year = 2030, Material = ObligationCalculationMaterial.Glass, Number = 0.85, GlassRemelt = 0.80 },
                new RecyclingTarget { Year = 2030, Material = ObligationCalculationMaterial.Aluminium, Number = 0.67 },
                new RecyclingTarget { Year = 2030, Material = ObligationCalculationMaterial.Steel, Number = 0.85 },
                new RecyclingTarget { Year = 2030, Material = ObligationCalculationMaterial.Plastic, Number = 0.65 },
                new RecyclingTarget { Year = 2030, Material = ObligationCalculationMaterial.Wood, Number = 0.5 },
            };

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var model = new ObligationCalculation();           

            return View(model);
        }

        [HttpPost]
        public IActionResult Calculate(ObligationCalculation model)
        {
            var target = _entries.FirstOrDefault(x => x.Material == model.Material && x.Year == model.SelectedYear);
            var service = new ObligationCalculatorService();

            if (model.Material == ObligationCalculationMaterial.Glass)
            {
                var (remelt, remainder) = service.CalculateGlass((double)target?.Number, (double)target?.GlassRemelt, model.Tonnage);
                model.GlassRemelt = remelt;
                model.GlassRemainder = remainder;
            }
            else
            {
                model.CalculatedObligation = new ObligationCalculatorService().Calculate((double)target?.Number, model.Tonnage);
            }

            return View("index",model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
