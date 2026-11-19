namespace LibrarySystemApi.Dtos.AccountDtos;

public sealed record UserRegisterRequest(string Email, string Password, string FullName);
