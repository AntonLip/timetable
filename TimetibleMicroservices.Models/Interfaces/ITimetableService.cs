using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TimetibleMicroservices.Models.DTOModels;

namespace TimetibleMicroservices.Models.Interfaces
{
    public interface ITimetableService
    {
        Task<LessonDto> AddLesson(AddLessonDto model, CancellationToken cancellationToken = default);
        Task<LessonDto> DeleteTimetable(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<LessonDto>> GetTimetable(CancellationToken cancellationToken = default);        
        Task<LessonDto> GetLessonById(Guid id, CancellationToken cancellationToken = default);
        Task<LessonDto> DeleteLesson(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<IEnumerable<LessonDto>>> GetFilteredTimetable(LessonFilter lessonFilter, CancellationToken cancellationToken = default);
        Task<int> CreateTimetableFromFile(IFormFile body, CancellationToken cancellationToken = default);
        Task<LessonDto> UpdateLesson(Guid lessonId, LessonDto lessonDto);
        Task<FileDto> GetTimetableInDocxAsync(LessonFilter lessonFilter, CancellationToken cancellationToken = default);
    }
}
