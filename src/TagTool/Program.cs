﻿using TagTool.Commands.TagOperations;
using TagTool.DbContext;
using TagTool.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();
app.Logger.LogInformation("Application created...");

app.MapPost("/TagFolder", TagFolder);
app.MapPost("/UntagFolder", UntagFolder);
app.MapDelete("/DeleteTag", DeleteTag);
app.MapPost("/AddTag", AddTag);
app.MapGet("/PrintTags", PrintTags);

app.Logger.LogInformation("Launching application...");
await app.RunAsync();

async Task TagFolder(string path, string tagName)
{
    var tag = new Tag {Name = tagName};
    var command = new TagFolderCommand(path, tag);
    await command.Execute();
}

async Task UntagFolder(string path, string tagName)
{
    var tag = new Tag {Name = tagName};
    var command = new UntagFolderCommand(path, tag);
    await command.Execute();
}

async Task DeleteTag(string tagName)
{
    var command = new DeleteTagCommand(tagName);
    await command.Execute();
}

void AddTag()
{
    using var db = new TagContext();
    Console.WriteLine("Inserting a new tag");

    db.Add(new Tag {Name = $"Tag{Random.Shared.Next()}", HierarchyValue = 1f});
    db.SaveChanges();
}

Tag[] PrintTags()
{
    using var db = new TagContext();

    return db.Tags.ToArray();
}
