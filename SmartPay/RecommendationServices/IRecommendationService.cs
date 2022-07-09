using SmartPay.Models;

namespace SmartPay.RecommendationServices;

public interface IRecommendationService
{
    public Task<List<Recommendation>> GetRecommendations(HttpContext context);
}