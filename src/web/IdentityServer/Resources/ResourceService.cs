using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace tomware.Microsts.Web
{
  public interface IResourceService
  {
    Task<IEnumerable<ApiResourceViewModel>> GetApiResourcesAsync();

    Task<ApiResourceViewModel> GetApiResourceAsync(string name);

    Task<string> CreateApiResourceAsync(ApiResourceViewModel model);

    Task UpdateApiResourceAsync(ApiResourceViewModel model);

    Task DeleteApiResourceAsync(string name);
  }

  public class ResourceService : IResourceService
  {
    private readonly ConfigurationDbContext context;

    public ResourceService(
      ConfigurationDbContext context
    )
    {
      this.context = context;
    }

    public async Task<IEnumerable<ApiResourceViewModel>> GetApiResourcesAsync()
    {
      var items = await this.LoadAll()
        .AsNoTracking()
        .ToListAsync();

      return items.Select(x => ToModel(x));
    }

    public async Task<ApiResourceViewModel> GetApiResourceAsync(string name)
    {
      if (name == null) throw new ArgumentNullException(nameof(name));

      var apiResource = await this.GetApiResourceByName(name);

      return apiResource != null ? ToModel(apiResource) : null;
    }

    public async Task<string> CreateApiResourceAsync(ApiResourceViewModel model)
    {
      if (model is null) throw new System.ArgumentNullException(nameof(model));

      var apiResource = new ApiResource
      {
        Enabled = model.Enabled,
        Name = model.Name,
        DisplayName = model.DisplayName,
        Description = string.Empty
      };

      HandleCollectionProperties(model, apiResource);

      this.context.ApiResources.Add(apiResource);

      await this.context.SaveChangesAsync();

      return apiResource.Name;
    }

    public async Task UpdateApiResourceAsync(ApiResourceViewModel model)
    {
      if (model == null) throw new ArgumentNullException(nameof(model));

      var apiResource = await this.GetApiResourceByName(model.Name);
      if (apiResource == null) throw new ArgumentNullException(nameof(apiResource));

      apiResource.Enabled = model.Enabled;
      apiResource.Name = model.Name;
      apiResource.DisplayName = model.DisplayName;
      apiResource.Description = string.Empty;

      HandleCollectionProperties(model, apiResource);

      this.context.ApiResources.Update(apiResource);

      await this.context.SaveChangesAsync();
    }

    public async Task DeleteApiResourceAsync(string name)
    {
      if (name == null) throw new ArgumentNullException(nameof(name));

      var apiResource = await this.context.ApiResources
        .FirstOrDefaultAsync(c => c.Name == name);

      this.context.ApiResources.Remove(apiResource);

      await this.context.SaveChangesAsync();
    }

    private IOrderedQueryable<ApiResource> LoadAll()
    {
      return this.context.ApiResources
              .Include(x => x.Scopes)
              .Include(x => x.UserClaims)
              .OrderBy(x => x.Name);
    }

    private async Task<ApiResource> GetApiResourceByName(string name)
    {
      List<ApiResource> items = await this.LoadAll()
        .Where(x => x.Name == name)
        .ToListAsync();

      return items.Count() == 1 ? items.First() : null;
    }

    private ApiResourceViewModel ToModel(ApiResource entity)
    {
      return new ApiResourceViewModel
      {
        Id = entity.Id,
        Enabled = entity.Enabled,
        Name = entity.Name,
        DisplayName = entity.DisplayName,
        Scopes = entity.Scopes
          .Select(s => s.Scope).ToList(),
        UserClaims = entity.UserClaims
          .Select(x => x.Type).ToList()
      };
    }

    private static void HandleCollectionProperties(
      ApiResourceViewModel model,
      ApiResource apiResource
    )
    {
      // deassign them
      if (apiResource.Scopes != null) apiResource.Scopes.Clear();
      if (apiResource.UserClaims != null) apiResource.UserClaims.Clear();

      // assign them
      apiResource.Scopes = model.Scopes
        .Select(s => new ApiResourceScope
        {
          ApiResource = apiResource,
          Scope = s
        }).ToList();

      apiResource.UserClaims = model.UserClaims
        .Select(x => new ApiResourceClaim
        {
          ApiResource = apiResource,
          Type = x
        }).ToList();
    }
  }
}