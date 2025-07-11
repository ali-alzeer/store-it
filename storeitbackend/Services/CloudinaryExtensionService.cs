using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using storeitbackend.Interfaces;

namespace storeitbackend.Services
{
  public class CloudinaryExtensionService : ICloudinaryExtensionService
  {
    private readonly ICloudinary _cloudinary;
    public CloudinaryExtensionService(ICloudinary cloudinary)
    {
      _cloudinary = cloudinary;
    }
    public SearchResult ExecuteSearch(string expression)
    {
      SearchResult searchResult = _cloudinary.Search().Expression(expression).Execute();
      return searchResult;
    }

  }
}