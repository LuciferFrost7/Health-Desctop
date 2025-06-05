using System.IO;
using System.Text;

namespace HealthDesctop.source.Util
{
    // Системная утилита для работы с файлами
    public class FileWorker
    {
        // Функция для записи текста в файл с кодировкой UTF-8
        public static void WriteInFile(String filePath, String content)
        {
            using (var streamWriter = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                streamWriter.Write(content);
            }
        }

// Функция для чтения содержимого файла с кодировкой UTF-8
        public static String ReadFile(String filePath)
        {
            using (var streamReader = new StreamReader(filePath, Encoding.UTF8))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}