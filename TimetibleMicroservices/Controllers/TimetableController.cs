using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Threading.Tasks;
using TimetibleMicroservices.Models;
using TimetibleMicroservices.Models.DTOModels;
using TimetibleMicroservices.Models.Interfaces;

namespace TimetibleMicroservices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimetableController : ControllerBase
    {

        private readonly ITimetableService _timetableService;
        private readonly ILogger<TimetableController> _logger;
        public TimetableController(ITimetableService timetableService,
                                    ILogger<TimetableController> logger)
        {
            _timetableService = timetableService;
            _logger = logger;
        }
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Add history", Type = typeof(ResultDto<LessonDto>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<LessonDto>> AddLesson([FromBody] AddLessonDto lesson)
        {
            var result = await _timetableService.AddLesson(lesson);
            return Ok(result);
        }
    }
}
