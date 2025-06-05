using System.Text.Json;
using System.Text.Json.Serialization;
using HealthDesctop.source.Util;
using HealthDesctop.source.User.Converter;

namespace HealthDesctop.source.User
{
    public class Aim
    {
        public DateOnly StartDate { get; set; }
        public DateOnly FinishDate { get; set; }

        public double StartWeight { get; set; }
        public double FinishWeight { get; set; }

        public bool IsFinished { get; set; }

        public int UserId { get; set; } // Привязка к пользователю

        public Aim() { }

        // Метод для добавления цели в БД
        public static void AddAim(Aim newAim)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            options.Converters.Add(new DateOnlyJsonConverter());

            string dbPath = DatabasePaths.AimsDatabasePath;

            List<Aim> aims;
            string dbContent = FileWorker.ReadFile(dbPath);

            if (!string.IsNullOrWhiteSpace(dbContent))
            {
                aims = JsonSerializer.Deserialize<List<Aim>>(dbContent, options);
            }
            else
            {
                aims = new List<Aim>();
            }

            aims.Add(newAim);
            string updatedJson = JsonSerializer.Serialize(aims, options);
            FileWorker.WriteInFile(dbPath, updatedJson);
        }

        // Метод для получения всех целей
        public static List<Aim> GetAllAims()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new DateOnlyJsonConverter());

            string dbPath = DatabasePaths.AimsDatabasePath;
            string dbContent = FileWorker.ReadFile(dbPath);

            if (string.IsNullOrWhiteSpace(dbContent))
                return new List<Aim>();

            return JsonSerializer.Deserialize<List<Aim>>(dbContent, options) ?? new List<Aim>();
        }
    }
}
