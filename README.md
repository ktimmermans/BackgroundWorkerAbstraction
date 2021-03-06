# BackgroundWorkerAbstraction
Abstraction layer for the DependencyInjection Backgroundworker class

This library provides wrapper classes to provide easier use of the background services. 
Both a background task that provides an implementation for an object returning process as well as a void process.
Also both base classes provide a created IServiceScope to be used for getting required services from DI.


Usage as follows:

# Object returning

1. Create a service that extends ObjectBackgroundWorker and override Task ask RunTaskWithItem(IServiceScope scope, T itemToProcess).
within that function the scope can be used to retreive required services from DI:

var _testService = scope.ServiceProvider.GetRequiredService<ITestService>();
  
2. Add the IObjectBackgroundQueue service to DI:
services.TestBackgroundService<TestClass>();

3. Add the background worker process to DI:
services.AddHostedService<TestBackgroundService>()
  
# Void returning
1. In order to make sure the correct worker receives the task create a class that extends the TaskSettings class. 
2. next create a service that extends VoidBackgroundWorker and override Task RunTask(IServiceScope scope).

public class TestBackgroundService : VoidBackgroundWorker<TestTaskDescriptor>
  
within that function the scope can be used to retreive required services from DI:

var _testService = scope.ServiceProvider.GetRequiredService<ITestService>();
  
3. Add the IObjectBackgroundQueue service to DI:
services.TestBackgroundService<TestTaskDescriptor>();
  
4. Add the background worker process to DI:
services.AddHostedService<TestBackgroundService>();
  
The TaskSettings class contains a DelayMilliSeconds property which if set will set the delay on picking up the next task. If not set this defaults to 500ms.
This will also work for the ObjectBackgroundWorker (make your object extend the TaskSettings class to use it)

# Taskmanager
Simply add it to the DI:
services.AddScoped<IBackgroundTaskManager, BackgroundTaskManager>();

inject it in any of your own services and retreive current tasks by:
var currentQueues = this._taskManager.GetCurrentBackgroundTasks();
