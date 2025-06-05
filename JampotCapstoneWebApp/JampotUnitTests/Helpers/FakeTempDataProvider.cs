﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

public class FakeTempDataProvider : ITempDataProvider
{
    public IDictionary<string, object> LoadTempData(HttpContext context)
    {
        return new Dictionary<string, object>();
    }

    public void SaveTempData(HttpContext context, IDictionary<string, object> values)
    {
        // No action needed for testing.
    }
}