using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;

namespace Reveal.WebAPI.Controllers;

[ApiController]
[Route("api/mpeg")]
public class MpegUploadController : ControllerBase
{
    private readonly IMpegUploadService _mpegUploadService;

    public MpegUploadController(IMpegUploadService mpegUploadService)
    {
        _mpegUploadService = mpegUploadService ?? throw new ArgumentNullException(nameof(mpegUploadService));
    }

    [HttpPost]
    [Route("upload")]
    public async Task<IActionResult> UploadMpeg(IFormFile file)
    {
        try
        {
            if (!IsValidMpegFile(file))
            {
                return BadRequest("Invalid file type. Expected MPEG file.");
            }

            var uploadResult = await _mpegUploadService.ProcessMpegUploadAsync(file.FileName, file.OpenReadStream());

            if (!uploadResult.Success)
            {
                return BadRequest(uploadResult.ErrorMessage);
            }

            return Ok(new
            {
                message = "MPEG file uploaded successfully",
                fileId = uploadResult.FileId,
                storageLocation = uploadResult.StorageLocation,
                databaseRecordId = uploadResult.DatabaseRecordId
            });

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    private static readonly List<string> AllowedExtensions = new List<string> { ".mp3", ".mp4", ".mpeg", ".mpg" };
    private static readonly List<string> AllowedMimeTypes = new List<string> { "video/mpeg", "audio/mpeg" };
    private static bool IsValidMpegFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        bool validExtension = AllowedExtensions.Contains(Path.GetExtension(file.FileName).ToLowerInvariant());
        bool validMimeType = AllowedMimeTypes.Contains(file.ContentType);

        return validExtension || validMimeType;
    }
}