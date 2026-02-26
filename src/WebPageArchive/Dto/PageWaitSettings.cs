namespace WebPageArchive.Dto;

sealed record PageWaitSettings
{
    public int TimeoutMs { get; init; }
}

