using iteam.Libo.Common;
using iteam.Libo.Common.Dto;
using Microsoft.EntityFrameworkCore;

namespace iteam.Libo.Api.EndPoints
{
    public class ItemEndpoints
    {
        public static void MapEndpoints(WebApplication app)
        {
            app.MapGet("/items", async (LiboContext db) =>
            {
                const int BookCategoryID = 1; // ID for books
                const int VideoCategoryID = 2; // ID for videos

                var itemsDto = await db.Items
                    .Select(item => new ItemDto(
                        item.Id,
                        item.Article.Title, // Always use the ArticleTitle property for the title
                        item.Article.Category.Name,
                        item.Article.CategoryId == BookCategoryID ? item.Article.Isbn : null,
                        item.Article.CategoryId == VideoCategoryID ? item.Article.Url : null,
                        item.Loans.Any(loan => loan.ReturnDate == null) // This is the IsBorrowed argument
    ))
    .ToArrayAsync();



                return Results.Ok(itemsDto);
            })
            .WithName("GetItems")
            .WithOpenApi();

            /* app.MapGet("/itemsAndLoans", async (LiboContext db) =>
             {
                 var itemsAndLoansDto = await db.Items
                 .GroupJoin(db.Loans,
                     item => item.Id,
                     loan => loan.BorrowedItemId,
                     (item, loans) => new
                     {
                         Item = item,
                         Loans = loans.DefaultIfEmpty()
                     })
                 .SelectMany(
                     x => x.Loans,
                     (x, loan) => new
                     {
                         Item = x.Item,
                         Loan = loan
                     })
                 .Where(x => x.Loan!.ReturnDate == null)
                 .Select(x => new ItemAndLoanDto(
                     x.Item.Id,
                     x.Item.Article.Title,
                     x.Item.Article.Category.Name,
                     x.Loan != null,
                     x.Loan != null ? x.Loan.BorrowedDate : (DateTime?)null,
                     x.Loan != null ? x.Loan.DueDate : (DateTime?)null
                 ))
                 .ToArrayAsync();


                 return Results.Ok(itemsAndLoansDto);
             })
             .WithName("GetItemsAndLoans")
             .WithOpenApi();

             app.MapPost("/items", async (LiboContext db, Item itemDto) =>
             {
                 var item = new Item
                 {
                     ArticleId = itemDto.ArticleId,

                 };

                 db.Items.Add(item);
                 await db.SaveChangesAsync();

                 return Results.Created($"/items/{item.Id}", item);
             })
             .WithName("CreateItem")
             .WithOpenApi();*/

            app.MapPost("/items/borrow", async (LiboContext db, BorrowItemDto borrowItemDto) =>
            {
                // Retrieve the item based on the provided ItemId
                var item = await db.Items
                    .Include(i => i.Loans)
                    .FirstOrDefaultAsync(i => i.Id == borrowItemDto.ItemId);

                // Validate the existence of the item
                if (item == null)
                {
                    return Results.NotFound("Item not found.");
                }

                // Check if the item is already on loan
                var currentLoan = item.Loans.FirstOrDefault(l => l.ReturnDate == null);
                if (currentLoan != null)
                {
                    return Results.BadRequest("Item is already on loan.");
                }

                // Create a new loan for the item
                var newLoan = new Loan
                {
                    BorrowedDate = DateTime.Now,
                    BorrowedItemId = item.Id,
                    BorrowerId = borrowItemDto.BorrowerId, // Add the borrower's ID to the loan
                    DueDate = DateTime.Now.AddDays(14)
                };

                db.Loans.Add(newLoan);
                await db.SaveChangesAsync();

                return Results.Ok();
            })
    .WithName("BorrowItem")
    .WithOpenApi();



            /* app.MapPut("/items/{id}/return", async (LiboContext db, int id) =>
             {
                 var item = await db.Items.FindAsync(id);

                 if (item == null)
                 {
                     return Results.NotFound();
                 }

                 if (item.Loans != null && item.Loans.Any(l => l.ReturnDate == null))
                 {
                     var loan = item.Loans.FirstOrDefault(l => l.ReturnDate == null);
                     loan.ReturnDate = DateTime.Now;

                     await db.SaveChangesAsync();

                     return Results.Ok();
                 }
                 else
                 {

                     return Results.BadRequest("No active loan found for the item.");
                 }
             })
             .WithName("ReturnItem")
             .WithOpenApi();*/

            app.MapPut("/items/{id}/return", async (LiboContext db, int id) =>
            {
                var item = await db.Items
                    .Include(i => i.Loans) // Eager load the Loans collection
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (item == null)
                {
                    return Results.NotFound();
                }

                // Check if there is any active loan
                var activeLoan = item.Loans?.FirstOrDefault(l => l.ReturnDate == null);

                if (activeLoan != null)
                {
                    // Set the return date and save changes
                    activeLoan.ReturnDate = DateTime.Now;
                    await db.SaveChangesAsync();

                    return Results.Ok();
                }
                else
                {
                    return Results.BadRequest("No active loan found for the item.");
                }
            })
          .WithName("ReturnItem")
          .WithOpenApi();


            /*  app.MapPut("/items/{id}/borrow", async (LiboContext db, int id) =>
              {
                  var item = await db.Items.FindAsync(id);

                  if (item == null)
                  {
                      return Results.NotFound();
                  }

                  if (item.Loans.Any(l => l.ReturnDate == null))
                  {
                      return Results.BadRequest("Item is already on loan.");
                  }
                  else
                  {
                      var loan = new Loan
                      {
                          BorrowedDate = DateTime.Now,
                          BorrowedItemId = item.Id
                      };

                      db.Loans.Add(loan);
                      await db.SaveChangesAsync();

                      return Results.Ok();
                  }
              })
              .WithName("BorrowItem")
              .WithOpenApi();*/

           /* app.MapPut("/items/{id}/borrow", async (LiboContext db, int id, int borrowerId) =>
            {
                // Retrieve the item, including the Loans collection
                var item = await db.Items
                    .Include(i => i.Loans) // Ensure Loans collection is loaded
                    .FirstOrDefaultAsync(i => i.Id == id);

                if (item == null)
                {
                    return Results.NotFound();
                }

                // Check if the item has any active loans
                var activeLoan = item.Loans?.FirstOrDefault(l => l.ReturnDate == null);

                if (activeLoan != null)
                {
                    // The item is already on loan; return a bad request response
                    return Results.BadRequest("Item is already on loan.");
                }
                else
                {
                    // Create a new loan for the item
                    var newLoan = new Loan
                    {
                        BorrowedDate = DateTime.Now,
                        DueDate = DateTime.Now.AddDays(14),
                        BorrowedItemId = item.Id,
                        BorrowerId = borrowerId // Set the BorrowerId from the request
                    };

                    // Add the new loan to the Loans collection and save changes
                    item.Loans.Add(newLoan);
                    await db.SaveChangesAsync();

                    return Results.Ok();
                }
            })
        .WithOpenApi();*/



            app.MapDelete("/items/{id}", async (LiboContext db, int id) =>
            {
                var item = await db.Items.FindAsync(id);

                if (item == null)
                {
                    return Results.NotFound();
                }

                db.Items.Remove(item);
                await db.SaveChangesAsync();

                return Results.Ok();
            })
            .WithName("DeleteItem")
            .WithOpenApi();


        }
    }
}
