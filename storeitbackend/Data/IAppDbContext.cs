using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using storeitbackend.Models;

namespace storeitbackend.Data
{
  public interface IAppDbContext
  {
    DbSet<File> Files { get; set; }
    DbSet<FileType> FileTypes { get; set; }
    DbSet<FileExtension> FileExtensions { get; set; }
    DbSet<UserFile> UsersFiles { get; set; }
    DbSet<OwnerFile> OwnersFiles { get; set; }
    DbSet<User> Users { get; set; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    DatabaseFacade Database { get; }
  }

}