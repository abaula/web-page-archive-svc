using Microsoft.Extensions.Options;
using WebPageArchive.Const;
using WebPageArchive.Dto;
using WebPageArchive.Services.Abstractions;

namespace WebPageArchive.Services;

class GetPageEvaluateSettings : IGetPageEvaluateSettings
{
    private readonly PageWaitSettings _settings;
    private readonly Lazy<IGetResourceText> _getResourceText;

    public GetPageEvaluateSettings(IOptions<PageWaitSettings> settings,
        Lazy<IGetResourceText> getResourceText)
    {
        _settings = settings.Value;
        _getResourceText = getResourceText;
    }

    public PageEvaluateSettings? Execute(Request request)
    {
        var waitScript = request.WaitScript;

        if (string.IsNullOrWhiteSpace(waitScript))
        {
            if (!request.UseDefaultWaitScript)
                return default;

            // Get default script from resource.
            waitScript = _getResourceText.Value.Execute(ResourceConst.Scripts.PageEvaluateDefault);
        }

        var timeout = TimeSpan.FromMilliseconds(request.WaitTimeoutMs ?? _settings.TimeoutMs);

        return new PageEvaluateSettings(waitScript!, timeout);
    }
}
