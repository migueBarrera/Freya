namespace Freya.Desktop.Updater;

public class ProcessRunnerBuilder
{
    public static Mock<ProcessRunner> BuildMock()
    {
        return new Mock<ProcessRunner>(null, null);
    }
}
