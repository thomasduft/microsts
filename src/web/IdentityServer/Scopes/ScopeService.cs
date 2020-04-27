using System.Collections.Generic;
using System.Threading.Tasks;

namespace tomware.Microsts.Web
{
  public interface IScopeService
  {
    Task<IEnumerable<ScopeViewModel>> GetScopesAsync();

    Task<ScopeViewModel> GetAsync(string name);

    Task<string> CreateAsync(ScopeViewModel model);

    Task UpdateAsync(ScopeViewModel model);

    Task DeleteAsync(string name);
  }

  public class ScopeService : IScopeService
  {
    private readonly STSContext context;

    public ScopeService(STSContext context)
    {
      this.context = context;
    }

    public Task<IEnumerable<ScopeViewModel>> GetScopesAsync()
    {
      throw new System.NotImplementedException();
    }

    public Task<ScopeViewModel> GetAsync(string name)
    {
      throw new System.NotImplementedException();
    }

    public Task<string> CreateAsync(ScopeViewModel model)
    {
      throw new System.NotImplementedException();
    }

    public Task UpdateAsync(ScopeViewModel model)
    {
      throw new System.NotImplementedException();
    }

    public Task DeleteAsync(string name)
    {
      throw new System.NotImplementedException();
    }
  }
}