using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using TrackerLibrary.Models;
using Dapper;
using TrackerLibrary.Configuration;

namespace TrackerLibrary.DataAccess
{
    public class MySqlConnector : IDataConnection
    {
        private const string db = "ConnectionString_Tournaments_DB";

        public void CreatePrize(PrizeModel prize)
        {
            using (IDbConnection connection = new MySqlConnection(GlobalConfig.GetConnectionString(db)))
            {
                var parameters = new DynamicParameters();
                parameters.Add("argPlaceNumber", prize.Number);
                parameters.Add("argPlaceName", prize.Name);
                parameters.Add("argPlaceAmount", prize.Amount);
                parameters.Add("argPrizePercentage", prize.Percentage);
                parameters.Add("id", 0, DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("tournaments.spPrizes_Insert", parameters, commandType: CommandType.StoredProcedure);

                prize.Id = parameters.Get<int>("id");
            }
        }

        public void CreatePerson(PersonModel person)
        {
            using (IDbConnection connection = new MySqlConnection(GlobalConfig.GetConnectionString(db)))
            {
                var parameters = new DynamicParameters();
                parameters.Add("argFirstName", person.Firstname);
                parameters.Add("argLastName", person.Lastname);
                parameters.Add("argEmailAddress", person.EmailAddress);
                parameters.Add("argPhoneNumber", person.PhoneNumber);
                parameters.Add("id", 0, DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("tournaments.spPeople_Insert", parameters, commandType: CommandType.StoredProcedure);

                person.Id = parameters.Get<int>("id");
            }
        }

        public void CreateTeam(TeamModel team)
        {
            using (IDbConnection connection = new MySqlConnection(GlobalConfig.GetConnectionString(db)))
            {
                var parameters = new DynamicParameters();
                parameters.Add("argTeamName", team.Name);
                parameters.Add("id", 0, DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("tournaments.spTeams_Insert", parameters, commandType: CommandType.StoredProcedure);

                team.Id = parameters.Get<int>("id");

                foreach (PersonModel person in team.Members)
                {
                    parameters = new DynamicParameters();
                    parameters.Add("argTeamId", team.Id);
                    parameters.Add("argPersonId", person.Id);
                    parameters.Add("id", 0, DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("tournaments.spTeamMembers_Insert", parameters, commandType: CommandType.StoredProcedure);
                }
            }
        }

        public void CreateTournament(TournamentModel tournament)
        {
            using (IDbConnection connection = new MySqlConnection(GlobalConfig.GetConnectionString(db)))
            {
                SaveTournament(connection, tournament);
                SaveTournamentEntries(connection, tournament);
                SaveTournamentPrizes(connection, tournament);
            }
        }

        public List<PersonModel> GetPerson_All()
        {
            using (IDbConnection connection = new MySqlConnection(GlobalConfig.GetConnectionString(db)))
            {
                List<PersonModel> output;
                output = connection.Query<PersonModel>("tournaments.spPeople_GetAll").ToList();
                return output;
            }
        }

        public List<TeamModel> GetTeam_All()
        {
            using (IDbConnection connection = new MySqlConnection(GlobalConfig.GetConnectionString(db)))
            {
                List<TeamModel> teams = connection.Query<TeamModel>("tournaments.spTeam_GetAll").ToList();

                foreach (TeamModel team in teams)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("requested_team_id", team.Id);
                    team.Members = connection.Query<PersonModel>("tournaments.spTeamMembers_GetByTeam", parameters, commandType: CommandType.StoredProcedure).ToList();
                }

                return teams;
            }
        }

        private void SaveTournament(IDbConnection connection, TournamentModel tournament)
        {
            var parameters = new DynamicParameters();
            parameters.Add("argTournamentName", tournament.Name);
            parameters.Add("argEntryFee", tournament.EntryFee);
            parameters.Add("id", 0, DbType.Int32, direction: ParameterDirection.Output);

            connection.Execute("tournaments.spTournaments_Insert", parameters, commandType: CommandType.StoredProcedure);

            tournament.Id = parameters.Get<int>("id");
        }

        private void SaveTournamentEntries(IDbConnection connection, TournamentModel tournament)
        {
            foreach (TeamModel team in tournament.EnteredTeams)
            {
                var parameters = new DynamicParameters();
                parameters.Add("argTournamentId", tournament.Id);
                parameters.Add("argTeamId", team.Id);
                parameters.Add("id", 0, DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("tournaments.spTournament_Entries_Insert", parameters, commandType: CommandType.StoredProcedure);
            }
        }

        private void SaveTournamentPrizes(IDbConnection connection, TournamentModel tournament)
        {
            foreach (PrizeModel prize in tournament.Prizes)
            {
                var parameters = new DynamicParameters();
                parameters.Add("argTournamentId", tournament.Id);
                parameters.Add("argPrizeId", prize.Id);
                parameters.Add("id", 0, DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("tournaments.spTournament_Prizes_Insert", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}

