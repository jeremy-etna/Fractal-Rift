// public static class WalkableMap
// {
    // public static readonly bool[,] Map = new bool[16, 16]
    // {
    //     { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true },
    //     { true, true, true, true, false, true, true, true, true, true, true, true, true, true, true, true },
    //     { true, true, true, true, false, true, true, false, false, false, true, true, true, false, true, true },
    //     { true, true, true, true, false, true, true, true, true, true, true, true, true, false, true, true },
    //     { true, true, true, true, false, false, false, false, false, false, false, true, true, true, true, true },
    //     { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true },
    //     { true, true, false, false, false, true, true, true, false, true, true, false, false, false, true, true },
    //     { true, true, true, true, true, true, false, true, true, true, false, true, true, true, true, true },
    //     { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true },
    //     { true, true, true, false, false, false, true, true, true, true, false, false, false, true, true, true },
    //     { true, true, true, false, true, false, true, true, true, true, false, true, false, true, true, true },
    //     { true, true, true, false, false, false, true, true, true, true, false, false, false, true, true, true },
    //     { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true },
    //     { true, true, true, true, true, true, true, false, false, false, true, true, true, true, true, true },
    //     { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true },
    //     { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true }
    // };
// }

public static class WalkableMap
{
    public static readonly bool[,] Map = new bool[10, 11]
    {
        { false, false, true, true, true, true, true, true, true, false, false },
        { false, false, true, false, false, false, false, false, true, false, false },
        { true, true, true, false, false, false, false, false, true, true, true },
        { true, false, true, true, true, true, true, true, true, false, true },
        { true, true, true, true, true, false, true, true, true, true, true },
        { true, false, false, true, true, true, true, true, false, false, true },
        { true, false, false, true, true, false, true, true, false, false, true },
        { true, true, true, true, false, false, false, true, true, true, true },
        { true, true, true, false, false, true, false, false, true, true, true },
        { true, true, true, true, true, true, true, true, true, true, true }
    };
}

