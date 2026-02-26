namespace WebPageArchive.Dto;

record Request
(
    string Url,
    bool UseDefaultWaitScript,
    string? WaitScript,
    int? WaitTimeoutMs
);
