using System.Text.Json.Serialization;

namespace GuessTheYear.Data;

public class GamePlay
{
    public event EventHandler StageChanged = default!;

    private int _stage;
    public int Stage
    {
        get => _stage;
        set
        {
            if (_stage != value)
            {
                _stage = value;
                OnStageChanged();
            }
        }
    }

    public List<Player> Players = new() { };

    public Dictionary<int, int> Stages = new()
    {
        { 1,  1907},
        { 2,  1928},
        { 3,  1930},
        { 4,  1965},
        { 5,  1969},
        { 6,  1989},
        { 7,  1992},
        { 8,  1968},
        { 9,  1979},
        { 10,  1968},
        { 11,  1916},
        { 12,  1966},
        { 13,  1990},
        { 14,  2011},
        { 15,  1999},
        { 16,  1963},
        { 17,  1932},
        { 18,  1936},
        { 19,  1937},
        { 20,  1936},
        { 21,  1960},
        { 22,  1943},
        { 23,  1946},
        { 24,  1934},
        { 25,  1955},
        { 26,  1961},
        { 27,  1960},
        { 28,  2013},
        { 29,  1945},
        { 30,  1908},
    };

    public Player? AddPlayer(string code, string name)
    {
        Player? player = Players.FirstOrDefault(x => x.Id == code);

        if (player is not null)
        {
            return player;
        }

        player = new Player
        {
            Id = code,
            Name = name
        };

        this.Players.Add(player);

        OnStageChanged();

        return player;
    }

    public void RemovePlayer(Player player)
    {
        Players.Remove(player);
        OnStageChanged();
    }

    public void Answer(Player player, int stage, int answer)
    {
        //Already Answered
        if (player.Answers.Keys.Contains(stage))
            return;

        //The stage is completed.Reject the answer
        if (stage != this.Stage)
            return;

        player.Answers[stage] = answer;

        var score = 0;

        foreach (var stageAnswer in player.Answers)
        {
            var actualValue = Stages[stageAnswer.Key];
            var guess = stageAnswer.Value; ;
            var difference = Math.Abs(actualValue - guess);

            score += Math.Max(0, 100 - difference);
        }

        player.Score = score;

        OnStageChanged();
    }

    protected virtual void OnStageChanged()
    {
        StageChanged?.Invoke(this, EventArgs.Empty);
    }
}

public class Player {
    public string Id { get; set; }  = default!;
    public string Name { get; set; } = default!;
    public int Score { get; set; }

    [JsonIgnore]
    public Dictionary<int, int> Answers { get; set; } = new Dictionary<int, int> { };
}
