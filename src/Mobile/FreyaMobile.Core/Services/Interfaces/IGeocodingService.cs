﻿namespace FreyaMobile.Core.Services.Interfaces;

public interface IGeocodingService
{
    Task<Placemark?> GetPlaceMark(double latidude, double longitude);
}
