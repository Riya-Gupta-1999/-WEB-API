using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NS.EMS.API.Entities;
using System.Collections.Generic;
using System.Linq;

namespace NS.EMS.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    
    public class EmployeeController : ControllerBase
    {
        //[Route("[action]")]
        [HttpGet]
        //[AllowAnonymous]
        [Authorize]
        public IActionResult GetEmployees()
        {
            var employeeList = new List<Employee>();
            using (var context=new EmployeeDbContext())
            {
                employeeList=context.Employee.ToList();
            }
                return Ok(employeeList);
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetEmployeeByEid(int eId)
        {
            var employee = new Employee();
            using (var context = new EmployeeDbContext())
            {
                employee = context.Employee.Find(eId);
            }
            if(employee == null)
            {
                return NotFound("Employee Not Found");
            }
            return Ok(employee);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddEmployee(Employee employee)
        {
            using (var context = new EmployeeDbContext())
            {
                context.Employee.Add(employee);
                context.SaveChanges();
            }
            return Ok("Record Inserted Successfully");
        }

        [Authorize]
        [HttpPut]
        public IActionResult UpdateEmployee(Employee employee)
        {
            using (var context = new EmployeeDbContext())
            {
                context.Employee.Update(employee);
                context.SaveChanges();
            }
            return Ok("Record Updated Successfully");
        }
        [Authorize]
        [HttpDelete]
        public IActionResult DeleteEmployeeByEid(int eId)
        {
            var employee = new Employee();
            using (var context = new EmployeeDbContext())
            {
                employee=context.Employee.Find(eId);
                context.Employee.Remove(employee);
            }
            if (employee == null)
            {
                return NotFound("Employee Not Found");
            }
            return Ok(employee);
        }


    }
}
