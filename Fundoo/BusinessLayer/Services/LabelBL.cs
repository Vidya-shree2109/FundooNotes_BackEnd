using BusinessLayer.Interfaces;
using RepoLayer.Entities;
using RepoLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class LabelBL : ILabelBL
    {
        ILabelRL labelRl;
        public LabelBL(ILabelRL labelRl)
        {
            this.labelRl = labelRl;
        }
        public LabelEntity Addlabel(long noteid, long userid, string labels)
        {
            try
            {
                return this.labelRl.Addlabel(noteid, userid, labels);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<LabelEntity> GetlabelsByNoteid(long noteid, long userid)
        {
            try
            {
                return this.labelRl.GetlabelsByNoteid(noteid, userid);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool RemoveLabel(long userID, string labelName)
        {
            try
            {
                return this.labelRl.RemoveLabel(userID, labelName);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public IEnumerable<LabelEntity> RenameLabel(long userID, string oldLabelName, string labelName)
        {
            try
            {
                return this.labelRl.RenameLabel(userID, oldLabelName, labelName);
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
