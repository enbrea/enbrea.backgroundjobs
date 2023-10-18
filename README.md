[![NuGet Gallery](https://img.shields.io/badge/NuGet%20Gallery-enbrea.backgroundjobs-blue.svg)](https://www.nuget.org/packages/Enbrea.BackgroundJobs/)
![GitHub](https://img.shields.io/github/license/enbrea/enbrea.backgroundjobs)

# ENBREA BackgroundJobs

A .NET library for queueing and executing long running background jobs:

+ Supports .NET 6 and .NET 7.
+ Based on [Microsoft.Extensions.Hosting.BackgroundService](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.backgroundservice).
+ Implements a thread-safe job queue. You can have as many job queues as you like.
+ Implementing a custom background job is easy, just implement the IBackgroundJob interface.

## Installation

```
dotnet add package Enbrea.BackgroundJobs
```

## Getting started

Documentation is available in the [GitHub wiki](https://github.com/enbrea/enbrea.backgroundjobs/wiki).

## Can I help?

Yes, that would be much appreciated. The best way to help is to post a response via the Issue Tracker and/or submit corrections via a Pull Request.
