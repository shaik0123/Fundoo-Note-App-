using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepoLayer.Interface
{
    public interface INoteRepo
    {
        public NoteEntity CreateNote(NoteModel noteEntity, long userId);
        public List<NoteEntity> GetAllNotes(long userId);
        public List<NoteEntity> GetNoteById(long id);
        public string UpdateNote(long id, string takeNote);
        public long DeleteNote(long id);
        public string UpdateColour(long id, string colour, long userid);
        public Task<Tuple<int, string>> Image(long id, long usedId, IFormFile imageFile);

        public bool Archive(long id, long userId);
        public bool Pin(long Id, long userId);
        public bool Trash(long Id, long userId);
    }
}
