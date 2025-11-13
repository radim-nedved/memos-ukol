using Ukol3;

Person pickard = new Person("Jean Luc Pickard");
Person riker = new Person("William Riker");
Person troi = new Person("Deanna Troi");
Person forge = new Person("Geordi La Forge");
Person mog = new Person("Wolfgang Mog");
Person guinan = new Person("Guinan");
Person crusher = new Person("Beverly Crusher");
Person lwaxanaTroi = new Person("Lwaxana Troi");
Person barkley = new Person("Reginald Barkley");
Person data = new Person("Mr. Data");
Person obrien = new Person("Miles O'Brien");
Person yar = new Person("Tasha Yar");
Person ehleyr = new Person("K Ehleyr");
Person rozhenko = new Person("Alexander Rozhenko");
Person wessleyCrusher = new Person("Wesley Crusher");
Person ogawa = new Person("Alyssa Ogawa");
Person bashir = new Person("Julian Bashir");


pickard.AddSubordinate(riker);
pickard.AddSubordinate(troi);
pickard.AddSubordinate(forge);

riker.AddSubordinate(mog);
riker.AddSubordinate(guinan);
riker.AddSubordinate(crusher);

troi.AddSubordinate(lwaxanaTroi);
troi.AddSubordinate(barkley);

forge.AddSubordinate(data);
forge.AddSubordinate(obrien);

mog.AddSubordinate(yar);
mog.AddSubordinate(ehleyr);

ehleyr.AddSubordinate(rozhenko);

crusher.AddSubordinate(wessleyCrusher);
crusher.AddSubordinate(ogawa);

ogawa.AddSubordinate(bashir);


var subordinates = GetAllSubordinates(mog);

foreach (var sub in subordinates)
{
    Console.WriteLine(sub.Name);
}

Console.WriteLine();

PrintInfection(crusher, pickard);

List<Person> GetAllSubordinates(Person boss)
{
    var result = new List<Person>();

    if (boss == null)
    {
        return result;
    }

    foreach (var sub in boss.Subordinates)
    {
        result.Add(sub);
        result.AddRange(GetAllSubordinates(sub));
    }

    return result;
}


void PrintInfection(Person first, Person captain)
{
    if (first == null)
        return;

    if (captain == null)
        return;

    var visited = new HashSet<Person>();
    visited.Add(first);

    var currentWave = new List<Person> { first };
    int step = 0;

    while (currentWave.Count > 0)
    {
        Console.WriteLine($"Krok {step}:");

        bool captainInWave = false;
        var nextWave = new List<Person>();

        foreach (var current in currentWave)
        {
            Console.WriteLine($"Nakazen: {current.Name}");
            if (current == captain)
            {
                captainInWave = true;
                continue;
            }

            if (current.Boss != null && !visited.Contains(current.Boss))
            {
                visited.Add(current.Boss);
                nextWave.Add(current.Boss);
            }

            foreach (var sub in current.Subordinates)
            {
                if (!visited.Contains(sub))
                {
                    visited.Add(sub);
                    nextWave.Add(sub);
                }
            }
        }

        Console.WriteLine("--------");

        if (captainInWave)
            break;

        currentWave = nextWave;
        step++;
    }
}