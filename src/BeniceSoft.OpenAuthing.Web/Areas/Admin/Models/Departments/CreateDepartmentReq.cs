namespace BeniceSoft.OpenAuthing.Areas.Admin.Models.Departments;

public class CreateDepartmentReq
{
    public string Code { get;  set; }
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public int Seq { get; set; } = 0;
}