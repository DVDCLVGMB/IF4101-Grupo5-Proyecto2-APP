public class OrderRequestDto
{
    public int OrderId { get; set; }      // 0 al crear
    public int ClientId { get; set; }
    public int EmployeeId { get; set; }
    public int CityId { get; set; }
    public DateTime OrderDate { get; set; }

    public List<OrderDetailRequestDto> OrderDetails { get; set; } = new();
}
public class OrderDetailRequestDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}