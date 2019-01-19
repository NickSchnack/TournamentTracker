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

namespace TrackerUI
{
    public partial class CreateTeamForm : Form
    {
        private List<PersonModel> availableTeamMembers = GlobalConfig.Connection.GetPerson_All();
        private List<PersonModel> selectedTeamMembers = new List<PersonModel>();
        ITeamRequestor callingForm;

        public CreateTeamForm(ITeamRequestor caller)
        {
            InitializeComponent();

            callingForm = caller;

            // TODO - Remove sample data method call
            //CreateSampleData();

            WireUpLists();
        }

        // TODO - Remove sample data method
        private void CreateSampleData()
        {
            availableTeamMembers.Add(new PersonModel("Nick","Schnack","309-258-7777","nschnack@yahoo.net"));
            availableTeamMembers.Add(new PersonModel("George", "Washington", "309-656-5555", "gwash@yahoo.net"));

            selectedTeamMembers.Add(new PersonModel("Abe", "Lincoln", "309-999-7777", "alincoln@gmail.com"));
            selectedTeamMembers.Add(new PersonModel("Donald", "Trump", "309-101-5555", "thedonald@hotmail.net"));
        }

        private void WireUpLists()
        {
            selectTeamMemberDropDown.DataSource = null;

            selectTeamMemberDropDown.DataSource = availableTeamMembers;
            selectTeamMemberDropDown.DisplayMember = "Fullname";

            teamMembersListBox.DataSource = null;

            teamMembersListBox.DataSource = selectedTeamMembers;
            teamMembersListBox.DisplayMember = "Fullname";
        }

        private void createMemberButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                PersonModel model = new PersonModel(memberFirstNameValue.Text, memberLastNameValue.Text, memberEmailValue.Text, memberPhoneValue.Text);

                model = GlobalConfig.Connection.CreatePerson(model);

                selectedTeamMembers.Add(model);

                WireUpLists();

                memberFirstNameValue.Text = "";
                memberLastNameValue.Text = "";
                memberEmailValue.Text = "";
                memberPhoneValue.Text = "";
            }
            else
            {
                MessageBox.Show("This form has invalid information. Please check it and try again.");
            }
        }

        private bool ValidateForm()
        {
            // TODO - In ValidateForm() method, add regex for checking phone number input
            // TODO - In ValidateForm() method, add regex for checking email input
            bool output = true;
            
            if (memberFirstNameValue.Text.Length == 0)
            {
                output = false;
            }

            if (memberLastNameValue.Text.Length == 0)
            {
                output = false;
            }

            if (memberPhoneValue.Text.Length == 0)
            {
                output = false;
            }

            if (memberEmailValue.Text.Length == 0)
            {
                output = false;
            }
            
            return output;
        }

        private void addMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel person = (PersonModel)selectTeamMemberDropDown.SelectedItem;
            if (person != null)
            {
                availableTeamMembers.Remove(person);
                selectedTeamMembers.Add(person);

                WireUpLists();
            }
        }

        private void removeSelectedMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel person = (PersonModel)teamMembersListBox.SelectedItem;
            if (person != null)
            {
                selectedTeamMembers.Remove(person);
                availableTeamMembers.Add(person);

                WireUpLists();
            }
        }

        private void createTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel team = new TeamModel();
            if (teamNameValue.Text != "")
            {
                team.Name = teamNameValue.Text;
                team.Members = selectedTeamMembers;
                GlobalConfig.Connection.CreateTeam(team);

                callingForm.TeamComplete(team);

                this.Close();
            }
            else
            {
                MessageBox.Show("This form has invalid information. Please check it and try again.");
            }
        }
    }
}