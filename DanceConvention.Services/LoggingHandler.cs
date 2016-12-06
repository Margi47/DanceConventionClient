using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace DanceConventionClient.Services
{
	public class LoggingHandler : DelegatingHandler
	{
		private readonly ILogger _logger;
		public LoggingHandler(HttpMessageHandler innerHandler)
			: base(innerHandler)
		{
			_logger = Serilog.Log.ForContext(GetType());
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			
			if (request.Content != null)
			{
				_logger.Verbose("Sending request {HttpRequest}", request);
			}

			var response = await base.SendAsync(request, cancellationToken);
			
			if (response.Content != null)
			{
				_logger.Verbose("Received response {HttpResponse}", response);
			}

			return response;
		}
	}
}
