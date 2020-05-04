public class LevelPhase : PhaseSerialComposition
{
    public int LevelID { get; set; }

    public LevelPhase(int id, params PhaseBaseNode[] childNodeArr) 
        : base(id, childNodeArr)
    {
    }
}
