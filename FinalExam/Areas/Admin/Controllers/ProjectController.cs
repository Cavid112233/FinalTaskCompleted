using FinalExam.Business.Exceptions;
using FinalExam.Business.Services.Abstracts;
using FinalExam.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalExam.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public IActionResult Index()
        {
            var projects = _projectService.GetAllProjects();
            return View(projects);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Project project)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                await _projectService.AddProject(project);
            }
            catch (ImageContextException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View();
            }
            catch (ImageSizeException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View();
            }
            catch (FileNullReferenceException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            var existProject = _projectService.GetProject(x => x.Id == id);
            if (existProject == null) return NotFound();
            return View(existProject);
        }

        [HttpPost]
        public IActionResult Update(Project project)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                _projectService.UpdateProject(project.Id, project);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound();
            }
            catch (ImageContextException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View();
            }
            catch (ImageSizeException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View();
            }
            catch (FinalExam.Business.Exceptions.FileNotFoundException ex)
            {
                ModelState.AddModelError("ImageFile", ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var existProject = _projectService.GetProject(x => x.Id == id);
            if (existProject == null) return NotFound();
            return View(existProject);
        }

        [HttpPost]
        public IActionResult DeletePost(int id)
        {

            try
            {
                _projectService.DeleteProject(id);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound();
            }
            catch (FinalExam.Business.Exceptions.FileNotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return RedirectToAction("Index");
        }

    }
}
