namespace FreyaMobile.Core.Services.Interfaces
{
    public interface INativeMediaService
    {
        bool SaveImageFromByte(byte[] imageByte, string filename);
    }
}
