using iteam.Libo.Common;
using iteam.Libo.Common.Dto;
using Microsoft.EntityFrameworkCore;

namespace iteam.Libo.Api.EndPoints
{
    public class RoleEndpoints
    {
        public static void MapEndpoints(WebApplication app)
        {
            app.MapGet("/roles", async (LiboContext db) =>
            {
                var roles = await db.Roles
                .ToArrayAsync();
                return Results.Ok(roles);
            })
             .WithName("GetRoles")
             .WithOpenApi();


            app.MapPost("/roles", async (LiboContext db, AddRoleDto roleDto) =>
            {
                var role = new Role
                {
                    Name = roleDto.Name,
                    Description = roleDto.Description,
                };

                db.Roles.Add(role);
                await db.SaveChangesAsync();

                return Results.Created($"/roles/{role.Id}", role);
            })
            .WithName("CreateRole")
            .WithOpenApi();

            app.MapPut("/roles/{id}/NameDescription", async (LiboContext db, int id, string newName, string? newDescription) =>
            {
                var role = await db.Roles.FindAsync(id);

                if (role == null)
                {
                    return Results.NotFound();
                }

                role.Name = newName;
                role.Description = newDescription;

                await db.SaveChangesAsync();

                return Results.Ok();
            })
            .WithName("UpdateRole")
            .WithOpenApi();
        }
    }
}
