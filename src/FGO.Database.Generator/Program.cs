using FGO.Database;
using FGO.Database.Generator;
using Microsoft.EntityFrameworkCore;
using System.CommandLine;
using System.Text.Json;

string title = Console.Title = "FallGuys.org SQL Database Generator";
Console.WriteLine(title);

var pathArg = new Argument<Uri>("database path or URL", getDefaultValue: () => new Uri("https://raw.githubusercontent.com/FallGuys-org/TheDatabase/main/Database/"));
var outputTypeOption = new Option<OutputType>("--outputType", getDefaultValue: () => OutputType.SQLite);
var outputPathOption = new Option<Uri>("--outputPath", getDefaultValue: () => new Uri("fallguys.db", UriKind.Relative));

var rootCommand = new RootCommand { pathArg, outputTypeOption, outputPathOption };
rootCommand.Description = "Generates a SQL database from https://github.com/FallGuys-org/TheDatabase";

rootCommand.SetHandler<Uri, OutputType, Uri>(GenerateAsync, pathArg, outputTypeOption, outputPathOption);

await rootCommand.InvokeAsync(args);

static async Task GenerateAsync(Uri dbUri, OutputType outputType, Uri outputPath)
{
    FallGuysDbContext db = outputType switch
    {
        OutputType.SQLite => new SqliteFallGuysDbContext(outputPath),
        _ => throw new ArgumentException("Unsupported output type")
    };

    Console.WriteLine($"Source: {dbUri}");
    Console.WriteLine($"Output: {outputType} {outputPath}");

    HttpClient httpClient = new();

    JsonSerializerOptions jsonOptions = new()
    {
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };

    async Task LoadAsync<TEntity>(DbSet<TEntity> dbSet, Func<TEntity, string?> getName) where TEntity : class
    {
        string path = Path.Combine(dbUri.OriginalString, $"{typeof(TEntity).Name}.json");

        using Stream stream = dbUri.Scheme switch
        {
            "file" => File.OpenRead(path),
            "http" or "https" => await httpClient.GetStreamAsync(path),
            _ => throw new ArgumentException("Unsupported scheme")
        };

        var records = JsonSerializer.DeserializeAsyncEnumerable<TEntity>(stream, jsonOptions);
        await foreach (TEntity? record in records)
        {
            if (record is not null)
            {
                Console.WriteLine($"{typeof(TEntity).Name}: {getName(record) ?? "{null}"}");
                dbSet.Add(record);
            }
        }
    }

    try
    {
        while (!db.Database.EnsureCreated() && db.Database.EnsureDeleted()) ; // Always output a fresh db

        await LoadAsync(db.Rarities, rarity => rarity.Id);
        await LoadAsync(db.Currencies, currency => currency.Id);
        await LoadAsync(db.Seasons, season => $"{season.Id} - {season.Name?.English}");
        await LoadAsync(db.CustomisationItemTypes, itemType => itemType.Id);
        await LoadAsync(db.CustomisationItemSources, itemSource => itemSource.Id);

        db.SaveChanges();

        Console.WriteLine("Database saved!");
    }
    finally
    {
        db.Dispose();
        httpClient.Dispose();
    }
}

enum OutputType
{
    SQLite
}