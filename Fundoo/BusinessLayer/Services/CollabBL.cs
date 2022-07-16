using BusinessLayer.Interfaces;
using RepoLayer.Entities;
using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class CollabBL : ICollabBL
    {
        ICollabBL CollabRl;
        public CollabBL(ICollabRL collabRl)
        {
            this.CollabRl = CollabRl;
        }
        public CollabEntity AddCollab(long noteid, long userid, string email)
        {
            try
            {
                return this.CollabRl.AddCollab(noteid, userid, email);
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
                return this.CollabRl.Remove(collabid);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IEnumerable<CollabEntity> GetAllByNoteID(long noteid)
        {
            try
            {
                return this.CollabRl.GetAllByNoteID(noteid);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
