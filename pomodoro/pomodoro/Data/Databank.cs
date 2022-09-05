using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database.Query;
namespace pomodoro.Data
{
    class Databank
    {

        static FirebaseClient fbClient = new FirebaseClient("https://projectwebrtdb-default-rtdb.europe-west1.firebasedatabase.app/");

        public static List<PomTask> Tasks = new List<PomTask>();

        public static string userLoggedIn = string.Empty;
        public static string userKey = string.Empty;
        public async static Task<bool> initializeDataBank()
        {
       
            userKey = (await fbClient.Child("Users").OnceAsync<Users>()).Where(x => x.Object.Username == userLoggedIn).FirstOrDefault().Key;

            var taskData = await fbClient.Child("Users").Child(userKey).Child("Tasks").OnceAsync<PomTask>();
            foreach (var task in taskData)
            {
                var newTask = new PomTask(task.Object.TaskName.ToString(), Convert.ToInt32( task.Object.CurrPom), Convert.ToInt32(task.Object.MaxPom));
                newTask.taskId = Convert.ToInt32(task.Object.taskId);
                Tasks.Add(newTask);
            }

            return true;
        }

        public async static void updateTask(PomTask task)
        {

            var taskToUpdate = (await fbClient.Child("Users").Child(userKey).Child("Tasks").OnceAsync<PomTask>()).Where(x => x.Object.taskId == task.taskId).FirstOrDefault();

            var updatedTask = new PomTask(task.TaskName, task.CurrPom, task.MaxPom);
            updatedTask.taskId = task.taskId;
            
            await fbClient
                .Child("Users")
                .Child(userKey)
                .Child("Tasks")
                .Child(taskToUpdate.Key)
                .PutAsync(updatedTask);
        }
        public async static void deleteTask(PomTask task)
        {
            var taskToDelete = (await fbClient.Child("Users").Child(userKey).Child("Tasks").OnceAsync<PomTask>()).Where(x => x.Object.taskId == task.taskId).FirstOrDefault();
            await fbClient
               .Child("Users")
               .Child(userKey)
               .Child("Tasks")
               .Child(taskToDelete.Key)
               .DeleteAsync();
        }


            public async static Task addTask(PomTask newTask)
        {
            int id;
            if (!Tasks.Any())
            {
                id = 0;
            }
            else
            {
                id = Tasks.Max(x => x.taskId) + 1;
            }
             
            newTask.taskId = id;
            Tasks.Add(newTask);
            await fbClient
                .Child("Users")
                .Child(userKey)
                .Child("Tasks")
                .PostAsync(JsonConvert.SerializeObject(newTask)); 
        }

        public async static Task Register(string email, string username, string password)
        {
            var checkEmail = (await fbClient.Child("Users").OnceAsync<Users>()).Where(x => x.Object.Email == email).FirstOrDefault();
            if(checkEmail == null)
            {
                var newUser = new Users() { Email = email, Username = username, Password = password};

                await fbClient
                .Child("Users")
                .PostAsync(JsonConvert.SerializeObject(newUser));
            }
        }

        public async static Task<bool> Login(string email, string password)
        {
            var userToCheck = (await fbClient.Child("Users").OnceAsync<Users>()).Where(x => x.Object.Email == email && x.Object.Password == password).FirstOrDefault();

            if (userToCheck != null)
            {
                userLoggedIn = userToCheck.Object.Username;
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
