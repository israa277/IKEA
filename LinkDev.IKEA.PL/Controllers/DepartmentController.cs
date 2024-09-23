using LinkDev.IKEA.BLL.Models.Departments;
using LinkDev.IKEA.BLL.Services.Departments;
using LinkDev.IKEA.DAL.Entities.Department;
using LinkDev.IKEA.PL.ViewModels.Departments;
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

        [HttpGet] //GET :/Department/Details
        public IActionResult Details(int? id)
        {
            if (id is null)

                return BadRequest();
            var department = _departmentService.GetDepartmentById(id.Value);
            if (department is null)
                return NotFound();
            return View(department);

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
                message = _environment.IsDevelopment() ? ex.Message : "An Error has occured during creating The Department :(";

            }
            ModelState.AddModelError(string.Empty, message);
                return View(department);
            
        }

        [HttpGet] // Get: Department/Edit/id
        public IActionResult Edit(int? id)
        {
            if (id is null)
                return BadRequest(); // 400

            var department = _departmentService.GetDepartmentById(id.Value);
            if (department is null)
                return NotFound(); // 404


            return View(new DepartmentEditViewModel()
            {
                Code = department.Code,
                Name = department.Name,
                Description = department.Description,
                CreationDate = department.CreationDate,
            });
        }


        [HttpPost] //Post
        public IActionResult Edit([FromRoute] int id, DepartmentEditViewModel departmentVM)
        {
            if (!ModelState.IsValid) // Server-Side Validation
                return View(departmentVM);
            var message = string.Empty;
            try
            {
                var departmentToUpdate = new UpdateDepartmentDto()
                {
                    Id = id,
                    Code = departmentVM.Code,
                    Name = departmentVM.Name,
                    Description = departmentVM.Description,
                    CreationDate = departmentVM.CreationDate,
                };
                var Updated = _departmentService.UpdateDepartment(departmentToUpdate) > 0;
                if (Updated)
                    return RedirectToAction(nameof(Index));
                message = "An Error has occured during Updating The Department :(";

            }
            catch (Exception ex)
            {
                // 1. Log Exception
                _logger.LogError(ex, ex.Message);
                // 2. Set Message
                message = _environment.IsDevelopment() ? ex.Message : "An Error During Updating The Department :(";

            }
            ModelState.AddModelError(string.Empty, message);
            return View(departmentVM);

        }
    }
}
