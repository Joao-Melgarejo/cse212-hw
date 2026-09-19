// TODO Problem 5 - ADD YOUR CODE HERE
// Create additional classes as necessary
//
// PLAN (written before implementing):
// The USGS "all_day.geojson" feed looks like this (trimmed):
//
// {
//   "type": "FeatureCollection",
//   "metadata": { ... },
//   "features": [
//     {
//       "type": "Feature",
//       "properties": { "mag": 2.36, "place": "1km NE of Pahala, Hawaii", ... },
//       "geometry": { ... },
//       "id": "hv74501917"
//     },
//     ...
//   ]
// }
//
// JSON is nothing more than a map of maps, and a C# class is a map whose keys are
// fixed at compile time. So each level of the JSON becomes one class, and each key we
// care about becomes one property:
//   "features"            -> Feature[] Features        (a JSON array => a C# array)
//   "properties"          -> Properties Properties     (a nested object => a class)
//   "place" / "mag"       -> string Place / double? Mag
//
// Two decisions worth defending:
// 1. We only declare the properties we actually use. System.Text.Json simply ignores
//    every other key in the JSON ("geometry", "id", "metadata", ...), so there is no
//    need to model the whole feed.
// 2. Mag is double? (nullable) and not double: the real feed does publish records with
//    "mag": null, and a non-nullable double would throw during deserialization.
// The names match the JSON keys except for casing, and the caller already passes
// PropertyNameCaseInsensitive = true, so no [JsonPropertyName] attributes are needed.

public class FeatureCollection
{
    public Feature[] Features { get; set; }
}

public class Feature
{
    public Properties Properties { get; set; }
}

public class Properties
{
    public string Place { get; set; }

    public double? Mag { get; set; }
}