using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Vralumglass.Core;
using Vralumglass.Core.Interfaces;
using Vralumglass.Core.Models;
using VralumGlassWeb.Data;

namespace VralumGlassWeb.Pages
{
    [Authorize]
    public class SmartCutModel : PageModel
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger<SupportModel> _logger;
        public SmartCutModel(IHostingEnvironment environment, ILogger<SupportModel> logger)
        {
            _hostingEnvironment = environment;
            _logger = logger;
        }

        [BindProperty] public int PlankReserve { set; get; } = 50;

        [BindProperty] public string ProjectName { set; get; }

        [BindProperty]
        public List<float> Planks { get; set; } = new List<float>();

        [BindProperty]
        public List<CoronaSnippet> Snippets { get; set; } = new List<CoronaSnippet>();

        [BindProperty]
        public IFormFile ImportExcel { get; set; }

        public void OnGet()
        {
            var desiredLengths = new List<(float length, int appartment, int floor)>
            {
                (1870, 4, 1), (5500, 4, 1), (2170, 4, 1), (1450, 5, 1), (360, 5, 1), (10450, 5, 1), (1450, 6, 1), (360, 6, 1), (2170, 7, 1), (4580, 7, 1), (2170, 7, 1),
                (1870, 8, 2), (5500, 8, 2), (2170, 8, 2), (1450, 9, 2), (360, 9, 2), (10450, 9, 2), (1450, 10, 2), (360, 10 ,2), (2170, 11, 2), (4580, 11, 2), (2170, 11, 2),
                (1870, 12, 3), (5500, 12, 3), (2170, 12, 3), (2600, 13, 3), (9020, 13, 3), (2770, 14, 3), (3910, 14, 3), (470, 14, 3), (2170, 15, 3), (4580, 15, 3), (2170, 15, 3),
                (1870, 16, 4), (5500, 16, 4), (2170, 16, 4), (2770, 17, 4), (3950, 17, 4), (2770, 18, 4), (3910, 18, 4), (470, 18, 4), (2170, 19, 4), (4580, 19, 4), (2170, 19, 4),
                (1870, 20, 5), (5500, 20, 5), (2170, 20, 5), (2770, 21, 5), (3950, 21, 5), (2770, 22, 5), (3910, 22, 5), (470, 22, 5), (2170, 23, 5), (4580, 23, 5), (2170, 23, 5),
                (1870, 24, 6), (5500, 24, 6), (2170, 24, 6), (2770, 25, 6), (3950, 25, 6), (2770, 26, 6), (3910, 26, 6), (470, 26, 6), (2170, 27, 6), (4580, 27, 6), (2170, 27, 6),
                (1870, 28, 7), (5500, 28, 7), (2170, 28, 7), (330, 29, 7), (1430, 29, 7), (3750, 29, 7), (330, 30, 7), (1430, 30, 7), (6150, 30, 7), (1600, 31, 7), (1820, 31, 7), (3680, 31, 7), (5440, 31, 7),
                (1870, 32, 8), (5500, 32, 8), (2170, 32, 8), (330, 33, 8), (1430, 33, 8), (3750, 33, 8), (330, 34, 8), (1430, 34, 8), (6150, 34, 8), (2170, 35, 8), (4580, 35, 8), (2170, 35, 8),
                (1870, 36, 9), (10220, 36, 9), (2170, 36, 9), (9600, 37, 9), (1100, 37, 9), (13400, 38, 9), (1100, 38, 9),  (2200, 39, 9), (12220, 39, 9), (2070, 39, 9), (400, 39, 9)
            };

            foreach (var (length, apartment, floor) in desiredLengths)
            {
                Snippets.Add(new CoronaSnippet(length) { Apartment = $"{apartment}", Floor = $"{floor}" });
            }

            Planks = new List<float> { 6000, 6900, 7000 };
        }

        public async Task<JsonResult> OnPostCalculateAsync()
        {
            var stream = new MemoryStream();
            Request.Body.CopyTo(stream);
            stream.Position = 0;
            using (var reader = new StreamReader(stream))
            {
                var requestBody = await reader.ReadToEndAsync();
                if (requestBody.Length <= 0)
                    throw new ArgumentException("no data found in request");

                var obj = JsonConvert.DeserializeObject<SmartCutModel>(requestBody);
                if(obj == null)
                    throw new ArgumentException("no valid data found in request");
                if(!obj.Snippets.Any())
                    throw new ArgumentException("no snippets found in request");
                if(!obj.Planks.Any())
                    throw new ArgumentException("no planks found in request");

                ProjectName = obj.ProjectName;
                Snippets = obj.Snippets;
                Planks = obj.Planks;
            }

            var cuttingStock = new CuttingStock(Snippets.Cast<ISnippet>().ToList());
            var purePlanks = new List<float>();
            Planks.ForEach(p => purePlanks.Add(p - PlankReserve));
            var planks = cuttingStock.CalculateCuts(purePlanks);
            var free = CuttingStock.GetFree(planks);
            var sWebRootFolder = _hostingEnvironment.WebRootPath;
            const string sFileName = @"SmartCutCalculation.xlsx";
            var file = Path.Combine(sWebRootFolder, "storage", sFileName);
            var ie = new ImportExport();
            var data = ie.Export2(ProjectName, planks, free, PlankReserve);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await fileStream.WriteAsync(data, 0, data.Length);
            }

            return new JsonResult(new { Planks = planks, Free = free });
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var ie = new ImportExport();

            using (var ms = new MemoryStream())
            {
                await ImportExcel.CopyToAsync(ms);
                Snippets.Clear();
                Snippets.AddRange(ie.ImportSnippets(ms.ToArray()));
                Planks.Clear();
                Planks.Add(7000);
            }

            return Page();
        }

        public async Task<IActionResult> OnGetExportAsync()
        {
            var sWebRootFolder = _hostingEnvironment.WebRootPath;
            const string sFileName = @"SmartCutCalculation.xlsx";
            var file = Path.Combine(sWebRootFolder, "storage", sFileName);

            if (!System.IO.File.Exists(file))
            {
                _logger.LogInformation($"The file {file} does not exists!");
            }

            var memory = new MemoryStream();
            using (var fs = new FileStream(file, FileMode.Open))
            {
                await fs.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SmartCutCalculation.xlsx");
        }
    }

    public class CoronaSnippet : Snippet
    {
        public CoronaSnippet(float length) : base(length)
        {

        }

        public string Apartment { get; set; }

        public string Floor { get; set; }

        public override string ToString()
        {
            return $"{Length}[{Floor}/{Apartment}]";
        }

        public override ISnippet Clone(float length)
        {
            return new CoronaSnippet(length) { Apartment = Apartment, Floor = Floor };
        }
    }
}