using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TimetibleMicroservices.Models.DBModels;
using TimetibleMicroservices.Models.DTOModels;
using TimetibleMicroservices.Models.Interfaces;
using TimetibleMicroservices.Models.SettingsClass;

namespace TimetibleMicroservices.DataAccess
{
    public class TimetableRepository : BaseRepository<Lesson>, ITimetableRepository
    {
        public TimetableRepository(IOptions<MongoDBSettings> mongoDbSettings) : base(mongoDbSettings)
        {

        }
        public async Task<IEnumerable<Lesson>> GetFilteredAsync(LessonFilter LessonFilter, CancellationToken cancellationToken = default)
        {
            FilterDefinition<Lesson> filter =
                 Builders<Lesson>.Filter.Eq(new ExpressionFieldDefinition<Lesson, bool>(x => x.IsDeleted), false);
            if (LessonFilter?.FilterBy?.Lectural != null)
            {
                filter = filter & Builders<Lesson>.Filter.Eq(new ExpressionFieldDefinition<Lesson, string>(x => x.LecturalName),
                    LessonFilter?.FilterBy?.Lectural);
            }

            if (!String.IsNullOrEmpty(LessonFilter?.FilterBy?.Group))
            {
                filter = filter & Builders<Lesson>.Filter.Eq(new ExpressionFieldDefinition<Lesson, string>(x => x.GroupNumber),
                    LessonFilter?.FilterBy?.Group);
            }

            if (!String.IsNullOrEmpty(LessonFilter?.FilterBy?.Discipline))
            {
                filter = filter & Builders<Lesson>.Filter.Eq(new ExpressionFieldDefinition<Lesson, string>(x => x.DisciplineName),
                    LessonFilter?.FilterBy?.Discipline);
            }
            if (!String.IsNullOrEmpty(LessonFilter?.FilterBy?.Group))
            {
                filter = filter & Builders<Lesson>.Filter.Gte(new ExpressionFieldDefinition<Lesson, DateTime>(x => x.LessonDate),
                    (DateTime)LessonFilter?.FilterBy?.DateStart);
            }
            if (!String.IsNullOrEmpty(LessonFilter?.FilterBy?.Group))
            {
                filter = filter & Builders<Lesson>.Filter.Lte(new ExpressionFieldDefinition<Lesson, DateTime>(x => x.LessonDate),
                      (DateTime)LessonFilter?.FilterBy?.DateEnd);
            }
            return await GetCollection().Find(filter).Sort(Builders<Lesson>.Sort.Ascending(LessonFilter?.SortBy ?? "LessonDate")).ToListAsync(cancellationToken);
        }
    }
}
