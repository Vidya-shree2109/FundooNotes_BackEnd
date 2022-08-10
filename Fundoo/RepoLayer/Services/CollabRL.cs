using Microsoft.Extensions.Configuration;
using RepoLayer.Context;
using RepoLayer.Entities;
using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepoLayer.Services
{
    public class CollabRL : ICollabRL
    {
        FundooContext fundooContext;
        private readonly IConfiguration config;
        public CollabRL(FundooContext fundooContext, IConfiguration config)
        {
            this.config = config;
            this.fundooContext = fundooContext;
        }
        public CollabEntity AddCollab(long noteid, long userid, string email)
        {
            try
            {
                CollabEntity Entity = new CollabEntity();
                Entity.CollabEmail = email;
                Entity.UserId = userid;
                Entity.NoteId = noteid;
                this.fundooContext.Collaborator.Add(Entity);
                int result = this.fundooContext.SaveChanges();
                if (result > 0)
                {
                    return Entity;
                }
                return null;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool Remove(long collabid)
        {
            try
            {
                var result = this.fundooContext.Collaborator.FirstOrDefault(x => x.CollabId == collabid);
                fundooContext.Remove(result);
                int deletednote = this.fundooContext.SaveChanges();
                if (deletednote > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<CollabEntity> GetAllByNoteID(long noteid)
        {
            return fundooContext.Collaborator.Where(n => n.NoteId == noteid).ToList();
        }
    }
}
