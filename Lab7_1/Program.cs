using System;
using System.Data;
using System.Data.OleDb;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace Lab7_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\РБТ\source\repos\Lab7\Lab7_1\7_1.accdb";
            string query = "SELECT ФИО, Факультет, Курс, Группа, [Средняя успеваемость] FROM Студенты";

            // Чтение данных из базы
            DataTable studentsTable = ReadDataFromAccess(connectionString, query);

            // Отображение данных в консоли
            DisplayDataInConsole(studentsTable);

            // Меню для выбора действия
            while (true)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1. Записать данные в Word");
                Console.WriteLine("2. Записать данные в Excel");
                Console.WriteLine("0. Выход");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        ExportToWord(studentsTable);
                        Console.WriteLine("Данные успешно записаны в Word.");
                        break;
                    case "2":
                        ExportToExcel(studentsTable);
                        Console.WriteLine("Данные успешно записаны в Excel.");
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }

        static DataTable ReadDataFromAccess(string connectionString, string query)
        {
            DataTable table = new DataTable();
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                    adapter.Fill(table);
                }
            }
            return table;
        }

        static void DisplayDataInConsole(DataTable table)
        {
            foreach (DataColumn column in table.Columns)
            {
                Console.Write($"{column.ColumnName}\t");
            }
            Console.WriteLine();

            foreach (DataRow row in table.Rows)
            {
                foreach (var item in row.ItemArray)
                {
                    Console.Write($"{item}\t");
                }
                Console.WriteLine();
            }
        }

        static void ExportToWord(DataTable table)
        {
            Word.Application wordApp = new Word.Application();
            Word.Document wordDoc = wordApp.Documents.Add();

            // Добавление таблицы в документ Word
            Word.Table wordTable = wordDoc.Tables.Add(wordDoc.Range(0, 0), table.Rows.Count + 1, table.Columns.Count);

            // Заполнение заголовков
            for (int i = 0; i < table.Columns.Count; i++)
            {
                wordTable.Cell(1, i + 1).Range.Text = table.Columns[i].ColumnName;
            }

            // Заполнение данных
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    wordTable.Cell(i + 2, j + 1).Range.Text = table.Rows[i][j].ToString();
                }
            }

            wordDoc.SaveAs2("Студенты.docx");
            wordDoc.Close();
            wordApp.Quit();
        }

        static void ExportToExcel(DataTable table)
        {
            Excel.Application excelApp = new Excel.Application();
            Excel.Workbook workbook = excelApp.Workbooks.Add();
            Excel.Worksheet worksheet = workbook.Sheets[1];

            // Заполнение заголовков
            for (int i = 0; i < table.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1] = table.Columns[i].ColumnName;
            }

            // Заполнение данных
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    worksheet.Cells[i + 2, j + 1] = table.Rows[i][j];
                }
            }

            workbook.SaveAs("Студенты.xlsx");
            workbook.Close();
            excelApp.Quit();
        }
    }
}