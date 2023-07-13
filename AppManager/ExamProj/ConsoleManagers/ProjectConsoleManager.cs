using BLL.Abstractions;
using BLL.Implementations;
using Core.Entities;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.ConsoleManagers
{
    internal class ProjectConsoleManager : BaseConsoleManager<IProjectService, Project>
    {
        private readonly ToDoTaskConsoleManager _toDoTaskConsoleManager;
        private User authorizationUser;
        private readonly IUserService _userService;
        private readonly IProjectUserService _projectUserService;
        public ProjectConsoleManager(IProjectService service, ToDoTaskConsoleManager toDoTaskConsoleManager, IUserService userService, IProjectUserService projectUserService) : base(service)
        {
            _toDoTaskConsoleManager = toDoTaskConsoleManager;
            _userService = userService;
            _projectUserService = projectUserService;
        }

        internal async Task<User> StartProcessAsync(User currentUser)
        {
            authorizationUser = currentUser;
            switch (authorizationUser.Role)
            {
                case UserRole.StakeHolder:
                    return await ManageTaskHolderProjectsAsync();
                default:
                    return await ManageTesterAndDevProjectsAsync();
            }

        }
        internal async Task<User> ManageTaskHolderProjectsAsync()
        {
            while (true)
            {

                Console.WriteLine("Choose action on 'Project':\n 1.Create\n 2.Delete\n 3.Update\n 4.Manage Tasks\n 5.Exit");
                switch (int.Parse(Console.ReadLine()))
                {
                    case 1:
                        await CreateProjectAsync();
                        break;
                    case 2:
                        await DeleteProjectAsync();
                        break;
                    case 3:
                        await UpdateProjectAsync();
                        break;
                    case 4:
                        var projects = await _service.GetProjects(authorizationUser);
                        if (projects.Count == 0)
                        {
                            Console.WriteLine("You haven`t created any projects yet");
                            return authorizationUser;
                        }
                        else
                        {
                            await ManageTasksAsync();
                            break;
                        }
                    case 5: return authorizationUser;
                    default:
                        Console.WriteLine("Invalid input operation number");
                        break;
                }

            }
        }
        internal async Task<User> ManageTesterAndDevProjectsAsync()
        {
            while (true)
            {

                Console.WriteLine("Choose action on 'Project':\n1.Manage Tasks\n 2.Exit");
                switch (int.Parse(Console.ReadLine()))
                {
                    case 1:
                        var projects = await _service.GetProjects(authorizationUser);
                        if (projects.Count == 0)
                        {
                            Console.WriteLine("You haven`t created any projects yet");
                            return authorizationUser;
                        }
                        else
                        {
                            await ManageTasksAsync();
                            break;
                        }
                    case 2: return authorizationUser;
                    default:
                        Console.WriteLine("Invalid input operation number");
                        break;
                }

            }
        }
        private async Task ManageTasksAsync()
        {
            Console.WriteLine("What project do you want to check tasks in?");
            await ShowProjectsNamesAsync();
            var projectIndex = int.Parse(Console.ReadLine()) - 1;
            var projects = await _service.GetProjects(authorizationUser);
            var managedProject = projects[projectIndex];
            var changedProject = await _toDoTaskConsoleManager.StartProcessAsync(authorizationUser, managedProject);
            var result = await _service.UpdateItem(changedProject);
            if (!result.IsSuccessful)
            {
                Console.WriteLine(result.Message);
            }
            else
            {
                //var oldProjectUser = await _userProjectService.GetByIds(authorizationUser.Id, managedProject.Id);
                //var updatedProjectUser = new ProjectUser() { Id = oldProjectUser.Id,User = authorizationUser,Project = changedProject,UserId = authorizationUser.Id,ProjectId = changedProject.Id};
                //await _userProjectService.UpdateItem(updatedProjectUser);
            }
        }
        internal async Task CreateProjectAsync()
        {
            string name = await CreateProjectNameAsync();
            Console.WriteLine("Write description for this project:");
            string description = Console.ReadLine();
            var responseUsers = new List<User>() { authorizationUser };
            Console.WriteLine("You should set response users to your project. Do you want to do it now?\n1.Yes\n2.Later");
            switch (int.Parse(Console.ReadLine()))
            {
                case 1:
                    var responseDeveloper = await GetEachUserRoleAsync(UserRole.Developer);
                    if (responseDeveloper is not null)
                    {
                        responseUsers.Add(responseDeveloper);
                    }
                    var responseTester = await GetEachUserRoleAsync(UserRole.Tester);
                    if (responseTester is not null)
                    {
                        responseUsers.Add(responseTester);
                    }
                    break;
                default:
                    break;
            }
            var resultProject = new Project() { Name = name,Description = description};
            for (int i = 0; i < responseUsers.Count; i++)
            {
                await _projectUserService.AddItem(new ProjectUser() { User = responseUsers[i], Project = resultProject, UserId = responseUsers[i].Id, ProjectId = resultProject.Id });
            }

            //var result = await _service.AddItem(resultProject);
            //if (!result.IsSuccessful)
            //{
            //    Console.WriteLine(result.Message);
            //}
        }
        private async Task<User> GetEachUserRoleAsync(UserRole role)
        {
            var result = await _userService.GetUsersByRole(role);
            if (!result.IsSuccessful)
            {
                Console.WriteLine(result.Message);
                return null;
            }
            var users = result.Data;
            if (users.Count == 0) { return null; }
            Console.WriteLine($"What user do you want to set as {role}?");
            int index = 1;
            foreach (var user in users)
            {
                Console.WriteLine($"{index}.{user.Username}");
                index++;
            }
            index = int.Parse(Console.ReadLine()) - 1;
            var newResponseUser = users[index];
            return newResponseUser;
        }
        private async Task<string> CreateProjectNameAsync()
        {
            Console.WriteLine("Enter project name:");
            string projectName = Console.ReadLine();
            if (await _service.IfProjectNameAlreadyExists(projectName, authorizationUser))
            {
                Console.WriteLine("You have project with such name. Try another.");
                return await CreateProjectNameAsync();
            }
            return projectName;
        }
        internal async Task DeleteProjectAsync()
        {
            Console.WriteLine("What project do you want to delete?");
            await ShowProjectsNamesAsync();
            var projectIndex = int.Parse(Console.ReadLine()) - 1;
            var projects = await _service.GetProjects(authorizationUser);
            var deletedProject = projects[projectIndex];

            var result = await _service.DeleteItem(deletedProject.Id);
            if (!result.IsSuccessful)
            {
                Console.WriteLine(result.Message);
            }
            else
            {
                var userProjects = await _projectUserService.GetAll();
                foreach (var item in userProjects.Data)
                {
                    if (item.ProjectId==deletedProject.Id)
                    {
                        await _projectUserService.DeleteItem((await _projectUserService.GetByIds(authorizationUser.Id, item.ProjectId)).Id);
                    }
                }
            }

        }
        internal async Task UpdateProjectAsync()
        {
            Console.WriteLine("What project do you want to update?");
            await ShowProjectsNamesAsync();
            var projectIndex = int.Parse(Console.ReadLine()) - 1;
            var projects = await _service.GetProjects(authorizationUser);
            var updatedProject = projects[projectIndex];

            Console.WriteLine("What do you want to update?\n 1.Name\n 2.Description\n 3.Response Users");
            switch (int.Parse(Console.ReadLine()))
            {
                case 1:
                    Console.WriteLine("Enter new project name:");
                    updatedProject.Name = Console.ReadLine();
                    break;
                case 2:
                    Console.WriteLine("Enter new description:");
                    updatedProject.Description = Console.ReadLine();
                    break;
                case 3:
                    Console.WriteLine("What position do you want to update?\n1.Developer\n2.Tester");
                    switch (int.Parse(Console.ReadLine()))
                    {
                        case 1:
                            var responseDeveloper = await GetEachUserRoleAsync(UserRole.Developer);
                            await _projectUserService.AddItem(new ProjectUser() { User = responseDeveloper, Project = updatedProject, UserId = responseDeveloper.Id, ProjectId = updatedProject.Id });
                            break;
                        case 2:
                            var responseTester = await GetEachUserRoleAsync(UserRole.Tester);
                            await _projectUserService.AddItem(new ProjectUser() { User = responseTester, Project = updatedProject, UserId = responseTester.Id, ProjectId = updatedProject.Id });
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    Console.WriteLine("Invalid operation number");
                    await UpdateProjectAsync();
                    break;
            }
            var result = await _service.UpdateItem(updatedProject);
            if (!result.IsSuccessful)
            {
                Console.WriteLine(result.Message);
            }
        }
        private async Task ShowProjectsNamesAsync()
        {
            var projects = await _service.GetProjects(authorizationUser);
            if (projects.Count == 0)
            {
                Console.WriteLine("You haven`t created any projects yet. Create now:");
                await CreateProjectAsync();
            }
            else
            {
                var index = 1;
                foreach (var project in projects)
                {
                    Console.WriteLine($"{index}.{project.Name}");
                    index++;
                }
            }

        }
    }
}
