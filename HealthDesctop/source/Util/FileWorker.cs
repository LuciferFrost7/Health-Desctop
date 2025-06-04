using System.IO;

namespace HealthDesctop.source.Util
{
    // Системная утилита для работы с файлами
    public class FileWorker
    {
        // Функция для записи текста в файл
        public static void WriteInFile(String filePath, String content)
        {
            using (var streamWriter = new StreamWriter(filePath))
            {
                streamWriter.Write(content);
            }
        }

        // Функция для чтения содержимого файла
        public static String ReadFile(String filePath)
        {
            String content = "";
            using (var streamReader = new StreamReader(filePath))
            {
                content = streamReader.ReadToEnd();
            }
            return content;
        }
    }
}