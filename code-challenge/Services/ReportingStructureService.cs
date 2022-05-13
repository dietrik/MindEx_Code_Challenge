using challenge.Models;
using challenge.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<ReportingStructureService> _logger;

        public ReportingStructureService(ILogger<ReportingStructureService> logger, IEmployeeRepository employeeRepository)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
        }

        public ReportingStructure GetByEmployeeId(String employeeId)
        {
            if (String.IsNullOrEmpty(employeeId))
            {
                return null;
            }

            Employee employee = _employeeRepository.GetById(employeeId);
            
            if(employee == null)
            {
                return null;
            }

            ReportingStructure reportingStructure = new ReportingStructure();
            reportingStructure.Employee = employeeId;

            HashSet<String> directReportEmployeeIds = new HashSet<String>();
            List<Employee> directReportEmployeesToCheck = new List<Employee>();

            if (employee.DirectReports != null)
            {
                foreach (Employee directEmployee in employee.DirectReports)
                {
                    directReportEmployeeIds.Add(directEmployee.EmployeeId);
                    directReportEmployeesToCheck.Add(directEmployee);
                }
            }

            while (directReportEmployeesToCheck.Count > 0)
            {
                Employee employeeCheck = _employeeRepository.GetById(directReportEmployeesToCheck.First().EmployeeId);
                directReportEmployeesToCheck.RemoveAt(0);

                if (employeeCheck.DirectReports != null)
                {
                    foreach (Employee directEmployee in employeeCheck.DirectReports)
                    {
                        if (directReportEmployeeIds.Add(directEmployee.EmployeeId))
                        {
                            directReportEmployeesToCheck.Add(directEmployee);
                        }
                    }
                }
            }

            reportingStructure.NumberOfReports = directReportEmployeeIds.Count;
            return reportingStructure;
        }
    }
}
