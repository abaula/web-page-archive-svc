namespace WebPageArchive.Dto;

sealed record PageWaitSettings
{
    public bool UseDefaultScript { get; init; }
    public int TimeoutMs { get; init; }
}

