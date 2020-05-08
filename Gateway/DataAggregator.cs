using Newtonsoft.Json;
using Ocelot.Middleware;
using Ocelot.Middleware.Multiplexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gateway
{
    public class DataAggregator : IDefinedAggregator
    {
        public async Task<DownstreamResponse> Aggregate(List<DownstreamContext> responses)
        {
            IList<string> allResponses = new List<string>();
            string message = string.Empty;
            foreach (DownstreamContext context in responses)
            {
                string v = await context.DownstreamResponse.Content.ReadAsStringAsync();
                object obj =  JsonConvert.DeserializeObject(v);         
                message += await context.DownstreamResponse.Content.ReadAsStringAsync();
            }            

            var headers = responses.SelectMany(x => x.DownstreamResponse.Headers).ToList();
            DownstreamResponse response = new DownstreamResponse(new StringContent(message), System.Net.HttpStatusCode.OK, headers, "Data Integrated");
            return response;
        }
    }
}
