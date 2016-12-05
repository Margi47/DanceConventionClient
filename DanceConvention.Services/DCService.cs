using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DanceConventionClient.Services
{
	public class DCService:IDCService
	{
		private readonly HttpClient _client;

		public DCService(HttpClient client)
		{
			_client = client;
		}

		public async Task<Profile> GetProfile()
		{
			HttpResponseMessage eventResponse = await _client.GetAsync("/eventdirector/rest/mobile/profile");
			eventResponse.EnsureSuccessStatusCode();
			var profile = await eventResponse.Content.ReadAsAsync<Profile>();
			return profile;
		}

		public async Task<DanceEvent[]> GetEvents()
		{
			HttpResponseMessage eventResponse = await _client.GetAsync("/eventdirector/rest/mobile/events");
			eventResponse.EnsureSuccessStatusCode();
			var danceEvents = await eventResponse.Content.ReadAsAsync<DanceEvent[]>();
			//returns wrong dates 
			return danceEvents;
		}

		public async Task<EventPermission[]> GetPermissions(int eventId)
		{
			HttpResponseMessage eventResponse = await _client.GetAsync($"/eventdirector/rest/mobile/event/{eventId}/permissions");
			eventResponse.EnsureSuccessStatusCode();
			var permission = await eventResponse.Content.ReadAsAsync<EventPermission[]>();
			return permission;
		}

		public async Task<Signup[]> SearchSignups(int eventId, string text)
		{
			HttpResponseMessage response = await _client.GetAsync($"/eventdirector/rest/mobile/event/{eventId}/search?q={text}");
			response.EnsureSuccessStatusCode();
			var signups = await response.Content.ReadAsAsync<Signup[]>();
			return signups;
		}

		public async Task<Signup> GetSignup(int eventId, int userId)
		{
			HttpResponseMessage response = await _client.GetAsync($"/eventdirector/rest/mobile/event/{eventId}/signup/{userId}");
			response.EnsureSuccessStatusCode();
			var signup = await response.Content.ReadAsAsync<Signup>();
			return signup;
		}

		public async Task<Signup> UpdateSignupState(int eventId, int userId, string state)
		{
			var signupState = new SignupState();
			signupState.State = state;
			HttpResponseMessage response = await _client.PostAsJsonAsync($"/eventdirector/rest/mobile/event/{eventId}/signup/{userId}/state", signupState);
			response.EnsureSuccessStatusCode();
			var signup = await response.Content.ReadAsAsync<Signup>();

			return signup;
		}

		public async Task<Signup> UpdateAttendanceStatus(int eventId, int userId)
		{
			HttpResponseMessage response = await _client.PostAsync($"/eventdirector/rest/mobile/event/{eventId}/signup/{userId}/toggleattendance", null);
			response.EnsureSuccessStatusCode();
			var signup = await response.Content.ReadAsAsync<Signup>();

			return signup;
		}

		public async Task<Signup> RecordPayment(int eventId, int participantId, decimal paymentAmount, string comment)
		{
			var payment = new Payment() { EventId = eventId, ParticipantId = participantId, PaymentAmount = paymentAmount, Comment = comment };
			HttpResponseMessage response = await _client.PostAsJsonAsync($"/eventdirector/rest/mobile/event/{eventId}/payment", payment);
			response.EnsureSuccessStatusCode();
			var signup = await response.Content.ReadAsAsync<Signup>();

			return signup;
		}

		public async Task<Contest[]> GetContests(int eventId)
		{
			HttpResponseMessage response = await _client.GetAsync($"/eventdirector/rest/mobile/event/{eventId}/contests?checkin=true");
			response.EnsureSuccessStatusCode();
			var competitions = await response.Content.ReadAsAsync<Contest[]>();
			return competitions;
		}

		public async Task<Competitor[]> GetCompetitors(int eventId, int contestId)
		{
			HttpResponseMessage response = await _client.GetAsync($"/eventdirector/rest/mobile/event/{eventId}/contest/{contestId}/competitors");
			response.EnsureSuccessStatusCode();
			var competitors = await response.Content.ReadAsAsync<Competitor[]>();
			return competitors;
		}

		public async Task<Competitor[]> SearchCompetitor(int eventId, int contestId, string text)
		{
			HttpResponseMessage response = await _client.GetAsync($"/eventdirector/rest/mobile/event/{eventId}/contest/{contestId}/search?q={text}");
			response.EnsureSuccessStatusCode();
			var competitor = await response.Content.ReadAsAsync<Competitor[]>();
			return competitor;
		}

		public async Task<EntranceInf> ContestCheckin(int eventId, int contestId, int participantId, int bibNumber, bool checkinAll)
		{
			var checkin = new Chekin() { ParticipantId = participantId, BibNumber = bibNumber, CheckinAll = checkinAll };
			HttpResponseMessage response = await _client.PostAsJsonAsync($"/eventdirector/rest/mobile/event/{eventId}/contest/{contestId}/checkin", checkin);
			response.EnsureSuccessStatusCode();
			var entrance = await response.Content.ReadAsAsync<EntranceInf>();

			return entrance;
		}

		public async Task<EntranceInf[]> AllContestCheckin(int eventId, int participantId, int bibNumber, bool checkinAll)
		{
			var checkin = new Chekin() { ParticipantId = participantId, BibNumber = bibNumber, CheckinAll = checkinAll };
			HttpResponseMessage response = await _client.PostAsJsonAsync($"/eventdirector/rest/mobile/event/{eventId}/activecontests/checkin", checkin);
			response.EnsureSuccessStatusCode();
			var entrances = await response.Content.ReadAsAsync<EntranceInf[]>();

			return entrances;
		}
	}
}
