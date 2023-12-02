using System.Diagnostics;
using System.Reflection;

List<Game> games = Load();

var possibleGames = FindPossibleGames(games, 12, 13, 14);

int idSum = (from game in possibleGames select game.Id).Sum();
Console.WriteLine($"Matching games id sum: {idSum}");

int powerSum = (from game in games select game.Power).Sum();
Console.WriteLine($"Sum of game powers: {powerSum}");
Debug.Assert(powerSum == 84911);

List<Game> Load()
{
    // Load the input file
    Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Day2.input.txt");
    if (stream == null)
    {
        Console.WriteLine("Failed to load input.");
        return [];
    }

    // Parse each line
    StreamReader reader = new(stream);
    List<Game> games = new();
    while (!reader.EndOfStream)
    {
        string? line = reader.ReadLine();
        if (line != null)
        {
            // Id
            int idStart = line.IndexOf("Game ") + 5;
            int idEnd = line.IndexOf(":");
            int id = int.Parse(line.Substring(idStart, idEnd - idStart));

            // Rounds
            var roundLines = line.Substring(idEnd + 1).Split(";");
            List<Round> rounds = new();
            foreach(string roundLine in roundLines)
            {
                int red = 0;
                int green = 0;
                int blue = 0;
                var colors = roundLine.Split(",");
                foreach(string color in colors)
                {
                    int count = int.Parse(color.Substring(1, color.LastIndexOf(" ")));
                    if (color.EndsWith("red"))
                    {
                        red = count;   
                    }
                    else if (color.EndsWith("green"))
                    {
                        green = count;
                    }
                    else
                    {
                        blue = count;
                    }
                    
                }
                rounds.Add(new Round(red, green, blue));
            }

            games.Add(new Game(id, rounds));
        }
    }

    return games;
}

IEnumerable<Game> FindPossibleGames(List<Game> games, int maxRed, int maxGreen, int maxBlue)
{
    var match = from game in games
                where game.MaxRed <= maxRed
                where game.MaxGreen <= maxGreen
                where game.MaxBlue <= maxBlue
                select game;

    return match;
}


record Game(int Id, List<Round> Rounds)
{
    public int MaxRed
    {
        get
        {
            return (from round in Rounds select round.Red).DefaultIfEmpty().Max();
        }
    }

    public int MaxGreen
    {
        get
        {
            return (from round in Rounds select round.Green).DefaultIfEmpty().Max();
        }
    }

    public int MaxBlue
    {
        get
        {
            return (from round in Rounds select round.Blue).DefaultIfEmpty().Max();
        }
    }

    public int Power
    {
        get
        {
            return MaxRed * MaxGreen * MaxBlue;
        }
    }
}

record Round(int Red, int Green, int Blue)
{
    
}
