public class AlgorithmSpecialist
{
    public static List<Player> players = Player.players;


    public AlgorithmSpecialist(List<Player> players)
    {
        AlgorithmSpecialist.players = players;
    }

    // 🔹 Selection Sort: sắp xếp người chơi theo số trận thắng (giảm dần)
    public static void DisplayLeaderboard()
    {
        for (int i = 0; i < players.Count - 1; i++)
        {
            int minIndex = i;
            for (int j = i + 1; j < players.Count; j++)
            {
                if (players[j].Win > players[minIndex].Win)
                {
                    minIndex = j;
                }
            }
            // Hoán đổi
            var temp = players[i];
            players[i] = players[minIndex];
            players[minIndex] = temp;
        }
        foreach (Player p in players)
        {
            DisplayPlayer(p);
        }
    }

    // 🔹 Overload: gọi hàm search theo kiểu dữ liệu
    public static Player Search(string name)
    {
        foreach (var player in players)
        {
            if (player.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                return player;
        }
        return null;
    }

    public static void DisplayPlayer(Player p)
    {
        Console.WriteLine($"{p.Name} - Win: {p.Win} - Lose: {p.Lose}");
    }
}