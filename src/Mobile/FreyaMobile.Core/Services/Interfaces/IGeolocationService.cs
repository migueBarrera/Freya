﻿namespace FreyaMobile.Core.Services.Interfaces;

public interface IGeolocationService
{
    Task<Location?> GetLastKnownLocationAsync();
}
