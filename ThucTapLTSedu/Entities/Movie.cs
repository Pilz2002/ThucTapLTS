﻿using static System.Net.Mime.MediaTypeNames;
using System.IO;

namespace ThucTapLTSedu.Entities
{
	public class Movie:BaseEntity
	{
		public int MovieDuration { get; set; }
		public DateTime EndTime { get; set; }
		public DateTime PremiereDate { get; set; }
		public string Description { get; set; }
		public string Director { get; set; }
		public string Image { get; set; }
		public string HeroImage { get; set; }
		public string Language { get; set; }
		public int MovieTypeId { get; set; }
		public MovieType MovieType { get; set; }
		public string Name { get; set; }
		public int?  RateId { get; set; }
		public Rate? Rate { get; set; }
		public string? Trailer { get; set; }
		public bool IsActive { get; set; }
		public IEnumerable<Schedule> Schedules { get; set; }
	}
}
