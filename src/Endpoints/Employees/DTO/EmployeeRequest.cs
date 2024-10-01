namespace IWantApp.Endpoints.Employees.DTO;

public record EmployeeRequest(
    string Email,
    string Password,
    string Name,
    string EmployeeCode);
