
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

        public async Task UpdateAsync(string Id, Note updatedNote) =>
            await _notesCollection.ReplaceOneAsync(x => x.Id == (Id), updatedNote);

        public async Task RemoveAsync(string Id) =>
            await _notesCollection.DeleteOneAsync(x => x.Id == (Id));

        public async Task<Note> GetByIdAsync(string id) =>
             await _notesCollection.Find(note => note.Id == id).FirstOrDefaultAsync();

        internal void SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}

