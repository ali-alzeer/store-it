using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;

namespace storeitbackend.Interfaces
{
  public interface ICloudinaryExtensionService
  {
    public SearchResult ExecuteSearch(string expression);
  }
}