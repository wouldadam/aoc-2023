using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

var board = Load();

int adjacentSum = SumAdjacentNums(board);
Console.WriteLine($"Adjacent nums sum: {adjacentSum}");
Debug.Assert(adjacentSum == 531932);

int gearSum = SumGearRatios(board);
Console.WriteLine($"Gear ratios sum: {gearSum}");
Debug.Assert(gearSum == 73646890);

List<string> Load()
{
    // Load the input file
    Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Day3.input.txt");
    if (stream == null)
    {
        Console.WriteLine("Failed to load input.");
        return [];
    }

    // Parse each line
    StreamReader reader = new(stream);
    List<string> board = new();
    while (!reader.EndOfStream)
    {
        string? line = reader.ReadLine();
        if (line != null)
        {
            board.Add(line);
        }
    }

    return board;
}

int SumAdjacentNums(List<string> board)
{
    int sum = 0;

    for (int y = 0; y < board.Count; ++y)
    {
        bool isTouching = false;
        int numStartIdx = -1;

        for (int x = 0; x < board[y].Length; ++x)
        {
            if (board[y][x] == '.' || IsSpecial(board[y][x]))
            {
                // Possible end of num
                if (numStartIdx != -1)
                {
                    string numStr = board[y].Substring(numStartIdx, x - numStartIdx);
                    if (isTouching)
                    {
                        sum += int.Parse(numStr);
                    }

                    numStartIdx = -1;
                    isTouching = false;
                }
            }
            else
            {
                // Part of a num
                if (numStartIdx == -1)
                {
                    numStartIdx = x;
                }
                isTouching |= IsTouching(board, x, y);
            }
        }

        if (numStartIdx != -1)
        {
            if (isTouching)
            {
                sum += int.Parse(board[y].Substring(numStartIdx, board[y].Length - numStartIdx));
            }
            numStartIdx = -1;
            isTouching = false;
        }
    }

    return sum;
}

int SumGearRatios(List<string> board)
{
    int sum = 0;

    for (int y = 0; y < board.Count; ++y)
    {
        for (int x = 0; x < board[y].Length; ++x)
        {
            if (board[y][x] == '*')
            {
                var nums = GetSurroundingNumbers(board, x, y);
                if (nums.Count == 2)
                {
                    sum += nums[0] * nums[1];
                }
            }
        }
    }

    return sum;
}

List<int> GetSurroundingNumbers(List<string> board, int x, int y)
{
    int[] dirs = [-1, 1];

    List<int> surrounding = new();

    // Left/right
    foreach (var dir in dirs)
    {
        int checkX = x + dir;
        if (checkX >= 0 && checkX < board[y].Length)
        {
            if (char.IsDigit(board[y][checkX]))
            {
                var exp = ExpandNum(board, checkX, y);
                surrounding.Add(exp.num);
            }
        }
    }

    // Top/bottom
    foreach (var dir in dirs)
    {
        int checkY = y + dir;
        int checkX = Math.Max(x - 1, 0);

        while(checkX < board[checkY].Length && checkX <= x + 1)
        {
            if (char.IsDigit(board[checkY][checkX]))
            {
                var exp = ExpandNum(board, checkX, checkY);
                surrounding.Add(exp.num);
                checkX = exp.right;
            }
            checkX += 1;
        }
    }

    return surrounding;
}

(int num, int left, int right) ExpandNum(List<string> board, int x, int y)
{
    int start = x;
    while(start > 0 && char.IsDigit(board[y][start - 1]))
    {
        start -= 1;
    }

    int end = x;
    while(end < board[y].Length - 1 && char.IsDigit(board[y][end + 1]))
    {
        end += 1;
    }

    string numStr = board[y].Substring(start, end - start + 1);

    return (int.Parse(numStr), start, end);
}

bool IsSpecial(char c)
{
    return c != '.' && !char.IsDigit(c);
}

bool IsTouching(List<string> board, int x, int y)
{
    (int xOff, int yOff)[] dirs = [(-1, 0), (1, 0), (0, -1), (0, 1), (-1, -1), (-1, 1), (1, 1), (1, -1)];

    foreach(var dir in dirs)
    {
        int checkX = x + dir.xOff;
        int checkY = y + dir.yOff;
        if (checkX > 0 && checkX < board[y].Length &&
            checkY > 0 && checkY < board.Count)
        {
            if (IsSpecial(board[checkY][checkX]))
            {
                return true;
            }
        }
    }

    return false;
}