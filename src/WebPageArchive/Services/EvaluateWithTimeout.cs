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
            // It's important to await the task to properly observe any exception
            // that may have occurred during the script evaluation. Simply checking which task
            // completed first is not enough — without awaiting, potential exceptions
            // inside evalTask would remain unobserved and could lead to silent failures
            // or background task exceptions later.
            await evalTask;
        }
    }
}
