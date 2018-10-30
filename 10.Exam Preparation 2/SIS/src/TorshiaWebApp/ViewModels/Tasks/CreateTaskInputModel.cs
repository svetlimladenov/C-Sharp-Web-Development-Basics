namespace TorshiaWebApp.ViewModels
{
    public class CreateTaskInputModel
    {
        public string Title { get; set; }

        public string DueDate { get; set; }

        public string Description { get; set; }

        public string Participants { get; set; }

        public string InternalSector { get; set; }

        public string ManagementSector { get; set; }

        public string FinancesSector { get; set; }

        public string MarketingSector { get; set; }

        public string CustomersSector { get; set; }
    }
}
