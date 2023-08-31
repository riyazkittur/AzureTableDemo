// See https://aka.ms/new-console-template for more information
using Azure.Data.Tables;
using AzureTableDemo;

Console.WriteLine("Azure Tables");
/// Use storage Account name and Account Key
var storageAccountName = "riyazkittur";
var accountKey = Environment.GetEnvironmentVariable("STORAGE_ACCOUNT_KEY");
var credential = new TableSharedKeyCredential(storageAccountName, accountKey);
var tableName = "demotableriyazkittur";

var tableServiceClient = new TableServiceClient(new Uri($"https://{storageAccountName}.table.core.windows.net/{tableName}"), credential);
var tableClient = tableServiceClient.GetTableClient( tableName: tableName);
var result = await tableClient.CreateIfNotExistsAsync();

#region AddEntity
/// Using TableEntity
var entity = new TableEntity()
{
    PartitionKey = "DigitalOcean",
    RowKey = "1"
};
entity.Add("Name", "BlueWhales");

var addResponse = await tableClient.AddEntityAsync(entity);

/// Using Dictionary<string, object> 
var keyValues = new Dictionary<string, object>()
{
    {"PartitionKey" , "DigitalOcean" },
    {"RowKey" , "2" },
    {"Name" , "Titans" },
    {"ProjectId", Guid.NewGuid() }

};

var dictEntity = new TableEntity(keyValues);
await tableClient.AddEntityAsync(dictEntity);


var clubDetails = new ClubDetails()
{
    PartitionKey = "DigitalOcean",
    RowKey = "3",
    Name = "Super Kings",
};

await tableClient.AddEntityAsync(clubDetails);

#endregion

#region ReadEntity
/// Using GetEntity
var response = await tableClient.GetEntityAsync<ClubDetails>(partitionKey: "DigitalOcean", rowKey: "2", cancellationToken: default);
var clubTitans = response.Value;
Console.WriteLine("Name of club is {0}", clubTitans.Name);

/// Using LINQ Syntax
Console.WriteLine("Below are the club names in DigitalOcean");
var allclubs =  tableClient.Query<ClubDetails>(c => c.PartitionKey == "DigitalOcean");
foreach(var club in allclubs)
{
    Console.WriteLine(club.Name);
}
#endregion

await tableClient.UpdateEntityAsync(clubTitans with { Name = "World of Titans" }, clubTitans.ETag, cancellationToken: default);

await tableClient.DeleteEntityAsync(partitionKey: "DigitalOcean", rowKey: "1");