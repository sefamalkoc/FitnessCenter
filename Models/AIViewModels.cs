namespace FitnessCenter.Models
{
    public class FitnessPlanRequest
    {
        public int Height { get; set; } // cm
        public int Weight { get; set; } // kg
        public string BodyType { get; set; } = string.Empty; // Ectomorph, Mesomorph, Endomorph
        public string Goal { get; set; } = string.Empty; // Lose weight, Build muscle, etc.
        public string Gender { get; set; } = string.Empty;
        public int Age { get; set; }
    }

    public class FitnessPlanResult
    {
        public string PlanContent { get; set; } = string.Empty;
    }

    public class VisualizationRequest
    {
        public string Description { get; set; } = string.Empty;
    }

    public class VisualizationResult
    {
        public string ImageUrl { get; set; } = string.Empty;
    }
}
