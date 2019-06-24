# WonkaBuild

## Web Client for managing the output and history of DevOps projects


![Alt text](thumb.png?raw=true "User Interface for Wonka Build Web site")


### Task
A task is the single unit that performs something meaningful. It could be to run a script, copy a directory, contact a web service or as simple as outputting a message to the console.

### Tasks Definition
A build, or execution of a set of steps, starts with a definition. A tasks definition is a collection of tasks that can run sequentially or in parallel.

### Coded Tasks
For more complex scenarios that require existing or new executable binaries to be run, custom build tasks can be created.
This enables proprietory tasks to live in your own source code repository and strung together as part of the definition via reflection. This makes it a simple process to string together binaries and scripts as part of a managed flow with very little effort. It also enables de-coupling of development as tasks can be developed separately, by different teams, with a degree of confidence as communication is handles by a simple API that all tasks adhere to.

### Task Runner
The task runner takes a tasks definition and executes each task either sequentially or in parallel.

*The Task runner project is part of the open source DevOps library, also here on Github* https://github.com/assemblysoft/AssemblySoft.DevOps


## License

AssemblySoft.DevOps is distributed under the MIT License.
