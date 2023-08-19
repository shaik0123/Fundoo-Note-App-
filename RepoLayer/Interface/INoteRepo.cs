using CommonLayer.Models;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepoLayer.Interface
{
    public interface INoteRepo
    {
        public NoteEntity CreateNote(NoteModel noteEntity, long userId);
        public List<NoteEntity> GetAllNotes(long userId);
        public List<NoteEntity> GetNoteById(long id);
        public string UpdateNote(long id, string takeNote);
        public long DeleteNote(long id);
    }
}
