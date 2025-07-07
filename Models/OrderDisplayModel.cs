namespace Steady_Management_App.Models
{
    public class OrderDisplayModel
    {
        public int OrderId { get; set; }
        public string ClientName { get; set; }
        public string EmployeeName { get; set; }
        public int CityId { get; set; } 
        public DateTime OrderDate { get; set; }
        public object CityName { get; set; }
    }
}
