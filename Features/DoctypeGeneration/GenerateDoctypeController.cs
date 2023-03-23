using Microsoft.AspNetCore.Mvc;
using Our.Umbraco.Synthscribe.Features.DoctypeGeneration.Models;
using Our.Umbraco.Synthscribe.Features.DoctypeGeneration.Service;
using Our.Umbraco.Synthscribe.General.Models.Interrfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Controllers;

namespace Our.Umbraco.Synthscribe.Features.DoctypeGeneration
{
    [PluginController("Synthscribe")]
    public class GenerateDoctypeController: UmbracoAuthorizedApiController
    {
        private readonly IGenerateDoctypeService _generateDoctypeService;
        public GenerateDoctypeController(IGenerateDoctypeService generateDoctypeService)
        {
            _generateDoctypeService = generateDoctypeService;

        }

        [HttpPost]
        public async Task<IActionResult> GenerateDoctype([FromBody] GenerateDoctypeViewModel vm)
        {
            var response = await _generateDoctypeService.GenerateDoctype(vm.Context);
            if(response.Succes)
                return Ok(response);

            return BadRequest(response);
        }
    }
}
