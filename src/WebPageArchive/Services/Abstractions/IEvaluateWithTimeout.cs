using Microsoft.Playwright;
using WebPageArchive.Dto;

namespace WebPageArchive.Services.Abstractions;

interface IEvaluateWithTimeout
{
    Task Execute(IPage page, PageEvaluateSettings settings);
}
