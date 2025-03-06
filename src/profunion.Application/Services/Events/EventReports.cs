using OfficeOpenXml;
using profunion.Domain.Models.EventModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace profunion.Applications.Services.Events
{
    public class EventReports
    {
        public void GenerateEventsStatusReport(string filePath, List<Event> events)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            // Создаем новый Excel пакет
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                // Добавляем новый лист
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Events Status");

                // Заголовки столбцов
                worksheet.Cells[1, 1].Value = "Название мероприятия";
                worksheet.Cells[1, 2].Value = "Дата и время проведения";
                worksheet.Cells[1, 3].Value = "Общее количество мест";
                worksheet.Cells[1, 4].Value = "Количество зарегистрированных участников";
                worksheet.Cells[1, 5].Value = "Количество оставшихся мест";

                // Заполняем данными
                int row = 2;
                foreach (var ev in events)
                {
                    worksheet.Cells[row, 1].Value = ev.title;
                    worksheet.Cells[row, 2].Value = ev.eventDate.ToString("dd.MM.yyyy HH:mm");
                    worksheet.Cells[row, 3].Value = ev.totalPlaces;
                    worksheet.Cells[row, 4].Formula = $"=C{row}-D{row}";
                    worksheet.Cells[row, 5].Value = ev.Places;
                    row++;
                }

                // Автонастройка ширины столбцов
                worksheet.Cells.AutoFitColumns();

                // Сохраняем файл Excel
                FileInfo excelFile = new FileInfo(filePath);
                excelPackage.SaveAs(excelFile);
            }
        }
    }
}
