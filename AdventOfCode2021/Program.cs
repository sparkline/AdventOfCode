/** Borrowed some stuf from: 
 * - https://github.com/viceroypenguin/adventofcode/
 * 
 **/

using AdventOfCode2021.Common;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Reflection;

HttpClient httpClient;
IEnumerable<Solver> solvers = GetSolvers();
bool testMode;
solvers = FilterSolvers(args, solvers);
FetchInput(solvers);
Solve(solvers);


/* Functions */

void Solve(IEnumerable<Solver> solvers)
{
    foreach (Solver solver in solvers)
    {
        if (testMode)
        {
            var solutionA = solver.TestPartAResult();
            var solutionB = solver.TestPartBResult();
        }
        else
        {
            var solutionA = solver.PartA();
            var solutionB = solver.PartB();
        }
    }
}

void FetchInput(IEnumerable<Solver> solvers)
{
    var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("SessionId.json", optional: true, reloadOnChange: true);

    var configuration = builder.Build();
    var sessionId = configuration["sessionId"];

    if (string.IsNullOrWhiteSpace(sessionId))
        throw new ArgumentNullException(nameof(sessionId), "Please provide an AoC session id in the configuration file.");

    var baseAddress = new Uri("https://adventofcode.com");
    var cookieContainer = new CookieContainer();
    cookieContainer.Add(baseAddress, new Cookie("session", sessionId));
    httpClient = new HttpClient(
            new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                AutomaticDecompression = DecompressionMethods.All,
            })
    {
        BaseAddress = baseAddress,
    };
    List<Task> tasks = new List<Task>();
    foreach (Solver solver in solvers)
    {
        string inputFile = $"Input.{solver.Day:00}";
        if (!File.Exists(inputFile))
        {
            tasks.Add(fetchInputFile(inputFile, solver.Year, solver.Day));
        }
    }
    Task.WaitAll(tasks.ToArray());
}

async Task fetchInputFile(string inputFile, int year, int day)
{
    try
    {
        var response = await httpClient.GetAsync($"{year}/day/{day}/input");
        response.EnsureSuccessStatusCode();
        File.WriteAllText(inputFile, await response.Content.ReadAsStringAsync());
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine($"Could not fetch inputfile {inputFile}: {ex.Message}");
    }

}


static IEnumerable<Solver> GetSolvers() =>
    Assembly.GetExecutingAssembly().GetTypes()
        .Where(t => t.BaseType == typeof(Solver))
        //.Where(t => t.Name != nameof(DummyDay))
        .Select(t => (Solver?)Activator.CreateInstance(t))
        .OfType<Solver>() // filter out nulls
        .OrderBy(d => d.Year)
        .ThenBy(d => d.Day)
        .ThenBy(d => d.iteration)
        .ToArray();

IEnumerable<Solver> FilterSolvers(string[] args, IEnumerable<Solver> solvers)
{
    if (args.Length == 0)
    {
        Console.WriteLine($"Usage: ");
        Console.WriteLine($"	Day:		-d xx");
        Console.WriteLine($"	Year:		-y xxxx");
        Console.WriteLine($"	Iteration:	-i x");
    }

    if (Int32.TryParse(args.SkipWhile(d => d != "-d").Skip(1).FirstOrDefault(), out int day))
    {
        solvers = solvers.Where(s => s.Day == day);
    }
    if (Int32.TryParse(args.SkipWhile(d => d != "-y").Skip(1).FirstOrDefault(), out int year))
    {
        solvers = solvers.Where(s => s.Year == year);
    }
    if (Int32.TryParse(args.SkipWhile(d => d != "-i").Skip(1).FirstOrDefault(), out int iteration))
    {
        solvers = solvers.Where(s => s.iteration == iteration);
    }
    testMode = args.Contains("-t");

    return solvers;
}