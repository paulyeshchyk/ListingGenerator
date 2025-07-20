namespace ListingGenerator;

// Класс для построения и сохранения итогового листинга
public class ListingBuilder
{
    private readonly string _outputFile;
    private readonly string _rootPath;
    private readonly string _rootFolderName;
    private readonly List<Entry> _entries = new();

    public ListingBuilder(string outputFile, string srcPath)
    {
        _outputFile = outputFile;
        _rootPath = Path.GetFullPath(srcPath);
        _rootFolderName = new DirectoryInfo(_rootPath).Name;
    }

    public void Append(string file)
    {
        // Получаем относительный путь от корневой папки
        string rawRelative = Path.GetRelativePath(_rootPath, file).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

        // Всегда начинаем с имени корневой папки
        string relativePath = $"{_rootFolderName}{Path.DirectorySeparatorChar}{rawRelative}";

        string content;
        try
        {
            content = File.ReadAllText(file);
        }
        catch
        {
            // Пропускаем файлы, которые не удалось прочесть
            return;
        }

        _entries.Add((relativePath, content));
    }

    public void Save()
    {
        using var writer = new StreamWriter(_outputFile, false);
        foreach(var entry in _entries)
        {
            writer.WriteLine($"// === {entry.RelativePath} ===");
            writer.WriteLine(entry.Content);
            writer.WriteLine();
        }
        Console.WriteLine($"Листинг записан в {_outputFile}");
    }
}

internal record struct Entry(string RelativePath, string Content)
{
    public static implicit operator (string RelativePath, string Content)(Entry value)
    {
        return (value.RelativePath, value.Content);
    }

    public static implicit operator Entry((string RelativePath, string Content) value)
    {
        return new Entry(value.RelativePath, value.Content);
    }
}