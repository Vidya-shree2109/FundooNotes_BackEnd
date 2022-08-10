using BusinessLayer.Interfaces;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RepoLayer.Context;
using RepoLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        //private readonly IMemoryCache memoryCache;
        //FundooContext fundooContext;
        //private readonly IDistributedCache distributedCache;
        INoteBL inoteBl;
        //public NoteController(INoteBL inoteBl)
        //{
        //    this.inoteBl = inoteBl;
        //}
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private readonly FundooContext context;
        public NoteController(INoteBL inoteBl, IMemoryCache memoryCache, FundooContext context, IDistributedCache distributedCache)
        {
            this.inoteBl = inoteBl;
            this.memoryCache = memoryCache;
            this.context = context;
            this.distributedCache = distributedCache;
        }
        [Authorize]
        [HttpPost("Add")]
        public IActionResult AddNotes(NoteModel addnote)
        {
            try
            {
                long noteid = Convert.ToInt32(User.Claims.First(e => e.Type == "id").Value);
                var result = inoteBl.AddNote(addnote, noteid);
                if (result != null)
                {
                    return this.Ok(new
                    {
                        Success = true,
                        message = "Note Added Successfully",
                        Response = result
                    });
                }
                else
                {
                    return this.BadRequest(new
                    {
                        Success = false,
                        message = "Unable to add note"
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpDelete("Delete")]
        public IActionResult DeleteNotes(long noteid)
        {
            try
            {
                if (inoteBl.DeleteNote(noteid))
                {
                    return this.Ok(new
                    {
                        Success = true,
                        message = "Note Deleted Successfully"
                    });
                }
                else
                {
                    return this.BadRequest(new
                    {
                        Success = false,
                        message = "Unable to delete note"
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpPut("Update")]
        public IActionResult UpdateNotes(NoteModel addnote, long noteid)
        {
            try
            {
                var result = inoteBl.UpdateNote(addnote, noteid);
                if (result != null)
                {
                    return this.Ok(new
                    {
                        Success = true,
                        message = "Note Updated Successfully",
                        Response = result
                    });
                }
                else
                {
                    return this.BadRequest(new
                    {
                        Success = false,
                        message = "Unable to Update note"
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpPut("Pin")]
        public IActionResult Ispinnedornot(long noteid)
        {
            try
            {
                var result = inoteBl.IsPinnedOrNot(noteid);
                if (result != null)
                {
                    return this.Ok(new
                    {
                        message = "Note Unpinned ",
                        Response = result
                    });
                }
                else
                {
                    return this.BadRequest(new
                    {
                        message = "Note Pinned Successfully"
                    });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpPut("Archive")]
        public IActionResult IsArchivedORNot(long noteid)
        {
            try
            {
                var result = inoteBl.IsArchivedOrNot(noteid);
                if (result != null)
                {
                    return this.Ok(new
                    {
                        message = "Note Unarchived",
                        Response = result
                    });
                }
                else
                {
                    return this.BadRequest(new
                    {
                        message = "Note Archived Successfully"
                    });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpGet("AllNotes")]
        public IEnumerable<NoteEntity> GetAllNote()
        {
            try
            {
                return inoteBl.GetAllNotes();
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpGet("UserID")]
        public IEnumerable<NoteEntity> GetAllNotesbyuser(long userid)
        {
            try
            {
                return inoteBl.GetAllNotesByUserId(userid);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpPut("Upload")]
        public IActionResult UploadImage(long noteid, IFormFile img)
        {
            try
            {
                var result = inoteBl.UploadImage(noteid, img);
                if (result != null)
                {
                    return this.Ok(new { message = "uploaded ", Response = result });
                }
                else
                {
                    return this.BadRequest(new { message = "Not uploaded" });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPut("Color")]
        public IActionResult Color(long noteid, string color)
        {
            try
            {
                var result = inoteBl.Color(noteid, color);
                if (result != null)
                {
                    return this.Ok(new
                    {
                        message = "Color is changed ",
                        Response = result
                    });
                }
                else
                {
                    return this.BadRequest(new
                    {
                        message = "Unable to change color"
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("RedisCache")]
        public async Task<IActionResult> GetAllNotesByRedisCache()
        {
            var cacheKey = "noteList";
            string serializednoteList;
            var noteList = new List<NoteEntity>();
            var redisnoteList = await distributedCache.GetAsync(cacheKey);
            if (redisnoteList != null)
            {
                serializednoteList = Encoding.UTF8.GetString(redisnoteList);
                noteList = JsonConvert.DeserializeObject<List<NoteEntity>>(serializednoteList);
            }
            else
            {
                noteList = await context.Notes.ToListAsync();
                serializednoteList = JsonConvert.SerializeObject(noteList);
                redisnoteList = Encoding.UTF8.GetBytes(serializednoteList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisnoteList, options);
            }
            return Ok(noteList);
        }
    }
}