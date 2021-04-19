using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TimetibleMicroservices.Models.DBModels;
using TimetibleMicroservices.Models.DTOModels;
using TimetibleMicroservices.Models.Interfaces;

namespace TimetibleMicroservices.Services
{
    public class TimetableService : ITimetableService
    {
        private readonly ITimetableRepository _timetableRepository;
        private readonly IMapper _mapper;

        public TimetableService(ITimetableRepository timetableRepository, IMapper mapper)
        {
            _mapper = mapper;
            _timetableRepository = timetableRepository;

        }

        public async Task<LessonDto> AddLesson(AddLessonDto model, CancellationToken cancellationToken = default)
        {
            if (model is null)
            {
                throw new ArgumentNullException();
            }
            var lesson = _mapper.Map<Lesson>(model);
            await _timetableRepository.AddAsync(lesson, cancellationToken);
            return _mapper.Map<LessonDto>(lesson);
        }

        public async Task<LessonDto> DeleteLesson(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException();
            }
            var returnModel = await _timetableRepository.GetByIdAsync(id);
            if (returnModel is null)
            {
                throw new ArgumentNullException();
            }

            await _timetableRepository.RemoveAsync(id, cancellationToken);
            var history = await _timetableRepository.GetByIdAsync(id);

            return !(history is null) ? _mapper.Map<LessonDto>(history) : throw new ArgumentNullException();
        }

        public async Task<LessonDto> DeleteTimetable(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException();
            }

            var lesson = await _timetableRepository.RemoveAsync(id, cancellationToken);
            return !(lesson is null) ? _mapper.Map<LessonDto>(lesson) : throw new ArgumentNullException();
        }

        public async Task<IEnumerable<LessonDto>> GetFilteredTimetable(LessonFilter lessonFilter, CancellationToken cancellationToken = default)
        {
            if (lessonFilter is null)
            {
                throw new ArgumentNullException();
            }
            var lessons = await _timetableRepository.GetFilteredAsync(lessonFilter, cancellationToken);
            return lessons is null ? throw new ArgumentNullException() : _mapper.Map<List<LessonDto>>(lessons);
        }

        public async Task<LessonDto> GetLessonById(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException();
            }
            var lesson = await _timetableRepository.GetByIdAsync(id, cancellationToken);
            return !(lesson is null) ? _mapper.Map<LessonDto>(lesson) : throw new ArgumentNullException();
        }

        public async Task<IEnumerable<LessonDto>> GetTimetable(CancellationToken cancellationToken = default)
        {
            var timetable = await _timetableRepository.GetAllAsync(cancellationToken);
            return timetable is null ? throw new ArgumentNullException() : _mapper.Map<List<LessonDto>>(timetable);
        }

       
    }
}
