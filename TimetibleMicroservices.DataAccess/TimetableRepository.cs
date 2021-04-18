using Microsoft.Extensions.Options;
using TimetibleMicroservices.Models.DBModels;
using TimetibleMicroservices.Models.Interfaces;
using TimetibleMicroservices.Models.SettingsClass;

namespace TimetibleMicroservices.DataAccess
{
    internal class TimetableRepository : BaseRepository<Lesson>,ITimetableRepository
    {
        public TimetableRepository(IOptions<MongoDBSettings> mongoDbSettings) : base(mongoDbSettings)
        {
        }
    }
}
