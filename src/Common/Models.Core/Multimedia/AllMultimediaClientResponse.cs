namespace Models.Core.Multimedia;

public class AllMultimediaClientResponse
{
    public IEnumerable<BaseMultimedia> Videos { get; set; } = new List<BaseMultimedia>();

    public IEnumerable<BaseMultimedia> Ultrasounds { get; set; } = new List<BaseMultimedia>();

    public IEnumerable<BaseMultimedia> Sounds { get; set; } = new List<BaseMultimedia>();

}
