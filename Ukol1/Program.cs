Random random = new Random();
int[] numbers = new int[1000000];

for (int i = 0; i < numbers.Length; i++)
{
    numbers[i] = random.Next(1, 20);
}

var duplicates = numbers
    .GroupBy(x => x)
    .Select(g => new { Number = g.Key, Count = g.Count() })
    .Where(g => g.Count > 1)
    .ToList();

Console.WriteLine($"Nalezeno {duplicates.Count()} duplicitních čísel.");

foreach (var duplicate in duplicates)
{
    Console.WriteLine($"Cislo {duplicate.Number} je v poli {duplicate.Count} krát.");
}

