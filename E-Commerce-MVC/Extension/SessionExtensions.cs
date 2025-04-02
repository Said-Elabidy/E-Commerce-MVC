using System.Text.Json;

namespace E_Commerce_MVC.Extension
{
    public static class SessionExtensions
    {
        // Serialize and store an object in the session
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Deserialize and retrieve an object from the session
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
