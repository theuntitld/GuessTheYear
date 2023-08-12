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
        { 1,  1950},
        { 2,  1960},
        { 3,  1970},
        { 4,  1980},
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
