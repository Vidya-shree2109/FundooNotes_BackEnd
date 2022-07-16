using Microsoft.EntityFrameworkCore;
using RepoLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepoLayer.Context
{
    public class FundooContext : DbContext
    {
        public FundooContext(DbContextOptions options)
           : base(options)
        {
        }
        public DbSet <UserEntity> Users { get; set; }
        public DbSet <NoteEntity> Notes { get; set; }
        public DbSet<CollabEntity> Collaborator { get; set; }
        public DbSet<LabelEntity> Labels { get; set; }
    }
}