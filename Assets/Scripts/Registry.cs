using System.Collections.Generic;

public class Registry<T>
{
    private readonly List<T> _values = new List<T>();
    
    public List<T> GetValues() => _values;
}