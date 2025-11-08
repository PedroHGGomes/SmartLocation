using SmartLocation.ML;
using Xunit;

namespace SmartLocation.UnitTests;

public class MlServiceTests
{
    [Fact]
    public void Prever_DeveRetornarProbabilidade()
    {
        var svc = new ManutencaoMlService();
        var pred = svc.Prever(8000, 2, 12);
        Assert.InRange(pred.Probability, 0f, 1f);
    }
}