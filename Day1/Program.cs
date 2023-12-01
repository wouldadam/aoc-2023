using System.Reflection;

int FindNum(string line)
{
    char? first = null;
    char last = '0';

    (string Name, char Val)[] numbers = [("one", '1'),
        ("two" ,'2'), ("three", '3'), ("four", '4'), 
        ("five", '5'), ("six", '6'), ("seven", '7'), 
        ("eight", '8'), ("nine", '9')];

    for(int idx = 0; idx < line.Length; idx++)
    {
        if ("0123456789".Contains(line[idx]))
        {
            last = line[idx];
            if (first == null)
            {
                first = line[idx];
            }
        }
        else
        {
            foreach (var num in numbers)
            {
                if (line.Substring(idx).StartsWith(num.Name))
                {
                    last = num.Val;
                    if (first == null)
                    {
                        first = num.Val;
                    }
                }
            }
        }
    }

    return Int32.Parse($"{first}{last}");
}

// Load file
Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Day1.input.txt");
if (stream == null)
{
    Console.WriteLine("Failed to load input.");
    return -1;
}

// Process each line
int sum = 0;
StreamReader reader = new(stream);
while (!reader.EndOfStream)
{
    string? line = reader.ReadLine();
    if (line != null)
    {
        int num = FindNum(line);
        sum += num;
    }
}

// Output the sum
Console.WriteLine($"Sum of calibration values: {sum}");

return 0;