using AssessmentApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AssessmentApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly AssessmentDbContext _context;
        public BaseController(AssessmentDbContext context)
        {
            this._context = context;
           
        }
    }
}
