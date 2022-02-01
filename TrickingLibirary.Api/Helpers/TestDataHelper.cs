using TrickingLibirary.Domain.Interfaces;

namespace TrickingLibirary.Api.Helpers;

public static class TestDataHelper
{
    public static void AddTestData(IServiceProvider serviceProvider, IWebHostEnvironment _env)
    {
        if (_env.IsDevelopment())
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IDbContext>();
            dbContext.Difficulties.AddRange(
                new Domain.Entities.Difficulty { Id = "easy", Name = "Easy", Description = "Easy Test" },
                new Domain.Entities.Difficulty { Id = "medium", Name = "Medium", Description = "Medium Test" },
                new Domain.Entities.Difficulty { Id = "hard", Name = "Hard", Description = "Hard Test" });

            dbContext.Categories.AddRange(
               new Domain.Entities.Category { Id = "kick", Name = "Kick", Description = "Kick Test" },
               new Domain.Entities.Category { Id = "flip", Name = "Flip", Description = "Flip Test" },
               new Domain.Entities.Category { Id = "transition", Name = "Transition", Description = "Transition Test" });

            dbContext.Tricks.AddRange(
                new Domain.Entities.Trick
                {
                    Id = "backwards-roll",
                    Name = "Backwards Roll",
                    Description = "This is a test backwards roll",
                    Difficulty = "easy",
                    TrickCategories = new List<Domain.Entities.TrickCategory>
                    {
                        new Domain.Entities.TrickCategory {CategoryId="flip"}
                    }
                },
                new Domain.Entities.Trick
                {
                    Id = "back-flip",
                    Name = "Back Flip",
                    Description = "This is a test back flip",
                    Difficulty = "medium",
                    TrickCategories = new List<Domain.Entities.TrickCategory>
                    {
                        new Domain.Entities.TrickCategory {CategoryId="flip"}
                    },
                    Prerequisites = new List<Domain.Entities.TrickRelationship>
                    {
                        new Domain.Entities.TrickRelationship{PrerequisiteId="backwards-roll"}
                    }
                }
                );

            //dbContext.Submissions.AddRange(
            //    new Domain.Entities.Submission
            //    {
            //        TrickId = "back-flip",
            //        Description = "test desceiption",
            //        Video = "vid1.mp4",
            //        VideoProcessed=true,
            //    },
            //    new Domain.Entities.Submission
            //    {
            //        TrickId = "back-flip",
            //        Description = "test desceiption",
            //        Video = "vid2.mp4",
            //        VideoProcessed = true,
            //    }
            //    );
            dbContext.SaveChanges();
        }
    }
}
