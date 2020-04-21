﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace tomware.Microsts.Web.Areas.Identity.Pages.Account.Manage
{
  public class DownloadPersonalDataModel : PageModel
  {
    private readonly UserManager<ApplicationUser> userManager;
    private readonly ILogger<DownloadPersonalDataModel> logger;

    public DownloadPersonalDataModel(
        UserManager<ApplicationUser> userManager,
        ILogger<DownloadPersonalDataModel> logger)
    {
      this.userManager = userManager;
      this.logger = logger;
    }

    public async Task<IActionResult> OnPostAsync()
    {
      var user = await userManager.GetUserAsync(User);
      if (user == null)
      {
        return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
      }

      logger.LogInformation(
        "User with ID '{UserId}' asked for their personal data.",
        userManager.GetUserId(User)
      );

      // Only include personal data for download
      var personalData = new Dictionary<string, string>();
      var personalDataProps = typeof(ApplicationUser)
        .GetProperties()
        .Where(prop => Attribute.IsDefined(
            prop,
            typeof(PersonalDataAttribute)
        ));
      foreach (var p in personalDataProps)
      {
        personalData.Add(p.Name, p.GetValue(user)?.ToString() ?? "null");
      }

      Response.Headers.Add("Content-Disposition", "attachment; filename=PersonalData.json");
      return new FileContentResult(
        Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(personalData)),
        "text/json"
      );
    }
  }
}
