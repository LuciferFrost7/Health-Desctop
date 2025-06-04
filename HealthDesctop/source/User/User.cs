using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using HealthDesctop.source.User.Converter; // Подключаем конвертер для OnlyDate формата
using HealthDesctop.source.Util; // Подключаем Утилиту для работы с файлами

namespace HealthDesctop.source.User
{
    public class User
    {
        public Int32 Id { get; set; } // Айди пользователя
        public String FirstName { get; set; } // Имя
        public String LastName { get; set; } // Фамилия
        public Double Weight { get; set; } // Вес в килограммах
        public Double Height { get; set; } // Рост в метрах 1.82
        public Int32 Age { get; set; } // Возраст в годах
        public DateOnly BirthDate { get; set; } // Дата рождения

        // Пустой конструктор для работы с БД
        public User()
        {
        }
        
        // Функция для регистрации нового пользователя в БД
        public static void RegisterNewUser(User newUser)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            options.Converters.Add(new DateOnlyJsonConverter());

            string dbPath = DatabasePaths.UsersDatabasePath;

            List<User> users;
            
            string dbContent = FileWorker.ReadFile(dbPath);
            
            if (!string.IsNullOrWhiteSpace(dbContent))
            {
                users = JsonSerializer.Deserialize<List<User>>(dbContent, options);
            }
            else
            {
                users = new List<User>();
            }
            
            int newId = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
            newUser.Id = newId;
            
            users.Add(newUser);
            
            string updatedJson = JsonSerializer.Serialize(users, options);
            FileWorker.WriteInFile(dbPath, updatedJson);
        }
    }
}