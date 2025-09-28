using MongoDB.Bson;
using MongoDB.Driver;
using SoftCare.Models;

namespace SoftCare.Repository;

public class QuestionRepository(IMongoDatabase database) : IQuestionRepository
{
    private readonly IMongoCollection<QuestionBank> _questionBankCollection = database.GetCollection<QuestionBank>(COLLECTIONNAME);
    
    private const string COLLECTIONNAME = "question_bank";

    public async Task<QuestionBank>GetQuestionBankCodeAsync(string questionCode)
    {
        var question = await _questionBankCollection.Find(q => q.QuestionCode == questionCode).FirstOrDefaultAsync();

        return question;
    }

    public async Task<QuestionBank> GetNextQuestionBankCategoryAsync(string category, string? lastId)
    {
        var filtroCategory = Builders<QuestionBank>.Filter.Eq(doc => doc.Category, category);

        FilterDefinition<QuestionBank> filtroCursor;

        if (string.IsNullOrEmpty(lastId))
        {
            filtroCursor = Builders<QuestionBank>.Filter.Empty;
        }
        else
        {
            if (!ObjectId.TryParse(lastId, out _))
            {
                throw new ArgumentException("O 'lastId' fornecido não é um ObjectId válido.", nameof(lastId));
            }
            filtroCursor = Builders<QuestionBank>.Filter.Gt(q => q.Id, lastId);
        }

        var filtroCompleto = Builders<QuestionBank>.Filter.And(filtroCategory, filtroCursor);

        return await _questionBankCollection
            .Find(filtroCompleto)
            .SortBy(q => q.Id)
            .Limit(1)
            .FirstOrDefaultAsync();
    }

    public async Task<QuestionBank> GetAllQuestionBankAsync(string? lastId)
    {
        FilterDefinition<QuestionBank> filtroCursor;

        if (string.IsNullOrEmpty(lastId))
        {
            filtroCursor = Builders<QuestionBank>.Filter.Empty;
        }
        else
        {
            if (!ObjectId.TryParse(lastId, out _))
            {
                throw new ArgumentException("O 'lastId' fornecido não é um ObjectId válido.", nameof(lastId));
            }
            filtroCursor = Builders<QuestionBank>.Filter.Gt(q => q.Id, lastId);
        }
        
        return await _questionBankCollection
            .Find(filtroCursor)
            .SortBy(q => q.Id)
            .Limit(1)
            .FirstOrDefaultAsync();
    }
}