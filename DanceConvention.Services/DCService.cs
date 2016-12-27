using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DanceConventionClient.Services.Models;
using Serilog;
using Xamarin.Forms;

namespace DanceConventionClient.Services
{
	public class DCService:IDCService
	{
		private readonly ILogger _logger;
		private readonly HttpClientProvider _clientProvider;

		public DCService(HttpClientProvider clientProvider)
		{
			_logger = Log.ForContext(GetType());
			_clientProvider = clientProvider;

		}

		public async Task<LoginResult> Login(DCLogin login)
		{
			_logger.Information("Trying to login user {User}", login.Username);
			var response = await _clientProvider.Client.PostAsJsonAsync("/eventdirector/rest/mobile/auth", login);

			if (response.StatusCode == HttpStatusCode.OK)
			{
				_logger.Information("Login succeded for user {UserName}", login.Username);
				var result = new LoginResult()
				{
					Login = login,
				};
				return result;
			}
			else if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.BadRequest)
			{
				_logger.Information("User {UserName} was not authorized", login.Username);
				var result = new LoginResult()
				{
					ErrorMessage = "Invalid Username or Password"
				};
				return result;
			}
			else
			{
				_logger.Information("Login failed for user {UserName} - received {StatusCode} status code, {Reason}, {Response}", login.Username, response.StatusCode, response.ReasonPhrase, response.Content);
				var result = new LoginResult()
				{
					ErrorMessage = "Login failed: " + response.ReasonPhrase
				};
				return result;
			}
		}

		public async Task<Profile> GetProfile()
		{
			_logger.Information("Getting Profile information");
			HttpResponseMessage eventResponse = await _clientProvider.Client.GetAsync("/eventdirector/rest/mobile/profile");
			eventResponse.EnsureSuccessStatusCode();
			var profile = await eventResponse.Content.ReadAsAsync<Profile>();		
			return profile;
		}

		public async Task<DanceEvent[]> GetEvents()
		{
			_logger.Information("Getting available events");
			HttpResponseMessage eventResponse = await _clientProvider.Client.GetAsync("/eventdirector/rest/mobile/events");
			eventResponse.EnsureSuccessStatusCode();
			var danceEvents = await eventResponse.Content.ReadAsAsync<DanceEvent[]>();		
			return danceEvents;
		}

		public async Task<EventPermission[]> GetPermissions(int eventId)
		{
			_logger.Information("Getting user permissions for event {EventId}", eventId);
			HttpResponseMessage eventResponse = await _clientProvider.Client.GetAsync($"/eventdirector/rest/mobile/event/{eventId}/permissions");
			eventResponse.EnsureSuccessStatusCode();
			var permission = await eventResponse.Content.ReadAsAsync<EventPermission[]>();		
			return permission;
		}

		public async Task<Signup[]> SearchSignups(int eventId, string text)
		{
			_logger.Information("Searching {Text} in signups for event {EventId}", text, eventId);
			HttpResponseMessage response = await _clientProvider.Client.GetAsync($"/eventdirector/rest/mobile/event/{eventId}/search?q={text}");
			response.EnsureSuccessStatusCode();
			var signups = await response.Content.ReadAsAsync<Signup[]>();
			return signups;
		}

		public async Task<Signup> GetSignup(int eventId, int userId)
		{
			_logger.Information("Getting event {EventId} signup for {UserId}", eventId, userId);
			HttpResponseMessage response = await _clientProvider.Client.GetAsync($"/eventdirector/rest/mobile/event/{eventId}/signup/{userId}");
			response.EnsureSuccessStatusCode();
			var signup = await response.Content.ReadAsAsync<Signup>();		
			return signup;
		}

		public async Task<Signup> UpdateSignupState(int eventId, int userId, string state)
		{
			_logger.Information("Updating user {UserId} signup state {State} for event {EventId}", userId, state, eventId);
			var signupState = new SignupState();
			signupState.State = state;
			HttpResponseMessage response = await _clientProvider.Client.PostAsJsonAsync($"/eventdirector/rest/mobile/event/{eventId}/signup/{userId}/state", signupState);
			response.EnsureSuccessStatusCode();
			var signup = await response.Content.ReadAsAsync<Signup>();
			_logger.Information("State updated to {State}", signup.Status);
			return signup;
		}

		public async Task<Signup> UpdateAttendanceStatus(int eventId, int userId)
		{
			_logger.Information("Updating user {UserId} attendance status for event {EventId}", userId, eventId);
			HttpResponseMessage response = await _clientProvider.Client.PostAsync($"/eventdirector/rest/mobile/event/{eventId}/signup/{userId}/toggleattendance", null);
			response.EnsureSuccessStatusCode();
			var signup = await response.Content.ReadAsAsync<Signup>();
			_logger.Information("Attendance status updated to {Status}", signup.Attended ? "ATTENDED" : "CHECK IN");
			return signup;
		}

		public async Task<Signup> RecordPayment(int eventId, int participantId, decimal paymentAmount, string comment)
		{
			_logger.Information("Recording participant {ParticipantId} payment {PaymentAmount} for event {EventId} with comment {Comment}", participantId, paymentAmount, eventId, comment);
			var payment = new Payment() { EventId = eventId, ParticipantId = participantId, PaymentAmount = paymentAmount, Comment = comment };
			HttpResponseMessage response = await _clientProvider.Client.PostAsJsonAsync($"/eventdirector/rest/mobile/event/{eventId}/payment", payment);
			response.EnsureSuccessStatusCode();
			var signup = await response.Content.ReadAsAsync<Signup>();
			_logger.Information("Payment {PaymentAmount} with comment {Comment} recorded", paymentAmount, comment);
			return signup;
		}

		public async Task<Contest[]> GetContests(int eventId)
		{
			_logger.Information("Getting active contests for event {EventId}", eventId);
			HttpResponseMessage response = await _clientProvider.Client.GetAsync($"/eventdirector/rest/mobile/event/{eventId}/contests?checkin=true");
			response.EnsureSuccessStatusCode();
			var competitions = await response.Content.ReadAsAsync<Contest[]>();		
			return competitions;
		}

		public async Task<Competitor[]> GetCompetitors(int eventId, int contestId)
		{
			_logger.Information("Getting contest {ContestId} competitors for event {EventId}", contestId, eventId);
			HttpResponseMessage response = await _clientProvider.Client.GetAsync($"/eventdirector/rest/mobile/event/{eventId}/contest/{contestId}/competitors");
			response.EnsureSuccessStatusCode();
			var competitors = await response.Content.ReadAsAsync<Competitor[]>();
			return competitors;
		}

		public async Task<Competitor[]> SearchCompetitor(int eventId, int contestId, string text)
		{
			_logger.Information("Searching {Text} in contest {ContestId} competitors for event {EventId}", text, contestId, eventId);
			HttpResponseMessage response = await _clientProvider.Client.GetAsync($"/eventdirector/rest/mobile/event/{eventId}/contest/{contestId}/search?q={text}");
			response.EnsureSuccessStatusCode();
			var competitor = await response.Content.ReadAsAsync<Competitor[]>();
			return competitor;
		}

		public async Task<EntranceInf> ContestCheckin(int eventId, int contestId, int participantId, int bibNumber, bool checkinAll)
		{
			_logger.Information("Check-in participant {ParticipantId} with bib {Bib} for event {EventId} contest {ContestId}", participantId, bibNumber, eventId, contestId);
			var checkin = new Chekin() { ParticipantId = participantId, BibNumber = bibNumber, CheckinAll = checkinAll };
			HttpResponseMessage response = await _clientProvider.Client.PostAsJsonAsync($"/eventdirector/rest/mobile/event/{eventId}/contest/{contestId}/checkin", checkin);
			response.EnsureSuccessStatusCode();
			var entrance = await response.Content.ReadAsAsync<EntranceInf>();
			_logger.Information("Participant {ParticipantName} check-ined for contest", participantId, contestId);
			return entrance;
		}

		public async Task<EntranceInf[]> AllContestCheckin(int eventId, int participantId, int bibNumber, bool checkinAll)
		{
			_logger.Information("Check-in participant {ParticipantId} with bib {Bib} for all event {EventId} contests", participantId, bibNumber, eventId);
			var checkin = new Chekin() { ParticipantId = participantId, BibNumber = bibNumber, CheckinAll = checkinAll };
			HttpResponseMessage response = await _clientProvider.Client.PostAsJsonAsync($"/eventdirector/rest/mobile/event/{eventId}/activecontests/checkin", checkin);
			response.EnsureSuccessStatusCode();
			var entrances = await response.Content.ReadAsAsync<EntranceInf[]>();
			_logger.Information("Participant {ParticipantId} check-ined for all contests", participantId);
			return entrances;
		}
	}
}
