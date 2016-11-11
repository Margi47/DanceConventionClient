using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanceConventionClient.Services
{
	public interface IDCService
	{
		Task<Profile> GetProfile();
		Task<DanceEvent[]> GetEvents();
		Task<EventPermission[]> GetPermissions(int eventId);
		Task<Signup[]> SearchSignups(int eventId, string text);
		Task<Signup> GetSignup(int eventId, int userId);
		Task<Signup> UpdateSignupState(int eventId, int userId, string state);
		Task<Signup> UpdateAttendanceStatus(int eventId, int userId);
		Task<Signup> RecordPayment(int eventId, int participantId, decimal paymentAmount, string comment);
		Task<Contest[]> GetContests(int eventId);
		Task<Competitor[]> GetCompetitors(int eventId, int contestId);
		Task<Competitor[]> SearchCompetitor(int eventId, int contestId, string text);
		Task<EntranceInf> ContestCheckin(int eventId, int contestId, int participantId, int bibNumber, bool checkinAll);
		Task<EntranceInf[]> AllContestCheckin(int eventId, int participantId, int bibNumber, bool checkinAll);
	}
}
