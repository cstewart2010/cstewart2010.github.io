using System.Text.Json;

namespace JackpotLibrary;

public class JackpotService
{
    private static readonly string AssemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
    private static readonly string WorkingDirectory = Path.GetDirectoryName(AssemblyLocation)!;
    
    public ICollection<JackpotRecord> GetJackpotRecords()
    {
        var file = Path.Combine(WorkingDirectory, "data.json");
        var json = File.ReadAllText(file);
        var records = JsonSerializer.Deserialize<List<JackpotRecord>>(json)!;
        return records;
    }
}