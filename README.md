# WonkaBuild

## Web client for running and managing DevOps tasks

Wonka build utilises https://github.com/assemblysoft/AssemblySoft.DevOps library for running tasks and the build lifecycle

### Task
A task is the single unit that performs something meaningful. It could be to run a script, copy a directory, contact a web service or as simple as outputting a message to the console.

### Build Definition
A build starts with a definition. A build definition is a collection of tasks that can run sequentially or in parallel.

### Coded Build Tasks
For more complex scenarios that require existing or new executable binaries to be run, custom build tasks can be created.
This enables proprietory tasks to live in your own source code repository and strung together as part of the definition via reflection. This makes it a simple process to string together binaries and scripts as part of a managed flow with very little effort.


