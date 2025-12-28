using System;
using System.Collections.Generic;


class GameLogic
{   
    static char[,] board = new char[3, 3];
    static int moves, selRow, selCol;
    static bool playAgain, exit;
    static char currentPlayer;
    static bool gameOver = false;
    public static void TurnCheck(bool fromSaveFile = false, char[,] loadedBoard = null, char loadedCurrentPlayer = 'X', int loadedMoves = 0)
    {
        do
        {
            if (fromSaveFile) ResetGame(ref fromSaveFile, loadedBoard, loadedCurrentPlayer, loadedMoves);
            else ResetGame(board);
            while (!gameOver)
            {
                exit = GameControl(board, ref selRow, ref selCol);
                if (exit) return;
                Console.Clear();
                char winner = CheckWinner(board);
                if (winner == 'X' || winner == 'O')
                {
                    DrawBoard(board);
                    Console.Write("🎉 Player ");
                    WritePlayer(winner);
                    Console.WriteLine(" won!");
                    if(winner == 'X') {
                        Player.UpdateStats("X", true);
                        Player.UpdateStats("O", false);
                    }
                    else {
                        Player.UpdateStats("O", true);
                        Player.UpdateStats("X", false);
                    }
                    gameOver = true;
                }
                else if (moves == 9)
                {
                    DrawBoard(board);
                    Console.WriteLine("🤝 Tie!");
                    gameOver = true;
                }
            }
            playAgain = ResetControl();
        } while (playAgain);
       
    }

    // 🧩 Vẽ bàn cờ
    public static void DrawBoard(char[,] board, int selRow = -1, int selCol = -1)
    {
        Console.Clear();
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                bool isSelected = false;
                char cell = board[row, col];
                Console.Write(" ");
                if (row == selRow && col == selCol) isSelected = true;
                if (cell == 'X' || cell == 'O') WritePlayer(cell, isSelected);
                else if (isSelected) WritePlayer('_', isSelected);
                else Console.Write(" ");
                Console.Write(" ");
                if (col < 2) Console.Write("|");
            }
            Console.WriteLine();
            if (row < 2) Console.WriteLine("---+---+---");
        }
        Console.WriteLine();
    }

    public static bool GameControl(char[,] board, ref int selRow, ref int selCol) {
        ConsoleKey key;
        int err = 0;

        // Control movement
        do {
            Console.Clear();
            DrawBoard(board, selRow, selCol);
            WritePlayer(currentPlayer);
            Console.Write("'s turn ");
            Console.WriteLine("\nUse W/A/S/D or ↑/↓/←/→ to move, Enter to select, Esc to save and exit.");
            if (err == 1) {
                Console.WriteLine("Tile is selected, please select another tile!");
            }
            key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.W: case ConsoleKey.UpArrow:
                    selRow = (selRow + 2) % 3;
                    break;
                case ConsoleKey.S: case ConsoleKey.DownArrow:
                    selRow = (selRow + 1) % 3;
                    break;
                case ConsoleKey.A: case ConsoleKey.LeftArrow:
                    selCol = (selCol + 2) % 3;
                    break;
                case ConsoleKey.D: case ConsoleKey.RightArrow:
                    selCol = (selCol + 1) % 3;
                    break;
                case ConsoleKey.Enter:
                    if (board[selRow, selCol] == '\0')
                    {
                        board[selRow, selCol] = currentPlayer;
                        currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';
                        moves++;
                        err = 0;
                        return false;
                    }
                    else {
                        err = 1;
                        break;
                    }
                    return false;
                case ConsoleKey.Escape:
                    SaveManager.SaveGameToFile(Program.SAVE_FILE, board, moves, currentPlayer);
                    Console.WriteLine("Game saved successfully");
                    Menu.pause();
                    return true;

            }
        } while (true);
    }

    static bool ResetControl()
    {
        bool choice = true;
        Menu.pause();
        ConsoleKey key;
        do
        {
            Console.Clear();
            Console.WriteLine("Play again?");
            Console.WriteLine((choice == true ? ">> " : "   ") + "Yes");
            Console.WriteLine((choice == false ? ">> " : "   ") + "No");
            key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.W: case ConsoleKey.UpArrow:
                case ConsoleKey.S: case ConsoleKey.DownArrow:
                    choice = !choice;
                    break;
                case ConsoleKey.Escape:
                    return false;
                case ConsoleKey.Enter:
                    return choice;
            }
        } while (true);
    }

    // 🟥 X đỏ, 🟦 O xanh
    public static void WritePlayer(char player, bool selected = false)
    {
        var defaultColor = Console.ForegroundColor;
        if (player == 'X') Console.ForegroundColor = ConsoleColor.Red;
        if (player == 'O') Console.ForegroundColor = ConsoleColor.Cyan;
        if (selected) {
            Console.ForegroundColor = ConsoleColor.Yellow;
            selected = false;
        }
        Console.Write(player);
        Console.ForegroundColor = defaultColor;
    }


    // 🏆 Kiểm tra người thắng
    static char CheckWinner(char[,] b)
    {
        for (int i = 0; i < 3; i++)
            if (b[i, 0] != '\0' && b[i, 0] == b[i, 1] && b[i, 1] == b[i, 2])
                return b[i, 0];


        for (int j = 0; j < 3; j++)
            if (b[0, j] != '\0' && b[0, j] == b[1, j] && b[1, j] == b[2, j])
                return b[0, j];


        if (b[0, 0] != '\0' && b[0, 0] == b[1, 1] && b[1, 1] == b[2, 2])
            return b[0, 0];


        if (b[0, 2] != '\0' && b[0, 2] == b[1, 1] && b[1, 1] == b[2, 0])
            return b[0, 2];


        return '\0';
    }


    // ♻️ Reset bàn cờ
    static void ResetGame(char[,] board)
    {
        for (int row = 0; row < 3; row++)
            for (int col = 0; col < 3; col++)
                board[row, col] = '\0';
        currentPlayer = 'X';
        moves = 0;
        gameOver = false;
        selRow = 0;
        selCol = 0;
    }

    static void ResetGame(ref bool fromSaveFile, char[,] loadedBoard, char loadedCurrentPlayer, int loadedMoves)
    {
        board = loadedBoard;
        currentPlayer = loadedCurrentPlayer;
        moves = loadedMoves;
        gameOver = false;
        fromSaveFile = false;
        selRow = 0;
        selCol = 0;
        Console.ReadKey();
    }
}




