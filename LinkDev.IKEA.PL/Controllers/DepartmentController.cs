using LinkDev.IKEA.BLL.Models.Departments;
using LinkDev.IKEA.BLL.Services.Departments;
using Microsoft.AspNetCore.Mvc;

namespace LinkDev.IKEA.PL.Controllers
{
    // Inheritance : DepartmentController is a Controller
    // Composition : DepartmentController Has a IDepartmentService
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IWebHostEnvironment _environment;
        public DepartmentController(
            IDepartmentService departmentService,
            ILogger<DepartmentController> logger,
        IWebHostEnvironment environment
          )
        {
            _logger = logger;
            _environment = environment;
            _departmentService = departmentService;
        }
        [HttpGet] //GET :/Department/Index
        public IActionResult Index() 
        {
            var departments = _departmentService.GetAllDepartment();
            return View(departments);
        }

        [HttpGet] //GET :/Department/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Creatre(CreatedDepartmentDto department)
        {
            if (!ModelState.IsValid)
                return View(department);
            var message = string.Empty;
            try
            {
                var Result = _departmentService.CreateDepartment(department);
                if (Result > 0)
                    return RedirectToAction(nameof(Index));
                else
                {
                    message = "Department is not Created";
                    ModelState.AddModelError(string.Empty, message);
                    return View(department);
                }
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                _logger.LogError(ex, ex.Message);
                //2. Set Message
                if (_environment.IsDevelopment())
                {
                    message = ex.Message;
                    return View(department);
                }
                else
                {
                    message = "Department is not Created";
                    return View("ERROR", message);
                }
            }
        }
    }
}

