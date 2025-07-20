using System.Text.RegularExpressions;

namespace ListingGenerator;

public class FileCollector
{
    private readonly string _rootPath;
    private readonly HashSet<string>? _extensions;
    private readonly List<Regex>? _excludedPatterns;

    public FileCollector(string rootPath, List<string>? extensions, List<string>? excludedMasks)
    {
        _rootPath = Path.GetFullPath(rootPath);
        _extensions = extensions != null
            ? new HashSet<string>(extensions, StringComparer.OrdinalIgnoreCase)
            : null;

        _excludedPatterns = excludedMasks != null
            ? excludedMasks
                .Select(mask =>
                    new Regex(
                        "^" +
                        Regex.Escape(mask)
                            .Replace("\\*", ".*")
                            .Replace("\\?", ".")
                        + "$",
                        RegexOptions.IgnoreCase | RegexOptions.Compiled))
                .ToList()
            : null;
    }

    public IEnumerable<string> Collect()
    {
        if(!Directory.Exists(_rootPath))
            throw new Exception($"Папка не найдена: {_rootPath}");

        foreach(var file in Directory.EnumerateFiles(_rootPath, "*", SearchOption.AllDirectories))
        {
            // Фильтрация по расширениям
            if(_extensions != null &&
                !_extensions.Contains(Path.GetExtension(file).ToLowerInvariant()))
            {
                continue;
            }

            // Фильтрация по маскам исключения
            if(_excludedPatterns != null &&
                _excludedPatterns.Any(rx => rx.IsMatch(Path.GetFileName(file))))
            {
                continue;
            }

            yield return file;
        }
    }
}
