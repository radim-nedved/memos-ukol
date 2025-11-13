namespace Ukol3;

internal class Person
{
    public string Name { get; set; }
    public Person? Boss { get; set; }
    public List<Person> Subordinates { get; set; } = new List<Person>();

    public Person(string name)
    {
        Name = name;
    }

    public void AddSubordinate(Person subordinate)
    {
        ArgumentNullException.ThrowIfNull(subordinate);

        Subordinates.Add(subordinate);
        subordinate.Boss = this;
    }
}
