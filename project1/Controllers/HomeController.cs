using Microsoft.AspNetCore.Mvc;
using project1_lib;
namespace project1.Controllers
{
    public class HomeController : Controller 
       
    {

        Class1 ob = new Class1();
        
        [HttpGet]
        public ActionResult user_register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult user_register(Register r)
        {
            int isUserCreated = ob.add_user(r);
            if (isUserCreated > 0)
            {
                ViewData["Message"] = "User created successfully";
            }
            else
            {
                ViewData["Message"] = "User not created, fill it again";
            }
            return View();
        }
        [HttpGet]
        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(string email,string password)
        {
            bool isUserValid = ob.login(email, password);
            if (isUserValid)
            {
                HttpContext.Session.SetString("login_eamil", email);
                return RedirectToAction("home");
            }
            else
            {
                ViewData["Message"] = "Invalid user";
            }
            return View();
        }
        public ActionResult home()
        {
            //side navbar display of projects by that user
            int currentUserId = Convert.ToInt32(ob.currentUserId(HttpContext.Session.GetString("login_eamil")));
            var dispProjects = ob.DisplayProjects(currentUserId);
            return View(dispProjects);
        }
      
        public ActionResult trail(int projectId, string projectName)
        {
            ViewData["title"] = projectName;
            HttpContext.Session.SetString("currentProjectName", projectName);
            //tasks present under that particular project
            HttpContext.Session.SetInt32("currentProjectId", projectId);

            //display of tasks
            var displayTasks = ob.DisplayTasks(projectId); 
            ViewBag.displayTasks = displayTasks;
            
            //side navbar display
            int currentUserId = Convert.ToInt32(ob.currentUserId(HttpContext.Session.GetString("login_eamil")));
            var dispProjects = ob.DisplayProjects(currentUserId);
            return View(dispProjects);
    
        }
        [HttpGet]
        public ActionResult add_project()
        {
            return View();

        }
        [HttpPost]
        public ActionResult add_project(string ProjectTitle, string ProjectDesc)
        {
            int currentUserId = Convert.ToInt32(ob.currentUserId(HttpContext.Session.GetString("login_eamil")));
            int isProjectCreated = ob.add_project(ProjectTitle, ProjectDesc, currentUserId);
            if (isProjectCreated > 0)
            {
                return RedirectToAction("home");
            }
            else
            {
                ViewData["Message"] = "Project creation insuccessfull";
            }
            return View();

        }
        [HttpGet]
        public ActionResult add_tasks()
        {
            return View();
        }
        [HttpPost]
        public ActionResult add_tasks(string taskName, string taskDesc, DateTime taskDate)
        {
            int currentUserId = Convert.ToInt32(ob.currentUserId(HttpContext.Session.GetString("login_eamil")));
            int currentProjectId = Convert.ToInt32(HttpContext.Session.GetInt32("currentProjectId"));
            int isTaskAdded = ob.add_task(taskName, taskDesc, taskDate, currentUserId, currentProjectId);
            if(isTaskAdded > 0)
            {
                return RedirectToAction("home");
            }
            return View();
        }

        public ActionResult logout()
        {
            HttpContext.Session.Remove("login_eamil");
            return RedirectToAction("login");   
        }
        public ActionResult status_change(int id)
        {
            ViewData["title"] = HttpContext.Session.GetString("currentProjectName");
            
            int isTaskUpdated = ob.status_change(id);
            
            int currentProjectId = Convert.ToInt32(HttpContext.Session.GetInt32("currentProjectId"));
            var displayTasks = ob.DisplayTasks(currentProjectId);
            ViewBag.displayTasks = displayTasks;

            //side navbar display
            int currentUserId = Convert.ToInt32(ob.currentUserId(HttpContext.Session.GetString("login_eamil")));
            var dispProjects = ob.DisplayProjects(currentUserId);
            return View("trail",dispProjects);
        }
        

    }
}


