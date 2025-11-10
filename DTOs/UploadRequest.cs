using Microsoft.AspNetCore.Http;

namespace Theater_Management_FE.DTOs;

public record UploadRequest(IFormFile File, string Type, Guid Id);
