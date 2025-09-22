namespace Application.Account;

public record CurrentUser(string Id,
    string Email, 
    IEnumerable<string> Roles,
    string FirstName,
    string LastName
    )
{
    public bool IsInRole(string role) => Roles.Contains(role);
}
