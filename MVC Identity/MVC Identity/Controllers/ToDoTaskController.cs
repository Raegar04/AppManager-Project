using BLL.Abstractions;
using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_First.DAL.Repositories;
using MVC_First.Models;
using MVC_Identity.ViewModels;

namespace MVC_First.Controllers
{
    [Route("[controller]")]
    public class ToDoTaskController : Controller
    {
        private readonly IToDoTaskService _toDoTaskService;
        private readonly IProjectService _projectService;
        private readonly UserManager<AppUser> _userManager;

        public ToDoTaskController(IToDoTaskService toDoTaskService, IProjectService projectService,UserManager<AppUser> userManager)
        {
            _toDoTaskService = toDoTaskService;
            _projectService = projectService;
            _userManager = userManager;
        }
        [HttpPost]
        [Route("GetUpdatedProject")]
        public async Task<IActionResult> GetUpdatedProject(ManageEntitiesViewModel<Project> manageEntitiesViewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var project = user.ProjectUsers.Select(item => item.Project).ToList().
                FirstOrDefault(proj => proj.Name == manageEntitiesViewModel.Name);

            Project.currentProject = project;

            return RedirectToAction("Index", "ToDoTask");
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {

            //var tasks = Project.currentProject.ToDoTasks;
            var tasks = (await _projectService.GetByPredicate(proj => proj.Id == Project.currentProject.Id)).Data.ToDoTasks;
            var manageTasksViewModel = new ManageEntitiesViewModel<ToDoTask>() { Items = tasks };
            return View(manageTasksViewModel);
        }
        [HttpPost]
        [Route("Index")]
        public async Task<IActionResult> ManageTasks(ManageEntitiesViewModel<ToDoTask> manageTasksViewModel)
        {

            var result = (await _userManager.GetUserAsync(User)).
                ProjectUsers.FirstOrDefault(item=>item.Id==Project.currentProject.Id).
                Project.ToDoTasks.FirstOrDefault(task=>task.Name==manageTasksViewModel.Name);
            ToDoTask.toDoTask = result;
            return RedirectToAction("ManageActions");
        }
        [Route("Create")]
        [Authorize(Roles = "StakeHolder")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = "StakeHolder")]
        public async Task<IActionResult> Create(InfoToDoTaskViewModel createToDoTaskViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createToDoTaskViewModel);
            }
            var task = new ToDoTask()
            {
                Name = createToDoTaskViewModel.Name,
                Description = createToDoTaskViewModel.Description,
                Priority = createToDoTaskViewModel.Priority,
                StartTime = createToDoTaskViewModel.StartTime,
                DeadLine = createToDoTaskViewModel.DeadLine,
            };
            //task.Project = Project.currentProject;
            //task.ProjectId = Project.currentProject.Id;
            if (task.StartTime > DateTime.Now)
            {
                task.Status = TaskCurrentStatus.Planned;
            }
            else
            {
                task.Status = TaskCurrentStatus.Process;
            }
            if (!ModelState.IsValid)
            {
                return View(createToDoTaskViewModel);
            }
            var project = _projectService.GetByPredicate(proj => proj.Id == Project.currentProject.Id).Result.Data;
            project.ToDoTasks.Add(task);
            await _projectService.UpdateItem(project);
            await _toDoTaskService.AddItem(task);
            //Project.currentProject.ToDoTasks.Add(task);

            //await _toDoTaskService.AddItem(task);
            return RedirectToAction("Index");
        }

        [Route("ManageActions")]
        public IActionResult ManageActions(ToDoTask task)
        {

            var manageTaskActionsViewModel = new ManageActionsViewModel()
            {
                Actions = new List<string>()
                {
                    "Update",
                    "Delete",
                    "Check info"
                }
            };
            return View(manageTaskActionsViewModel);
        }
        [HttpPost]
        [Route("ManageActions")]
        public async Task<IActionResult> ManageActions(ManageActionsViewModel manageTaskActionsViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(manageTaskActionsViewModel);
            }
            switch (manageTaskActionsViewModel.SelectedAction)
            {
                case "Update":
                    return RedirectToAction("Update");
                case "Delete":
                    await _toDoTaskService.DeleteItem(ToDoTask.toDoTask.Id);
                    return RedirectToAction("Index", "Project");
                case "Check info":
                    return RedirectToAction("ShowInfo");
                default:
                    return View(manageTaskActionsViewModel);
            }
        }

        [HttpPost]
        [Route("GetTaskToUpdate")]
        public async Task<IActionResult> GetTaskToUpdate(ManageEntitiesViewModel<ToDoTask> manageTaskActionsViewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var task = user.ProjectUsers.Select(item => item.Project).ToList().
                FirstOrDefault(proj => proj.Id == Project.currentProject.Id).ToDoTasks.FirstOrDefault(task=>task.Name==manageTaskActionsViewModel.Name);

            ToDoTask.toDoTask = task;

            return RedirectToAction("Update", "ToDoTask");
        }

        [HttpPost]
        [Authorize(Roles = "StakeHolder")]
        [Route("Delete")]
        public async Task<IActionResult> DeleteTask(InfoToDoTaskViewModel infotaskViewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var task = user.ProjectUsers.Select(item => item.Project).ToList().
                FirstOrDefault(proj => proj.Id == Project.currentProject.Id).ToDoTasks.FirstOrDefault(task => task.Name == infotaskViewModel.Name);
            await _toDoTaskService.DeleteItem(task.Id);
            return RedirectToAction("Index", "Project");
        }

        //[HttpGet]
        //[Route("ShowInfo")]
        //public async Task<IActionResult> ShowInfo()
        //{
        //    var task = ToDoTask.toDoTask;
        //    var showTaskViewModel = new InfoToDoTaskViewModel()
        //    {
        //        Name = task.Name,
        //        Description = task.Description,
        //        Priority = task.Priority,
        //        StartTime = task.StartTime,
        //        DeadLine = task.DeadLine,
        //        Status = task.Status,
        //    };
        //    return View(showTaskViewModel);
        //}

        //[Route("ManageActionsDevTest")]
        //public IActionResult ManageActionsDevTest(ToDoTask task)
        //{

        //    var manageTaskActionsViewModel = new ManageActionsViewModel()
        //    {
        //        Actions = new List<string>()
        //        {
        //            "Update status",
        //            "Check info"
        //        }
        //    };
        //    return View(manageTaskActionsViewModel);
        //}
        //[HttpPost]
        //[Route("ManageActionsDevTest")]
        //public async Task<IActionResult> ManageActionsDevTest(ManageActionsViewModel manageTaskActionsViewModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(manageTaskActionsViewModel);
        //    }
        //    switch (manageTaskActionsViewModel.SelectedAction)
        //    {
        //        case "Update status":
        //            return RedirectToAction("Update");
        //        case "Check info":
        //            return RedirectToAction("ShowInfo");
        //        default:
        //            return View(manageTaskActionsViewModel);
        //    }
        //}

        [Authorize(Roles ="StakeHolder")]
        [Route("Update")]
        public async Task<IActionResult> Update()
        {
            var task = ToDoTask.toDoTask;
            var showTaskViewModel = new InfoToDoTaskViewModel()
            {
                Name = task.Name,
                Description = task.Description,
                Priority = task.Priority,
                StartTime = task.StartTime,
                DeadLine = task.DeadLine,
                Status = task.Status,
            };
            return View(showTaskViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "StakeHolder")]
        [Route("Update")]
        public async Task<IActionResult> GetUpdatedTask(InfoToDoTaskViewModel infoToDoTaskViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(infoToDoTaskViewModel);
            }
            var task = new ToDoTask()
            {
                Name = infoToDoTaskViewModel.Name,
                Description = infoToDoTaskViewModel.Description,
                Priority = infoToDoTaskViewModel.Priority,
                StartTime = infoToDoTaskViewModel.StartTime,
                DeadLine = infoToDoTaskViewModel.DeadLine,
                Status = infoToDoTaskViewModel.Status
            };

            var project = _projectService.GetByPredicate(proj => proj.Id == Project.currentProject.Id).Result.Data;
            project.ToDoTasks.RemoveAt(project.ToDoTasks.FindIndex(tsk => tsk.Id == ToDoTask.toDoTask.Id));
            task.Id = ToDoTask.toDoTask.Id;
            ToDoTask.toDoTask = task;
            project.ToDoTasks.Add(task);
            await _projectService.UpdateItem(project);
            await _toDoTaskService.UpdateItem(task);

            return RedirectToAction("Index", "Project");
        }
    }
}
