using FinalExam.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam.Business.Services.Abstracts
{
    public interface IProjectService
    {
        Task AddProject(Project project);
        void DeleteProject(int id);
        void UpdateProject(int id,Project newProject);
        Project GetProject(Func<Project, bool>? predicate = null);
        List<Project> GetAllProjects(Func<Project, bool>? predicate = null); 
    }
}
