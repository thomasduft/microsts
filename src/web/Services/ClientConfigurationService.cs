using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace tomware.Microsts.Web
{
  public interface IClientConfigurationService
  {
    Task<ClientConfigurationViewModel> GetClient(string clientId);
  }

  public class ClientConfigurationService : IClientConfigurationService
  {
    private readonly ClientSettings clientSettings;

    public ClientConfigurationService(
      IOptions<ClientSettings> options
    )
    {
      this.clientSettings = options.Value;
    }

    public async Task<ClientConfigurationViewModel> GetClient(string clientId)
    {
      if (clientId is null)
      {
        throw new System.ArgumentNullException(nameof(clientId));
      }

      var clientConfig = this.clientSettings.Clients.FirstOrDefault(c => c.ClientId == clientId);
      if (clientConfig == null) return null;

      var model = new ClientConfigurationViewModel
      {
        ClientId = clientConfig.ClientId,
        Issuer = clientConfig.Issuer,
        RedirectUri = clientConfig.RedirectUri,
        ResponseType = clientConfig.ResponseType,
        Scope = clientConfig.Scope,
        LoginUrl = clientConfig.LoginUrl,
        LogoutUrl = clientConfig.LogoutUrl
      };

      return await Task.FromResult(model);
    }
  }
}