using System;

class Menu
{

    public static void ShowMenu()
    {
        int choice = 1;
        ConsoleKey key;
        do
        {
            Console.Clear();
            Console.WriteLine((choice == 1 ? ">> " : "   ") + "Play new game");
            Console.WriteLine((choice == 2 ? ">> " : "   ") + "Continue saved game");
            Console.WriteLine((choice == 3 ? ">> " : "   ") + "Search player");
            Console.WriteLine((choice == 4 ? ">> " : "   ") + "Current leaderboard");
            Console.WriteLine((choice == 5 ? ">> " : "   ") + "DEV: EDIT PLAYERS DATA");
            Console.WriteLine((choice == 0 ? ">> " : "   ") + "Save and exit");


            key = Console.ReadKey(true).Key;


            switch (key)
            {
                case ConsoleKey.W: case ConsoleKey.UpArrow:
                    choice--;
                    if (choice < 0)
                        choice = 5;
                    break;


                case ConsoleKey.S: case ConsoleKey.DownArrow:
                    choice = (choice + 1) % 6;
                    break;

                case ConsoleKey.Enter:
                    switch (choice)
                    {
                        case 1:
                            GameLogic.TurnCheck();
                            break;
                        case 2:
                            SaveManager.LoadGameFromFile(Program.SAVE_FILE, out char[,] board, out char currentPlayer, out int moves);
                            GameLogic.TurnCheck(true, board, currentPlayer, moves);
                            break;
                        case 3:
                            Console.Clear();
                            Console.Write("Enter player name to search: ");
                            Player p = AlgorithmSpecialist.Search(Console.ReadLine());
                            if (p != null) {
                                Console.Clear();
                                AlgorithmSpecialist.DisplayPlayer(p);
                            }
                            else Console.WriteLine("Player not found!");
                            pause();
                            break;
                        case 4:
                            Console.Clear();
                            AlgorithmSpecialist.DisplayLeaderboard(); 
                            pause();
                            break;
                        case 5:
                            Console.Clear();
                            devMenu();
                            break;
                        case 0:
                            Player.WriteFile(Program.PLAYER_FILE);
                            Console.WriteLine("Game saved. Exiting...");
                            return;
                    }
                break;
                case ConsoleKey.Escape:
                    return;
            }
        } while (true);
    }

    static void devMenu() 
    {
        Console.Clear();
        Console.WriteLine("Developer Mode");
        int choice = 1;
        ConsoleKey key;
        do
        {
            Console.Clear();
            Console.WriteLine((choice == 1 ? ">> " : "   ") + "Import players data");
            Console.WriteLine((choice == 2 ? ">> " : "   ") + "Edit players data");
            Console.WriteLine((choice == 3 ? ">> " : "   ") + "Delete players data");
            Console.WriteLine((choice == 0 ? ">> " : "   ") + "Return");
            key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.W: case ConsoleKey.UpArrow:
                    choice--;
                    if (choice < 0) choice = 1;
                    break;
                case ConsoleKey.S: case ConsoleKey.DownArrow:
                    choice = (choice + 1) % 4;
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    switch (choice)
                    {
                        case 1:
                            Player p = new Player();
                            AlgorithmSpecialist.players.Add(p);
                            Console.WriteLine("Player imported.");
                            pause();
                            break;
                        case 2:
                            Console.Write("Enter player name to search: ");
                            p = AlgorithmSpecialist.Search(Console.ReadLine());
                            if (p != null)
                            {
                                Console.Clear();
                                AlgorithmSpecialist.DisplayPlayer(p);
                                Console.Write("Enter new name: ");
                                var input = Console.ReadLine()?.Trim();
                                if (!string.IsNullOrEmpty(input)) p.Name = input;
                                Console.Write("Enter new wins: ");
                                p.Win = int.TryParse(Console.ReadLine(), out int newWins) ? newWins : p.Win;
                                Console.Write("Enter new losses: ");
                                p.Lose = int.TryParse(Console.ReadLine(), out int newLosses) ? newLosses : p.Lose;
                                Console.WriteLine("Player data updated.");
                            }
                            else Console.WriteLine("Player not found!");
                            pause();
                            break;
                        case 3:
                            Console.Write("Enter player name to search: ");
                            p = AlgorithmSpecialist.Search(Console.ReadLine());
                            if (p != null)
                            {
                                AlgorithmSpecialist.DisplayPlayer(p);
                                Console.WriteLine("Are you sure you want to delete this player? (Y/N)");
                                if (Console.ReadKey(true).Key == ConsoleKey.Y)
                                {
                                    AlgorithmSpecialist.players.Remove(p);
                                    Console.WriteLine("Player deleted.");
                                }
                            }
                            else Console.WriteLine("Player not found!");
                            pause();
                            break;
                        case 0:
                            return;
                    }
                break;
            }
        } while (true);
    }
    public static void pause()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
}

