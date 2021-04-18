using System;
using TimetibleMicroservices.Models.DBModels;

namespace TimetibleMicroservices.Models.Interfaces
{
    public interface ILessonRepository : IRepository<Lesson, Guid>
    {
    }
}
