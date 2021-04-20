using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TimetibleMicroservices.Models.DBModels;
using TimetibleMicroservices.Models.DTOModels;

namespace TimetibleMicroservices.Models.Interfaces
{
    public interface ITimetableRepository : IRepository<Lesson, Guid>
    {
        Task<IEnumerable<Lesson>> GetFilteredAsync(LessonFilter LessonFilter, CancellationToken cancellationToken = default);
        Task InsertManyLesson(List<Lesson> lessons, CancellationToken cancellationToken = default);
        Task DeleteAllLessons(CancellationToken cancellationToken);
    }
}
