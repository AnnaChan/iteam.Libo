namespace iteam.Libo.Common;

public class User
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; } = true;

    public int RoleId { get; set; }
    public virtual Role Role { get; set; }

    public virtual ICollection<Loan> Loans { get; set; }
    


}
