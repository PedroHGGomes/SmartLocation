using Microsoft.ML.Data;

namespace SmartLocation.ML;

public class ManutencaoData
{
    [LoadColumn(0)] public float Km { get; set; }
    [LoadColumn(1)] public float Falhas { get; set; }
    [LoadColumn(2)] public float IdadeMeses { get; set; }
    [LoadColumn(3)] public bool Label { get; set; } // 1 = precisou manutenção
}

public class ManutencaoPrediction
{
    [ColumnName("PredictedLabel")] public bool Predito { get; set; }
    public float Probability { get; set; }
    public float Score { get; set; }
}