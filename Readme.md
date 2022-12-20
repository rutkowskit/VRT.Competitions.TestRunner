# Test Runner App

This application helps testing algorithmic tasks solutions (compiled to executable file).
Application works well for tasks from the following programming contests:

1. [**Potyczki algorytniczme**](https://potyczki.mimuw.edu.pl/)


## Installation Instructions

You will need to build the project to create an executable. 
In the solution directory, run this command at the command line:

```dotnet publish TestRunner.Wpf -r win-x64 -c Release /p:PublishSingleFile=true --self-contained false```

Change the ```--self-contained``` flag to ```true``` if you do not want to install .NET Framework runtime on your machine.

## Current features

## Possible ideas

* Create **Console App** to enable testing on Linux machines,
