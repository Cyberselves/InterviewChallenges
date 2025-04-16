namespace SendingCommands;

using System;
using System.Diagnostics;

/// <summary>
/// Your task is to continuously process a very large string array stored in a file. The file is too big to be loaded in
/// memory and has been stored to disk in chunks of unknown size. You are provided with a function, `GetNextChunk`,
/// that will retrieve the next chunk of strings for processing. This function returns a variable number of strings
/// and each string contains a variable number of characters. You need to process the information at a variable frequency
/// provided by `processingRate` in fixed length strings of `processingChunkSize`. The data should be processed by the
/// function `ProcessString` which introduces a variable amount of delay and expects strings of size `processingChunkSize`.
/// 
/// To complete the task you will need to:
/// 1. Create a processing loop that consumes chunks at the defined data rate.
/// 2. Create a loading loop that keeps the buffer fed with enough data.
/// 
/// Additional information:
/// The buffer must be of type char[] and the buffer memory allocated once.
/// Strings provided by `GetNextChunk` can be concatenated to each other
/// The loops must be designed so that the processing loop is never ever blocked or waiting
/// The solution should use `readPos` and `writePos` variables to track buffer positions. These are printed inside
/// `ProcessString` for easy debugging
/// </summary>

class StreamProcessor
{
    private readonly Random _random = new Random(42);
    private Stopwatch metrics;
    private int processingRate = 80;
    private int processingChunkSize = 30;
    private int metricsRefreshMs = 250;

    private int readPos = 0;
    private int writePos = 0;

    private int processedCount = 0;
    private float lastMeasuredFreq = 0;
    private bool keepProcessing = true;

    public StreamProcessor(int rate, int chunk)
    {
        processingRate = rate;
        processingChunkSize = chunk;
    }
    
    public float GetLastMeasuredFreq()
    {
        return lastMeasuredFreq;
    }

    public string[] GetNextChunk()
    {
        int numStrings = _random.Next(11, 60);
        string[] chunk = new string[numStrings];

        for (int i = 0; i < numStrings; i++)
        {
            chunk[i] = GenerateRandomString(_random.Next(processingChunkSize/2, processingChunkSize*2));
        }

        // Simulate read time
        Thread.Sleep(_random.Next(50, 101));

        return chunk;
    }

    private string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(Enumerable.Range(0, length)
            .Select(_ => chars[_random.Next(chars.Length)])
            .ToArray());
    }
    
    private void ProcessString(string sample)
    {
        if (sample.Length != processingChunkSize)
        {
            Console.WriteLine($"ERROR - Sample {sample} has length {sample.Length}, expected {processingChunkSize}!");
            Environment.Exit(-1);
        }
        
        processedCount++;
        if (metrics.ElapsedMilliseconds >= 250)
        {
            lastMeasuredFreq = (float)processedCount * 1000 / metrics.ElapsedMilliseconds;
            processedCount = 0;
            metrics.Restart();
        }
        
        Console.WriteLine(
            $"Processing: {sample,-12} " +
            $"ReadPos:{readPos,5}  WritePos:{writePos,5} Freq: {lastMeasuredFreq,3:F2}"
        );

        Thread.Sleep(_random.Next(1*processingRate/100, 5*processingRate/100));
    }
    
    public void StartProcessing()
    {
        metrics = new Stopwatch();
        metrics.Start();
        // Implement your solution here
    }

    public void Close()
    {
        keepProcessing = false;
    }
}
