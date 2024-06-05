using iteam.Libo.Common;
using iteam.Libo.Common.Dto;
using Microsoft.EntityFrameworkCore;

namespace iteam.Libo.Api.EndPoints
{
    public class ArticleEndpoints
    {
        public static void MapEndpoints(WebApplication app)
        {
            app.MapGet("/articles", async (LiboContext db) =>
            {
                var articles = await db.Articles
                .ToArrayAsync();
                return Results.Ok(articles); 
            })
            .WithName("GetArticles")
            .WithOpenApi();


            app.MapPost("/articles/add", async (LiboContext db, ArticleDto articleDto, int copies) =>
            {
                // Log the received ArticleDto and CategoryId
                Console.WriteLine($"Received ArticleDto: {articleDto}");
                Console.WriteLine($"Received CategoryId: {articleDto.CategoryId}");

                // Check if the article already exists in the articles collection
                var existingArticle = await db.Articles.FirstOrDefaultAsync(a => a.Title == articleDto.Title && a.CategoryId == articleDto.CategoryId);

                Article article;
                if (existingArticle == null)
                {
                    // Create a new Article if it doesn't exist
                    article = new Article
                    {
                        Title = articleDto.Title,
                        Description = articleDto.Description,
                        Author = articleDto.Author,
                        Isbn = articleDto.Isbn,
                        Url = articleDto.Url,
                        CategoryId = articleDto.CategoryId, // Use CategoryId from ArticleDto
                        Category = await db.Categories.FindAsync(articleDto.CategoryId)
                    };

                    db.Articles.Add(article);
                    await db.SaveChangesAsync(); // Save the new article to the database
                }
                else
                {
                    // Use the existing article
                    article = existingArticle;
                }

                // Add the specified number of copies of the article to the items collection
                for (int i = 0; i < copies; i++)
                {
                    var newItem = new Item
                    {
                        ArticleId = article.Id, // Reference the article ID
                                                // Add other properties as needed
                    };

                    db.Items.Add(newItem);
                }

                // Save the new items to the database
                await db.SaveChangesAsync();

                // Return a successful response
                return Results.Created($"/articles/{article.Id}", $"Added {copies} copies of article '{article.Title}' to the library.");
            })
    .WithName("AddArticleAndCopies")
    .WithOpenApi();




            app.MapDelete("/articles/{id}", async (LiboContext db, int id) =>
            {
                var article = await db.Articles.FindAsync(id);

                if (article == null)
                {
                    return Results.NotFound();
                }

                db.Articles.Remove(article);
                await db.SaveChangesAsync();

                return Results.Ok();
            })
            .WithName("DeleteArticle")
            .WithOpenApi();
        }

    }
}
