using Microsoft.ML;

namespace SmartLocation.ML;

public class ManutencaoMlService
{
    private readonly MLContext _ml;
    private readonly PredictionEngine<ManutencaoData, ManutencaoPrediction> _engine;

    public ManutencaoMlService()
    {
        _ml = new MLContext();

        var dados = new List<ManutencaoData>
        {
            new ManutencaoData{ Km=1000, Falhas=0, IdadeMeses=3, Label=false },
            new ManutencaoData{ Km=5000, Falhas=2, IdadeMeses=12, Label=true },
            new ManutencaoData{ Km=8000, Falhas=1, IdadeMeses=10, Label=true },
            new ManutencaoData{ Km=2000, Falhas=0, IdadeMeses=4, Label=false },
            new ManutencaoData{ Km=12000, Falhas=3, IdadeMeses=18, Label=true },
            new ManutencaoData{ Km=3000, Falhas=0, IdadeMeses=5, Label=false },
        };

        var train = _ml.Data.LoadFromEnumerable(dados);

        var pipeline = _ml.Transforms.Concatenate("Features", nameof(ManutencaoData.Km), nameof(ManutencaoData.Falhas), nameof(ManutencaoData.IdadeMeses))
            .Append(_ml.BinaryClassification.Trainers.FastTree());

        var model = pipeline.Fit(train);
        _engine = _ml.Model.CreatePredictionEngine<ManutencaoData, ManutencaoPrediction>(model);
    }

    public ManutencaoPrediction Prever(float km, float falhas, float idadeMeses)
    {
        var input = new ManutencaoData { Km = km, Falhas = falhas, IdadeMeses = idadeMeses };
        return _engine.Predict(input);
    }
}