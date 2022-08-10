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
    public class LabelController : ControllerBase
    {
        ILabelBL lables;

        //public LabelController(ILabelBL lables)
        //{
        //    this.lables = lables;
        //}
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private readonly FundooContext context;

        public LabelController(ILabelBL lables, IMemoryCache memoryCache, FundooContext context, IDistributedCache distributedCache)
        {
            this.lables = lables;
            this.memoryCache = memoryCache;
            this.context = context;
            this.distributedCache = distributedCache;
        }
        [HttpPost("Add")]
        public IActionResult AddLabels(long noteid, string label)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.First(e => e.Type == "id").Value);
                var result = lables.Addlabel(noteid, userid, label);
                if (result != null)
                {
                    return this.Ok(new
                    {
                        Success = true,
                        message = "Labels Added Successfully",
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
        [Authorize]
        [HttpDelete("Remove")]
        public IActionResult RemoveLabel(string lableName)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "id").Value);
                if (lables.RemoveLabel(userID, lableName))
                {
                    return this.Ok(new
                    {
                        success = true,
                        message = "Label removed successfully"
                    });
                }
                else
                {
                    return this.BadRequest(new
                    {
                        success = false,
                        message = "User access denied"
                    });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [Authorize]
        [HttpPut("Rename")]
        public IActionResult RenameLabel(string lableName, string newLabelName)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "id").Value);
                var result = lables.RenameLabel(userID, lableName, newLabelName);
                if (result != null)
                {
                    return this.Ok(new
                    {
                        success = true,
                        message = "Label renamed successfully",
                        Response = result
                    });
                }
                else
                {
                    return this.BadRequest(new
                    {
                        success = false,
                        message = "Unable to rename"
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet("User")]
        public IEnumerable<LabelEntity> GetByuserid(long noteid)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "id").Value);
                return lables.GetlabelsByNoteid(noteid, userID);
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
            var NotesList = new List<LabelEntity>();
            var redisNotesList = await distributedCache.GetAsync(cacheKey);
            if (redisNotesList != null)
            {
                serializedNotesList = Encoding.UTF8.GetString(redisNotesList);
                NotesList = JsonConvert.DeserializeObject<List<LabelEntity>>(serializedNotesList);
            }
            else
            {
                NotesList = await context.Labels.ToListAsync();
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