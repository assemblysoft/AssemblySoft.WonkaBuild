using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

using AssemblySoft.DevOps;
using AssemblySoft.WonkaBuild.Hubs;
using AssemblySoft.WonkaBuild.Models;
using AssemblySoft.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblySoft.WonkaBuild.Controllers
{
    public class BuildController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewData["Message"] = "Ver 1.0.0";

            return View();
        }

        public ActionResult Help()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }


        #region Task Actions

        public ActionResult LoadTasks()
        {
            try
            {
                AddMessage("Loading tasks...");
                 var model = LoadTaskDefinitions();
                if(model == null || !model.Any())
                {
                    AddMessage("Unable to find any tasks to load.");
                }
                else
                {
                    AddMessage("Completed loading tasks.");
                }

                
                return PartialView("_LoadTasks", model);
            }
            catch(Exception e)
            {
                HandleException(e);
            }

            return PartialView("_LoadTasks");
        }



        public ActionResult Run(TaskModel taskModel)
        {

            var startTime = DateTime.Now;
            Session.Clear();

            AddMessage("Starting Build");
            string runPath = string.Empty;

            try
            {
                runPath = InitialiseBuildRun(taskModel.Path);

                Session["runpath"] = runPath;
            }
            catch (Exception e)
            {
                HandleException(e);
                return PartialView("_TasksFail");
            }


            var taskRunner = new TaskRunner(runPath);
            taskRunner.TaskStatus += (e) =>
            {
                AddMessage(e.Status);
            };

            taskRunner.TasksCompleted += (e) =>
            {
                while (Broadcaster.Instance.MessageCount > 1)
                {
                    Thread.Sleep(2000);
                }

                if (e.Status == TaskStatus.Faulted.ToString())
                {
                    System.IO.File.Create(Path.Combine(runPath, "error.dat"));
                }
                else
                {
                    System.IO.File.Create(Path.Combine(runPath, "completed.dat"));
                }

            };

            string tasksPath = string.Empty;

            try
            {
                CancellationTokenSource _cts = new CancellationTokenSource();
                var token = _cts.Token;

                Task t1 = new Task(() =>
                {
                    var path = Path.Combine(runPath, "processing.dat");
                    System.IO.File.Create(Path.Combine(runPath, "processing.dat"));

                    tasksPath = Path.Combine(runPath, taskModel.FullName);

                    taskRunner.Run(token, Path.Combine(tasksPath));

                });

                Task t2 = t1.ContinueWith((ante) =>
                {
                }, TaskScheduler.FromCurrentSynchronizationContext());

                t1.Start();
            }
            catch (DevOpsTaskException ex)
            {
                HandleException(ex);
                return PartialView("_TasksFail");
            }
            catch (AggregateException ag)
            {
                HandleException(ag);
                return PartialView("_TasksFail");
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return PartialView("_TasksFail");
            }
            finally
            {
                var tasks = taskRunner.GetDevOpsTaskWithState();
                //could serialize the state back to the tasks file for verification
                //taskRunner.S.SerializeTasksToFile(tasks, ConfigurationManager.AppSettings["tasksPath"]);
            }

            var model = new TaskInformationModel()
            {
                TaskName = taskModel.Task,
                TasksPath = taskModel.Path,
                TasksStartTime = startTime.ToString(),
            };

            return PartialView("_TasksRunning", model);
        }

        public ActionResult RunFromSpecificConfigSource()
        {
            var startTime = DateTime.Now;
            Session.Clear();

            AddMessage("Starting Build");
            string runPath = string.Empty;

            try
            {
                runPath = InitialiseBuildRun();

                Session["runpath"] = runPath;
            }
            catch (Exception e)
            {
                HandleException(e);
                return PartialView("_TasksFail");
            }


            var taskRunner = new TaskRunner(runPath);
            taskRunner.TaskStatus += (e) =>
            {
                AddMessage(e.Status);
            };

            taskRunner.TasksCompleted += (e) =>
            {
                while (Broadcaster.Instance.MessageCount > 1)
                {
                    Thread.Sleep(2000);
                }

                System.IO.File.Create(Path.Combine(runPath, "completed.dat"));

            };

            string tasksPath = string.Empty;

            try
            {
                CancellationTokenSource _cts = new CancellationTokenSource();
                var token = _cts.Token;

                Task t1 = new Task(() =>
                {
                    var path = Path.Combine(runPath, "processing.dat");
                    System.IO.File.Create(Path.Combine(runPath, "processing.dat"));
                    tasksPath = Path.Combine(runPath, "build.tasks");
                    taskRunner.Run(token, Path.Combine(tasksPath));

                });

                Task t2 = t1.ContinueWith((ante) =>
                {
                }, TaskScheduler.FromCurrentSynchronizationContext());

                t1.Start();
            }
            catch (DevOpsTaskException ex)
            {
                HandleException(ex);
                return PartialView("_TasksFail");
            }
            catch (AggregateException ag)
            {
                HandleException(ag);
                return PartialView("_TasksFail");
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return PartialView("_TasksFail");
            }
            finally
            {
                var tasks = taskRunner.GetDevOpsTaskWithState();
                //could serialize the state back to the tasks file for verification
                //taskRunner.S.SerializeTasksToFile(tasks, ConfigurationManager.AppSettings["tasksPath"]);
            }

            var model = new TaskInformationModel()
            {                
                TasksPath = tasksPath,
                TasksStartTime = startTime.ToString(),

            };

            return PartialView("_TasksRunning", model);
        }

        public ActionResult TasksComplete()
        {
            return PartialView("_TasksComplete");
        }

        public ActionResult TasksSummary()
        {
            var runPath = GetRunPath();
            if (!string.IsNullOrEmpty(runPath))
            {
                var log = FileClient.ReadAllText(Path.Combine(runPath, "build.log"));
                var tasks = FileClient.ReadAllText(Path.Combine(runPath, "build.tasks"));
                

                var model = new TaskSummaryModel()
                {
                    BuildTasks = tasks,
                    BuildLabel = "build x.x.x.",
                    BuildTime = "2 minutes",
                    BuildErrors = "None",
                    BuildWarnings = "None",
                    BuildLog = log,
                };

                return PartialView("_TasksSummary", model);
            }

            return PartialView("_TasksSummary");
        }

        public ActionResult TasksFail()
        {
            Session.Clear();
            return PartialView("_TasksFail");
        }

        #endregion


        #region Task Management

        /// <summary>
        /// Retrieves the status of the task run
        /// </summary>
        /// <returns></returns>
        public JsonResult GetStatus()
        {
            var d_status = GetTaskStatus();
            var d_detailsMarkup = GetDetails(d_status);

            dynamic data = new
            {
                status = d_status.ToString(),
            };

            return Json(data);
        }

        private string GetDetails(DevOpsTaskStatus d_status)
        {
            if (d_status == DevOpsTaskStatus.Completed)
            {

                string runPath = GetRunPath();

                if (!string.IsNullOrEmpty(runPath))
                {
                    return string.Format("<a href='{0}'>Build Packages</a>", Path.Combine(runPath, "build.log"));
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Retrieves the task status
        /// </summary>
        /// <returns></returns>
        private DevOpsTaskStatus GetTaskStatus()
        {
            string runPath = GetRunPath();

            if (string.IsNullOrEmpty(runPath))
                return DevOpsTaskStatus.Idle;


            if (System.IO.File.Exists(Path.Combine(runPath, "completed.dat")))
            {
                return DevOpsTaskStatus.Completed;

            }
            else if (System.IO.File.Exists(Path.Combine(runPath, "error.dat")))
            {
                return DevOpsTaskStatus.Faulted;
            }

            if (System.IO.File.Exists(Path.Combine(runPath, "processing.dat")))
            {
                return DevOpsTaskStatus.Running;
            }

            return DevOpsTaskStatus.Idle;
        }

        /// <summary>
        /// Retrieves the run path
        /// </summary>
        /// <returns></returns>
        private string GetRunPath()
        {
            string runPath = string.Empty;

            if (Session["runpath"] != null)
            {
                runPath = Session["runpath"].ToString();
            }

            return runPath;
        }

        /// <summary>
        /// Adds a message for broadcast
        /// </summary>
        /// <param name="msg"></param>
        private void AddMessage(string msg)
        {
            if (string.IsNullOrEmpty(msg))
                return;

            Broadcaster.Instance.AddMessage(new MessageModel
            {
                Message = msg,
                Id = Guid.NewGuid().ToString(),
            });

        }

        private IEnumerable<TaskModel> LoadTaskDefinitions()
        {
            List<TaskModel> tasks = new List<TaskModel>();
            var tasksDestinationRootPath = ConfigurationManager.AppSettings["tasksDefinitionsRootPath"];
                        
            if (!Directory.Exists(tasksDestinationRootPath))
            {                
                throw new DirectoryNotFoundException("Cannot find the tasks definition directory");
            }

            DirectoryInfo info = new DirectoryInfo(tasksDestinationRootPath);
            var directories = info.EnumerateDirectories();
            foreach (var dir in directories)
            {                
                var files = dir.GetFiles("*.tasks");
                
                foreach (var file in files)
                {
                    var model = (new TaskModel()
                    {
                        Task = Path.GetFileNameWithoutExtension(file.Name),
                        FullName = file.Name,
                        Path = dir.FullName,
                        Project = dir.Name,

                    });

                    var definition = FileClient.ReadAllText(file.FullName);

                    //StringBuilder definitionBuilder = SplitLinesWithHtmlBR(definition);

                    tasks.Add(new TaskModel()
                    {
                        Task = Path.GetFileNameWithoutExtension(file.Name),
                        FullName = file.Name,
                        Path = dir.FullName,
                        Project = dir.Name,
                        //Definition = definitionBuilder.ToString(),
                        Definition = definition,

                    });





                }
            }


            return tasks;
        }

        private static StringBuilder SplitLinesWithHtmlBR(string definition)
        {
            string[] definitionLines = definition.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            StringBuilder definitionBuilder = new StringBuilder();
            foreach (var line in definitionLines)
            {
                definitionBuilder.AppendFormat("{0}{1}", line, "<br>");
            }

            return definitionBuilder;
        }

        /// <summary>
        /// Initialises the build
        /// </summary>
        /// <returns></returns>
        private string InitialiseBuildRun()
        {           

            var tasksDestinationPath = ConfigurationManager.AppSettings["tasksRunnerRootPath"];

            //root path for the source task artifacts
            var tasksSourcePath = ConfigurationManager.AppSettings["tasksSourcePath"];

            //create new directory for tasks to run
            if (!Directory.Exists(tasksDestinationPath))
            {
                Directory.CreateDirectory(tasksDestinationPath);
            }

            int latestCount = GetNextBuildNumber(tasksDestinationPath);

            var runPath = Path.Combine(tasksDestinationPath, string.Format("{0}", latestCount));
            Directory.CreateDirectory(runPath);

            //generate basic log to identify task run
            string path = Path.Combine(runPath, string.Format("{0}", "build.log"));
            // This text is added only once to the file.
            if (!System.IO.File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = System.IO.File.CreateText(path))
                {
                    sw.WriteLine(string.Format("{0} Ver: {1}", "Build Runner version", "2.1"));
                    sw.WriteLine(string.Format("{0} {1}", DateTime.UtcNow, runPath));
                }
            }

            //copy build artifacts                
            DirectoryClient.DirectoryCopy(tasksSourcePath, runPath, true);
            return runPath;
        }

        private string InitialiseBuildRun(string sourcePath)
        {
            var tasksDestinationPath = ConfigurationManager.AppSettings["tasksRunnerRootPath"];

            //root path for the source task artifacts
            var tasksSourcePath = sourcePath;

            //create new directory for tasks to run
            if (!Directory.Exists(tasksDestinationPath))
            {
                Directory.CreateDirectory(tasksDestinationPath);
            }

            int latestCount = GetNextBuildNumber(tasksDestinationPath);

            var runPath = Path.Combine(tasksDestinationPath, string.Format("{0}", latestCount));
            Directory.CreateDirectory(runPath);

            //generate basic log to identify task run
            string path = Path.Combine(runPath, string.Format("{0}", "build.log"));
            // This text is added only once to the file.
            if (!System.IO.File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = System.IO.File.CreateText(path))
                {
                    sw.WriteLine(string.Format("{0} Ver: {1}", "Build Runner version", "2.1"));
                    sw.WriteLine(string.Format("{0} {1}", DateTime.UtcNow, runPath));
                }
            }

            //copy build artifacts                
            DirectoryClient.DirectoryCopy(tasksSourcePath, runPath, true);
            return runPath;
        }

        /// <summary>
        /// Gets the next build number
        /// </summary>
        /// <param name="rootPath"></param>
        /// <returns></returns>
        private static int GetNextBuildNumber(string rootPath)
        {
            DirectoryInfo info = new DirectoryInfo(rootPath);
            var directories = info.GetDirectories();
            int latestCount = 0;
            foreach (var dir in directories)
            {
                int res;
                if (int.TryParse(dir.Name, out res))
                {
                    if (res > latestCount)
                    {
                        latestCount = res;
                    }
                }

            }
            latestCount++;
            return latestCount;
        }

        /// <summary>
        /// Handles exceptions
        /// </summary>
        /// <param name="e"></param>
        private void HandleException(Exception e)
        {
            if (e is DevOpsTaskException)
            {
                var devOpsEx = e as DevOpsTaskException;
                AddMessage(string.Format("{0} failed with error: {1}", devOpsEx.Task != null ? devOpsEx.Task.Description : string.Empty, devOpsEx.Message));
            }
            else
            {
                AddMessage(string.Format("Failed with error: {0}", e.Message));
            }
        }

        delegate void AddStatusCallback(string text);

        #endregion
    }
}
