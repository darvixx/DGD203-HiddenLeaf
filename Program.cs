using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        Console.WriteLine("Welcome to the RPG game HiddenLeaf");

        Console.Write("May I learn your name: ");
        string playerName = Console.ReadLine();

        while (!IsAllLetters(playerName))
        {
            Console.Write("Please enter a valid name with only letters: ");
            playerName = Console.ReadLine();
        }

        Console.Write($"Hello, {playerName}! How old are you? ");
        int playerAge;

        while (!int.TryParse(Console.ReadLine(), out playerAge))
        {
            Console.Write("Please enter a valid age: ");
        }

        Console.WriteLine("Game has begun, 5x5 map has been created.");

        int[] playerPosition = { 0, 0 };
        int[] swordPosition = { 1, 4 };
        bool hasSword = false;
        int[] elekidPosition = { 4, 3 };
        bool hasElekid = false;
        int[] goblin1Position = { 1, 3 };
        int[] goblin2Position = { 3, 3 };
        int[] npcPaulPosition = { 5, 5 };
        List<string> inventory = new List<string>();
        int attemptsRemaining = 2;
        bool hasAnsweredCorrectly = false;

        while (true)
        {
            Console.Write("Enter your move (e.g., 2R for 2 steps to the right, 'O' to view inventory, 'S' to save, 'L' to load): ");
            string move = Console.ReadLine();

            if (move.ToUpper() == "O")
            {
                Console.WriteLine("Inventory:");
                foreach (var item in inventory)
                {
                    Console.WriteLine("- " + item);
                }
                continue;
            }
            else if (move.ToUpper() == "S")
            {
                SaveGame(playerName, playerAge, playerPosition, hasSword, hasElekid, inventory, hasAnsweredCorrectly);
                Console.WriteLine("Game saved successfully!");
                continue;
            }
            else if (move.ToUpper() == "L")
            {
                if (LoadGame(out playerName, out playerAge, out playerPosition, out hasSword, out hasElekid, out inventory, out hasAnsweredCorrectly))
                {
                    Console.WriteLine("Game loaded successfully!");
                }
                else
                {
                    Console.WriteLine("No saved game found. Starting a new game.");
                }
                continue;
            }

            if (IsValidMove(move))
            {
                UpdatePlayerPosition(playerPosition, move);
                Console.WriteLine($"Your current position: [{playerPosition[0]}, {playerPosition[1]}]");

                if (!hasSword && playerPosition[0] == swordPosition[0] && playerPosition[1] == swordPosition[1])
                {
                    Console.WriteLine("You found a sword at [1, 4]! You now have a sword.");
                    hasSword = true;
                    inventory.Add("Sword");
                }

                if (!hasElekid && playerPosition[0] == elekidPosition[0] && playerPosition[1] == elekidPosition[1])
                {
                    Console.WriteLine("You found an Elekid at [4, 3]! You now have an Elekid.");
                    hasElekid = true;
                    inventory.Add("Elekid");
                }

                if ((playerPosition[0] == goblin1Position[0] && playerPosition[1] == goblin1Position[1]) ||
                    (playerPosition[0] == goblin2Position[0] && playerPosition[1] == goblin2Position[1]))
                {
                    if (inventory.Contains("Sword"))
                    {
                        Console.WriteLine("You encountered a Goblin!");

                        string goblinItem = (playerPosition[0] == goblin1Position[0] && playerPosition[1] == goblin1Position[1]) ? "X" : "I";

                        Console.WriteLine($"With your sword, you defeat the Goblin and receive a {goblinItem} as a reward.");

                        inventory.Add(goblinItem);
                    }
                    else
                    {
                        Console.WriteLine("You encountered a Goblin! You need a sword to defeat it.");
                    }
                }

                if (playerPosition[0] == npcPaulPosition[0] && playerPosition[1] == npcPaulPosition[1])
                {
                    Console.WriteLine("Can't believe my eyes! He is 'The Sword of The Thunder' Paul.");
                    Console.WriteLine("Then he approaches you and says, 'Hi lad, looks like you lost? I'm sorry, but in order to pass here, you need to solve my riddle.'");

                    if (hasElekid)
                    {
                        Console.WriteLine("I rumble and roar with a deafening sound,");
                        Console.WriteLine("In storms, I'm the king, shaking the ground.");
                        Console.WriteLine("Electric sparks dance in my mighty display.");
                        Console.WriteLine("In order to control that thing, you must use me.");
                        Console.WriteLine("What am I?");
                        Console.WriteLine("A-) Raiken");
                        Console.WriteLine("B-) Munsha");
                        Console.WriteLine("C-) Reeve");
                        Console.WriteLine("D-) Palye");
                        Console.WriteLine("E-) Xilef");

                        string correctAnswer = "E";
                        string answer;

                        for (int i = 0; i < 2; i++)
                        {
                            Console.Write("Your answer (A, B, C, D, E): ");
                            answer = Console.ReadLine().ToUpper();

                            if (answer == correctAnswer)
                            {
                                Console.WriteLine("Correct! The Sword of The Thunder Paul says, 'Well done, lad! You may pass.'");
                                hasAnsweredCorrectly = true;
                                break;
                            }
                            else
                            {
                                Console.WriteLine($"Incorrect! You have {1 - i} attempts remaining.");

                                if (i == 0)
                                {
                                    Console.WriteLine("Would you like to answer again immediately? (Y/N)");
                                    string retry = Console.ReadLine().ToUpper();

                                    if (retry == "Y")
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }

                        if (!hasAnsweredCorrectly)
                        {
                            Console.WriteLine("The Sword of The Thunder Paul says, 'You failed to solve my riddle. Prepare to face the consequences.'");
                            return;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Answer the riddle to pass:");
                        Console.WriteLine("A-) Raiken");
                        Console.WriteLine("B-) Reeve");
                        Console.WriteLine("C-) Palye");
                        Console.WriteLine("D-) Munsha");
                        Console.WriteLine("E-) Darvi");

                        Console.Write("Your answer (A, B, C, D, E): ");
                        string answer = Console.ReadLine().ToUpper();

                        Console.WriteLine($"Incorrect! You have {--attemptsRemaining} attempts remaining.");

                        if (attemptsRemaining == 0)
                        {
                            Console.WriteLine("The Sword of The Thunder Paul says, 'You failed to solve my riddle. Prepare to face the consequences.'");
                            return;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid move. Please enter a valid move.");
            }
        }

        if (hasAnsweredCorrectly)
        {
            Console.WriteLine("Congratulations! You have successfully passed 'The Sword of The Thunder' Paul's riddle and won the game.");
        }
    }

    static bool IsAllLetters(string input)
    {
        foreach (char c in input)
        {
            if (!char.IsLetter(c))
            {
                return false;
            }
        }
        return true;
    }

    static bool IsValidMove(string move)
    {
        if (move.ToUpper() == "O" || move.ToUpper() == "S" || move.ToUpper() == "L")
            return true;

        if (move.Length != 2)
            return false;

        char direction = char.ToUpper(move[1]);
        int steps;

        if (!int.TryParse(move.Substring(0, 1), out steps))
            return false;

        if (direction != 'U' && direction != 'D' && direction != 'L' && direction != 'R')
            return false;

        return true;
    }

    static void UpdatePlayerPosition(int[] position, string move)
    {
        char direction = char.ToUpper(move[1]);
        int steps = int.Parse(move.Substring(0, 1));

        switch (direction)
        {
            case 'D':
                position[0] -= steps;
                break;
            case 'U':
                position[0] += steps;
                break;
            case 'L':
                position[1] -= steps;
                break;
            case 'R':
                position[1] += steps;
                break;
        }

        position[0] = Math.Max(0, Math.Min(position[0], 5));
        position[1] = Math.Max(0, Math.Min(position[1], 5));
    }

    static void SaveGame(string playerName, int playerAge, int[] playerPosition, bool hasSword, bool hasElekid, List<string> inventory, bool hasAnsweredCorrectly)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter("savegame.txt"))
            {
                writer.WriteLine(playerName);
                writer.WriteLine(playerAge);
                writer.WriteLine($"{playerPosition[0]},{playerPosition[1]}");
                writer.WriteLine(hasSword);
                writer.WriteLine(hasElekid);
                foreach (string item in inventory)
                {
                    writer.WriteLine(item);
                }
                writer.WriteLine(hasAnsweredCorrectly);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error saving the game: {e.Message}");
        }
    }

    static bool LoadGame(out string playerName, out int playerAge, out int[] playerPosition, out bool hasSword, out bool hasElekid, out List<string> inventory, out bool hasAnsweredCorrectly)
    {
        playerName = "";
        playerAge = 0;
        playerPosition = new int[2];
        hasSword = false;
        hasElekid = false;
        inventory = new List<string>();
        hasAnsweredCorrectly = false;

        try
        {
            string filePath = Path.Combine(Environment.CurrentDirectory, "savegame.txt");

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);

                playerName = lines[0];
                if (!int.TryParse(lines[1], out playerAge))
                {
                    return false;
                }

                string[] position = lines[2].Split(',');
                if (position.Length != 2 || !int.TryParse(position[0], out playerPosition[0]) || !int.TryParse(position[1], out playerPosition[1]))
                {
                    return false;
                }

                if (!bool.TryParse(lines[3], out hasSword))
                {
                    return false;
                }

                if (!bool.TryParse(lines[4], out hasElekid))
                {
                    return false;
                }

                for (int i = 5; i < lines.Length - 1; i++)
                {
                    inventory.Add(lines[i]);
                }

                if (!bool.TryParse(lines[lines.Length - 1], out hasAnsweredCorrectly))
                {
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error loading the game: {e.Message}");
            return false;
        }
    }

}