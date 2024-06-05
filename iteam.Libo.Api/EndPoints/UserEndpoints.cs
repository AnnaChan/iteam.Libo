using iteam.Libo.Common;
using iteam.Libo.Common.Dto;
using Microsoft.EntityFrameworkCore;

namespace iteam.Libo.Api.EndPoints
{
    public class UserEndpoints
    {
        public static void MapEndpoints(WebApplication app)
        {
            app.MapGet("/usersAndRoles", async (LiboContext db) =>
            {
                var usersAndRoles = await db.Users
                    .Select(u => new UserAndRoleDto(
                        u.UserId,
                        u.IsActive,
                        u.Name,
                        u.Phone,
                        u.Email,
                        u.Role.Name,
                        u.Role.Description ?? "No Description"))
                    .ToArrayAsync();

                return Results.Ok(usersAndRoles);
            })
            .WithName("GetUsersAndRoles")
            .WithOpenApi();

            app.MapPost("/users", async (LiboContext db, AddUserDto userDto) =>
            {
                var user = new User
                {
                    Name = userDto.Name,
                    Phone = userDto.Phone,
                    Email = userDto.Email,
                    RoleId = userDto.RoleId
                };

                db.Users.Add(user);
                await db.SaveChangesAsync();

                return Results.Created($"/users/{user.UserId}", user);
            })
            .WithName("CreateUser")
            .WithOpenApi();

            app.MapPut("/users/{id}/Name", async (LiboContext db, int id, string newName) =>
            {
                var user = await db.Users.FindAsync(id);

                if (user == null)
                {
                    return Results.NotFound();
                }

                user.Name = newName;

                await db.SaveChangesAsync();

                return Results.Ok();
            })
            .WithName("UpdateUserName")
            .WithOpenApi();

            app.MapPut("/users/{id}/isActive", async (LiboContext db, int id, bool newIsActive) =>
            {
                var user = await db.Users.FindAsync(id);

                if (user == null)
                {
                    return Results.NotFound();
                }

                user.IsActive = newIsActive;

                await db.SaveChangesAsync();

                return Results.Ok();
            })
            .WithName("UpdateUserIsActive")
            .WithOpenApi();

            app.MapPut("/users/{id}/RoleId", async (LiboContext db, int id, int newRoleId) =>
            {
                var user = await db.Users.FindAsync(id);

                if (user == null)
                {
                    return Results.NotFound();
                }

                user.RoleId = newRoleId;

                await db.SaveChangesAsync();

                return Results.Ok();
            })
            .WithName("UpdateUserRole")
            .WithOpenApi();

            app.MapPut("/users/{id}/Phone", async (LiboContext db, int id, string newPhone) =>
            {
                var user = await db.Users.FindAsync(id);

                if (user == null)
                {
                    return Results.NotFound();
                }

                user.Phone = newPhone;

                await db.SaveChangesAsync();

                return Results.Ok();
            })
            .WithName("UpdateUserPhone")
            .WithOpenApi();

            app.MapPut("/users/{id}/Email", async (LiboContext db, int id, string newEmail) =>
            {
                var user = await db.Users.FindAsync(id);

                if (user == null)
                {
                    return Results.NotFound();
                }

                user.Email = newEmail;

                await db.SaveChangesAsync();

                return Results.Ok();
            })
            .WithName("UpdateUserEmail")
            .WithOpenApi();
        }
    }
}
