using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using ViewModels;

namespace AutoBazarPinya.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;
        private readonly IVehicleFilterMapper _vehicleFilterMapper;

        private readonly IMapper _mapper;

        public VehicleController(IVehicleService vehicleService, IMapper mapper, IVehicleFilterMapper vehicleFilterMapper)
        {
            _vehicleService = vehicleService;
            _mapper = mapper;
            _vehicleFilterMapper = vehicleFilterMapper;
        }

        // GET: /Vehicle
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var vehicles = await _vehicleService.GetAllAsync(cancellationToken);
            var viewModels = _mapper.Map<IEnumerable<VehicleViewModel>>(vehicles);

            return View(viewModels);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetPaged([FromForm] DataTableRequest request, CancellationToken cancellationToken)
        {
            var result = await _vehicleService.GetPagedAsync(request, cancellationToken);
            return Json(result);
        }
        // GET: /Vehicle/Details/5
        public async Task<IActionResult> Details(long id, CancellationToken cancellationToken, string? returnUrl = null)
        {
            var vehicle = await _vehicleService.GetByIdAsync(id, cancellationToken);
            if (vehicle == null)
                return NotFound();

            var viewModel = _mapper.Map<VehicleViewModel>(vehicle);
            ViewBag.ReturnUrl = returnUrl;
            return View(viewModel);
        }
        [HttpGet]
        public async Task<JsonResult> CheckLicensePlate(string licensePlate, CancellationToken cancellationToken, long? id = null)
        {
            var existing = await _vehicleService.GetByLicensePlateAsync(licensePlate, cancellationToken);

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
        [HttpGet]
        public IActionResult Stats()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Count([FromForm] VehicleFilterVm vm, CancellationToken cancellationToken)
        {
            var filters = _vehicleFilterMapper.Map(vm);
            var count = await _vehicleService.GetFilteredCountAsync(filters, cancellationToken);
            return Json(new { count });
        }
    }
}
