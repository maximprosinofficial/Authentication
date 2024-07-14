namespace Authentication.Contracts;

public record CreateUserRequest(string Login, string Email, string Password, string[] Roles);