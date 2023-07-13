using BLL.Abstractions;
using Core.Entities;
using Core.Enums;
using Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UI.ConsoleManagers
{
    internal class UserConsoleManager : BaseConsoleManager<IUserService, User>
    {
        private readonly ProjectConsoleManager _projectConsoleManager;
        private readonly NotificationConsoleManager _notificationConsoleManager;
        private readonly IProjectUserService _projectUserService;
        private User authorizationUser;
        public UserConsoleManager(IUserService service, ProjectConsoleManager projectConsoleManager, NotificationConsoleManager notificationConsoleManager, IProjectUserService projectUserService) : base(service)
        {
            _projectConsoleManager = projectConsoleManager;
            _notificationConsoleManager = notificationConsoleManager;
            _projectUserService = projectUserService;
        }
        internal async Task StartProcessAsync()
        {
            Console.WriteLine("Welcome! Please, authenticate yourself:");
            await AuthenticateUserAsync();
            await ManageNotificationsAsync();
            await ManageActionsAsync();
        }
        internal async Task ManageNotificationsAsync()
        {
            authorizationUser = await _notificationConsoleManager.StartProcessAsync(authorizationUser);
            await UpdateUserAsync();
        }
        internal async Task ManageActionsAsync()
        {
            while (true)
            {
                Console.WriteLine(" Choose your next action:\n 1.Manage account\n 2.Manage projects\n 3.Exit");
                switch (int.Parse(Console.ReadLine()))
                {
                    case 1:
                        await ManageAccountAsync();
                        break;
                    case 2:
                        authorizationUser = await _projectConsoleManager.StartProcessAsync(authorizationUser);
                        await UpdateUserAsync();
                        break;
                    case 3:
                        await StartProcessAsync();
                        return;
                    default:
                        Console.WriteLine("Invalid input number of action");
                        break;
                }
            }
        }
        private async Task AuthenticateUserAsync()
        {
            Console.WriteLine("1.Log in \n2.Sign up");
            switch (int.Parse(Console.ReadLine()))
            {
                case 1:
                    await AuthorizateUserAsync();
                    break;
                case 2:
                    await RegistrateUserAsync();
                    break;
                default:
                    Console.WriteLine("Invalid operation number");
                    await AuthenticateUserAsync();
                    break;
            }
        }
        private async Task RegistrateUserAsync()
        {
            Console.WriteLine("Create your UserName");
            var userName = await GetUserName();
            Console.WriteLine("Create reliable password:");
            var password = Console.ReadLine();
            Console.WriteLine("Enter your email:");
            var email = Console.ReadLine();
            Console.WriteLine("Enter your phone number:");
            var phoneNumber = Console.ReadLine();
            Console.WriteLine("Enter your role:\n1.Stake Holder\n2.Developer\n3.Tester");
            var role = await GetUserRoleAsync();
            var registrationUser = new User(userName, password, email, phoneNumber, role);

            authorizationUser = registrationUser;
            var result = await _service.AddItem(registrationUser);
            if (!result.IsSuccessful)
            {
                Console.WriteLine(result.Message);
            }
        }
        private async Task<UserRole> GetUserRoleAsync()
        {
            UserRole role;
            switch (int.Parse(Console.ReadLine()))
            {
                case 1:
                    role = UserRole.StakeHolder;
                    break;
                case 2:
                    role = UserRole.Developer;
                    break;
                case 3:
                    role = UserRole.Tester;
                    break;
                default:
                    Console.WriteLine("Invalid operation number");
                    return await GetUserRoleAsync();
            }
            return role;
        }
        private async Task<string> GetUserName()
        {
            string userName = Console.ReadLine();
            var result = await _service.CheckUserNameIfExists(userName);
            if (!result.IsSuccessful)
            {
                Console.WriteLine(result.Message);
                return await GetUserName();
            }
            if (result.Data)
            {
                Console.WriteLine("This UserName already exists. Please, try another");
                return await GetUserName();
            }

            return userName;
        }
        private async Task AuthorizateUserAsync()
        {
            while (true)
            {
                Console.WriteLine("Enter your UserName");
                var userName = Console.ReadLine();
                Console.WriteLine("Enter your password:");
                var password = Console.ReadLine();
                var result = await _service.Authorization(userName, password);
                if (!result.IsSuccessful)
                {
                    Console.WriteLine(result.Message);
                    switch (int.Parse(Console.ReadLine()))
                    {
                        case 1:
                            await StartProcessAsync();
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    authorizationUser = result.Data;
                    return;
                }
            }
        }
        private async Task ManageAccountAsync()
        {
            Console.WriteLine("What do you want to update in account?\n 1.UserName\n 2.Password\n 3.Email\n 4.Mobile phone\n 5.Check notifications\n 6.Exit");
            switch (int.Parse(Console.ReadLine()))
            {
                case 1:
                    Console.WriteLine("Enter new UserName:");
                    string newUserName = await GetUserName();
                    authorizationUser.Username = newUserName;
                    await UpdateUserAsync();
                    break;
                case 2:
                    await UpdatePasswordAsync();
                    break;
                case 3:
                    Console.WriteLine("Enter new email adress:");
                    string newEmail = Console.ReadLine();
                    authorizationUser.Email = newEmail;
                    await UpdateUserAsync();
                    break;
                case 4:
                    Console.WriteLine("Enter new phone number:");
                    string newPhone = Console.ReadLine();
                    authorizationUser.PhoneNumber = newPhone;
                    await UpdateUserAsync();
                    break;
                case 5:
                    await _notificationConsoleManager.PrintAllNotificationsAsync();
                    break;
                case 6:
                    break;
                default:
                    Console.WriteLine("Invalid operation number");
                    await ManageAccountAsync();
                    break;
            }
        }
        private async Task UpdatePasswordAsync()
        {
            Console.WriteLine("Enter old password:");
            string oldPasswordHash = UserHelper.GetPasswordHash(Console.ReadLine());
            if (oldPasswordHash == authorizationUser.PasswordHash)
            {
                Console.WriteLine("Enter new password:");
                string newPasswordHash = UserHelper.GetPasswordHash(Console.ReadLine());
                authorizationUser.PasswordHash = newPasswordHash;
                await UpdateUserAsync();
            }
            else
            {
                Console.WriteLine("Wrong password\n'1' Exit");
                switch (Console.ReadLine())
                {
                    case "1":
                        await ManageAccountAsync();
                        break;
                    default:
                        await UpdatePasswordAsync();
                        break;
                }
            }
        }
        private async Task UpdateUserAsync()
        {
            var result = await _service.UpdateItem(authorizationUser);
            if (!result.IsSuccessful)
            {
                Console.WriteLine(result.Message);
            }
        }
    }
}
