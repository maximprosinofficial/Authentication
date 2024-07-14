namespace Authentication.Contracts;

public record AuthUserRequest(string Login, string Password, string[] Roles);