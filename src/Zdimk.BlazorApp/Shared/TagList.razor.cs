using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Zdimk.Abstractions.Dtos;
using Zdimk.Abstractions.Queries;

namespace Zdimk.BlazorApp.Shared
{
    public partial class TagList
    {
        private IEnumerable<TagDto> Tags { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var query = CreateQuery();
                Tags = await Mediator.Send(query);
                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private GetTagsQuery CreateQuery()
        {
            return new GetTagsQuery
            {
                Offset = 0,
                Count = 10,
            };
        }
    }
}