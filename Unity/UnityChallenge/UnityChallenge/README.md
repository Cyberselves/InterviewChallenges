Your task is to continuously process a very large string array stored in a file. The file is too big to be loaded in memory and has been stored to disk in chunks of unknown size. You are provided with a function, `GetNextChunk`, that will retrieve the next chunk of strings for processing. This function returns a variable number of strings and each string contains a variable number of characters. You need to process the information at a variable frequency provided by `processingRate` in fixed length strings of `processingChunkSize`. The data should be processed by the function `ProcessString` which introduces a variable amount of delay and expects strings of size `processingChunkSize`.

To complete the task you will need to:
1. Create a processing loop that consumes chunks at the defined data rate.
2. Create a loading loop that keeps the buffer fed with enough data.

Additional information:
- The buffer must be of type char[] and the buffer memory allocated once.
- Strings provided by `GetNextChunk` can be concatenated to each other
- The loops must be designed so that the processing loop is never ever blocked or waiting
- The solution should use readPos and writePos variables to track buffer positions. These are printed inside `ProcessString` for easy debugging
