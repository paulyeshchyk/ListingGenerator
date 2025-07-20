namespace ListingGenerator;

public static class ArgumentsConverter
{
    private static List<string> HelpStrings = new List<string>()
    {
        "Необходимо указать -srcPath и -dstFileName.",
        "Пример: ListingGenerator.exe -srcPath C:\\projects\\ListingGenerator -dstFileName ListingGenerator_Complete.cs -extList cs -exclList *AssemblyAttributes*;*GlobalUsings*;*AssemblyInfo*;*Resources.Designer.*",
    };

    public static ParsedArguments ParseArguments(string[] args)
    {
        var dict = new Dictionary<string, string>();
        for(int i = 0; i < args.Length - 1; i++)
        {
            if(args[i].StartsWith("-") && !args[i + 1].StartsWith("-"))
                dict[args[i]] = args[i + 1];
        }

        if(!dict.ContainsKey("-srcPath") || !dict.ContainsKey("-dstFileName"))
        {
            throw new Exception(string.Join("\\r\\\n", HelpStrings));
        }

        string srcPath = dict["-srcPath"];
        string dstFileName = dict["-dstFileName"];

        List<string>? extList = null;
        if(dict.TryGetValue("-extList", out var rawExtList))
        {
            extList = rawExtList
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(ext => ext.StartsWith('.') ? ext.ToLower() : $".{ext.ToLower()}")
                .ToList();
        }

        List<string>? exclList = null;
        if(dict.TryGetValue("-exclList", out var rawExclList))
        {
            exclList = rawExclList.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList();
        }


        return new ParsedArguments(srcPath, dstFileName, extList, exclList);
    }
}

public record ParsedArguments
{
    public string SrcPath;
    public string DstFileName;
    public List<string>? ExtList;
    public List<string>? ExclList;

    public ParsedArguments(string srcPath, string dstFileName, List<string>? extList, List<string>? exclList)
    {
        SrcPath = srcPath;
        DstFileName = dstFileName;
        ExtList = extList;
        ExclList = exclList;
    }
}