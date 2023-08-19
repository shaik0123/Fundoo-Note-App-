using BusinessLayer.Interface;
using CommonLayer.Models;
using RepoLayer.Entity;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class NoteBusiness : INoteBusiness
    {
        private readonly INoteRepo _noteRepo;
        public NoteBusiness(INoteRepo noteRepo)
        {
            _noteRepo = noteRepo;
        }
        public NoteEntity CreateNote(NoteModel noteEntity, long userId)
        {
            try
            {
                return _noteRepo.CreateNote(noteEntity, userId);

            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<NoteEntity> GetAllNotes(long userId)
        {
            try
            {
                return _noteRepo.GetAllNotes(userId);

            }
            catch (Exception)
            {

                throw;
            }
        }
        public List<NoteEntity> GetNoteById(long id)
        {
            try
            {
                return _noteRepo.GetNoteById(id);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string UpdateNote(long id, string takeNote)
        {
            try
            {
                return _noteRepo.UpdateNote(id, takeNote);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public long DeleteNote(long id)
        {
            try
            {
                return _noteRepo.DeleteNote(id);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
