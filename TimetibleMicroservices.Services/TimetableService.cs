using AutoMapper;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public async Task<int> CreateTimetableFromFile(IFormFile body, CancellationToken cancellationToken = default)
        {
            if (body == null || body.Length == 0)
                throw new ArgumentNullException();

            string fileExtension = Path.GetExtension(body.FileName);
            if (fileExtension != ".xls" && fileExtension != ".xlsx")
                throw new ArgumentException();

            int count = 0;
           await _timetableRepository.DeleteAllLessons(cancellationToken);
            using (var memoryStream = new MemoryStream())
            {
                await body.CopyToAsync(memoryStream);
                using (var document = SpreadsheetDocument.Open(memoryStream, true))
                {
                    //create the object for workbook part  
                    WorkbookPart wbPart = document.WorkbookPart;
                    //statement to get the count of the worksheet  
                    int worksheetcount = document.WorkbookPart.Workbook.Sheets.Count();
                    //statement to get the sheet object  
                    Sheet mysheet = (Sheet)document.WorkbookPart.Workbook.Sheets.ChildElements.GetItem(0);
                    //statement to get the worksheet object by using the sheet id  
                    Worksheet Worksheet = ((WorksheetPart)wbPart.GetPartById(mysheet.Id)).Worksheet;
                    //statement to get the sheetdata which contains the rows and cell in table  
                    IEnumerable<Row> Rows = Worksheet.GetFirstChild<SheetData>().Descendants<Row>();
                    //Loop through the Worksheet rows
                    foreach (var row in Rows)
                    {
                        if (row.RowIndex.Value != 0)
                        {
                            //var qq = Program.GetSharedStringItemById(wbPart ,0);
                            var lesson = new Lesson();
                            int idx = 1; int idy = 0;
                            foreach (Cell cell in row.Descendants<Cell>())
                            {

                                var val = TimetableService.GetValue(document, cell);
                                if (val == "Неделя")
                                    break;
                                if (idx < 8)
                                {
                                    if (idx == 1)
                                    {
                                        int x;
                                        Int32.TryParse(val, out x);
                                        lesson.NumberOfWeek = x;
                                    }
                                    if (idx == 2)
                                    {
                                        lesson.DayOfWeek = val;
                                    }

                                    if (idx == 3)
                                    {
                                        int x;
                                        Int32.TryParse(val, out x);
                                        lesson.DayInWeekNumber = x;
                                    }
                                    if (idx == 4)
                                    {
                                        int day, month, year;
                                        string[] z = val.Split(' ');
                                        Int32.TryParse(z[0], out day);
                                        if (z[2].Length == 4)
                                            Int32.TryParse(z[2], out year);
                                        else
                                        {
                                            Int32.TryParse(z[2].Substring(0, 3), out year);
                                        }
                                        switch (z[1])
                                        {
                                            case "СЕНТЯБРЯ":
                                                month = 09; break;

                                            case "ОКТЯБРЯ":
                                                month = 10; break;

                                            case "НОЯБРЯ":
                                                month = 11; break;

                                            case "ДЕКАБРЯ":
                                                month = 12; break;

                                            case "ЯНВАРЯ":
                                                month = 1; break;

                                            case "ФЕВРАЛЯ":
                                                month = 2; break;

                                            case "МАРТ":
                                                month = 3; break;

                                            case "АПРЕЛЬ":
                                                month = 4; break;

                                            case "МАЙ":
                                                month = 5; break;

                                            case "ИЮНЬ":
                                                month = 6; break;

                                            case "ИЮЛЬ":
                                                month = 7; break;

                                            case "АВГУСТА":
                                                month = 8; break;
                                            default:
                                                month = 0; break;

                                        }
                                        DateTime dateTime = new DateTime(year, month, day);
                                        lesson.LessonDate = dateTime;
                                    }
                                    if (idx == 5)
                                    {
                                        int x;
                                        Int32.TryParse(val, out x);
                                        lesson.LessonInDayNumber = x;
                                    }
                                    if (idx == 7)
                                    {
                                        int x;
                                        Int32.TryParse(val, out x);
                                        if (x != 4)
                                            break;
                                    }
                                }
                                else
                                {

                                    idy++;
                                    if (idy == 1)
                                    {
                                        lesson.GroupNumber = val;
                                    }
                                    if (idy == 2)
                                    {
                                        lesson.DisciplineName = val;
                                    }

                                    if (idy == 3)
                                    {
                                        int x = 0;
                                        if (val != "" && val != "ОХРАНА" && val != "ПРАЗДНИК")
                                        {
                                            string[] z = val.Split(' ');
                                            lesson.LessonType = z[0];
                                            Int32.TryParse(z[z.Length - 1], out x);
                                            lesson.LessonNumber = x;
                                        }
                                    }
                                    if (idy == 4)
                                    {
                                        lesson.LecturalName = val;
                                    }
                                    if (idy == 5)
                                    {
                                        if (val != null)
                                            lesson.AuditoreNumber = val;
                                        else
                                            lesson.AuditoreNumber = "";
                                    }
                                    if (idy == 7)
                                    {
                                        idy = 0;
                                        //await sendLessonToAPIAsync(lesson);
                                        if (lesson.GroupNumber != "431А" && lesson.GroupNumber != "431Б" &&
                                            lesson.GroupNumber != "441А" && lesson.GroupNumber != "441Б" &&
                                            lesson.GroupNumber != "451А" && lesson.GroupNumber != "451Б")
                                        {
                                            Console.WriteLine(lesson.ToString());
                                            if (lesson.LecturalName != "" && lesson.DisciplineName != "")
                                                try
                                                {
                                                   await _timetableRepository.AddAsync(lesson);                                                   
                                                }
                                                catch (Exception e)
                                                {
                                                    Console.WriteLine(e.Message);
                                                }

                                        }
                                    }
                                }
                                idx++;
                            }
                            count = idx;
                        }
                    }
                }
            }
            return count == 0 ? throw new ArgumentNullException() : count;
        }

        private static string GetValue(SpreadsheetDocument doc, Cell cell)
        {
            string value = cell.CellValue.InnerText;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value)).InnerText;
            }
            return value;
        }
        
    }
}
