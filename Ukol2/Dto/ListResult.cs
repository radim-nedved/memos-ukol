namespace Ukol2.Dto;

internal record PagedResult<T>(int Count, string? Next, string? Previous, List<T> Results);