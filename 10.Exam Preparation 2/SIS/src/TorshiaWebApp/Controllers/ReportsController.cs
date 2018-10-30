using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using TorshiaWebApp.Models;
using TorshiaWebApp.ViewModels.Reports;

namespace TorshiaWebApp.Controllers
{
    public class ReportsController : BaseController
    {
        [Authorize]
        public IHttpResponse Details(int id, string status)
        {
            if (this.User.Role != "Admin")
            {
                return BadRequestError("You dont have permission.");
            }
            var report = this.Db.Reports
                .Include(r => r.Task)
                .ThenInclude(t => t.TaskSectors)
                .Include(r => r.Reporter)
                .FirstOrDefault(r => r.Id == id);
            if (report == null)
            {
                return BadRequestError("Invalid Report");
            }

            var task = report.Task;
            var reporter = report.Reporter;

            var viewModel = new ReportDetailsViewModel()
            {
                Report = report,
                Task = task,
                Reporter = reporter.Username,
                TaskLevel = task.TaskSectors.Select(x => x.Sector).Count(),
                TaskDueDate = task.DueDate.ToString("d"),
                ReportReportedOn = report.ReportedOn.ToString("d"),
                AffectedSectors = GetAffectedSectorsString(task.Id),
                TaskStatus = status
            };

            return this.View(viewModel);
        }

        private string GetAffectedSectorsString(int id)
        {
            var affectedSectors = this.Db.TaskSectors.Where(ts => ts.TaskId == id).Select(ts => ts.Sector).ToArray();
            var affectedSectorsString = string.Empty;
            for (int i = 0; i < affectedSectors.Length; i++)
            {
                if (i == affectedSectors.Length - 1)
                {
                    affectedSectorsString += affectedSectors[i].Name;
                    continue;
                }

                affectedSectorsString += affectedSectors[i].Name + ", ";
            }

            return affectedSectorsString;
        }

        [Authorize]
        public IHttpResponse All()
        {
            if (this.User.Role != "Admin")
            {
                return this.BadRequestError("You dont have permission");
            }
            var reports = new ReportViewModel();
            reports.Reports = this.Db.Reports.Select(r => new BaseReportViewModel()
            {
                Id = r.Id,
                Title = r.Task.Title,
                Level = r.Task.TaskSectors.Select(x => x.Sector).Count().ToString(),
                Status = r.Status.ToString(),
            }).ToArray();


            return this.View(reports);
        }

        public IHttpResponse Report(int id, string username)
        {
            Random rnd = new Random();
            int percents = rnd.Next(0, 100);
            var status = new Status();
            if (percents < 75)
            {
                status = Status.Completed;
            }
            else
            {
                status = Status.Archived;
            }



            var report = new Report()
            {
                ReportedOn = DateTime.UtcNow,
                Reporter = this.Db.Users.FirstOrDefault(u => u.Username == username),
                Status = status,
                Task = this.Db.Tasks.FirstOrDefault(t => t.Id == id)
            };

            this.Db.Tasks.FirstOrDefault(t => t.Id == id).IsReported = true;

            try
            {
                this.Db.Reports.Add(report);
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                return this.ServerError(e.Message);
            }

            if (this.User.Role == "Admin")
            {
               return this.Redirect("/Reports/All"); 
            }
            else
            {
                return this.Redirect("/");
            }

            
        }
    }
}
