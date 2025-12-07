namespace AoC_2025_Day7;

internal class Manifold
{
    public required int Height { get; init; }
    private List<Splitter> _splitters = new List<Splitter>();

    private List<Beam> _beams = new List<Beam>();

    public void Init(Beam startingBeam)
    {
        _beams.Add(startingBeam);
    }

    public void AddSplitter(Splitter splitter)
    {
        _splitters.Add(splitter);
    }

    public int Run()
    {
        int startRow = _beams.Min(x => x.Row);
        
        for (int currentRow = startRow; currentRow < Height; currentRow++)
        {
            List<Beam> newBeams = new List<Beam>();
            foreach (Beam beam in _beams)
            {
                Splitter? splitter = _splitters.FirstOrDefault(x => x.Row == beam.Row && x.Column == beam.Column);
                
                if (splitter is not null)
                {
                    foreach (Beam newBeam in splitter.Split())
                    {
                       if(!newBeams.Any(x=>x.Row==newBeam.Row && x.Column==newBeam.Column))
                       {
                          newBeams.Add(newBeam);
                       }
                    }                    
                }
                else
                {
                    Beam newBeam = beam.MoveDown();
                    if (!newBeams.Any(x => x.Row == newBeam.Row && x.Column == newBeam.Column))
                    {
                        newBeams.Add(newBeam);
                    }
                }
            }
            _beams = newBeams;
        }

        return _splitters.Count(x=>x.Triggered);
    }
}
