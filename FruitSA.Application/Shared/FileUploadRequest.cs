using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FruitSA.Application.Shared
{
    public class FileUploadRequest
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
