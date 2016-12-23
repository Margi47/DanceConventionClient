using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;

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
			if (_logger.IsEnabled(LogEventLevel.Verbose))
			{
				if (request.Content != null)
				{
					var requestContent = await request.Content.ReadAsStringAsync();
					_logger.Verbose("Sending request {HttpRequest}, {RequestContent}", request.ToString(), requestContent);
				}
				else
				{
					_logger.Verbose("Sending request {HttpRequest}", request.ToString());
				}

			}

			var response = await base.SendAsync(request, cancellationToken);

			if (_logger.IsEnabled(LogEventLevel.Verbose))
			{
				
				if (response.Content != null)
				{
					var responseContent = await response.Content.ReadAsStringAsync();
					_logger.Verbose("Received response {HttpResponse}, {ResponseContent}", response.ToString(), responseContent);

				}
				else
				{
					_logger.Verbose("Received response {HttpResponse}", response.ToString());
				}
			}

			return response;
		}
	}
}
