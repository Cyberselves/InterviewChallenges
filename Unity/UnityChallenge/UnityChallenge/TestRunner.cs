namespace SendingCommands;

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading;

class TestRunner
{
    private static readonly int testDuration = 3000; // 5 seconds per test
    private static readonly int[] processingRates = { 10, 50, 80 };
    private static readonly int[] processingChunkSizes = { 20, 80 };
    private static int threshold = 15;
    
    public static void Main()
    {
        List<(int rate, int chunk, float measuredRate, float error)> results = new List<(int, int, float, float)>();

        foreach (var rate in processingRates)
        {
            foreach (var chunk in processingChunkSizes)
            {
                Console.WriteLine($"Testing rate={rate}, chunkSize={chunk}...");
                float measuredRate = RunTest(rate, chunk);
                float error = 100 - (measuredRate*100/rate);
                results.Add((rate, chunk, measuredRate, error));
            }
        }

        Console.WriteLine("\nTest Results:");
        Console.WriteLine("Rate  | ChunkSize | Measured Rate | % Error ");
        Console.WriteLine("---------------------------------");
        var testFailed = false;
        foreach (var (rate, chunk, measuredRate, error) in results)
        {
            if (Math.Abs(error) > threshold)
            {
                testFailed = true;
            }
            Console.WriteLine($"{rate,5} | {chunk,6} | {measuredRate,6:F2} | {error,6:F2}");
        }

        if (testFailed)
        {
            Console.WriteLine($"Test Failed: Threshold of {threshold}% exceeded");
        }
        else
        {
            Console.WriteLine("Test Success");
        }
    }

    private static float RunTest(int rate, int chunk)
    {
        // Start the application with given parameters
        var thisProcessor = new StreamProcessorSolution(rate, chunk);
        thisProcessor.StartProcessing();

        // Wait for the test duration
        Thread.Sleep(testDuration);

        // Retrieve and return the last measured processing frequency
        var measuredFreq = thisProcessor.GetLastMeasuredFreq();
        thisProcessor.Close();
        return measuredFreq;
    }
}