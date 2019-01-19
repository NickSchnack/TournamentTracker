using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess.TextHelpers
{
    public static class TextConnectorProcessor
    {
        public static string FullFilePath(this string fileName)
        {
            return $"{ConfigurationManager.AppSettings["FilePath"]}\\{fileName}";
        }

        public static List<string> LoadFile(this string file)
        {
            if (!File.Exists(file))
            {
                return new List<string>();
            }

            return File.ReadAllLines(file).ToList();
        }

        public static List<PrizeModel> ConvertToPrizeModels(this List<string> lines)
        {
            List<PrizeModel> output = new List<PrizeModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                PrizeModel prize = new PrizeModel();
                prize.Id = int.Parse(cols[0]);
                prize.Number = int.Parse(cols[1]);
                prize.Name = cols[2];
                prize.Amount = decimal.Parse(cols[3]);
                prize.Percentage = double.Parse(cols[4]);
                output.Add(prize);
            }

            return output;
        }

        public static List<PersonModel> ConvertToPersonModels(this List<string> lines)
        {
            List<PersonModel> output = new List<PersonModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                PersonModel person = new PersonModel();
                person.Id = int.Parse(cols[0]);
                person.Firstname = cols[1];
                person.Lastname = cols[2];
                person.EmailAddress = cols[3];
                person.PhoneNumber = cols[4];
                output.Add(person);
            }

            return output;
        }

        public static List<TeamModel> ConvertToTeamModels(this List<string> lines, string personModelsFile)
        {
            List<TeamModel> output = new List<TeamModel>();
            List<PersonModel> persons = personModelsFile.FullFilePath().LoadFile().ConvertToPersonModels();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                TeamModel team = new TeamModel();
                team.Id = int.Parse(cols[0]);
                team.Name = cols[1];

                string[] personIds = cols[2].Split('|');
                foreach (string personId in personIds)
                {
                    team.Members.Add(persons.Where(x => x.Id == int.Parse(personId)).First());
                }

                output.Add(team);
            }

            return output;
        }

        public static List<TournamentModel> ConvertToTournamentModels(this List<string> lines, string personModelsFile,
            string teamModelsFile, string prizeModelsFile)
        {
            List<TournamentModel> output = new List<TournamentModel>();
            List<TeamModel> teams = teamModelsFile.FullFilePath().LoadFile().ConvertToTeamModels(personModelsFile);
            List<PrizeModel> prizes = prizeModelsFile.FullFilePath().LoadFile().ConvertToPrizeModels();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                TournamentModel tournament = new TournamentModel();
                tournament.Id = int.Parse(cols[0]);
                tournament.Name = cols[1];
                tournament.EntryFee = decimal.Parse(cols[2]);

                string[] teamIds = cols[3].Split('|');
                foreach (string teamId in teamIds)
                {
                    tournament.EnteredTeams.Add(teams.Where(x => x.Id == int.Parse(teamId)).First());
                }

                string[] prizeIds = cols[4].Split('|');
                foreach (string prizeId in prizeIds)
                {
                    tournament.Prizes.Add(prizes.Where(x => x.Id == int.Parse(prizeId)).First());
                }

                // TODO - Read in Rounds data

                output.Add(tournament);
            }

            return output;
        }

        public static void SaveToPrizeModelsFile(this List<PrizeModel> prizes, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (PrizeModel prize in prizes)
            {
                lines.Add($"{prize.Id},{prize.Number},{prize.Name},{prize.Amount},{prize.Percentage}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToPersonModelsFile(this List<PersonModel> persons, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (PersonModel person in persons)
            {
                lines.Add($"{person.Id},{person.Firstname},{person.Lastname},{person.EmailAddress},{person.PhoneNumber}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToTeamModelsFile(this List<TeamModel> teams, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (TeamModel team in teams)
            {
                string memberIds = ConvertMemberListToStringOfIds(team.Members);
                lines.Add($"{team.Id},{team.Name},{memberIds}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static void SaveToTournamentModelsFile(this List<TournamentModel> tournaments, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (TournamentModel tournament in tournaments)
            {
                string teamIds = ConvertTeamListToStringOfIds(tournament.EnteredTeams);
                string prizeIds = ConvertPrizeListToStringOfIds(tournament.Prizes);
                string roundIds = ConvertRoundListToStringOfIds(tournament.Rounds);
                lines.Add($"{tournament.Id},{tournament.Name},{tournament.EntryFee},{teamIds},{prizeIds},{roundIds}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        private static string ConvertMemberListToStringOfIds(List<PersonModel> persons)
        {
            string output = "";

            if (persons.Count == 0)
            {
                return "";
            }

            foreach (PersonModel person in persons)
            {
                output += $"{person.Id}|";
            }
            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertTeamListToStringOfIds(List<TeamModel> teams)
        {
            string output = "";

            if (teams.Count == 0)
            {
                return "";
            }

            foreach (TeamModel team in teams)
            {
                output += $"{team.Id}|";
            }
            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertPrizeListToStringOfIds(List<PrizeModel> prizes)
        {
            string output = "";

            if (prizes.Count == 0)
            {
                return "";
            }

            foreach (PrizeModel prize in prizes)
            {
                output += $"{prize.Id}|";
            }
            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertRoundListToStringOfIds(List<List<MatchupModel>> rounds)
        {
            string output = "";

            if (rounds.Count == 0)
            {
                return "";
            }

            foreach (List<MatchupModel> round in rounds)
            {
                output += $"{ConvertMatchupListToStringOfIds(round)}^";
            }
            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertMatchupListToStringOfIds(List<MatchupModel> matchups)
        {
            string output = "";

            if (matchups.Count == 0)
            {
                return "";
            }

            foreach (MatchupModel matchup in matchups)
            {
                output += $"{matchup.Id}^";
            }
            output = output.Substring(0, output.Length - 1);

            return output;
        }
    }
}
