namespace BeniceSoft.OpenAuthing.Data;

public interface IAuthingDbSchemaMigrator
{
    Task MigrateAsync();
}