using AssessmentApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssessmentApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  //  [Authorize]
    public class EmployeeController : BaseController
    {
        public EmployeeController(AssessmentDbContext context):base(context)
        {

        }
        [HttpGet]
        [Route("GetEmployees")]
        public List<Employee> GetEmployees()//This is th get method which get all the employee from the db and return  
        {
            List<Employee> employeeList = new List<Employee>();
            try
            {

                employeeList = _context.Employees.ToList();
                
            }
            catch(Exception ex)
            {
                return employeeList;
            }
            return employeeList;
        }
        [HttpGet]
        [Route("GetEmployeeById/{Id}")]
        public Employee GetEmployeeById(string Id)//This is th get method which get one record on the basis of Id   
        {
            Employee employee = new Employee();
            employee = _context.Employees.Find(Convert.ToInt32(Id));
            return (employee);
        }
        [HttpPost]
        [Route("InsertEmployee")]
        public IActionResult Create(Employee employee)//This method will insert the employee into db  
        {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                else
                {
                    _context.Employees.Add(employee);
                    _context.SaveChanges();
                    return Ok(employee);
                }
        }
        [HttpPut]
        [Route("UpdateEmployee")]
        public IActionResult Update(Employee employee)//Update method will update the employee  
        {
                if (ModelState.IsValid)
                {
                    _context.Entry(employee).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Ok(employee);
                }
                else
                {
                    return BadRequest(ModelState);
                }
        }
        [HttpDelete]
        [Route("DeleteEmployee/{Id}")]
        public IActionResult Delete(int Id)//this method will Delete the record  
        {
                Employee employee = _context.Employees.Find(Convert.ToInt32(Id));
                if (employee == null) { return NotFound(); }
                else
                {
                    _context.Employees.Remove(employee);
                    _context.SaveChanges();
                    return Ok(employee);
                }
        }
    }
}
