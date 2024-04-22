﻿using ThucTapLTSedu.Entities;

namespace ThucTapLTSedu.Payloads.DataResponses.AdminResponses
{
	public class DataResponse_Movie
	{
		public int MovieDuration { get; set; }
		public DateTime EndTime { get; set; }
		public DateTime PremiereDate { get; set; }
		public string Description { get; set; }
		public string Director { get; set; }
		public string Image { get; set; }
		public string HeroImage { get; set; }
		public string Language { get; set; }
		public string MovieTypeName { get; set; }
		public string Name { get; set; }
		public string RateDescription { get; set; }
		public string Trailer { get; set; }
		public bool IsActive { get; set; }
	}
}
