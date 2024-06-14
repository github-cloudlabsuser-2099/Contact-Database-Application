using CRUD_application_2.Models;
using System.Linq;
using System.Web.Mvc;

namespace CRUD_application_2.Controllers
{
    public class UserController : Controller
    {
        public static System.Collections.Generic.List<User> userlist = new System.Collections.Generic.List<User>();
        // GET: User
        public ActionResult Index()
        {
            // This method is responsible for displaying a list of all users.
            // It retrieves all users from the userlist and passes them to the Index view.
            return View(userlist);
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            // This method is responsible for displaying the details of a specific user.
            // It retrieves the user from the userlist based on the provided ID and passes it to the Details view.
            var user = userlist.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View("Create");
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            if (!ModelState.IsValid)
            {
                // Explicitly return the "Create" view with the user model when the model state is invalid
                return View("Create", user);
            }

            // Assuming User has an Id property that needs to be set manually.
            // Find the max ID and increment by 1 for the new user (simple auto-increment logic).
            user.Id = userlist.Any() ? userlist.Max(u => u.Id) + 1 : 1;
            userlist.Add(user);
            return RedirectToAction("Index");
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            // This method is responsible for displaying the view to edit an existing user with the specified ID.
            // It retrieves the user from the userlist based on the provided ID and passes it to the Edit view.
            var user = userlist.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                // Explicitly return the "Edit" view with the user model when the model state is invalid
                return View("Edit", user);
            }

            var existingUser = userlist.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
            {
                return HttpNotFound();
            }

            // Update the user details here
            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            // Update other properties as needed

            return RedirectToAction("Index");
        }


        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            // This method is responsible for displaying the confirmation view for deleting a specific user.
            // It retrieves the user from the userlist based on the provided ID and passes it to the Delete view.
            var user = userlist.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            // This method is responsible for handling the HTTP POST request to delete an existing user.
            // It finds the user in the userlist based on the provided ID and removes it.
            // If successful, it redirects to the Index action to display the updated list of users.
            var user = userlist.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                userlist.Remove(user);
            }
            return RedirectToAction("Index");
        }
    }
}
