using System.Data;
namespace project1_lib
{
    public class Class1
    {
        Task_ManagerContext data = new Task_ManagerContext();
        public int add_user(Register r)
        {
            data.Registers.Add(r);
            int isUserCreated = data.SaveChanges(); 
            return isUserCreated;   
        }
        public bool login(string email, string password) 
        { 
            bool isValidUser = data.Registers.Where(c => c.EmailId == email && c.Password == password).Count() > 0; 
            return isValidUser;
        }
        public IEnumerable<Project> DisplayProjects(int id) 
        {
         
            var res = from p in data.Projects
                      where p.UserId == id
                      select p;

            return res;
            
        }
        public object currentUserId(string email)
        {
            var currentUserId = (from id in data.Registers
                                 where id.EmailId == email
                                 select id.UserId).FirstOrDefault();
            return currentUserId;   

        }
        public int add_project(string ProjectTitle, string ProjectDesc, int currentUserId)
        {
            Project o = new Project();
            o.ProjectTitle = ProjectTitle;
            o.ProjectDescription = ProjectDesc; 
            o.UserId = currentUserId;
            data.Projects.Add(o);
      
            int isProjectCreated = data.SaveChanges();
            return isProjectCreated;
        }
        public int add_task(string taskName, string taskDesc, DateTime taskDate, int currentUserId, int currentProjectId)
        {
            Task t = new Task();    
            t.TaskName = taskName;
            t.TaskDescription = taskDesc;   
            t.TaskDueDate = taskDate;
            t.UserId = currentUserId;
            t.ProjectId = currentProjectId; 
            data.Tasks.Add(t);
            int isTaskAdded = data.SaveChanges();
            return isTaskAdded;
        }
        public IEnumerable<Task> DisplayTasks(int currentProjectId)
        {

            var res = from t in data.Tasks
                      where t.ProjectId == currentProjectId 
                      select t;

            return res;
        }

        public int status_change(int id)
        {
            var a = data.Tasks.First(g => g.TaskId == id);
            a.TaskStatus = true;
            data.Tasks.Update(a);
            int isTaskStatusUpdated = data.SaveChanges();
            return isTaskStatusUpdated;
        }


    }

}
