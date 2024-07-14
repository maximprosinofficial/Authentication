namespace Authentication.Models;

public class User
{
//    public User(string login, string email, string password, string[] roles)
//    {
//        Login = login;
//        Email = email;
//        Password = password;
//        Roles = roles;
//    }
    
    public Guid Id { get; set; } required   
    public string Login { get; set; } required 
    public string Email { get; set; } required 
    public string Password { get; set; } required 
    public string[] Roles { get; set; }
}