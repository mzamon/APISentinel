using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddSingleton<Store>();
var app = builder.Build();
app.MapOpenApi();
app.MapGet("/", () => Results.Ok(new { name = "API Sentinel", description = "API monitoring and testing dashboard backend", status = "ready" }));
app.MapGet("/health", () => Results.Ok(new { status = "healthy", utc = DateTime.UtcNow }));
app.MapGet("/api/endpointchecks", (Store store) => Results.Ok(store.All()));
app.MapGet("/api/endpointchecks/{id:int}", (int id, Store store) =>
    store.TryGet(id, out var item) ? Results.Ok(item) : Results.NotFound());
app.MapPost("/api/endpointchecks", ([FromBody] EndpointCheck item, Store store) =>
{
    if (item is null) return Results.BadRequest(new { error = "A request body is required." });
    var created = store.Add(item);
    return Results.Created($"/api/endpointchecks/{created.Id}", created);
});
app.MapPut("/api/endpointchecks/{id:int}", (int id, [FromBody] EndpointCheck item, Store store) =>
    store.Update(id, item) ? Results.NoContent() : Results.NotFound());
app.MapDelete("/api/endpointchecks/{id:int}", (int id, Store store) =>
    store.Delete(id) ? Results.NoContent() : Results.NotFound());
app.Run();

public sealed class Store
{
    private readonly ConcurrentDictionary<int, EndpointCheck> _items = new();
    private int _nextId;
    public IEnumerable<EndpointCheck> All() => _items.Values.OrderBy(x => x.Id);
    public bool TryGet(int id, out EndpointCheck item) => _items.TryGetValue(id, out item!);
    public EndpointCheck Add(EndpointCheck item)
    {
        var created = item with { Id = Interlocked.Increment(ref _nextId), CreatedAt = DateTime.UtcNow };
        _items[created.Id] = created;
        return created;
    }
    public bool Update(int id, EndpointCheck item)
    {
        if (!_items.ContainsKey(id)) return false;
        _items[id] = item with { Id = id, CreatedAt = _items[id].CreatedAt };
        return true;
    }
    public bool Delete(int id) => _items.TryRemove(id, out _);
}

public record EndpointCheck(int Id, string Url, int ExpectedStatus, bool IsEnabled, DateTime CreatedAt);
