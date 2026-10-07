// DTO trả về cho FE
public class AdminSaleEventDto
{
  public string Id { get; set; } = string.Empty;          // Mongo ObjectId string
  public string Title { get; set; } = string.Empty;       // Tên event
  public string Color { get; set; } = string.Empty;       // "Danger" | "Success" | "Primary" | "Warning"
  public DateTime StartDate { get; set; } // UTC
  public DateTime EndDate { get; set; }   // UTC
  public double Percent { get; set; }     // 0–1 (vd 0.2 = 20%)
  public List<string> ProductIds { get; set; } = new();
}

// DTO tạo mới
public class CreateAdminSaleEventDto
{
  public string Title { get; set; } = string.Empty;
  public string Color { get; set; } = string.Empty;
  public DateTime StartDate { get; set; }
  public DateTime EndDate { get; set; }
  public double Percent { get; set; }          // 0–1
  public List<string> ProductIds { get; set; } = new();
}

// DTO update
public class UpdateAdminSaleEventDto
{
  public string Title { get; set; } = string.Empty;
  public string Color { get; set; } = string.Empty;
  public DateTime StartDate { get; set; }
  public DateTime EndDate { get; set; }
  public double Percent { get; set; }
  public List<string> ProductIds { get; set; } = new();
}
