using System;
using System.Data.SqlClient;

namespace Lab7_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Строка подключения к SQL Server
            string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=master;Trusted_Connection=True;";

            // SQL-запросы для создания базы данных, таблицы и добавления тестовых данных
            string createDatabaseQuery = @"
                IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ShopDB')
                    BEGIN
                        CREATE DATABASE ShopDB;
                    END;
                ";

            string createTableQuery = @"
            USE ShopDB;

            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Магазины')
                BEGIN
                    CREATE TABLE Магазины (
                    ID INT IDENTITY(1,1) PRIMARY KEY,
                    Наименование NVARCHAR(100) NOT NULL,
                    [Количество сотрудников] INT NOT NULL,
                    [Количество товаров] INT NOT NULL,
                    [Адрес бутика] NVARCHAR(200) NOT NULL
                    );
                END;
            ";

            string insertTestDataQuery = @"
            USE ShopDB;

            INSERT INTO Магазины (Наименование, [Количество сотрудников], [Количество товаров], [Адрес бутика])
                VALUES
                    ('electronic', 15, 250, 'Lenina st., 10'),
                    ('clothaa', 8, 100, 'Pushkina st., 25'),
                    ('sport+', 12, 150, 'Soviet st., 5'),
                    ('PROfoodMarket', 20, 500, 'Space st., 33');
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Создание базы данных
                    using (SqlCommand command = new SqlCommand(createDatabaseQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("База данных успешно создана или уже существует.");
                    }

                    // Создание таблицы
                    using (SqlCommand command = new SqlCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Таблица 'Магазины' успешно создана или уже существует.");
                    }

                    // Добавление тестовых данных
                    using (SqlCommand command = new SqlCommand(insertTestDataQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Тестовые данные успешно добавлены.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}
