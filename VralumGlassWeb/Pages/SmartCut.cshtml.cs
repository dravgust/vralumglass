using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Vralumglass.Core;
using Vralumglass.Core.Interfaces;
using VralumGlassWeb.Data;

namespace VralumGlassWeb.Pages
{
    [Authorize]
    public class SmartCutModel : PageModel
    {
        [BindProperty]
        public List<float> Planks { get; set; } = new List<float>();

        [BindProperty]
        public List<TestSnippet> Snippets { get; set; } = new List<TestSnippet>();

        [BindProperty]
        public IFormFile ImportExcel { get; set; }

        public void OnGet()
        {
            var desiredLengths = new List<float>
            {
                3057, 6774, 2312, 2282, 7031, 1277, 1252, 6998, 2252, 2337, 6739, 2312, 3097, 6797, 2348, 2275, 7039,
                1292, 3027, 3804, 252, 1277, 6974, 2267, 2332, 6789, 2342, 3092, 6847, 2322, 2272, 7017, 1289, 3022,
                3784, 227, 1272, 6946, 2272, 2347, 6764, 2347, 3113, 6769, 2317, 2287, 6987, 1292, 3050, 3754, 242,
                1257, 6917, 2217, 2357, 6759, 2352, 3112, 6779, 2347, 2312, 6979, 1282, 3052, 3784, 237, 1252, 6921,
                2212, 2372, 6734, 2352, 3117, 6804, 2382, 2297, 6995, 1307, 3047, 3774, 247, 1267, 6926, 2217, 2377,
                6794, 2372, 3132, 6784, 2387, 2272, 6975, 1312, 3062, 3814, 272, 1257, 6899, 2212, 2397, 6804, 2337,
                3152, 6831, 2422, 2262, 7057, 1292, 3059, 3854, 272, 1262, 6940, 2202, 2407, 6769, 2352, 3147, 6839,
                2417, 2302, 7059, 1262, 3067, 3854, 267, 1272, 6964, 2207, 2412, 6749, 2342, 3152, 6856, 2387, 2322,
                7052, 1282, 3044, 3829, 272, 1272, 6986, 2222, 2407, 6769, 2342,
            };

            var counter = 0;
            for (var i = 1; i <= 50; i++)
            {
                if (i == 3) continue;

                Snippets.Add(new TestSnippet(desiredLengths[counter]) { Apartment = i });
                Snippets.Add(new TestSnippet(desiredLengths[counter + 1]) { Apartment = i });
                Snippets.Add(new TestSnippet(desiredLengths[counter + 2]) { Apartment = i });

                counter += 3;
            }

            Planks = new List<float> { 7050, 4950 };
        }

        public async Task<JsonResult> OnPostAsync()
        {
            var stream = new MemoryStream();
            Request.Body.CopyTo(stream);
            stream.Position = 0;
            using (var reader = new StreamReader(stream))
            {
                var requestBody = await reader.ReadToEndAsync();
                if (requestBody.Length > 0)
                {
                    var obj = JsonConvert.DeserializeObject<SmartCutModel>(requestBody);
                    if (obj != null)
                    {
                        Snippets = obj.Snippets;
                        Planks = obj.Planks;
                    }
                }
            }

            var cuttingStock = new CuttingStock(Snippets.Cast<ISnippet>().ToList());
            var planks = cuttingStock.CalculateCuts(Planks);

            return new JsonResult(new { Planks = planks, Free = CuttingStock.GetFree(planks) });
        }

        public async Task<IActionResult> OnPostImportAsync()
        {
            var ie = new ImportExport();

            using (var ms = new MemoryStream())
            {
                await ImportExcel.CopyToAsync(ms);
                Snippets.Clear();
                Snippets.AddRange(ie.ImportSnippets(ms.ToArray()));
            }

            return RedirectToPage("/SmartCut");
        }

        public async Task<IActionResult> OnPostExportAsync()
        {
            var ie = new ImportExport();

            var stream = new MemoryStream();
            Request.Body.CopyTo(stream);
            stream.Position = 0;
            using (var reader = new StreamReader(stream))
            {
                var requestBody = await reader.ReadToEndAsync();
                if (requestBody.Length > 0)
                {
                    var obj = JsonConvert.DeserializeObject<SmartCutModel>(requestBody);
                    if (obj != null)
                    {
                        Snippets = obj.Snippets;
                        Planks = obj.Planks;
                    }
                }
            }

            var cuttingStock = new CuttingStock(Snippets.Cast<ISnippet>().ToList());
            var planks = cuttingStock.CalculateCuts(Planks);

            var data = ie.Export(planks);

            return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Customers.xlsx");
        }
    }

    public class TestSnippet : Snippet
    {
        public TestSnippet(float length) : base(length)
        {

        }

        public int Apartment { get; set; }

        public override string ToString()
        {
            return $"{Length}[{Apartment}]";
        }
    }
}