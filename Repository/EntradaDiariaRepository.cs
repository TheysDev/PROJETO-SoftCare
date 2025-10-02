using MongoDB.Bson;
using MongoDB.Driver;
using SoftCare.Dtos.Questoes;
using SoftCare.Dtos.Respostas;
using SoftCare.Models;
using SoftCare.Retornos;

namespace SoftCare.Repository;

public class EntradaDiariaRepository(IMongoDatabase database) : IEntradaDiariaRepository
{
    private readonly IMongoCollection<DailyEntry> _dailyEntryCollection =
        database.GetCollection<DailyEntry>(COLLECTIONNAME);

    private const string COLLECTIONNAME = "daily_entries";

    public async Task<Result<string>> RegistrarEntradaDiariaAsync(DailyEntry check)
    {
        await _dailyEntryCollection.InsertOneAsync(check);

        return Result<string>.Ok(check.Id);
    }
    public async Task<List<ResumoRespostasDto>> BuscarDezUltimasEntradasAsync(string userId)
    {
        var pipeline = new BsonDocument[]
        {
            new("$match", new BsonDocument("user_id", new ObjectId(userId))),
            new("$sort", new BsonDocument("date", -1)),
            new("$limit", 10),
            
            new("$unwind", "$answers"),
            new("$group", new BsonDocument
            {
                {
                    "_id", new BsonDocument
                    {
                        { "question_code", "$answers.question_code" },
                        { "value", "$answers.value" }
                    }
                },
                { "count", new BsonDocument("$sum", 1) }
            }),
            
            new("$lookup", new BsonDocument
            {
                { "from", "question_bank" },
                { "localField", "_id.question_code" },
                { "foreignField", "code" },
                { "as", "question_details" }
            }),
            new("$unwind", "$question_details"),
            new("$addFields", new BsonDocument
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
                                                new BsonDocument("$eq", new BsonArray { "$$opt.value", "$_id.value" })
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
            
            new("$group", new BsonDocument
            {
                { "_id", "$_id.question_code" },
                { "question_text", new BsonDocument("$first", "$question_details.text") },
                {
                    "contagem_por_resposta", new BsonDocument
                    {
                        {
                            "$push", new BsonDocument
                            {
                                { "Resposta", "$answer_label" },
                                { "Quantidade", "$count" }
                            }
                        }
                    }
                }
            }),
            new("$project", new BsonDocument
            {
                { "_id", 0 },
                { "CodigoDaPergunta", "$_id" },
                { "TextoDaPergunta", "$question_text" },
                { "ContagemDetalhada", "$contagem_por_resposta" }
            })
        };
        
        var analysisResult = await _dailyEntryCollection.Aggregate<ResumoRespostasDto>(pipeline).ToListAsync();

        return analysisResult;
    }

    public async Task<List<ResumoCategoriaDto>> ResumoDasDezUltimasEntradasPorCategoriaAsync(string userId)
    {
        var pipeline = new BsonDocument[]
        {
            new("$match", new BsonDocument("user_id", new ObjectId(userId))),
            new("$sort", new BsonDocument("date", -1)),
            new("$limit", 10),
            
            new("$unwind", "$answers"),
            new("$lookup", new BsonDocument
            {
                { "from", "question_bank" },
                { "localField", "answers.question_code" },
                { "foreignField", "code" },
                { "as", "question_details" }
            }),
            new("$unwind", "$question_details"),
            
            new("$addFields", new BsonDocument
            {
                { "answer_label", new BsonDocument
                    {
                        { "$let", new BsonDocument
                            {
                                { "vars", new BsonDocument("matched_option", new BsonDocument("$filter", new BsonDocument {
                                    { "input", "$question_details.options" }, { "as", "opt" },
                                    { "cond", new BsonDocument("$eq", new BsonArray { "$$opt.value", "$answers.value" }) }
                                }))},
                                { "in", new BsonDocument("$arrayElemAt", new BsonArray { "$$matched_option.label", 0 }) }
                            }
                        }
                    }
                }
            }),
            
            new("$project", new BsonDocument
            {
                { "_id", 0 },
                { "Categoria", "$question_details.category" },
                { "RespostaTexto", "$answer_label" }
            })
        };
        return await _dailyEntryCollection.Aggregate<ResumoCategoriaDto>(pipeline).ToListAsync();
    }
}
    
        