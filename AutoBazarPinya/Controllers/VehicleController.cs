using Application.Dtos;
using Application.Interfaces;
using AutoBazarPinya.Models;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using ViewModels;

namespace AutoBazarPinya.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;

        public VehicleController(IVehicleService vehicleService, IMapper mapper)
        {
            _vehicleService = vehicleService;
            _mapper = mapper;
        }

        // GET: /Vehicle
        public async Task<IActionResult> Index()
        {
            var vehicles = await _vehicleService.GetAllAsync();
            var viewModels = _mapper.Map<IEnumerable<VehicleViewModel>>(vehicles);

            return View(viewModels);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetPaged([FromForm] DataTableRequest request)
        {
            var result = await _vehicleService.GetPagedAsync(request);
            return Json(result);
        }
        // GET: /Vehicle/Details/5
        public async Task<IActionResult> Details(long id, string? returnUrl = null)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);
            if (vehicle == null)
                return NotFound();

            var viewModel = _mapper.Map<VehicleViewModel>(vehicle);
            ViewBag.ReturnUrl = returnUrl;
            return View(viewModel);
        }
        [HttpGet]
        public async Task<JsonResult> CheckLicensePlate(string licensePlate, long? id = null)
        {
            var existing = await _vehicleService.GetByLicensePlateAsync(licensePlate);

            bool isValid = existing == null || existing.Id == id;

            return Json(isValid);
        }
        //GET: /Vehicle/Create
        public IActionResult Create(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new VehicleViewModel());
        }

        //POST: /Vehicle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VehicleViewModel viewModel, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            var dto = _mapper.Map<CreateVehicleDto>(viewModel);
            await _vehicleService.AddAsync(dto);
            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Vehicle/Edit/5
        public async Task<IActionResult> Edit(long id, string? returnUrl = null)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);
            if (vehicle == null)
                return NotFound();

            var viewModel = _mapper.Map<VehicleViewModel>(vehicle);
            ViewBag.ReturnUrl = returnUrl;
            return View(viewModel);
        }

        // POST: /Vehicle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VehicleViewModel viewModel, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var dto = _mapper.Map<UpdateVehicleDto>(viewModel);
            await _vehicleService.UpdateAsync(dto);

            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction(nameof(Index));
        }

        // POST: /Vehicle/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            await _vehicleService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Vehicle/Stats
        //mock stats
        public async Task<IActionResult> Stats()
        {
            var vehicles = await _vehicleService.GetAllAsync();

            var stats = new
            {
                Total = vehicles.Count(),
                LowMileage = vehicles.Count(v => v.Mileage < 100000),
                HighMileage = vehicles.Count(v => v.Mileage >= 100000),
                Petrol = vehicles.Count(v => v.Fuel.ToString().Contains("Petrol")),
                Diesel = vehicles.Count(v => v.Fuel.ToString().Contains("Diesel")),
                Excellent = vehicles.Count(v => v.Condition.ToString().Contains("Excellent")),
                Poor = vehicles.Count(v => v.Condition.ToString().Contains("Poor"))
            };

            return View(stats);
        }
    }
}
