using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using RepoLayer.Entity;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        public string UpdateColour(long id, string colour, long userid)
        {
            try
            {
                return _noteRepo.UpdateColour(id, colour, userid);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<Tuple<int, string>> Image(long id, long usedId, IFormFile imageFile)
        {
            try
            {

                Tuple<int, string> uploadResult = await _noteRepo.Image(id, usedId, imageFile);
                return uploadResult;


            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool Archive(long id, long userId)
        {
            try
            {
                return _noteRepo.Archive(id, userId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool Pin(long id, long userId)
        {
            try
            {
                return _noteRepo.Pin(id, userId);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool Trash(long id, long userId)
        {
            try
            {
                return _noteRepo.Trash(id, userId);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
