namespace iteam.Libo.Common.Dto;

public record ItemDto(int ItemId, string ArticleTitle, string CategoryName, string? ISBN,string? Url, bool IsBorrowed);
public record BorrowItemDto(int ItemId, int BorrowerId);
public record ItemAndLoanDto(int ItemId, string ArticleTitle, string CategoryName, bool IsBorrowed, DateTime? BorrowedDate, DateTime? DueDate);

public record ArticleDto(int Id, string Title, string Description, string? Author, string? Isbn, string? Url, int CategoryId, string CategoryName);
public record UserAndRoleDto(int UserId, bool IsActive, string UserName, string UserPhone, string UserEmail, string RoleName, string RoleDescription);
public record AddUserDto(string Name, string? Phone, string? Email, int RoleId);
public record AddRoleDto(string Name, string? Description);
