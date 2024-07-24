namespace SchoolSystemTask.BAL.Specefication
{
    public class SpecParameter
    {
        public int? ClassId { get; set; }


        private string? searchByName;

        public string? SearchByName
        {
            get { return searchByName; }
            set { searchByName = value?.ToLower().Trim(); }
        }


    }
}
