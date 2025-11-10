namespace Theater_Management_FE.DTOs;

public record SignUpRequest(
    string Username,
    string Email,
    string PhoneNumber,
    string Password);
