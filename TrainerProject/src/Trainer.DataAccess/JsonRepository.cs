using System.Text.Json;
using System.Text.Json.Serialization;

namespace Trainer.DataAccess;

public class JsonRepository<T> : IRepository<T> where T : class
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _options;

    public JsonRepository(string fileName)
    {
        var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        _filePath = Path.Combine(directory, fileName);

        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNameCaseInsensitive = true
        };
    }

    public IEnumerable<T> GetAll()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                return Enumerable.Empty<T>();
            }

            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<T>>(json, _options) ?? new List<T>();
        }
        catch (JsonException ex)
        {
            throw new InvalidDataException($"Помилка формату файлу {_filePath}: {ex.Message}");
        }
        catch (IOException ex)
        {
            throw new Exception($"Помилка доступу до файлу: {ex.Message}");
        }
    }

    public void SaveAll(IEnumerable<T> entities)
    {
        try
        {
            string json = JsonSerializer.Serialize(entities, _options);
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            throw new Exception($"Не вдалося зберегти дані у {_filePath}: {ex.Message}");
        }
    }
}