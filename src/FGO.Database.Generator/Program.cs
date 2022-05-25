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

    JsonSerializerOptions coreJsonOptions = new()
    {
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };

    JsonSerializerOptions nonCoreJsonOptions = new(coreJsonOptions);

    async Task LoadCoreAsync<TEntity>(DbSet<TEntity> dbSet, Func<TEntity, string?> getName) where TEntity : class
    {
        nonCoreJsonOptions.Converters.Add(new JsonDbEntityConverter<TEntity>(dbSet));
        await LoadAsync(dbSet, getName, coreJsonOptions);
    }

    async Task LoadAsync<TEntity>(DbSet<TEntity> dbSet, Func<TEntity, string?> getName, JsonSerializerOptions? jsonOptions = null) where TEntity : class
    {
        string path = Path.Combine(dbUri.OriginalString, $"{typeof(TEntity).Name}.json");

        using Stream stream = dbUri.Scheme switch
        {
            "file" => File.OpenRead(path),
            "http" or "https" => await httpClient.GetStreamAsync(path),
            _ => throw new ArgumentException("Unsupported scheme")
        };

        var records = JsonSerializer.DeserializeAsyncEnumerable<TEntity>(stream, jsonOptions ?? nonCoreJsonOptions);
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

        // Core Data (needs to be added first)
        await LoadCoreAsync(db.Rarities, rarity => rarity.Id);
        await LoadCoreAsync(db.Currencies, currency => currency.Id);
        await LoadCoreAsync(db.Seasons, season => $"{season.Id} - {season.Name?.English}");
        await LoadCoreAsync(db.CustomisationItemTypes, itemType => itemType.Id);
        await LoadCoreAsync(db.CustomisationItemSources, itemSource => itemSource.Id);

        // Non-Core Data
        await LoadAsync(db.CustomisationItems, item => $"{item.Id} - {item.Name?.English} ({item.ItemType?.Id})");

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