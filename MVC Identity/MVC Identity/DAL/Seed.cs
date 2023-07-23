using Core;
using MVC_First.Helpers;

namespace MVC_First.DAL
{
    public class Seed
    {
        public static async Task SeedData(IApplicationBuilder applicationBuilder) 
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var unitOfWork = new UnitOfWork(serviceScope.ServiceProvider.GetService<AppManagerContext>());
                if (!(await unitOfWork.UserRepository.GetAllAsync()).Data.Any())
                {
                    await unitOfWork.UserRepository.AddItemAsync(
                        new AppUser()
                        {
                            UserName = "Nikita",
                            PasswordHash = UserHelper.GetPasswordHash("Nikita2004"),
                            Email = "beliykharkov.n@gmail.com",
                            PhoneNumber = "1234567890",
                            //Role = Core.Enums.UserRole.StakeHolder
                        });
                    await unitOfWork.UserRepository.AddItemAsync(
                        new AppUser()
                        {
                            UserName = "Borys",
                            PasswordHash = UserHelper.GetPasswordHash("Nikita2004"),
                            Email = "beliykharkov.n@gmail.com",
                            PhoneNumber = "1234567890",
                            //Role = Core.Enums.UserRole.Developer
                        });
                    await unitOfWork.UserRepository.AddItemAsync(
                        new AppUser()
                        {
                            UserName = "Katya",
                            PasswordHash = UserHelper.GetPasswordHash("Nikita2004"),
                            Email = "beliykharkov.n@gmail.com",
                            PhoneNumber = "1234567890",
                            //Role = Core.Enums.UserRole.Tester
                        });
                }
                //if (!(await unitOfWork.ProjectRepository.GetAllAsync()).Data.Any())
                //{
                //    await unitOfWork.ProjectRepository.AddItemAsync(
                //       new Models.Project()
                //       {
                //           Name = "test",
                //           Description= "test",
                //       });
                //}
                //if (!(await unitOfWork.ProjectUserRepository.GetAllAsync()).Data.Any())
                //{
                //    await unitOfWork.ProjectUserRepository.AddItemAsync(
                //       new Models.ProjectUser()
                //       {
                //           ProjectId = 
                //       });
                //    await unitOfWork.ProjectRepository.AddItemAsync(
                //       new Models.Project()
                //       {
                //           Name = "test",
                //           Description = "test",
                //       });
                //    await unitOfWork.ProjectRepository.AddItemAsync(
                //       new Models.Project()
                //       {
                //           Name = "test",
                //           Description = "test",
                //       });
                //}
            }
        }
    }
}

