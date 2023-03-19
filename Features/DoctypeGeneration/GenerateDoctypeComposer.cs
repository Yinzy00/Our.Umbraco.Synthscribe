using Microsoft.Extensions.DependencyInjection;
using Our.Umbraco.Synthscribe.Features.DoctypeGeneration.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Our.Umbraco.Synthscribe.Features.DoctypeGeneration
{
    internal class GenerateDoctypeComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddTransient<IGenerateDoctypeService, GenerateDoctypeService>();
        }
    }
}
