using BusinessLayer.Interfaces;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using RepoLayer.Entities;
using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class NoteBL:INoteBL
    {
        INoteRL noteRL;
        public NoteBL(INoteRL noteRL)
        {
            this.noteRL = noteRL;
        }
        public NoteEntity AddNote(NoteModel noteModel, long userid)
        {
            try
            {
                return this.noteRL.AddNote(noteModel, userid);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool DeleteNote(long noteid)
        {
            try
            {
                return this.noteRL.DeleteNote(noteid);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public NoteEntity UpdateNote(NoteModel noteModel, long noteid)
        {
            try
            {
                return this.noteRL.UpdateNote(noteModel, noteid);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public NoteEntity IsPinnedOrNot(long noteid)
        {
            try
            {
                return this.noteRL.IsPinnedOrNot(noteid);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public NoteEntity IsArchivedOrNot(long noteid)
        {
            try
            {
                return this.noteRL.IsArchivedOrNot(noteid);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<NoteEntity> GetAllNotes()
        {
            try
            {
                return this.noteRL.GetAllNotes();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<NoteEntity> GetAllNotesByUserId(long userid)
        {
            try
            {
                return this.noteRL.GetAllNotesByUserId(userid);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public NoteEntity UploadImage(long noteid, IFormFile img)
        {
            try
            {
                return this.noteRL.UploadImage(noteid, img);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public NoteEntity Color(long noteid, string color)
        {
            try
            {
                return this.noteRL.Color(noteid, color);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}