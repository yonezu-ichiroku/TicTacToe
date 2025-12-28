using System;
    
public class Program {
    public const string PLAYER_FILE = "players";
    public const string SAVE_FILE = "save";
    public static void Main(string[] args) 
    {
        Player.ReadFile(Program.PLAYER_FILE);
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Menu.ShowMenu();
    }
}   
