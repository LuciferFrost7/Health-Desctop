namespace HealthDesctop.source.User
{
    public class DatabasePaths
    {
        private const String __usersDatabasePath = "database/users.json"; // Путь к Базе данных пользователей

        public static String UsersDatabasePath
        {
            get
            {
                return __usersDatabasePath;
            }
        }
        
        
        private const String __aimsDatabasePath = "database/aims.json"; // Путь к Базе данных Целей

        public static String AimsDatabasePath
        {
            get
            {
                return __aimsDatabasePath;
            }
        }
    }
}