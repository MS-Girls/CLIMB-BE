using System;

namespace CLIMB_BE.Services;

public interface IOCRServices
{
    Task<string> ReadDocument(string blobUrl);
}
