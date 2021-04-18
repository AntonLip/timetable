using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TimetibleMicroservices.Models.DTOModels;
using TimetibleMicroservices.Models.Interfaces;

namespace TimetibleMicroservices.Services
{
    public class TimetableService : ITimetableService
    {
        

        public Task<LessonDto> AddLesson(AddLessonDto model, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<LessonDto> DeleteTimetable(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<LessonDto> GetLessonById(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LessonDto>> GetTimetable(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
