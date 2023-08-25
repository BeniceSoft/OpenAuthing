namespace BeniceSoft.OpenAuthing;

public static class ApplicationContractsConstants
{
    public static class Lock
    {
        public const string DepartmentMemberLeader = "lock:department:{departmentId}:member:{userId}:leader";
        public const string DepartmentMemberMain = "lock:department:{departmentId}:member:{userId}:main";
    }
}