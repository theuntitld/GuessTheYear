using System.Text.Json.Serialization;

namespace GuessTheYear.Data;

public class GamePlay
{
    public GamePlay() {
        var validCodes = new List<string>() {
            "e7nrs9n6os", "rpk3a8lywk", "qsz4xamqer", "c1v3z5ox5k", "5lo42wg0s6", "1ehyis6gco", "9hci20obim",
            "llp7yu7w91", "gr9nsosjkq", "nfiu995atd", "va1ascb9rc", "hpm4fmg8gb", "m34fyr5hht", "bn58hudd61",
            "ubgprtokml", "73az1zl1i4", "3fzuckbkc4", "iywboa393v", "7xd2jtq04l", "c9iyc4shac", "lre9nt5sdq",
            "y8u4jkr133", "jocme7e65q", "bxxu7ttz90", "cz9ubc1uxa", "1q3jt2hwgh", "g3vbw96nyu", "drh67nrpg1", 
            "rq9t55an1z", "w8ylt2uyvp", "ao7y19b8r2", "99h7fkw4gi", "dvo4zyjuzu", "qpahabeaqf", "ikzafgq55y", 
            "0iqyr9opfd", "1nmdn2qitq", "kqya87lqbd", "czln1skfr9", "zfmtjcdxb4", "2i8pztuvnj", "681ik30wy9",
            "p3gu626wn7", "9i78d471ng", "lv8wfy915y", "ivs5qqtwnt", "eq0u787it2", "5f8k8j68lx", "t8qtqbm220",
            "w1928qbla3",
        };

        foreach (var code in validCodes)
        {
            this.Players.Add(new Player { Id = code });
        }
    }

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
        { 1,  1945},
        { 2,  1960},
        { 3,  1970},
        { 4,  1980},
    };

    public Player AddPlayer(string code, string name)
    {
        Player? player = Players.FirstOrDefault(x => x.Id == code);

        if (player is null)
        {
            return null;
        }

        player.Enrolled = true;
        player.Name = name;

        OnStageChanged();

        return player;
    }

    public void RemovePlayer(Player player)
    {
        player.Enrolled = false;
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

    [JsonIgnore]
    public bool Enrolled { get; set; }
}
