using BusinessLayer.Interfaces;
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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CollabController : ControllerBase
    {
        ICollabBL collab;
        //public CollabController(ICollabBL collab)
        //{
        //    this.collab = collab;
        //}
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private readonly FundooContext context;
        public CollabController(ICollabBL collab, IMemoryCache memoryCache, FundooContext context, IDistributedCache distributedCache)
        {
            this.collab = collab;
            this.memoryCache = memoryCache;
            this.context = context;
            this.distributedCache = distributedCache;
        }
        [HttpPost("Add")]
        public IActionResult AddCollab(long noteid, string email)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.First(e => e.Type == "id").Value);
                var result = collab.AddCollab(noteid, userid, email);
                if (result != null)
                {
                    return this.Ok(new
                    {
                        Success = true,
                        message = "Collaborator Added Successfully",
                        Response = result
                    });
                }
                else
                {
                    return this.BadRequest(new
                    {
                        Success = false,
                        message = "Unable To Add"
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpDelete("Remove")]
        public IActionResult Remove(long collabid)
        {
            try
            {
                if (collab.Remove(collabid))
                {
                    return this.Ok(new
                    {
                        Success = true,
                        message = "Deleted Successfully"
                    });
                }
                else
                {
                    return this.BadRequest(new
                    {
                        Success = false,
                        message = "Unable To Delete"
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("NoteId")]
        public IEnumerable<CollabEntity> GetAllByNoteID(long noteid)
        {
            try
            {
                return collab.GetAllByNoteID(noteid);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("RedisCache")]
        public async Task<IActionResult> GetAllNotesUsingRedisCache()
        {
            var cacheKey = "NodeList";
            string serializedNotesList;
            var NotesList = new List<CollabEntity>();
            var redisNotesList = await distributedCache.GetAsync(cacheKey);
            if (redisNotesList != null)
            {
                serializedNotesList = Encoding.UTF8.GetString(redisNotesList);
                NotesList = JsonConvert.DeserializeObject<List<CollabEntity>>(serializedNotesList);
            }
            else
            {
                NotesList = await context.Collaborator.ToListAsync();
                serializedNotesList = JsonConvert.SerializeObject(NotesList);
                redisNotesList = Encoding.UTF8.GetBytes(serializedNotesList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisNotesList, options);
            }
            return Ok(NotesList);
        }
    }
}
