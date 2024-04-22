namespace ThucTapLTSedu.Entities
{
	public class Bill: BaseEntity
	{
		public double TotalMoney { get; set; }
		public string TradingCode { get; set; }
		public DateTime CreateTime { get; set; }
		public int CustomerId { get; set; }
		public User Customer { get; set; }
		public string Name { get; set; }
		public DateTime UpdateTime { get; set; }
		public int? PromotionId { get; set; }
		public Promotion Promotion { get; set; }
		public int BillStatusId { get; set; }
		public BillStatus BillStatus { get; set; }
		public bool IsActive { get; set; }
		public IEnumerable<BillFood> BillFoods { get; set; }
		public IEnumerable<BillTicket> BillTickets { get; set; }

	}
}
