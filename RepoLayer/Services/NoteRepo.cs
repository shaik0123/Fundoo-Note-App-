using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using RepoLayer.Context;
using RepoLayer.Entity;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepoLayer.Services
{
    public class NoteRepo : INoteRepo
    {
        private readonly FundooContext _fundooContext;
        private readonly IConfiguration _configuration;
       
        public NoteRepo(FundooContext fundooContext, IConfiguration configuration)
        {
            _fundooContext = fundooContext;
            _configuration = configuration;
           

        }
        public NoteEntity CreateNote(NoteModel noteEntity, long userId)
        {
            try
            {
                NoteEntity note = new NoteEntity();
                note.Title = noteEntity.Title;
                note.TakeNote = noteEntity.TakeNote;

                note.UserId = userId;

                _fundooContext.Note.Add(note);
                _fundooContext.SaveChanges();

                if (note != null)
                {
                    return note;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        public List<NoteEntity> GetAllNotes(long userId)
        {
            return _fundooContext.Note.Where(x => x.UserId == userId).ToList();


        }
        public List<NoteEntity> GetNoteById(long id)
        {

            var result = _fundooContext.Note.FirstOrDefault(x => x.NoteId == id);
            if (result != null)
            {
                List<NoteEntity> values = new List<NoteEntity>();
                values.Add(result);
                return values;

            }
            else
            {
                return null;
            }
        }
        public string UpdateNote(long id, string takeNote)
        {
            var result = _fundooContext.Note.FirstOrDefault(x => x.NoteId == id);
            if (result != null)
            {
                result.TakeNote = result.TakeNote + takeNote;
                _fundooContext.Note.Update(result);
                _fundooContext.SaveChanges();
                return result.TakeNote;


            }
            return null;

        }
        public long DeleteNote(long id)
        {
            var result = _fundooContext.Note.FirstOrDefault(x => x.NoteId == id);
            if (result != null)
            {
                _fundooContext.Note.Remove(result);
                _fundooContext.SaveChanges();
                return result.NoteId;
            }
            return 0;
        }
    }
}
