using Microsoft.CodeAnalysis.CSharp.Syntax;
using Victuz_MVC.Models;

namespace Victuz_MVC.Services
{
    public class PictureService(
        ILogger<PictureService> logger
    )
    {
        public async Task<Picture> CreatePicture(IFormFile file)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var uploadsDirName = "uploads";
            var filePath = Path.Combine($"wwwroot/{uploadsDirName}", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Sla het pad van het bestand op in het productmodel
            return new Picture
            {
                FileName = fileName,
                FilePath = $"/{uploadsDirName}/{fileName}"
            };
        }

        public void DeletePicture(string filePath)
        {
            var systemFilePath = Path.Combine($"wwwroot/{filePath}");
            try
            {
                File.Delete(systemFilePath);
            }
            catch(IOException e)
            {
                logger.LogError(e, "Er ging its mis tijdens het verwijderen van een bestand");
            }

        }
    }
}
