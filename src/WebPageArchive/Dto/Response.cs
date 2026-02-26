namespace WebPageArchive.Dto;

record Response
(
    string OriginalUrl,
    byte[] ZipArchive,
    bool Timeout
);

