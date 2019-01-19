using System.Collections.Generic;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess.TextHelpers;
using System.Linq;

namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {
        private const string PrizeModelsFile = "PrizeModels.csv";
        private const string PersonModelsFile = "PersonModels.csv";
        private const string TeamModelsFile = "TeamModels.csv";
        private const string TournamentModelsFile = "TournamentModels.csv";

        public void CreatePrize(PrizeModel prize)
        {
            List<PrizeModel> prizes = PrizeModelsFile.FullFilePath().LoadFile().ConvertToPrizeModels();
            int currentId = 1;
            if (prizes.Count() > 0)
            {
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }
            prize.Id = currentId;
            prizes.Add(prize);
            prizes.SaveToPrizeModelsFile(PrizeModelsFile);
        }

        public void CreatePerson(PersonModel person)
        {
            List<PersonModel> persons = PersonModelsFile.FullFilePath().LoadFile().ConvertToPersonModels();
            int currentId = 1;
            if (persons.Count() > 0)
            {
                currentId = persons.OrderByDescending(x => x.Id).First().Id + 1;
            }
            person.Id = currentId;
            persons.Add(person);
            persons.SaveToPersonModelsFile(PersonModelsFile);
        }

        public void CreateTeam(TeamModel team)
        {
            List<TeamModel> teams = TeamModelsFile.FullFilePath().LoadFile().ConvertToTeamModels(PersonModelsFile);
            int currentId = 1;
            if (teams.Count() > 0)
            {
                currentId = teams.OrderByDescending(x => x.Id).First().Id + 1;
            }
            team.Id = currentId;
            teams.Add(team);
            teams.SaveToTeamModelsFile(TeamModelsFile);
        }

        public void CreateTournament(TournamentModel tournament)
        {
            List<TournamentModel> tournaments = TournamentModelsFile
                .FullFilePath()
                .LoadFile()
                .ConvertToTournamentModels(PersonModelsFile, TeamModelsFile, PrizeModelsFile);
            int currentId = 1;
            if (tournaments.Count() > 0)
            {
                currentId = tournaments.OrderByDescending(x => x.Id).First().Id + 1;
            }
            tournament.Id = currentId;
            tournaments.Add(tournament);
            tournaments.SaveToTournamentModelsFile(TournamentModelsFile);
        }

        public List<PersonModel> GetPerson_All()
        {
            return PersonModelsFile.FullFilePath().LoadFile().ConvertToPersonModels();
        }

        public List<TeamModel> GetTeam_All()
        {
            List<TeamModel> teams = TeamModelsFile.FullFilePath().LoadFile().ConvertToTeamModels(PersonModelsFile);
            return teams;
        }
    }
}
