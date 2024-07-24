using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SchoolSystemStak.DAL.Models;
using SchoolSystemStak.DAL.Models.Identity;
using SchoolSystemTask.BAL.Interfaces;
using SchoolSystemTask.BAL.Specefication;
using SchoolSystemTask.PL.Helper;
using SchoolSystemTask.PL.ViewModels;

namespace SchoolSystemTask.PL.Controllers
{
    public class StudentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _toastNotification;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentController(IUnitOfWork unitOfWork, IToastNotification toastNotification, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _toastNotification = toastNotification;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate, string? SearhName = null, int? classid = null)
        {
            IReadOnlyList<Student> students;
            SpecParameter specParameter = new SpecParameter() { ClassId = classid, SearchByName = SearhName };
            var spec = new StudentsWithSpecification(specParameter);
            students = await _unitOfWork.Repository<Student>().GetAllAsyncWithSpecification(spec);
            if (endDate.HasValue && startDate.HasValue)
                students = students.Where(item => item.DateOfBirth >= startDate && item.DateOfBirth <= endDate).ToList();

            var studentMapped = _mapper.Map<IEnumerable<StudentViewModel>>(students);
            return View(studentMapped);
        }
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0)
                return NotFound("Not Found Student");
            var spec = new StudentsWithSpecification(id);
            var Student = await _unitOfWork.Repository<Student>().GetByIdAsyncWithSpecification(spec);
            var mappedStudent = _mapper.Map<StudentViewModel>(Student);
            return View(mappedStudent);


        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.classes = await _unitOfWork.Repository<Class>().GetAllAsync();
            return View(new CreateOrEditViewModel());

        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrEditViewModel data)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.classes = await _unitOfWork.Repository<Class>().GetAllAsync();

                return View(data);
            }
            var user = new ApplicationUser()
            {
                Email = data.Email,
                UserName = data.Email.Split('@')[0],
            };
            var result = await _userManager.CreateAsync(user, data.Pass);
            if (!result.Succeeded)
            {
                ViewBag.classes = await _unitOfWork.Repository<Class>().GetAllAsync();
                ModelState.AddModelError(string.Empty, "Weak Pass OR User Duplicate");
                return View(data);
            }

            var mappedStudent = _mapper.Map<Student>(data);
            if (data.FileImage != null)
                mappedStudent.File = DocumentSetting.UplouadFile(data.FileImage, "Images");
            mappedStudent.ApplicationUser = user;
            _unitOfWork.Repository<Student>().AddEntity(mappedStudent);
            await _unitOfWork.CompleteAsync();
            _toastNotification.AddSuccessToastMessage("Created Suceesfully");

            return RedirectToAction(nameof(Index));


        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
                return NotFound("Not Found Student");
            ViewBag.classes = await _unitOfWork.Repository<Class>().GetAllAsync();

            var spec = new StudentsWithSpecification(id);
            var Student = await _unitOfWork.Repository<Student>().GetByIdAsyncWithSpecification(spec);
            if (Student == null)
                return BadRequest("Not Valid Dont Try This Again ..?");
            var mappedStudent = _mapper.Map<EditViewModel>(Student);
            return View(mappedStudent);


        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel data)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.classes = await _unitOfWork.Repository<Class>().GetAllAsync();
                return View(data);
            }
            var MappeddStudent = _mapper.Map<Student>(data);
            if (data.FileImage != null)
                MappeddStudent.File = DocumentSetting.UplouadFile(data.FileImage, "Images");
            _unitOfWork.Repository<Student>().UpdateEntity(MappeddStudent);
            await _unitOfWork.CompleteAsync();
            _toastNotification.AddInfoToastMessage("Updated Suceesfully");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
                return NotFound("Not Found");
            var student=await _unitOfWork.Repository<Student>().GetById(id);
            if (student == null) return BadRequest(string.Empty);
            if(student.File != null)
                 DocumentSetting.DeleteFile(student.File, "Images");
          _unitOfWork.Repository<Student>().DeleteEntity(student);
          await  _unitOfWork.CompleteAsync();
            _toastNotification.AddWarningToastMessage("Deleted  Success");
            return RedirectToAction(nameof(Index));
        }
    }
}
