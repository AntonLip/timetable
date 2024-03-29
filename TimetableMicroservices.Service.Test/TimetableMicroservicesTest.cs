﻿using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TimetibleMicroservices.Models;
using TimetibleMicroservices.Models.DBModels;
using TimetibleMicroservices.Models.DTOModels;
using TimetibleMicroservices.Models.Interfaces;
using TimetibleMicroservices.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace TimetableMicroservices.Service.Test
{
    public class TimetableMicroservicesTest
    {
        private Mock<ITimetableRepository> _timetableRepository;
        private ITimetableRepository _mockTimetableRepository;
        private TimetableService _timetableService;
        private IMapper _mapper;
        private List<LessonDto> _fakeTimetable;

        private static Random _random;

        public TimetableMicroservicesTest()
        {
            GenerateData();
        }

        [Fact]
        public async Task<IEnumerable<LessonDto>> Lesson_FindAllLessonAsync_Return_ListLessonDTO()
        {
            CreateDefaultDeviceServiceInstance();
            var timetable = await _timetableService.GetTimetable(default(CancellationToken));

            Assert.True(Equals(10, _fakeTimetable.Count()));
            return timetable;
        }
        
        [Fact]
        public async Task<LessonDto> Lesson_AddLesson_Return_LessonDTO()
        {
            CreateDefaultDeviceServiceInstance();
            var addLesson = _mapper.Map<AddLessonDto>(_fakeTimetable[0]);
            var lessonDto = await _timetableService.AddLesson(addLesson, default(CancellationToken));

            Assert.NotNull(lessonDto);
            return lessonDto;
        }

        [Fact]
        public async Task<LessonDto> Lesson_DeleteLesson_Return_LessonDTO()
        {
            CreateDefaultDeviceServiceInstance();
            var lessonDto = await _timetableService.DeleteLesson(_fakeTimetable[0].Id, default(CancellationToken));
            Assert.NotNull(lessonDto);
            Assert.True(Equals(lessonDto.Id, _fakeTimetable[0].Id));
            return lessonDto;
        }

        [Fact]
        public async Task<LessonDto> Lesson_UpdateLesson_Return_LessonDTO()
        {
            CreateDefaultDeviceServiceInstance();
            var lessonDto = await _timetableService.UpdateLesson(_fakeTimetable[0].Id, _fakeTimetable[0], default(CancellationToken));
            Assert.NotNull(lessonDto);
            Assert.True(Equals(lessonDto.Id, _fakeTimetable[0].Id));
            return lessonDto;
        }

        [Fact]
        public async Task<LessonDto> Lesson_GetLessonById_Return_LessonDTO()
        {
            CreateDefaultDeviceServiceInstance();
            var lessonDto = await _timetableService.GetLessonById(_fakeTimetable[0].Id, default(CancellationToken));
            Assert.NotNull(lessonDto);
            Assert.True(Equals(lessonDto.LecturalName, _fakeTimetable[0].LecturalName));
            return lessonDto;
        }

        [Fact]
        public async Task<IEnumerable<IEnumerable<LessonDto>>> Lesson_GetFilteredTimetable_Return_IEnumerableIEnumerableLessonDTO()
        {
            CreateDefaultDeviceServiceInstance();
            var lessonDto = await _timetableService.GetFilteredTimetable(new LessonFilter(), default(CancellationToken));
            Assert.NotNull(lessonDto);
            Assert.True(Equals(lessonDto.Count(), 1));
            return lessonDto;
        }
        
        [Fact]
        public async Task<FileDto> Lesson_GetTimetableInDocxAsync_Return_IEnumerableIEnumerableLessonDTO()
        {
            CreateDefaultDeviceServiceInstance();
            var fileDto = await _timetableService.GetTimetableInDocxAsync(new LessonFilter(), default(CancellationToken));
            Assert.NotNull(fileDto);
            return fileDto;
        }

        private void GenerateData()
        {
            _random = new Random();       

            for(int i = 0; i < 10; i++)
            {
                _fakeTimetable.Add(
                    new LessonDto
                    {
                        AuditoreNumber = RandomString(10),
                        DisciplineName = RandomString(10),
                        GroupNumber = RandomString(10),
                        Id = Guid.NewGuid(),
                        LecturalName = RandomString(10),
                        LessonDate = DateTime.Now,
                        LessonInDayNumber = _random.Next(1, 5),
                        LessonNumber = _random.Next(1, 30),
                        LessonType = RandomString(10)
                    });
            }

        }
        private void CreateDefaultDeviceServiceInstance()
        {
            
            var services = new ServiceCollection();
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var myProfile = new MapperProfile();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(myProfile);
                cfg.ConstructServicesUsing(serviceProvider.GetService);
            });
            _mapper = new Mapper(configuration);

            _timetableRepository = new Mock<ITimetableRepository>();
            _timetableRepository.Setup(s => s.GetAllAsync(default(CancellationToken))).ReturnsAsync(_mapper.Map<List<Lesson>>(_fakeTimetable));
            _timetableRepository.Setup(s => s.GetByIdAsync(It.IsAny<Guid>(), default(CancellationToken))).ReturnsAsync(_mapper.Map<Lesson>(_fakeTimetable[1]));
            _timetableRepository.Setup(s => s.RemoveAsync(It.IsAny<Guid>(), default(CancellationToken))).ReturnsAsync(_mapper.Map<Lesson>(_fakeTimetable[1]));
            _timetableRepository.Setup(s => s.AddAsync(It.IsAny<Lesson>(), default(CancellationToken)));
            _timetableRepository.Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Lesson>(), default(CancellationToken)));

            _timetableRepository.Setup(s => s.DeleteAllLessons(default(CancellationToken)));
            _timetableRepository.Setup(s => s.GetFilteredAsync(It.IsAny<LessonFilter>(), default(CancellationToken)));
            _timetableRepository.Setup(s => s.InsertManyLesson(It.IsAny<List<Lesson>>(), default(CancellationToken)));
            _mockTimetableRepository = _timetableRepository.Object;
            _timetableService = new TimetableService(_mockTimetableRepository, _mapper);
        }
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
    }
}
