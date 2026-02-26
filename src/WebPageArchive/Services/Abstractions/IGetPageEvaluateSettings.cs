using WebPageArchive.Dto;

namespace WebPageArchive.Services.Abstractions;

interface IGetPageEvaluateSettings
{
    PageEvaluateSettings? Execute(Request request);
}
