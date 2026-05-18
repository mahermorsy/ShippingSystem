using BusinessLayer.Contracts;
using BusinessLayer.Dtos;
using BusinessLayer.Services.ShipmentFile;
using DataAccessLayer;
using DataAccessLayer.Interfaces;
using Domains.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using UI.Models;

namespace UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IShippingTypes _IGenericRepository;
  

        public HomeController(ILogger<HomeController> Logger, IShippingTypes IGenericRepository) 
        {
            _logger = Logger;
            _IGenericRepository = IGenericRepository;
        }
        public async Task<IActionResult> Index()
        {
          
            var ShippingTyps = await _IGenericRepository.GetAllAsync();   
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Payment()
        {
            return View();
        }
        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult OurServices()
        {
            return View();
        }
        public IActionResult About()
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
