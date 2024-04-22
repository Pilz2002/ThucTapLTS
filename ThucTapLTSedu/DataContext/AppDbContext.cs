using Microsoft.EntityFrameworkCore;
using ThucTapLTSedu.Entities;

namespace ThucTapLTSedu.DataContext
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions options) : base(options)
		{

		}
		
		public AppDbContext()
		{
		}

		public DbSet<Banner> Banners { get; set; }
		public DbSet<Bill> Bills { get; set; }
		public DbSet<BillFood> BillFoods { get; set; }
		public DbSet<BillStatus> BillStatuses { get; set; }
		public DbSet<BillTicket> BillTickets { get; set; }
		public DbSet<Cinema> Cinemas { get; set; }
		public DbSet<ConfirmEmail> ConfirmEmails { get; set; }
		public DbSet<Food> Foods { get; set; }
		public DbSet<GeneralSetting> GeneralSettings { get; set; }
		public DbSet<Movie> Movies { get; set; }
		public DbSet<MovieType> MovieTypes { get; set; }
		public DbSet<Promotion> Promotions { get; set; }
		public DbSet<RankCustomer> RankCustomers { get; set; }
		public DbSet<Rate> Rates { get; set; }
		public DbSet<RefreshToken> RefreshTokens { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<Room> Rooms { get; set; }
		public DbSet<Schedule> Schedules { get; set; }
		public DbSet<Seat> Seats { get; set; }
		public DbSet<SeatType> SeatTypes { get; set; }
		public DbSet<SeatStatus> SeatStatus { get; set; }
		public DbSet<Ticket> Tickets { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<UserStatus> UserStatuses { get; set; }
	}
}
