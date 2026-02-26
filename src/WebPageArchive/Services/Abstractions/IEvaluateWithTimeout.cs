using Microsoft.Playwright;
using WebPageArchive.Dto;

namespace WebPageArchive.Services.Abstractions;

interface IEvaluateWithTimeout
{
    /// <summary>
    /// Executes a script on the specified page with a timeout.
    /// Returns <c>true</c> if a timeout occurred, otherwise <c>false</c>.
    /// </summary>
    /// <param name="page">
    /// The browser page on which the script will be evaluated.
    /// </param>
    /// <param name="settings">
    /// Evaluation settings, including the script to execute, timeout value,
    /// and any additional options required for running the script.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result is <c>true</c> if the evaluation was stopped
    /// due to a timeout; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> Execute(IPage page, PageEvaluateSettings settings);
}
