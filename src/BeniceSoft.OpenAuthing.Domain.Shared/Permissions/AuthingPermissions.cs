namespace BeniceSoft.OpenAuthing.Permissions;

public static class AuthingPermissions
{
    public const string Dashboard = "Dashboard";
    public const string ViewDashboard = Dashboard + ".View";

    public const string Organization = "Organization";
    public const string ManageDepartment = Organization + ".ManageDepartment";
    public const string CreateDepartment = ManageDepartment + ".Create";
    public const string UpdateDepartment = ManageDepartment + ".Update";
    public const string DeleteDepartment = ManageDepartment + ".Delete";
    
}