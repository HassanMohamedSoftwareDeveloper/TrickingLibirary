using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TrickingLibirary.Domain.Entities.Modertion;
using TrickingLibirary.Domain.Interfaces;

namespace TrickingLibirary.Api.Helpers;

public static class TestDataHelper
{
    public static void AddTestData(IServiceProvider serviceProvider, IWebHostEnvironment _env)
    {
        if (_env.IsDevelopment())
        {
            using var scope = serviceProvider.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var testUser = new IdentityUser("test") { Email = "test@test.com" };
            userManager.CreateAsync(testUser, "password").GetAwaiter().GetResult();

            var mod = new IdentityUser("mod") { Email = "mod@test.com" };
            userManager.CreateAsync(mod, "password").GetAwaiter().GetResult();
            userManager.AddClaimAsync(mod, new Claim(Tricking_LibiraryConstants.Claims.Role,
                Tricking_LibiraryConstants.Roles.Mod)).GetAwaiter().GetResult();


            var dbContext = scope.ServiceProvider.GetRequiredService<IDbContext>();
            dbContext.Difficulties.AddRange(
                new Domain.Entities.Difficulty { Slug = "easy", Version = 1, Active = true, Name = "Easy", Description = "Easy Test" },
                new Domain.Entities.Difficulty { Slug = "medium", Version = 1, Active = true, Name = "Medium", Description = "Medium Test" },
                new Domain.Entities.Difficulty { Slug = "hard", Version = 1, Active = true, Name = "Hard", Description = "Hard Test" });

            dbContext.Categories.AddRange(
               new Domain.Entities.Category { Id = 1, Slug = "kick", Version = 1, Active = true, Name = "Kick", Description = "Kick Test" },
               new Domain.Entities.Category { Id = 2, Slug = "flip", Version = 1, Active = true, Name = "Flip", Description = "Flip Test" },
               new Domain.Entities.Category { Id = 3, Slug = "transition", Version = 1, Active = true, Name = "Transition", Description = "Transition Test" });

            dbContext.Tricks.AddRange(
                new Domain.Entities.Trick
                {
                    Id = 1,
                    Slug = "backwards-roll",
                    Version = 1,
                    Active = true,
                    Name = "Backwards Roll",
                    Description = "This is a test backwards roll",
                    Difficulty = "easy",
                    TrickCategories = new List<Domain.Entities.TrickCategory>
                    {
                        new Domain.Entities.TrickCategory {CategoryId=2}
                    }
                },
                new Domain.Entities.Trick
                {
                    Id = 2,
                    Slug = "forwards-roll",
                    Version = 1,
                    Active = true,
                    Name = "Forwards Roll",
                    Description = "This is a test forwards roll",
                    Difficulty = "easy",
                    TrickCategories = new List<Domain.Entities.TrickCategory>
                    {
                        new Domain.Entities.TrickCategory {CategoryId=2}
                    }
                },
                new Domain.Entities.Trick
                {
                    Id = 3,
                    Slug = "back-flip",
                    Version = 1,
                    Active = true,
                    Name = "Back Flip",
                    Description = "This is a test back flip",
                    Difficulty = "medium",
                    TrickCategories = new List<Domain.Entities.TrickCategory>
                    {
                        new Domain.Entities.TrickCategory {CategoryId=2}
                    },
                    Prerequisites = new List<Domain.Entities.TrickRelationship>
                    {
                        new Domain.Entities.TrickRelationship{PrerequisiteId=1}
                    }
                }
                );

            dbContext.Submissions.AddRange(
                new Domain.Entities.Submission
                {
                    TrickId = "back-flip",
                    Description = "test desceiption",
                    Video = new Domain.Entities.Video
                    {
                        VideoLink = "https://localhost:44379/api/files/video/one.mp4",
                        ThumbLink = "https://localhost:44379/api/files/image/one.jpg",
                    },
                    VideoProcessed = true,
                    UserId = testUser.Id,

                },
                new Domain.Entities.Submission
                {
                    TrickId = "back-flip",
                    Description = "test desceiption",
                    Video = new Domain.Entities.Video
                    {
                        VideoLink = "https://localhost:44379/api/files/video/two.mp4",
                        ThumbLink = "https://localhost:44379/api/files/image/two.jpg",
                    },
                    VideoProcessed = true,
                    UserId = testUser.Id,
                }
                );

            dbContext.ModerationItems.AddRange(
                new ModerationItem
                {
                    Target = 2,
                    Type = MpderationTypes.Trick
                }
                );

            dbContext.SaveChanges();


        }
    }
}
