using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace RestaurantAPI.Controllers
{
    [Route("file")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetFile([FromQuery] string fileName)
        {
            var rootPath = Directory.GetCurrentDirectory();

            var filePath = $"{rootPath}/PrivateFolder/{fileName}";

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var contentExtension = new FileExtensionContentTypeProvider();
            contentExtension.TryGetContentType(fileName, out var contentType);

            var fileContent = System.IO.File.ReadAllText(filePath);

            return File(fileContent, contentType, fileName);
        }

        [HttpPost]
        public ActionResult Upload([FromForm] IFormFile formFile)
        {
            if (formFile != null && formFile.Length > 0)
            {
                var rootPath = Directory.GetCurrentDirectory();

                var fullPath = $"{rootPath}/PrivateFolder/{formFile.FileName}";
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    formFile.CopyTo(stream);
                }
                return Ok();
            }
            return BadRequest();
        }
    }
}
