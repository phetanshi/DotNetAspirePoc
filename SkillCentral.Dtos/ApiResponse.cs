namespace SkillCentral.Dtos;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public T Payload { get; set; }
}
