using BusinessLayer.Interface;
using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;

namespace FundooNoteApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteBusiness _noteBusiness;
        public NoteController(INoteBusiness noteBusiness)
        {
            _noteBusiness = noteBusiness;
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
        public IActionResult GetAllNotes()
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
        public IActionResult GetNoteById(long id)
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
        public IActionResult UpdateNoteById(long id, string takeNote)
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
        public IActionResult DeleteNote(long id)
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
    }
}
