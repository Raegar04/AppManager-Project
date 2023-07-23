using BLL.Abstractions;
using BLL.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_First.Models;
using MVC_Identity.Helpers;
using MVC_Identity.ViewModels;

namespace MVC_First.Controllers
{
    [Route("[controller]")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IProjectUserService _projectUserService;
        public ProjectController(IProjectService projectService, UserManager<AppUser> userManager, IProjectUserService projectUserService)
        {
            _projectService = projectService;
            _userManager = userManager;
            _projectUserService = projectUserService;
        }
        //private async Task ImplementUser() => authenticateUser = await _userManager.GetUserAsync(User);
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var projects = user.ProjectUsers.Select(userProj => userProj.Project).ToList();
            var manageProjectsViewModel = new ManageEntitiesViewModel<Project>() { Items = projects };
            return View(manageProjectsViewModel);
        }

        //[HttpPost]
        //[Route("Index")]
        //public async Task<IActionResult> ManageProjectsAsync(ManageEntitiesViewModel<Project> manageProjectsViewModel)
        //{
        //    //if (!ModelState.IsValid) { return View(manageProjectsViewModel); }
        //    var result = await _projectService.GetProjectByName(manageProjectsViewModel.Name);
        //    Project.currentProject = result.Data;
        //    switch ((await _userManager.GetRolesAsync(_userManager.GetUserAsync(User).Result))[0])
        //    {
        //        case "StakeHolder":
        //            return RedirectToAction("ManageActions", "Project");
        //        default:
        //            return RedirectToAction("ManageActionsDevTest", "Project");
        //    }

        //}

        [HttpGet]
        [Route("Create")]
        [Authorize(Roles = "StakeHolder")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = "StakeHolder")]
        public async Task<IActionResult> Create(InfoProjectViewModel createProjectViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createProjectViewModel);
            }

            var project = new Project()
            {
                Name = createProjectViewModel.Name,
                Description = createProjectViewModel.Description
            };
            var authUser = await _userManager.GetUserAsync(User);
            var responseUsers = new List<AppUser>() { authUser };
            if (createProjectViewModel.Developer is not null)
            {
                responseUsers.Add(await _userManager.FindByNameAsync(createProjectViewModel.Developer));
            }
            if (createProjectViewModel.Tester is not null)
            {
                responseUsers.Add(await _userManager.FindByNameAsync(createProjectViewModel.Tester));
            }
            foreach (var user in responseUsers)
            {
                await _projectUserService.AddItem(new ProjectUser()
                {
                    User = user,
                    UserId = user.Id,
                    Project = project,
                    ProjectId = project.Id
                });
                SendMail.Send(user.Email,"Assigning to a project", $"You've been assigned to a project {project.Name}");
            }
            return RedirectToAction("Index");
        }

        //[Authorize(Roles = "StakeHolder")]
        //[Route("ManageActions")]
        //public IActionResult ManageActions()
        //{

        //    var manageProjectActionsViewModel = new ManageActionsViewModel()
        //    {
        //        Actions = new List<string>()
        //        {
        //            "Update",
        //            "Delete",
        //            "Check info",
        //            "Manage Users",
        //            "Manage project's tasks",
        //        }
        //    };
        //    return View(manageProjectActionsViewModel);
        //}
        //[HttpPost]
        //[Authorize(Roles = "StakeHolder")]
        //[Route("ManageActions")]
        //public async Task<IActionResult> ManageActions(ManageActionsViewModel manageProjectActionsViewModel)
        //{
        //    switch (manageProjectActionsViewModel.SelectedAction)
        //    {
        //        case "Update":
        //            return RedirectToAction("Update");
        //        case "Delete":
        //            await _projectService.DeleteItem(Project.currentProject.Id);
        //            return RedirectToAction("Index", "Project");
        //        case "Check info":
        //            return RedirectToAction("Show");
        //        case "Manage Users":
        //            return RedirectToAction("ManageUsers","Project");
        //        case "Manage project's tasks":
        //            return RedirectToAction("Index", "ToDoTask");
        //        default:
        //            return View(manageProjectActionsViewModel);
        //    }
        //}

        //[Authorize(Roles = "Developer,Tester")]
        //[Route("ManageActionsDevTest")]
        //public IActionResult ManageActionsDevTest(Project project)
        //{

        //    var manageProjectActionsViewModel = new ManageActionsViewModel()
        //    {
        //        Actions = new List<string>()
        //        {
        //            "Check info",
        //            "Manage project's tasks",
        //        }
        //    };
        //    return View(manageProjectActionsViewModel);
        //}
        //[HttpPost]
        //[Authorize(Roles = "Developer,Tester")]
        //[Route("ManageActionsDevTest")]
        //public async Task<IActionResult> ManageActionsDevTest(ManageActionsViewModel manageProjectActionsViewModel)
        //{
        //    switch (manageProjectActionsViewModel.SelectedAction)
        //    {
        //        case "Check info":
        //            return RedirectToAction("Show");
        //        case "Manage project's tasks":
        //            return RedirectToAction("Index", "ToDoTask");
        //        default:
        //            return View(manageProjectActionsViewModel);
        //    }
        //}

        [HttpPost]
        [Route("GetCurrentProject")]
        public async Task<IActionResult> GetCurrentProject(ManageEntitiesViewModel<Project> manageEntitiesViewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var project = user.ProjectUsers.Select(item=>item.Project).ToList().
                FirstOrDefault(proj=>proj.Name==manageEntitiesViewModel.Name);

            Project.currentProject = project;
            return RedirectToAction(manageEntitiesViewModel.Action, "Project");
        }

        [HttpGet]
        [Authorize(Roles = "StakeHolder")]
        [Route("Update")]
        public async Task<IActionResult> Update()
        {
            var project = Project.currentProject;
            var showProjectViewModel = new InfoProjectViewModel()
            {
                Name = project.Name,
                Description = project.Description,
            };
            return View(showProjectViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "StakeHolder")]
        [Route("Update")]
        public async Task<IActionResult> Update(InfoProjectViewModel infoProjectViewModel)
        {
            var project = _projectService.GetByPredicate(proj => proj.Id == Project.currentProject.Id).Result.Data;
            project.Name = infoProjectViewModel.Name;
            project.Description = infoProjectViewModel.Description;
            await _projectService.UpdateItem(project);

            return RedirectToAction("Index", "Project");
        }

        [HttpPost]
        [Authorize(Roles = "StakeHolder")]
        [Route("Delete")]
        public async Task<IActionResult> DeleteProject(InfoProjectViewModel infoProjectViewModel)
        {
            var project = (await _projectService.GetByPredicate(proj => proj.Name == infoProjectViewModel.Name)).Data;
            await _projectService.DeleteItem(project.Id);
            return RedirectToAction("Index", "Project");
        }

        [Authorize(Roles = "StakeHolder")]
        [Route("ManageUsers")]
        public async Task<IActionResult> ManageUsers()
        {
            var project = _projectService.GetByPredicate(proj => proj.Id == Project.currentProject.Id).Result.Data;
            var manageEntitiesViewModel = new ManageEntitiesViewModel<AppUser>() { Items = project.ProjectUsers.Select(item => item.User).ToList() };
            return View(manageEntitiesViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "StakeHolder")]
        [Route("RemoveUser")]
        public async Task<IActionResult> RemoveUser(ManageEntitiesViewModel<Project> manageEntitiesViewModel)
        {
            var user = await _userManager.FindByNameAsync(manageEntitiesViewModel.Name);
            var projUser = await _projectUserService.GetByIds(user.Id, Project.currentProject.Id);
            await _projectUserService.DeleteItem(projUser.Id);
            return RedirectToAction("Index", "Project");
        }

        [HttpPost]
        [Authorize(Roles = "StakeHolder")]
        [Route("AddUser")]
        public async Task<IActionResult> AddUser(ManageEntitiesViewModel<Project> manageEntitiesViewModel)
        {
            var user = await _userManager.FindByNameAsync(manageEntitiesViewModel.Name);
            await _projectUserService.AddItem(new ProjectUser()
            {
                Project = Project.currentProject,
                ProjectId = Project.currentProject.Id,
                User = user,
                UserId = user.Id
            });
            return RedirectToAction("Index", "Project");
        }
    }
}
