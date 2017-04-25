using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MockStackOverflow.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MockStackOverflow.Controllers
{
    [Authorize]
    public class AnswersController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnswersController (UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;

        }

        public IActionResult Create(int id)
        {
            var thisQuestion = _db.Questions.FirstOrDefault(questions => questions.Id == id);
            ViewBag.thisQuestion = thisQuestion; 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Answer answer, int id)
        {
            var thisQuestion = _db.Questions.FirstOrDefault(questions => questions.Id == id);
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            answer.User = currentUser;
            answer.Question = thisQuestion;
            _db.Answers.Add(answer);
            _db.SaveChanges();
            return RedirectToAction("Details", "Questions", thisQuestion);

        }


    }
}
