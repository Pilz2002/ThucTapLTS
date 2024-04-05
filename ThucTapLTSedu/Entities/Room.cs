﻿namespace ThucTapLTSedu.Entities
{
	public class Room:BaseEntity
	{
		public int Capacity { get; set; }
		public string Type { get; set; }
		public string Description { get; set; }
		public int CinemaId { get; set; }
		public Cinema Cinema { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public bool IsActive { get; set; }
		public IEnumerable<Schedule> Schedules { get; set; }
		public IEnumerable<Seat> Seats { get; set; }
	}
}
