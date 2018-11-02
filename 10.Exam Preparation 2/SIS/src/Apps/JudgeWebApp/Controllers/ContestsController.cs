using System;
using System.Linq;
using JudgeWebApp.Models;
using JudgeWebApp.ViewModels.Contests;
using Microsoft.EntityFrameworkCore;
using SIS.HTTP.Responses;
using SIS.MvcFramework;

namespace JudgeWebApp.Controllers
{
    public class ContestsController : BaseController
    {
        public IHttpResponse All()
        {
            var viewModel = new AllContestsViewModel();
            viewModel.Contests = Db.Contests.Where(c => c.IsDeleted == false).Select(c => new BaseContestViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                CreatedByUser = c.User.Username == this.User.Username,
                SubmissionsCount = c.Submissions.Count,
            }).ToArray();

            return this.View(viewModel);
        }

        [Authorize]
        public IHttpResponse Create()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IHttpResponse Create(string name)
        {

            var user = this.Db.Users.FirstOrDefault(u => u.Username == this.User.Username);

            var contest = new Contest()
            {
                Name = name.Trim(),
                UserId = user.Id,
            };
            user.Contests.Add(contest);
            this.Db.Contests.Add(contest);

            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                return ServerError(e.Message);

            }

            return this.Redirect("/Contests/All");
        }

        public IHttpResponse Edit(int id)
        {
            var contestName = this.Db.Contests.FirstOrDefault(c => c.Id == id).Name;
            var viewModel = new EditAndDeleteViewModel() { ContestName = contestName };
            return this.View(viewModel);
        }
        [Authorize]
        [HttpPost]
        public IHttpResponse Edit(string name)
        {
            var contest = this.Db.Contests.Include(c => c.User).FirstOrDefault(c => c.Name == name);

            if (contest == null)
            {
                return BadRequestErrorWithView("Cant edit this contest.", new EditAndDeleteViewModel() { ContestName = name });
            }

            if (this.User.Role != "Admin")
            {
                if (contest.User.Username != this.User.Username)
                {
                    return BadRequestErrorWithView("Cant edit this contest.", new EditAndDeleteViewModel() { ContestName = name });
                }
            }

            return this.Redirect("/Contests/All");
        }

        [Authorize]
        public IHttpResponse Delete(int id)
        {
            var contestName = this.Db.Contests.FirstOrDefault(c => c.Id == id).Name;
            var viewModel = new EditAndDeleteViewModel() { ContestName = contestName };
            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public IHttpResponse Delete(string name)
        {
            var contest = this.Db.Contests.Include(c => c.User).FirstOrDefault(c => c.Name == name);

            if (contest == null)
            {
                return BadRequestErrorWithView("Cant delete this contest.", new EditAndDeleteViewModel() { ContestName = name });
            }

            if (this.User.Role != "Admin")
            {
                if (contest.User.Username != this.User.Username)
                {
                    return BadRequestErrorWithView("Cant delete this contest.", new EditAndDeleteViewModel() { ContestName = name });
                }
            }



            contest.IsDeleted = true;
            this.Db.SaveChanges();
            return this.Redirect("/Contests/All");
        }
    }
}
