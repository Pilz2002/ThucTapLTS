namespace ThucTapLTSedu.Entities
{
	public class GeneralSetting:BaseEntity
	{
		public DateTime BreakTime { get; set; }
		public int BusinessHours { get; set; }
		public DateTime CloseTime { get; set; }
		public double FixedTickerPrice { get; set; }
		public int PercentDay { get; set; }
		public int PercentWeekend { get; set; }
		public DateTime TimeBeginToChange { get; set; }
	}
}
