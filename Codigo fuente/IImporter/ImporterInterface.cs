using Models.In;

namespace IImporter
{
    public interface ImporterInterface
    {
        string GetName();

        List<ImportedDevice> ImportDevice();
    }
}
