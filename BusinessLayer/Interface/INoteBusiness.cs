using CommonLayer.Models;
using RepoLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface INoteBusiness
    {
        public NoteEntity CreateNote(NoteModel noteEntity, long userId);
        public List<NoteEntity> GetAllNotes(long userId);
        public List<NoteEntity> GetNoteById(long id);
        public string UpdateNote(long id, string takeNote);
        public long DeleteNote(long id);
    }
}
