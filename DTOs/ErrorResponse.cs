namespace Theater_Management_FE.DTOs;

public record ErrorResponse(
    DateTime Timestamp,
    int Status,
    string Error,
    string Message,
    string Path);
