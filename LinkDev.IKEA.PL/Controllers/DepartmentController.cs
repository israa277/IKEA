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
        #region Serivces
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
        #endregion

        #region Index
        [HttpGet] //GET :/Department/Index
        public IActionResult Index()
        {
            // Views Dictionary : Pass Data From Controller(Action) To View (From View ---> [Partial View , Layout])
            // 1 . ViewData is a Dictionary Type Property
            ViewData["Message"] = "Hello ViewData";
            // 2. ViewBag is a Dynamic Type Property
            ViewBag.Message = "Hello Bag";
            ViewBag.Message = new { Id = 10, Name = "ISRAA" };

            var departments = _departmentService.GetAllDepartment();
            return View(departments);
        }
        #endregion

        #region Details
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
        #endregion

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Creatre(DepartmentViewModel departmentVM)
        {
            if (!ModelState.IsValid)
                return View(departmentVM);
            var message = string.Empty;
            try
            {
                var CreatedDepartment = new CreatedDepartmentDto()
                {
                    Code = departmentVM.Code,
                    Name = departmentVM.Name,
                    Description = departmentVM.Description,
                    CreationDate = departmentVM.CreationDate,
                };
                var Result = _departmentService.CreateDepartment(CreatedDepartment);
                if (Result > 0)
                    return RedirectToAction(nameof(Index));
                else
                {
                    message = "Department is not Created";
                    ModelState.AddModelError(string.Empty, message);
                    return View(departmentVM);
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
            return View(departmentVM);

        }
        #endregion

        #region Update

        [HttpGet] // Get: Department/Edit/id
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? id)
        {
            if (id is null)
                return BadRequest(); // 400

            var department = _departmentService.GetDepartmentById(id.Value);
            if (department is null)
                return NotFound(); // 404


            return View(new DepartmentViewModel()
            {
                Code = department.Code,
                Name = department.Name,
                Description = department.Description,
                CreationDate = department.CreationDate,
            });
        }


        [HttpPost] //Post
        public IActionResult Edit([FromRoute] int id, DepartmentViewModel departmentVM)
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

        #endregion

        #region Delete

        [HttpGet] // Get /Department/Delete/id?
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if (id is null)
                return BadRequest();
            var department = _departmentService.GetDepartmentById(id.Value);
            if (department is null)
                return NotFound();
            return View(department);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var message = string.Empty;
            try
            {
                var deleted = _departmentService.DeleteDepartment(id);
                if (deleted)
                    return RedirectToAction(nameof(Index));
                message = "An Error has occured during Deleting The Department :(";
            }
            catch (Exception ex)
            {
                // 1. Log Exception
                _logger.LogError(ex, ex.Message);
                // 2. Set Message

                message = _environment.IsDevelopment() ? ex.Message : "An Error has occured during Deleting The Department :(";
            }
            //ModelState.AddModelError(string.Empty, message);
            return RedirectToAction(nameof(Index));
        }
        #endregion




    }
}
