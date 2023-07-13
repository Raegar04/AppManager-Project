using BLL.Abstractions;
using Core.Entities;
using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;

namespace UI.ConsoleManagers
{
    internal class ToDoTaskConsoleManager : BaseConsoleManager<IToDoTaskService, ToDoTask>
    {
        private User _currentUser;
        private Project _currentProject;
        public ToDoTaskConsoleManager(IToDoTaskService service) : base(service)
        {
        }
        internal async Task<Project> StartProcessAsync(User currentUser, Project currentProject)
        {
            _currentProject = currentProject;
            _currentUser = currentUser;
            switch (_currentUser.Role)
            {
                case UserRole.StakeHolder:
                    return await ManageTaskHolderTasksAsync();
                default:
                    return await ManageTesterAndDevTasksAsync();
            }
        }
        internal async Task<Project> ManageTaskHolderTasksAsync()
        {
            while (true)
            {

                Console.WriteLine("Choose action on 'Task':\n 1.Create\n 2.Delete\n 3.Update\n 4.Get information\n 5.Exit");
                switch (int.Parse(Console.ReadLine()))
                {
                    case 1:
                        await CreateTaskAsync();
                        break;
                    case 2:
                        await DeleteTaskAsync();
                        break;
                    case 3:
                        var updatedTask = await FindNeededTaskAsync("update?");
                        await UpdateTaskByHolderAsync(updatedTask);
                        break;
                    case 4:
                        await DisplayTaskAsync();
                        break;
                    case 5:
                        return _currentProject;
                    default:
                        Console.WriteLine("Invalid input operation number");
                        break;
                }
            }
        }
        internal async Task<Project> ManageTesterAndDevTasksAsync()
        {
            while (true)
            {

                Console.WriteLine("Choose action on 'Task':\n 1.Update status\n 2.Get information\n 3.Exit");
                switch (int.Parse(Console.ReadLine()))
                {
                    case 1:
                        var updatedTask = await FindNeededTaskAsync("update?");
                        switch (_currentUser.Role)
                        {
                            case UserRole.Developer:

                                await UpdateTaskByDevAsync(updatedTask);
                                break;
                            case UserRole.Tester:
                                await UpdateTaskByTesterAsync(updatedTask);
                                break;
                            default:
                                break;
                        }
                        break;
                    case 2:
                        await DisplayTaskAsync();
                        break;
                    case 3:
                        return _currentProject;
                    default:
                        Console.WriteLine("Invalid input operation number");
                        break;
                }

            }

        }
        internal async Task CreateTaskAsync()
        {
            Console.WriteLine("Enter name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter description:");
            string description = Console.ReadLine();
            Console.WriteLine("Set priority of the task:\n1.Urgent\n2.High\n3.Medium\n4.Low\n5.Minor");
            var priority = await GetTaskPriorityAsync();
            DateTime startDate = await GetDateTimeAsync(" start date");
            DateTime deadLineDate = await GetDateTimeAsync(" end date");
            TaskCurrentStatus currentStatus;
            if (startDate > DateTime.Now)
            {
                currentStatus = TaskCurrentStatus.Planned;
            }
            else
            {
                currentStatus = TaskCurrentStatus.Process;
            }

            var createdTask = new ToDoTask()
            {
                Name = name,
                Description = description,
                Priority = priority,
                StartTime = startDate,
                DeadLine = deadLineDate,
                Status = currentStatus,
            };

            var result = await _service.AddItem(createdTask);
            if (!result.IsSuccessful) { Console.WriteLine(result.Message); }
            else
            {
                _currentProject.ToDoTasks.Add(createdTask);
            }

        }
        private async Task<TaskPriority> GetTaskPriorityAsync()
        {
            TaskPriority priority;
            switch (int.Parse(Console.ReadLine()))
            {
                case 1:
                    priority = TaskPriority.Urgent;
                    break;
                case 2:
                    priority = TaskPriority.High;
                    break;
                case 3:
                    priority = TaskPriority.Medium;
                    break;
                case 4:
                    priority = TaskPriority.Low;
                    break;
                case 5:
                    priority = TaskPriority.Minor;
                    break;
                default:
                    Console.WriteLine("Invalid operation number");
                    return await GetTaskPriorityAsync();
            }
            return priority;
        }
        private async Task<DateTime> GetDateTimeAsync(string message)
        {
            try
            {
                Console.Write($"Enter {message} in format:0000.00.00: ");
                string[] date = Console.ReadLine().Split('.');
                return new DateTime(int.Parse(date[0]), int.Parse(date[1]), int.Parse(date[2]));
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}.Try again");
                return await GetDateTimeAsync(message);
            }

        }
        private async Task ShortDisplayAllTasksAsync()
        {
            if (_currentProject.ToDoTasks.Count == 0)
            {
                Console.WriteLine("You haven`t created any tasks yet. Create now:");
                await CreateTaskAsync();
            }
            else
            {
                int index = 1;
                foreach (var task in _currentProject.ToDoTasks)
                {
                    Console.WriteLine($"{index}.{task.Name}");
                    index++;
                }
            }

        }
        private async Task<ToDoTask> FindNeededTaskAsync(string request)
        {
            Console.WriteLine($"What task do you want to {request}");
            await ShortDisplayAllTasksAsync();
            int taskIndex = int.Parse(Console.ReadLine()) - 1;
            return _currentProject.ToDoTasks[taskIndex];
        }
        internal async Task DisplayTaskAsync()
        {
            var currentTask = await FindNeededTaskAsync("check information about");
            Console.WriteLine(
                $"Title:{currentTask.Name}\n" +
                $"Description:{currentTask.Description}\n" +
                $"Priority:{currentTask.Priority}\n" +
                $"Start Time:{currentTask.StartTime}\n" +
                $"Deadline: {currentTask.DeadLine}\n" +
                $"Status: {currentTask.Status}\nResponse Users:\n");
        }
        internal async Task DeleteTaskAsync()
        {
            var deletedTask = await FindNeededTaskAsync("delete?");

            var result = await _service.DeleteItem(deletedTask.Id);
            if (!result.IsSuccessful)
            {
                Console.WriteLine(result.Message);
            }
            else
            {
                _currentProject.ToDoTasks.RemoveAt(_currentProject.ToDoTasks.FindIndex(task => task.Id == deletedTask.Id));
            }
        }
        internal async Task UpdateTaskByHolderAsync(ToDoTask updatedTask)
        {

            Console.WriteLine("What do you want to update in this task?\n" +
                "1.Name\n" +
                "2.Description\n" +
                "3.Priority\n" +
                "4.DeadLine\n" +
                "5.Add File");
            switch (int.Parse(Console.ReadLine()))
            {
                case 1:
                    Console.WriteLine("Enter new task name:");
                    updatedTask.Name = Console.ReadLine();
                    break;
                case 2:
                    Console.WriteLine("Enter new task description:");
                    updatedTask.Description = Console.ReadLine();
                    break;
                case 3:
                    Console.WriteLine("Enter new task priority:\n1.Urgent\n2.High\n3.Medium\n4.Low\n5.Minor");
                    updatedTask.Priority = await GetTaskPriorityAsync();
                    break;
                case 4:
                    updatedTask.DeadLine = await GetDateTimeAsync("new task deadline");
                    break;
                case 5:
                    Console.WriteLine("Enter path of adding file:");
                    //await _service.AddFileToDirectory(Console.ReadLine());
                    break;
                default:
                    Console.WriteLine("Invalid operation number.");
                    await UpdateTaskByHolderAsync(updatedTask);
                    break;
            }
            await UpdateTaskAsync(updatedTask);
        }
        internal async Task UpdateTaskByTesterAsync(ToDoTask updatedTask)
        {
            Console.WriteLine("What do you want to update in this task?\n1.Change status\n2.Add File");
            switch (int.Parse(Console.ReadLine()))
            {
                case 1:
                    if (updatedTask.Status != TaskCurrentStatus.Testing)
                    {
                        Console.WriteLine($"Current status of this task is {updatedTask.Status}. You can manage tasks only with status 'testing'");
                        return;
                    }
                    Console.WriteLine("Has this task any problems?\n1.Yes, return to 'Process'\n2.No, set as 'Done'");
                    switch (int.Parse(Console.ReadLine()))
                    {
                        case 1:
                            updatedTask.Status = TaskCurrentStatus.Process;
                            break;
                        case 2:
                            updatedTask.Status = TaskCurrentStatus.Done;
                            break;
                        default:
                            break;
                    }
                    break;
                case 2:
                    Console.WriteLine("Enter path of adding file:");
                    //await _service.AddFileToDirectory(Console.ReadLine());
                    break;
                default:
                    break;
            }
            await UpdateTaskAsync(updatedTask);
        }
        internal async Task UpdateTaskByDevAsync(ToDoTask updatedTask)
        {
            Console.WriteLine("What do you want to update in this task?\n1.Change status\n2.Add File");
            switch (int.Parse(Console.ReadLine()))
            {
                case 1:
                    if (updatedTask.Status != TaskCurrentStatus.Process)
                    {
                        Console.WriteLine($"Current status of this task is {updatedTask.Status}. You can manage tasks only with status 'Process'");
                        return;
                    }
                    updatedTask.Status = TaskCurrentStatus.Testing;
                    break;
                case 2:
                    Console.WriteLine("Enter path of adding file:");
                    //await _service.AddFileToDirectory(Console.ReadLine());
                    break;
                default:
                    break;
            }
            await UpdateTaskAsync(updatedTask);
        }
        private async Task UpdateTaskAsync(ToDoTask updatedTask)
        {

            var result = await _service.UpdateItem(updatedTask);
            if (!result.IsSuccessful)
            {
                Console.WriteLine(result.Message);
            }
            else
            {
                _currentProject.ToDoTasks.RemoveAt(_currentProject.ToDoTasks.FindIndex(task => task.Id == updatedTask.Id));
                _currentProject.ToDoTasks.Add(updatedTask);
            }
        }
    }
}
