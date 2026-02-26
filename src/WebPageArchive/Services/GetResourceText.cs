using System.Reflection;
using WebPageArchive.Services.Abstractions;

namespace WebPageArchive.Services;

class GetResourceText : IGetResourceText
{
    public string Execute(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        using (var stream = assembly.GetManifestResourceStream(resourceName))
        {
            if (stream == default)
                throw new InvalidOperationException($"Invalid resource name {resourceName}");

            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
