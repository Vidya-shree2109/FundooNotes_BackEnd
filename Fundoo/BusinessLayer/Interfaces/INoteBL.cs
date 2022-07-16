using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using RepoLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface INoteBL
    {
        public NoteEntity AddNote(NoteModel notes, long userid);
        public bool DeleteNote(long noteid);
        public NoteEntity UpdateNote(NoteModel notes, long noteid);
        public NoteEntity IsPinnedOrNot(long noteid);
        public NoteEntity IsArchivedOrNot(long noteid);
        public IEnumerable<NoteEntity> GetAllNotes();
        public IEnumerable<NoteEntity> GetAllNotesByUserId(long userid);
        public NoteEntity UploadImage(long noteid, IFormFile img);
    }
}