using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
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
        private readonly IStringLocalizer<SmartCutModel> _resources;
        const string SFileName = @"optimization.xlsx";
        public SmartCutModel(IHostingEnvironment environment, ILogger<SupportModel> logger, IStringLocalizer<SmartCutModel> resources)
        {
            _hostingEnvironment = environment;
            _logger = logger;
            _resources = resources;
        }

        private readonly List<Clip> _clips = new List<Clip>
        {
            new Clip(83, 2.625),
            new Clip(100, 2.42),
            new Clip(116, 2.677),
            new Clip(120, 2.6),
            new Clip(130, 2.935)
        };

        [BindProperty] public List<Clip> Clips { set; get; }

        [BindProperty]
        [Display(Name = "Clip:")] 
        public int Clip { set; get; }

        [BindProperty] public int PlankReserve { set; get; } = 50;

        [Required(ErrorMessage = "* This field is required.")]
        [BindProperty]
        [Display(Name = "Name:")]
        public string ProjectName { set; get; }

        [BindProperty]
        public List<float> Planks { get; set; } = new List<float>();

        [BindProperty]
        public List<CoronaSnippet> Snippets { get; set; } = new List<CoronaSnippet>{ new CoronaSnippet(7000)};

        [BindProperty]
        public IFormFile ImportExcel { get; set; }

        public void OnGet()
        {
            Planks = new List<float> { 7000 };
            Clips = _clips;
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
                Clip = obj.Clip;
            }

            var cuttingStock = new CuttingStock(Snippets.Cast<ISnippet>().ToList());
            var purePlanks = new List<float>();
            Planks.ForEach(p => purePlanks.Add(p - PlankReserve));
            var planks = cuttingStock.CalculateCuts(purePlanks);
            var free = CuttingStock.GetFree(planks);

            decimal columnSum = 0;
            foreach (var columnValue in Snippets.Select(s => s.Columns))
            {
                if (!string.IsNullOrEmpty(columnValue) && int.TryParse(columnValue, out var column))
                {
                    columnSum += column;
                }
            }
            var column6300Count = Math.Ceiling(columnSum / 6);
            var columnWeight = 1.861;
            var clip = _clips.FirstOrDefault(c => c.Id == Clip);

            var sWebRootFolder = _hostingEnvironment.WebRootPath;
            var file = Path.Combine(sWebRootFolder, "storage", SFileName);
            var ie = new ImportExport();
            var data = ie.Export2(ProjectName, planks, free, columnSum, clip?.Weight?? 0.0, column6300Count, columnWeight, PlankReserve);
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await fileStream.WriteAsync(data, 0, data.Length);
            }

            return new JsonResult(new { Planks = planks, Free = free });
        }

        public async Task<IActionResult> OnGetExportAsync()
        {
            var sWebRootFolder = _hostingEnvironment.WebRootPath;
            
            var file = Path.Combine(sWebRootFolder, "storage", SFileName);

            if (!System.IO.File.Exists(file))
            {
                _logger.LogInformation($"The file {file} does not exists!");
            }

            var fileName = "cut_optimization";
            if (string.IsNullOrEmpty(fileName))
                fileName = SFileName.Replace(".xlsx", "");
            else if (fileName.Length > 20)
                fileName = fileName.Substring(0, 20);

            var memory = new MemoryStream();
            using (var fs = new FileStream(file, FileMode.Open))
            {
                await fs.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{fileName}.xlsx");
        }

        public async Task<IActionResult> OnPostImportAsync()
        {
            var ie = new ImportExport();

            byte[] snippets;
            using (var ms = new MemoryStream())
            {
                await ImportExcel.CopyToAsync(ms);
                snippets = ms.ToArray();
            }

            Snippets.Clear();
            Snippets.AddRange(ie.ImportSnippets(snippets));

            Clips = _clips;

            return Page();
        }
    }
}