namespace ListingGenerator;

static class Program
{
    static void Main(string[] args)
    {
        try
        {
            var parsed = ArgumentsConverter.ParseArguments(args);

            var collector = new FileCollector(parsed.SrcPath, parsed.ExtList, parsed.ExclList);
            var builder = new ListingBuilder(parsed.DstFileName, parsed.SrcPath);

            foreach(var file in collector.Collect())
                builder.Append(file);

            builder.Save();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}
