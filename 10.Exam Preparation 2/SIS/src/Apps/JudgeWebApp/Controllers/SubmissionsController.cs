using System;
using System.Linq;
using System.Net;
using JudgeWebApp.Models;
using JudgeWebApp.ViewModels.Contests;
using JudgeWebApp.ViewModels.Submissions;
using SIS.HTTP.Responses;
using SIS.MvcFramework;

namespace JudgeWebApp.Controllers
{
    public class SubmissionsController : BaseController
    {
        [Authorize]
        public IHttpResponse All()
        {
            var viewModel = new AllSubmissionsViewModel();

            viewModel.ContestAndSubmission = this.Db.Contests.Where(c => c.IsDeleted == false).Select(x => new BaseContestAndSubmissionViewModel()
            {
                Name = x.Name.Replace(" ", string.Empty),
                Submissions = x.Submissions.Select(s => new BaseSubmissionViewModel()
                {
                    Id = s.Id,
                    UserId = s.User.Id,
                    IsSuccessfull = s.IsSuccessfull,
                    Username = s.User.Username,
                }).ToArray(),

            }).ToArray();

            return this.View(viewModel);
        }

        [Authorize]
        public IHttpResponse Create(string contestName)
        {
            var viewModel = new AllContestsViewModel();

            viewModel.Contests = this.Db.Contests.Where(c => c.IsDeleted == false).Select(x => new BaseContestViewModel()
            {
                Name = x.Name,
            }).ToArray();

            contestName = WebUtility.UrlDecode(contestName);
            viewModel.ContestNameSelected = contestName;

            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost()]
        public IHttpResponse Create(CreateSubmissionInputModel model)
        {
            if (this.Db.Contests.FirstOrDefault(c => c.Name == model.Contest) == null || this.Db.Users.FirstOrDefault(u => u.Username == this.User.Username) == null)
            {
                return this.BadRequestError("Invalid contest.");
            }

            var submission = new Submission()
            {
                Code = model.Code,
                ContestId = this.Db.Contests.FirstOrDefault(c => c.Name == model.Contest).Id,
                UserId = this.Db.Users.FirstOrDefault(u => u.Username == this.User.Username).Id,
                IsSuccessfull = GetRandomSuccess()
            };

            this.Db.Submissions.Add(submission);
            this.Db.Contests.FirstOrDefault(c => c.Name == model.Contest).Submissions.Add(submission);
            this.Db.Users.FirstOrDefault(u => u.Username == this.User.Username).Submissions.Add(submission);

            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return this.Redirect("/");
        }

        private bool GetRandomSuccess()
        {
            var random = new Random();
            var percentage = random.Next(1, 100);
            if (percentage <= 70)
            {
                return false;
            }

            return true;

        }
    }
}
