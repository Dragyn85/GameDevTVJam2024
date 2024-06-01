using UnityEngine;

public interface IAlienCounter
{
    void       AdjustCount(int delta);
    public int Count { get; }
}
