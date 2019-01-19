using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary.Models;
using TrackerLibrary.Configuration;
using TrackerLibrary.Logic;

namespace TrackerUI
{
    public partial class CreateTournamentForm : Form, IPrizeRequestor, ITeamRequestor
    {
        List<TeamModel> availableTeams = GlobalConfig.Connection.GetTeam_All();
        List<TeamModel> selectedTeams = new List<TeamModel>();
        List<PrizeModel> selectedPrizes = new List<PrizeModel>();

        public CreateTournamentForm()
        {
            InitializeComponent();
            WireUpLists();
        }

        private void WireUpLists()
        {
            selectTeamDropDown.DataSource = null;

            selectTeamDropDown.DataSource = availableTeams;
            selectTeamDropDown.DisplayMember = "Name";

            tournamentTeamsListBox.DataSource = null;

            tournamentTeamsListBox.DataSource = selectedTeams;
            tournamentTeamsListBox.DisplayMember = "Name";

            prizesListBox.DataSource = null;

            prizesListBox.DataSource = selectedPrizes;
            prizesListBox.DisplayMember = "Name";
        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel team = (TeamModel)selectTeamDropDown.SelectedItem;
            if (team != null)
            {
                availableTeams.Remove(team);
                selectedTeams.Add(team);

                WireUpLists();
            }
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            CreatePrizeForm prizeForm = new CreatePrizeForm(this);
            prizeForm.Show();
        }

        private void createNewTeamButton_Click(object sender, EventArgs e)
        {
            CreateTeamForm teamForm = new CreateTeamForm(this);
            teamForm.Show();
        }

        private void removeSelectedTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel team = (TeamModel)tournamentTeamsListBox.SelectedItem;
            if (team != null)
            {
                selectedTeams.Remove(team);
                availableTeams.Add(team);

                WireUpLists();
            }
        }

        private void removeSelectedPrizeButton_Click(object sender, EventArgs e)
        {
            PrizeModel prize = (PrizeModel)prizesListBox.SelectedItem;
            if (prize != null)
            {
                selectedPrizes.Remove(prize);
                WireUpLists();
            }
        }

        public void PrizeComplete(PrizeModel prize)
        {
            selectedPrizes.Add(prize);
            WireUpLists();
        }

        public void TeamComplete(TeamModel team)
        {
            selectedTeams.Add(team);
            WireUpLists();
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            TournamentModel tournament = new TournamentModel();

            decimal entryFeeInput = 0;
            bool feeIsDecimal = decimal.TryParse(entryFeeValue.Text, out entryFeeInput);

            if (tournamentNameValue.Text != "" && entryFeeValue.Text != "" && feeIsDecimal)
            {
                tournament.Name = tournamentNameValue.Text;
                tournament.EntryFee = entryFeeInput;
                tournament.EnteredTeams = selectedTeams;
                tournament.Prizes = selectedPrizes;

                TournamentLogic.CreateRounds(tournament);

                GlobalConfig.Connection.CreateTournament(tournament);

                //TO-DO: Implement calling form at a later date if needed
                //callingForm.TeamComplete(team);

                //this.Close();

                tournamentNameValue.Text = "";
                entryFeeValue.Text = "";
            }
            else
            {
                MessageBox.Show("This form has invalid information. Please check it and try again.");
            }
        }
    }
}

