using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace BeniceSoft.OpenAuthing.OpenIddict;

public class AmOpenIddictStoreBase<TRepository>
    where TRepository : IRepository
{
    public ILogger<AmOpenIddictStoreBase<TRepository>> Logger { get; set; }

    protected TRepository Repository { get; }
    protected IUnitOfWorkManager UnitOfWorkManager { get; }
    protected IGuidGenerator GuidGenerator { get; }

    protected AmOpenIddictStoreBase(TRepository repository, IUnitOfWorkManager unitOfWorkManager, IGuidGenerator guidGenerator)
    {
        Repository = repository;
        UnitOfWorkManager = unitOfWorkManager;
        GuidGenerator = guidGenerator;

        Logger = NullLogger<AmOpenIddictStoreBase<TRepository>>.Instance;
    }

    protected virtual Guid ConvertIdentifierFromString(string identifier)
    {
        return string.IsNullOrEmpty(identifier) ? default : Guid.Parse(identifier);
    }

    protected virtual string ConvertIdentifierToString(Guid identifier)
    {
        return identifier.ToString("D");
    }

    protected virtual string WriteStream(Action<Utf8JsonWriter> action)
    {
        using (var stream = new MemoryStream())
        {
            using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions
                   {
                       Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                       Indented = false
                   }))
            {
                action(writer);
                writer.Flush();
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }

    protected virtual async Task<string> WriteStreamAsync(Func<Utf8JsonWriter, Task> func)
    {
        using (var stream = new MemoryStream())
        {
            using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions
                   {
                       Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                       Indented = false
                   }))
            {
                await func(writer);
                await writer.FlushAsync();
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }
}