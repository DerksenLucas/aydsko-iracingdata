﻿// © 2022 Adrian Clark
// This file is licensed to you under the MIT license.

namespace Aydsko.iRacingData.Hosted;

public class CarTypes
{
    [JsonPropertyName("car_type")]
    public string CarType { get; set; } = default!;
}