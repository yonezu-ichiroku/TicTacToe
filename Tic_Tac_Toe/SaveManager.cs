using System;
using System.IO;
using System.Linq;
using System.Text;

public static class SaveManager
{
    // --- GHI FILE (.txt) ---
    public static void SaveGameToFile(string path, char[,] board, int moves, char currentPlayer)
    {
        StreamWriter w = null;
        try
        {
            // 1) Tạo bản sao lưu .bak nếu file cũ tồn tại
            if(File.Exists(path + ".txt")) {
                File.Copy(path + ".txt", path + ".bak", overwrite: true);
            }
            // 2) Ghi file
            w = new StreamWriter(path + ".txt", false, Encoding.UTF8);
            w.WriteLine($"CurrentPlayer={currentPlayer}");
            w.WriteLine($"Moves={moves}");
            w.WriteLine("Board");
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    w.Write(board[row, col]);
                }
                w.WriteLine();
            }
            w.WriteLine($"Timestamp={DateTime.UtcNow:O}");
            
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving game: " + ex.Message);
            throw;
        }
        finally
        {
            w?.Close();
        }
    }


    // --- ĐỌC FILE (.txt) ---
    public static bool LoadGameFromFile(string path, out char[,] board, out char currentPlayer, out int moves)
    {
        board = new char[3, 3];
        currentPlayer = 'X';
        moves = 0;
        StreamReader r = null;
        if (!File.Exists(path + ".txt"))
        {
            Console.WriteLine("Savefile not found");
            return false;
        }

        try
        {
            // Đọc thông tin chung
            r = new StreamReader(path + ".txt", Encoding.UTF8);
            string line;
            for(int i = 0; i < 2; i++)
            {
                line = r.ReadLine();
                var parts = line.Split('=');
                if (parts[0] == "CurrentPlayer")
                    currentPlayer = parts[1][0];
                else if (parts[0] == "Moves")
                    moves = int.Parse(parts[1]);
            }

            // Đọc Board
            line = r.ReadLine();
            for (int row = 0; row < 3; row++)
            {
                line = r.ReadLine();
                for (int col = 0; col < 3; col++)
                {
                    board[row, col] = line[col];
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading game: " + ex.Message);
            return false;
        }
        finally
        {
            r?.Close();
        }
    }
}

