using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using TorshiaWebApp.Models;
using TorshiaWebApp.ViewModels;
using TorshiaWebApp.ViewModels.Tasks;

namespace TorshiaWebApp.Controllers
{
    public class TasksController : BaseController
    {
        [Authorize]
        public IHttpResponse Details(int id)
        {
            var viewModel = this.Db.Tasks.Where(t => t.Id == id).Select(t => new DetailsViewModel()
            {
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate.ToString("d"),
                Level = t.TaskSectors.Select(ts => ts.Sector).Count(), // maybe include sector and tasksector
                Participants = t.Participants,
                AffectedSectors = string.Join(", ", t.TaskSectors.Select(ts => ts.Sector.Name.ToString())),
            }).FirstOrDefault();

            //var affectedSectorsString = GetAffectedSectorsString(id);

            //viewModel.AffectedSectors = affectedSectorsString;

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
        public IHttpResponse Create()
        {
            if (this.User.Role != "Admin")
            {
                return BadRequestError("You dont have permission");
            }

            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IHttpResponse Create(CreateTaskInputModel model)
        {
            if (this.User.Role != "Admin")
            {
                return this.BadRequestError("You dont have permission");
            }
            var sectors = new List<string>()
            {
                model.CustomersSector,
                model.FinancesSector,
                model.InternalSector,
                model.ManagementSector,
                model.MarketingSector
            };

            FilterSectrors(sectors);

            var dueDate = DateTime.ParseExact(model.DueDate, "yyyy-mm-dd", CultureInfo.CurrentCulture);


            foreach (var sector in sectors)
            {
                if (this.Db.Sectors.Any(s => s.Name == sector))
                {
                    continue;
                }
                this.Db.Sectors.Add(new Sector() { Name = sector });
                this.Db.SaveChanges();
            }

            var task = this.Db.Tasks.Add(new Task()
            {
                Title = model.Title,
                Description = model.Description,
                DueDate = dueDate,
                Participants = model.Participants
            });
            this.Db.SaveChanges();

            foreach (var sector in sectors)
            {
                this.Db.TaskSectors.Add(new TaskSector()
                {
                    TaskId = this.Db.Tasks.FirstOrDefault(t => t.Title == model.Title).Id,
                    SectorId = this.Db.Sectors.FirstOrDefault(s => s.Name == sector).Id,
                });
                this.Db.SaveChanges();
            }


            

            return this.Redirect("/");
        }

        private static void FilterSectrors(List<string> sectors)
        {
            for (int i = 0; i < sectors.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(sectors[i]))
                {
                    sectors.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
