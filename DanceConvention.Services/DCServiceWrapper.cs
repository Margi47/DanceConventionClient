using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DanceConventionClient.Services.Models;
using Serilog;
using Xamarin.Forms;

namespace DanceConventionClient.Services
{
	public class DCServiceWrapper:IDCService
	{
		private readonly IDCService _service;
		private readonly ILogger _logger;
		private HttpClientProvider _clientProvider;
		private const int RETRY_COUNT = 3;

		public DCServiceWrapper()
		{
			_clientProvider = new HttpClientProvider();
			_service = new DCService(_clientProvider);
			_logger = Log.ForContext(GetType());
		}

		public async Task<T> RetryIfFails<T>(Func<IDCService, Task<T>> func)where T: class
		{
			bool retry = false;
			do
			{
				for (var i = 0; i < RETRY_COUNT; i++)
				{
					try
					{
						return await func(_service);
					}
					catch (HttpRequestException e)
					{
						_logger.Error(e, "Operation failed");
						break;
					}
					catch (Exception e)
					{
						_logger.Warning(e, "An error occured");
					}
				}


				var answer = Application.Current.MainPage
							.DisplayAlert("Error", "Operation failed. Please retry or re-run the application", "Retry", "Delay");
				retry = await answer;
				if (retry)
				{
					_clientProvider.InitClient();
				}
			} while (retry);

			return await Task.FromResult((T) null);
		}

		public Task<LoginResult> Login(DCLogin login)
		{
			return RetryIfFails((service) => service.Login(login));
		}

		public Task<Profile> GetProfile()
		{
			return RetryIfFails((service) => service.GetProfile());
		}

		public Task<DanceEvent[]> GetEvents()
		{
			return RetryIfFails((service) => service.GetEvents());
		}

		public Task<EventPermission[]> GetPermissions(int eventId)
		{
			return RetryIfFails((service) => service.GetPermissions(eventId));
		}

		public Task<Signup[]> SearchSignups(int eventId, string text)
		{
			return RetryIfFails((service) => service.SearchSignups(eventId, text));
		}

		public Task<Signup> GetSignup(int eventId, int userId)
		{
			return RetryIfFails((service) => service.GetSignup(eventId, userId));
		}

		public Task<Signup> UpdateSignupState(int eventId, int userId, string state)
		{
			return RetryIfFails((service) => service.UpdateSignupState(eventId, userId, state));
		}

		public Task<Signup> UpdateAttendanceStatus(int eventId, int userId)
		{
			return RetryIfFails((service) => service.UpdateAttendanceStatus(eventId, userId));
		}

		public Task<Signup> RecordPayment(int eventId, int participantId, decimal paymentAmount, string comment)
		{
			return RetryIfFails((service) => service.RecordPayment(eventId, participantId, paymentAmount, comment));
		}

		public Task<Contest[]> GetContests(int eventId)
		{
			return RetryIfFails((service) => service.GetContests(eventId));
		}

		public Task<Competitor[]> GetCompetitors(int eventId, int contestId)
		{
			return RetryIfFails((service) => service.GetCompetitors(eventId, contestId));
		}

		public Task<Competitor[]> SearchCompetitor(int eventId, int contestId, string text)
		{
			return RetryIfFails((service) => service.SearchCompetitor(eventId, contestId, text));
		}

		public Task<EntranceInf> ContestCheckin(int eventId, int contestId, int participantId, int bibNumber, bool checkinAll)
		{
			return RetryIfFails((service) => service.ContestCheckin(eventId, contestId, participantId, bibNumber, checkinAll));
		}

		public Task<EntranceInf[]> AllContestCheckin(int eventId, int participantId, int bibNumber, bool checkinAll)
		{
			return RetryIfFails((service) => service.AllContestCheckin(eventId, participantId, bibNumber, checkinAll));
		}
	}
}
