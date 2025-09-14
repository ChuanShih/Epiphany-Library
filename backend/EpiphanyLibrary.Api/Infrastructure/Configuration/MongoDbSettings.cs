namespace EpiphanyLibrary.Api.Infrastructure.Configuration;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string BookNotesCollectionName { get; set; } = "BookNotes";
    public string UsersCollectionName { get; set; } = "Users";
}
