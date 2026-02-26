using Microsoft.Playwright;
using WebPageArchive.Dto;
using WebPageArchive.Services.Abstractions;

namespace WebPageArchive.Services;

class EvaluateWithTimeout : IEvaluateWithTimeout
{
    public async Task Execute(IPage page, PageEvaluateSettings settings)
    {
        var evalTask = page.EvaluateAsync<object>(settings.Script);
        var delayTask = Task.Delay(settings.Timeout);
        var completed = await Task.WhenAny(evalTask, delayTask);

        if (completed == evalTask)
        {
            // важно доawait-ать, чтобы получить исключение, если оно было
            await evalTask;
        }

        // если нужно – можно бросить исключение вместо null
        //throw new TimeoutException(
        //    $"page.EvaluateAsync timed out after {timeoutMs} ms.");
    }
}
