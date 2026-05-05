using ApplicationCore.Commons.Repository;
using ApplicationCore.Models.QuizAggregate;
using BackendLab01;

namespace Infrastructure.Memory;
public static class SeedData
{
    public static void Seed(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var provider = scope.ServiceProvider;
            var quizRepo = provider.GetService<IGenericRepository<Quiz, int>>();
            var quizItemRepo = provider.GetService<IGenericRepository<QuizItem, int>>();
            
            
            QuizItem item1 = new QuizItem(id: 1, question: "2+4", correctAnswer: "6", incorrectAnswers: ["5", "7", "8"]);
            QuizItem item2 = new QuizItem(id: 2, question: "2*4", correctAnswer: "8", incorrectAnswers: ["4", "6", "9"]);
            QuizItem item3 = new QuizItem(id: 3, question: "8/2", correctAnswer: "4", incorrectAnswers: ["5", "7", "8"]);
            quizItemRepo?.Add(item1);
            quizItemRepo?.Add(item2);
            quizItemRepo?.Add(item3);
            Quiz quiz1 = new(id: 1, title: "Matematyka", items: [item1, item2, item3]);
            quizRepo?.Add(quiz1);

            QuizItem item4 = new QuizItem(id: 4, question: "Stolica Polski", correctAnswer: "Warszawa", incorrectAnswers: ["Kraków", "Gdańsk", "Poznań"]);
            QuizItem item5 = new QuizItem(id: 5, question: "Stolica Francji", correctAnswer: "Paryż", incorrectAnswers: ["Lyon", "Marsylia", "Nicea"]);
            QuizItem item6 = new QuizItem(id: 6, question: "Stolica Niemiec", correctAnswer: "Berlin", incorrectAnswers: ["Monachium", "Hamburg", "Frankfurt"]);
            quizItemRepo?.Add(item4);
            quizItemRepo?.Add(item5);
            quizItemRepo?.Add(item6);
            Quiz quiz2 = new(id: 2, title: "Geografia", items: [item4, item5, item6]);
            quizRepo?.Add(quiz2);
        }
    }
}