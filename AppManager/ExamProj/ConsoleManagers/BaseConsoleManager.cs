using BLL.Abstractions;
using BLL.Implementations;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.ConsoleManagers
{
    internal abstract class BaseConsoleManager<IService, CurrentEntity>
        where IService : IGenericService<CurrentEntity>
        where CurrentEntity : Entity
    {
        protected readonly IService _service;
        protected BaseConsoleManager(IService service)
        {
            if (service != null)
            {
                _service = service;
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
        
    }
}
