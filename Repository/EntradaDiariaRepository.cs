using MongoDB.Bson;
using MongoDB.Driver;
using SoftCare.Dtos.Analise;
using SoftCare.Models;
using SoftCare.Retornos;

namespace SoftCare.Repository;

public class EntradaDiariaRepository(IMongoDatabase database) : IEntradaDiariaRepository
{
    private readonly IMongoCollection<DailyEntry> _dailyEntryCollection = database.GetCollection<DailyEntry>(COLLECTIONNAME);
    
    private const string COLLECTIONNAME = "daily_entries";
    
    public async Task<Result<string>> RegistrarEntradaDiariaAsync(DailyEntry check)
    {
        await _dailyEntryCollection.InsertOneAsync(check);
        
        return Result<string>.Ok(check.Id);
    }

    public async Task<List<ItemAnalise>> MontarAnaliseComCategoriaAsync(string userId, string category, DateTime startDate,
        DateTime endDate)
    {
        var pipeline = new BsonDocument[]
        {
            new BsonDocument("$match", new BsonDocument
            {
                { "user_id", userId },
                { "date", new BsonDocument { { "$gte", startDate }, { "$lte", endDate } } }
            }),
            new BsonDocument("$unwind", "$answers"),
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "question_bank" },
                { "localField", "answers.question_code" },
                { "foreignField", "code" },
                { "as", "question_details" }
            }),
            new BsonDocument("$unwind", "$question_details"),
            new BsonDocument("$match", new BsonDocument("question_details.category", category)),
            new BsonDocument("$addFields", new BsonDocument
            {
                {
                    "answer_label", new BsonDocument
                    {
                        {
                            "$let", new BsonDocument
                            {
                                {
                                    "vars", new BsonDocument("matched_option", new BsonDocument("$filter",
                                        new BsonDocument
                                        {
                                            { "input", "$question_details.options" },
                                            { "as", "opt" },
                                            {
                                                "cond",
                                                new BsonDocument("$eq",
                                                    new BsonArray { "$$opt.value", "$answers.value" })
                                            }
                                        }))
                                },
                                {
                                    "in",
                                    new BsonDocument("$arrayElemAt", new BsonArray { "$$matched_option.label", 0 })
                                }
                            }
                        }
                    }
                }
            }),
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", "$answer_label" },
                { "Contagem", new BsonDocument("$sum", 1) }
            }),
            new BsonDocument("$project", new BsonDocument
            {
                { "_id", 0 },
                { "Resposta", "$_id" },
                { "Contagem", "$Contagem" }
            })
        };

        var analise = await _dailyEntryCollection.Aggregate<ItemAnalise>(pipeline).ToListAsync();
        return analise;
    }
}