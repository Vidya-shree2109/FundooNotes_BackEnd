using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepoLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FundooNotes.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CollabController : ControllerBase
    {
        ICollabBL collab;
        public CollabController(ICollabBL collab)
        {
            this.collab = collab;
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
                        message = "Unable to add"
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
                        message = "Unable to Delete"
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
    }
}
