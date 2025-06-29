using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using storeitbackend.Dtos.Account;

namespace storeitbackend.Dtos.File
{
  public class FileShareRemoveDto
  {
    public string UserEmailToRemove { get; set; }
    public string FileId { get; set; }

  }
}