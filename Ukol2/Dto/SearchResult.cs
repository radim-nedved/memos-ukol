namespace Ukol2.Dto;

internal record SearchResult(
    string StarshipName,
    string Model,
    string StarshipClass,
    string Manufacturer,
    string CostInCredits,
    string PilotName
);