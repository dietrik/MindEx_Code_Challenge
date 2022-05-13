using challenge.Models;
using challenge.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ILogger _logger;
        private readonly ICompensationRepository _compensationRepository; 

        public CompensationService(ILogger<ICompensationService> logger, ICompensationRepository compensationRepository)
        {
            _logger = logger;
            _compensationRepository = compensationRepository;
        }

        public Compensation Create(Compensation compensation)
        {
            if(compensation != null)
            {
                _compensationRepository.Add(compensation);
                _compensationRepository.SaveAsync().Wait();
            }
            return compensation;
        }

        public Compensation GetByEmployeeId(string employeeId)
        {
            if (String.IsNullOrEmpty(employeeId))
            {
                return null;
            }

            return _compensationRepository.GetById(employeeId);
        }
    }
}
