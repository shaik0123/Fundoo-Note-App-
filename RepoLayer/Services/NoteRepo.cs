using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RepoLayer.Context;
using RepoLayer.Entity;
using RepoLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepoLayer.Services
{
    public class NoteRepo : INoteRepo
    {
        private readonly FundooContext _fundooContext;
        private readonly IConfiguration _configuration;
        private readonly FileService _fileService;
        private readonly Cloudinary _cloudinary;

        public NoteRepo(FundooContext fundooContext, IConfiguration configuration, FileService fileService, Cloudinary cloudinary)
        {
            _fundooContext = fundooContext;
            _configuration = configuration;
            _fileService = fileService;
            _cloudinary = cloudinary;
           

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

        public string UpdateColour(long id, string colour, long userid)
        {
            var result = _fundooContext.Note.FirstOrDefault(x => x.NoteId == id && x.UserId == userid);
            if (result != null)
            {
                result.Colour = colour;
                _fundooContext.SaveChanges();
                return colour;

            }
            return null;
        }
        public async Task<Tuple<int, string>> Image(long id, long usedId, IFormFile imageFile)
        {
            var result = _fundooContext.Note.FirstOrDefault(x => x.NoteId == id && x.UserId == usedId);
            if (result != null)
            {
                try
                {
                    var data = await _fileService.SaveImage(imageFile);
                    if (data.Item1 == 0)
                    {
                        return new Tuple<int, string>(0, data.Item2);
                    }

                    var UploadImage = new ImageUploadParams
                    {
                        File = new CloudinaryDotNet.FileDescription(imageFile.FileName, imageFile.OpenReadStream())
                    };

                    ImageUploadResult uploadResult = await _cloudinary.UploadAsync(UploadImage);
                    string imageUrl = uploadResult.SecureUrl.AbsoluteUri;
                    result.Image = imageUrl;

                    _fundooContext.Note.Update(result);
                    _fundooContext.SaveChanges();

                    return new Tuple<int, string>(1, "Image Uploaded Successfully");
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            return null;
        }
        public bool Archive(long Id, long userId)
        {
            var result = _fundooContext.Note.FirstOrDefault(x => x.UserId == userId && x.NoteId == Id);
            if (result != null)
            {
                result.IsArchive = true;
                _fundooContext.Update(result);
                _fundooContext.SaveChanges();
                return true;
            }
            return false;

        }
        public bool Pin(long Id, long userId)
        {
            var result = _fundooContext.Note.FirstOrDefault(x => x.UserId == userId && x.NoteId == Id);
            if (result != null)
            {
                result.IsPin = true;
                _fundooContext.Update(result);
                _fundooContext.SaveChanges();
                return true;
            }
            return false;

        }
        public bool Trash(long Id, long userId)
        {
            var result = _fundooContext.Note.FirstOrDefault(x => x.UserId == userId && x.NoteId == Id);
            if (result != null)
            {
                result.IsTrash = true;
                _fundooContext.Update(result);
                _fundooContext.SaveChanges();
                return true;
            }
            return false;

        }
        
        
    }
}
