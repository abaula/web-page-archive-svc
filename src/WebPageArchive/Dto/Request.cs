namespace WebPageArchive.Dto;

record Request
(
    string Url,
    string? WaitScript,
    int? WaitTimeoutMs
);
