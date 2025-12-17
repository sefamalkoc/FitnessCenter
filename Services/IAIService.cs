using FitnessCenter.Models;

namespace FitnessCenter.Services
{
    public interface IAIService
    {
        Task<FitnessPlanResult> GenerateFitnessPlanAsync(FitnessPlanRequest request);
        Task<VisualizationResult> GenerateVisualizationAsync(VisualizationRequest request);
    }
}
