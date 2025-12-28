using System;
using System.Text;
using System.Collections.Generic;


public class Player
{
    public string Name { get; set; }
    public int Win { get; set; }
    public int Lose { get; set; }

    public static List<Player> players = new List<Player>();

    public Player()
    {
        Console.Write("Name: ");
        var input = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(input))
        {
            Name = input;
        }
        else
        {
            Name = "Unknown";
        }
        Console.Write("Wins: ");
        Win = int.Parse(Console.ReadLine() ?? "0");
        Console.Write("Losses: ");
        Lose = int.Parse(Console.ReadLine() ?? "0");
    }

    public Player(string name, int win, int lose)
    {
        Name = name;
        Win = win;
        Lose = lose;
        players.Add(this);
    }

    public static void UpdateStats(string player, bool isWin)
    {
        Player p = AlgorithmSpecialist.Search(player);
        if (p != null)
        {
            if (isWin) p.Win++;
            else p.Lose++;
        }
    }

    //Ichiroku's note: read/write functions to save player data (not used currently)
    public static void WriteFile(string FilePath)
    {
        StreamWriter w = null;
        try
        {
            w = new StreamWriter(FilePath + ".txt", false, Encoding.UTF8);
            foreach (var player in players)
            {
                w.WriteLine($"{player.Name},{player.Win},{player.Lose}");
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine("Error writing players file: " + ex.Message);
            throw;
        }
        finally
        {
            w?.Close();
        }
    }

    public static void ReadFile(string filePath)
    {
        StreamReader r = null;
        players = new List<Player>();
        try
        {
            if (File.Exists(filePath + ".txt"))
            {
                string? line;
                r = new StreamReader(filePath + ".txt", Encoding.UTF8);
                while ((line = r.ReadLine()) != null)
                {
                    string[] values = line.Split(',');
                    new Player(values[0], int.Parse(values[1]), int.Parse(values[2]));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error reading players file: " + ex.Message);
            throw;
        }
        finally
        {
            r?.Close();
        }
    }
}