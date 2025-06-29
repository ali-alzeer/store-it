using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace storeitbackend.Dtos.File
{
  public class FileRenameDto
  {
    public string FileId { get; set; }
    public string FileName { get; set; }
  }
}