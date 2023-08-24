using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RepoLayer.Entity;
using System.Collections.Generic;

namespace FundooNoteApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteBusiness _noteBusiness;
        private readonly IDistributedCache _distributedCache;
        public NoteController(INoteBusiness noteBusiness, IDistributedCache distributedCache)
        {
            _noteBusiness = noteBusiness;
            _distributedCache = distributedCache;
        }
        [Authorize]
        [HttpPost]
        [Route("Note")]
        public IActionResult CreatNote(NoteModel model)
        {
            long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var result = _noteBusiness.CreateNote(model, userId);
            if (result != null)
            {
                return Ok(new { success = true, messege = "Note Created Sucessfully", data = result });
            }
            else
            {
                return NotFound(new { success = false, messege = "Note Created UnSucessfully", data = result });
            }
        }
        [Authorize]
        [HttpGet]
        [Route("AllNotes")]
        public IActionResult AllNotes()
        {
            long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);

            var result = _noteBusiness.GetAllNotes(userId);
            if (result != null)
            {
                return Ok(new { success = true, messege = "Get All Notes Sucessfully", data = result });
            }
            else
            {
                return NotFound(new { success = false, messege = "Notes Not Found", data = result });
            }
        }
        [Authorize]
        [HttpGet]
        [Route("NoteById")]
        public IActionResult NoteById(long id)
        {
            var result = _noteBusiness.GetNoteById(id);
            if (result != null)
            {
                return Ok(new { success = true, messege = "Get Note By Id Sucessfully", data = result });
            }
            else
            {
                return NotFound(new { success = false, messege = "Notes Not Found", data = result });
            }

        }
        [Authorize]
        [HttpPatch]
        [Route("NoteById")]
        public IActionResult NoteById(long id, string takeNote)
        {
            var result = _noteBusiness.UpdateNote(id, takeNote);
            if (result != null)
            {
                return Ok(new { success = true, messege = "Update Note By Id Sucessfully", data = result });
            }
            else
            {
                return NotFound(new { success = false, messege = "Note Id Not Found", data = result });
            }

        }
        [Authorize]
        [HttpDelete]
        [Route("Note")]
        public IActionResult Note(long id)
        {
            var result = _noteBusiness.DeleteNote(id);
            if (result != 0)
            {
                return Ok(new { success = true, messege = "Delete  Note  Sucessfully", data = result });
            }
            else
            {
                return NotFound(new { success = false, messege = "Note Id Not Found", data = result });
            }

        }
        [Authorize]
        [HttpPatch]
        [Route("Colour")]
        public IActionResult Colour(long id, string colour)
        {
            long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var result = _noteBusiness.UpdateColour(id, colour, userId);
            if (result != null)
            {
                return Ok(new { success = true, messege = "Colour Update  Sucessfully", data = result });
            }
            else
            {
                return NotFound(new { success = false, messege = "Id's Not Found", data = result });
            }


        }
        [Authorize]
        [HttpPatch]
        [Route("AddImage")]
        public async Task<IActionResult> Image(long id, IFormFile imageFile)
        {
            long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            Tuple<int, string> result = await _noteBusiness.Image(id, userId, imageFile);
            if (result.Item1 == 1)
            {
                return Ok(new { success = true, messege = "Image Update  Sucessfully", data = result });
            }
            else
            {
                return NotFound(new { success = false, messege = "Image Update  Unucessfully", data = result });
            }
        }
        [Authorize]
        [HttpPatch]
        [Route("Archive")]
        public IActionResult Archive(long id)
        {
            long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var result = _noteBusiness.Archive(id, userId);
            if (result == true)
            {
                return Ok(new { success = true, messege = "Archive Sucessfully", data = result });
            }
            else
            {
                return NotFound(new { success = false, messege = "Id's Not Found", data = result });
            }

        }
        [Authorize]
        [HttpPatch]
        [Route("Pin")]
        public IActionResult Pin(long id)
        {
            long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var result = _noteBusiness.Pin(id, userId);
            if (result == true)
            {
                return Ok(new { success = true, messege = "pin Sucessfully", data = result });
            }
            else
            {
                return NotFound(new { success = false, messege = "Id's Not Found", data = result });
            }


        }
        [Authorize]
        [HttpPatch]
        [Route("Trash")]
        public IActionResult Trash(long id)
        {
            long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
            var result = _noteBusiness.Trash(id, userId);
            if (result == true)
            {
                return Ok(new { success = true, messege = "Trash Sucessfully", data = result });
            }
            else
            {
                return NotFound(new { success = false, messege = "Id's Not Found", data = result });
            }
        }
        [Authorize]
        [HttpGet]
        [Route("AllNotesByRedis")]
        public async Task<IActionResult> AllNotesByRadis()
        {
            var cacheKey = "NoteList";
            string serializeNoteList;
            List<NoteEntity> notesList;

            var redisNoteList = await _distributedCache.GetStringAsync(cacheKey);
            if (redisNoteList != null)
            {
                notesList = JsonConvert.DeserializeObject<List<NoteEntity>>(redisNoteList);
            }
            else
            {
                long userId = Convert.ToInt64(User.Claims.FirstOrDefault(x => x.Type == "UserId").Value);
                notesList = _noteBusiness.GetAllNotes(userId);
                serializeNoteList = JsonConvert.SerializeObject(notesList);

                await _distributedCache.SetStringAsync(cacheKey, serializeNoteList, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                    SlidingExpiration = TimeSpan.FromMinutes(2)

                });
            }

            if (notesList != null)
            {
                return Ok(new { success = true, messege = "Get All Notes Sucessfully", data = notesList });
            }
            else
            {
                return NotFound(new { success = false, messege = "Notes Not Found", data = notesList });
            }
        }
    }
}
