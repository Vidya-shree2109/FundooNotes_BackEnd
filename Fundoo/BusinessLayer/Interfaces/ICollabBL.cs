using RepoLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface ICollabBL
    {
        public CollabEntity AddCollab(long noteid, long userid, string email);
        public bool Remove(long collabid);
        IEnumerable<CollabEntity> GetAllByNoteID(long noteid);
    }
}