﻿
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MSNote.Models;

namespace MSNote.Services
{
    public class NotesService
    {
        private readonly IMongoCollection<Note> _notesCollection;

        public NotesService(
            IOptions<BookNoteDatabaseSettings> bookNotesDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookNotesDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
           bookNotesDatabaseSettings.Value.DatabaseName);

            _notesCollection = mongoDatabase.GetCollection<Note>(
                bookNotesDatabaseSettings.Value.NotesCollectionName);
        }
        public async Task<List<Note>> GetAsync() =>
        await _notesCollection.Find(_ => true).ToListAsync();

        
        public async Task<List<Note>> GetByPatIdAsync(int patId) =>
          await _notesCollection.Find(x => x.PatId == patId).ToListAsync();

        public async Task CreateAsync(Note newNote) =>
            await _notesCollection.InsertOneAsync(newNote);

        public async Task UpdateAsync(string id, Note updatedNote) =>
            await _notesCollection.ReplaceOneAsync(x => x.Id == (id), updatedNote);

        public async Task RemoveAsync(string id) =>
            await _notesCollection.DeleteOneAsync(x => x.Id == (id));
    }
}
