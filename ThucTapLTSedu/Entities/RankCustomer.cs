﻿namespace ThucTapLTSedu.Entities
{
	public class RankCustomer:BaseEntity
	{
		public int Point { get; set; }
		public string Description { get; set; }
		public string Name { get; set; }
		public bool IsActive { get; set; }
		public IEnumerable<User> Users { get; set; }
		public IEnumerable<Promotion> Promotions { get; set; }
	}
}
