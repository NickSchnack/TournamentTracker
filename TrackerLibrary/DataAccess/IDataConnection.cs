using System.Collections.Generic;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public interface IDataConnection
    {
        void CreatePrize(PrizeModel prize);
        void CreatePerson(PersonModel person);
        void CreateTeam(TeamModel team);
        void CreateTournament(TournamentModel tournament);
        List<PersonModel> GetPerson_All();
        List<TeamModel> GetTeam_All();
    }
}
