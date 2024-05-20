using FinalExam.Business.Exceptions;
using FinalExam.Business.Extensions;
using FinalExam.Business.Services.Abstracts;
using FinalExam.Core.Models;
using FinalExam.Core.RepositoryAbstract;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam.Business.Services.Concretes
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectrepository;
        private readonly IWebHostEnvironment _env;

        public ProjectService(IProjectRepository projectRepository, IWebHostEnvironment env)
        {
            _projectrepository = projectRepository;
            _env = env;
        }

        public async Task AddProject(Project project)
        {
            if (project.ImageFile == null)
                throw new FileNullReferenceException("File bos ola bilmez!");

            project.ImageUrl = Helper.SaveFile(_env.WebRootPath, @"uploads\projects", project.ImageFile);

            await _projectrepository.AddSync(project);
            await _projectrepository.CommitAsync();
        }

        public void DeleteProject(int id)
        {
            var existProject = _projectrepository.Get(x => x.Id == id);
            if (existProject == null)
                throw new EntityNotFoundException("Project tapimadi.");
            Helper.DeleteFile(_env.WebRootPath, @"uploads\projects", existProject.ImageUrl);
            _projectrepository.Delete(existProject);
            _projectrepository.Commit();
        }

        public List<Project> GetAllProjects(Func<Project, bool>? predicate = null)
        {
            return _projectrepository.GetAll(predicate);
        }

        public Project GetProject(Func<Project, bool>? predicate = null)
        {
           return _projectrepository.Get(predicate);
        }

        public void UpdateProject(int id, Project newProject)
        {
            var oldProject = _projectrepository.Get(x=>x.Id == id);
            if(oldProject == null)
                throw new EntityNotFoundException($"Project tapilmadi");

            if(newProject.ImageFile != null)
            {
                Helper.DeleteFile(_env.WebRootPath, @"uploads\projects", oldProject.ImageUrl);
                oldProject.ImageUrl = Helper.SaveFile(_env.WebRootPath, @"uploads\projects", newProject.ImageFile);
            }
            oldProject.Title = newProject.Title;
            oldProject.Description = newProject.Description;

            _projectrepository.Commit();
        }
    }
}
