namespace TrackerLibrary.Models
{
    public class PrizeModel
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public double Percentage { get; set; }

        public PrizeModel()
        {

        }

        public PrizeModel(string placeNumber, string placeName, string prizeAmount, string prizePercentage)
        {
            this.Name = placeName;

            int placeNumberConverted = 0;
            int.TryParse(placeNumber, out placeNumberConverted);
            this.Number = placeNumberConverted;

            decimal prizeAmountConverted = 0;
            decimal.TryParse(prizeAmount, out prizeAmountConverted);
            this.Amount = prizeAmountConverted;

            double prizePercentageConverted = 0;     
            double.TryParse(prizePercentage, out prizePercentageConverted);
            this.Percentage = prizePercentageConverted;
        }
    }
}

