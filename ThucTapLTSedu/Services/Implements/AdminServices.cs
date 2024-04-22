using Microsoft.EntityFrameworkCore;
using System.IO;
using ThucTapLTSedu.DataContext;
using ThucTapLTSedu.Entities;
using ThucTapLTSedu.Handler.Paging;
using ThucTapLTSedu.Payloads.Converter.CinemaConverter;
using ThucTapLTSedu.Payloads.DataRequests.AdminRequests;
using ThucTapLTSedu.Payloads.DataResponses.AdminResponses;
using ThucTapLTSedu.Payloads.Responses;
using ThucTapLTSedu.Services.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ThucTapLTSedu.Services.Implements
{
	public class AdminServices : IAdminServices
	{
		private readonly AppDbContext _context;
		private readonly ResponseObject<DataResponses_Cinema> _cinemaResponse;
		private readonly ResponseObject<List<DataResponse_Room>> _roomResponse;
		private readonly ResponseObject<DataResponse_Room> _singleRoomResponse;
		private readonly ResponseObject<List<DataResponse_Seat>> _seatResponse;
		private readonly ResponseObject<DataResponse_Seat> _singleSeatResponse;
		private readonly ResponseObject<DataResponse_Food> _foodResponse;
		private readonly ResponseObject<DataResponse_Movie> _movieResponse;
		private readonly ResponseObject<DataResponse_Schedule> _scheduleResponse;

		private readonly Cinema_Converter _cinemaConverter;
		private readonly Room_Converter _roomConverter;
		private readonly Seat_Converter _seatConverter;
		private readonly Food_Converter _foodConverter;
		private readonly Movie_Converter _movieConverter;
		private readonly Schedule_Converter _scheduleConverter;

		public AdminServices(AppDbContext context, ResponseObject<DataResponses_Cinema> cinemaResponse,
			Cinema_Converter cinemaConverter, ResponseObject<List<DataResponse_Room>> roomResponse,
			Room_Converter roomConverter, Seat_Converter seatConverter,
			ResponseObject<List<DataResponse_Seat>> seatResponse, ResponseObject<DataResponse_Food> foodResponse,
			Food_Converter foodConverter, ResponseObject<DataResponse_Room> singleRoomResponse,
			ResponseObject<DataResponse_Seat> singleSeatResponse, ResponseObject<DataResponse_Movie> movieResponse,
			Movie_Converter movieConverter, ResponseObject<DataResponse_Schedule> scheduleResponse, Schedule_Converter scheduleConverter)
		{
			_context = context;
			_cinemaResponse = cinemaResponse;
			_roomResponse = roomResponse;
			_seatResponse = seatResponse;
			_foodResponse = foodResponse;
			_singleRoomResponse = singleRoomResponse;
			_singleSeatResponse = singleSeatResponse;
			_movieResponse = movieResponse;
			_scheduleResponse = scheduleResponse;

			_seatConverter = seatConverter;
			_roomConverter = roomConverter;
			_cinemaConverter = cinemaConverter;
			_movieConverter = movieConverter;
			_foodConverter = foodConverter;
			_scheduleConverter = scheduleConverter;
		}

		public ResponseObject<DataResponses_Cinema> AddCinema(DataRequest_AddCinema cinemaRequest)
		{
			var codeCinema = _context.Cinemas.FirstOrDefault(x => x.Code == cinemaRequest.Code);
			if (codeCinema is not null)
			{
				return _cinemaResponse.ErrorResponse(StatusCodes.Status400BadRequest, "Code của rạp đã tồn tại", null);
			}
			var nameCinema = _context.Cinemas.FirstOrDefault(x => x.Code == cinemaRequest.Code);
			if (nameCinema is not null)
			{
				return _cinemaResponse.ErrorResponse(StatusCodes.Status400BadRequest, "Tên của rạp đã tồn tại", null);
			}
			var addressCinema = _context.Cinemas.FirstOrDefault(x => x.Address == cinemaRequest.Address);
			if (codeCinema is not null)
			{
				return _cinemaResponse.ErrorResponse(StatusCodes.Status400BadRequest, "Địa chỉ không khả dụng", null);
			}
			Cinema cinema = new Cinema
			{
				Address = cinemaRequest.Address,
				Code = cinemaRequest.Code,
				Description = cinemaRequest.Description,
				IsActive = true,
				NameOfCinema = cinemaRequest.NameOfCinema,
			};
			_context.Cinemas.Add(cinema);
			_context.SaveChanges();

			// Thêm phòng cho từng rạp
			if (cinemaRequest.Rooms != null && cinemaRequest.Rooms.Any())
			{
				var ret = AddRoomDefault(cinema.Id, cinemaRequest.Rooms);
				// trong quá trình thêm phòng mà xảy ra lỗi thì xóa rạp đang thêm và trả về lỗi
				if (ret.Data == null)
				{
					_context.Cinemas.Remove(cinema);
					_context.SaveChanges();
					return _cinemaResponse.ErrorResponse(ret.StatusCode, ret.Message, null);
				}
			}
			return _cinemaResponse.SuccessResponse("Thêm rạp mới thành công", _cinemaConverter.CinemaDTO(cinema));
		}


		private ResponseObject<List<DataResponse_Room>> AddRoomDefault(int cinemaId, List<DataRequest_AddRoom> listRoomRequests)
		{
			List<Room> listRooms = new List<Room>();
			foreach (var roomRequest in listRoomRequests)
			{
				if (_context.Rooms.Any(x => x.CinemaId == cinemaId && x.Code == roomRequest.Code))
				{
					return _roomResponse.ErrorResponse(StatusCodes.Status400BadRequest, "Code phòng tại rạp đang trùng lặp", null);
				}
				Room room = new Room
				{
					CinemaId = cinemaId,
					Code = roomRequest.Code,
					Capacity = roomRequest.Capacity,
					Description = roomRequest.Description,
					Name = roomRequest.Name,
					Type = roomRequest.Type,
					IsActive = true
				};
				_context.Rooms.Add(room);
				_context.SaveChanges();
				listRooms.Add(room);
			}
			//Thêm ghế cho từng phòng
			for (int i = 0; i < listRoomRequests.Count; i++)
			{
				if (listRoomRequests[i].Seats != null && listRoomRequests[i].Seats.Any())
				{
					var ret = AddSeatDefault(listRooms[i].Id, listRoomRequests[i].Seats);
					// trong quá trình thêm ghế xảy ra lỗi thì dừng quá trình thêm ghế trả về lỗi
					if (ret.Data == null)
					{
						return _roomResponse.ErrorResponse(ret.StatusCode, ret.Message, null);
					}
				}
			}
			return _roomResponse.SuccessResponse("Thêm phòng vào rạp thành công", listRooms.Select(x => _roomConverter.RoomDTO(x)).ToList());
		}

		private ResponseObject<List<DataResponse_Seat>> AddSeatDefault(int roomId, List<DataRequest_AddSeat> listSeatRequests)
		{
			List<Seat> list = new List<Seat>();
			foreach (var seatRequest in listSeatRequests)
			{
				var room = _context.Rooms.FirstOrDefault(x => x.Id == roomId);
				if (room == null)
				{
					throw new ArgumentNullException("Không tìm thấy phòng");
				}
				if (_context.Seats.Any(x => x.Line == seatRequest.Line && x.Number == seatRequest.Number && x.RoomId == roomId))
				{
					return _seatResponse.ErrorResponse(StatusCodes.Status400BadRequest, "Có vị trí ghế bị trùng lặp", null);
				}
				if (!_context.SeatStatus.Any(x => x.Id == seatRequest.SeatStatusId))
				{
					return _seatResponse.ErrorResponse(StatusCodes.Status400BadRequest, "Có ghế chứa thuộc tính SeatStatus không khả dụng", null);
				}
				if (!_context.SeatTypes.Any(x => x.Id == seatRequest.SeatTypeId))
				{
					return _seatResponse.ErrorResponse(StatusCodes.Status400BadRequest, "Có ghế chứa thuộc tính SeatType không khả dụng", null);
				}
				Seat seat = new Seat
				{
					Number = seatRequest.Number,
					Line = seatRequest.Line,
					RoomId = roomId,
					SeatStatusId = seatRequest.SeatStatusId,
					SeatTypeId = seatRequest.SeatTypeId,
					IsActive = true
				};
				_context.Seats.Add(seat);
				_context.SaveChanges();
				list.Add(seat);
			}
			return _seatResponse.SuccessResponse("Thêm ghế vào phòng thành công", list.Select(x => _seatConverter.SeatDTO(x)).ToList());
		}

		public ResponseObject<DataResponse_Food> AddFood(DataRequest_AddFood request)
		{
			if (_context.Foods.Any(x => x.NameOfFood == request.NameOfFood))
			{
				return _foodResponse.ErrorResponse(StatusCodes.Status400BadRequest, "Thức ăn này đã có", null);
			}
			Food food = new Food
			{
				Image = request.Image,
				NameOfFood = request.NameOfFood,
				Description = request.Description,
				Price = request.Price,
				IsActive = true
			};
			_context.Foods.Add(food);
			_context.SaveChanges();
			return _foodResponse.SuccessResponse("Thêm đồ ăn thành công", _foodConverter.FoodDTO(food));
		}
		public ResponseObject<DataResponses_Cinema> UpdateCinema(int cinemaId, DataRequest_UpdateCinema request)
		{
			var cinema = _context.Cinemas.FirstOrDefault(x => x.Id == cinemaId);
			if (cinema is null)
			{
				throw new ArgumentNullException("Không tìm thấy rạp");
			}
			cinema.IsActive = request.IsActive;
			cinema.NameOfCinema = request.NameOfCinema;
			cinema.Description = request.Description;
			cinema.Address = request.Address;
			_context.Cinemas.Update(cinema);
			_context.SaveChanges();
			return _cinemaResponse.SuccessResponse("Update rạp thành công", _cinemaConverter.CinemaDTO(cinema));
		}

		public ResponseObject<DataResponses_Cinema> DeleteCinema(int cinemaId)
		{
			var cinema = _context.Cinemas.FirstOrDefault(x => x.Id == cinemaId);
			if (cinema is null)
			{
				throw new ArgumentNullException("Không tìm thấy rạp");
			}
			_context.Cinemas.Remove(cinema);
			_context.SaveChanges();
			return _cinemaResponse.SuccessResponse("Xóa rạp thành công", _cinemaConverter.CinemaDTO(cinema));
		}

		public ResponseObject<DataResponse_Room> UpdateRoom(int roomId, DataRequest_UpdateRoom request)
		{
			var room = _context.Rooms.FirstOrDefault(x => x.Id == roomId);
			if (room is null)
			{
				throw new ArgumentNullException("Không tìm thấy phòng");
			}
			room.Capacity = request.Capacity;
			room.Type = request.Type;
			room.Description = request.Description;
			room.Name = request.Name;
			room.IsActive = request.IsActive;
			_context.Rooms.Update(room);
			_context.SaveChanges();
			return _singleRoomResponse.SuccessResponse("Update phòng thành công", _roomConverter.RoomDTO(room));
		}

		public ResponseObject<DataResponse_Room> DeleteRoom(int roomId)
		{
			var room = _context.Rooms.FirstOrDefault(x => x.Id == roomId);
			if (room is null)
			{
				throw new ArgumentNullException("Không tìm thấy phòng");
			}
			_context.Rooms.Remove(room);
			_context.SaveChanges();
			return _singleRoomResponse.SuccessResponse("Xóa phòng thành công", _roomConverter.RoomDTO(room));
		}

		public ResponseObject<DataResponse_Seat> UpdateSeat(int seatId, DataRequest_UpdateSeat request)
		{
			var seat = _context.Seats.FirstOrDefault(x => x.Id == seatId);
			if (seat is null)
			{
				throw new ArgumentNullException("Không tìm thấy ghế");
			}
			if (!_context.SeatStatus.Any(x => x.Id == seat.SeatStatusId))
			{
				return _singleSeatResponse.ErrorResponse(StatusCodes.Status400BadRequest, "Có ghế chứa thuộc tính SeatStatus không khả dụng", null);
			}
			if (!_context.SeatTypes.Any(x => x.Id == seat.SeatTypeId))
			{
				return _singleSeatResponse.ErrorResponse(StatusCodes.Status400BadRequest, "Có ghế chứa thuộc tính SeatType không khả dụng", null);
			}
			seat.Number = request.Number;
			seat.SeatStatusId = request.SeatStatusId;
			seat.SeatTypeId = seat.SeatTypeId;
			seat.Line = request.Line;
			seat.IsActive = seat.IsActive;
			_context.Seats.Update(seat);
			_context.SaveChanges();
			return _singleSeatResponse.SuccessResponse("Update ghế thành công", _seatConverter.SeatDTO(seat));
		}

		public ResponseObject<DataResponse_Seat> DeleteSeat(int seatId)
		{
			var seat = _context.Seats.FirstOrDefault(x => x.Id == seatId);
			if (seat is null)
			{
				throw new ArgumentNullException("Không tìm thấy ghế");
			}
			_context.Seats.Remove(seat);
			_context.SaveChanges();
			return _singleSeatResponse.SuccessResponse("Xóa ghế thành công", _seatConverter.SeatDTO(seat));
		}

		public ResponseObject<DataResponse_Food> UpdateFood(int foodId, DataRequest_UpdateFood request)
		{
			var food = _context.Foods.FirstOrDefault(x => x.Id == foodId);
			if (food is null)
			{
				throw new ArgumentNullException("Không tìm thấy đồ ăn");
			}
			food.Price = request.Price;
			food.Description = request.Description;
			food.Image = request.Image;
			food.NameOfFood = request.NameOfFood;
			food.IsActive = request.IsActive;
			_context.Foods.Update(food);
			_context.SaveChanges();
			return _foodResponse.SuccessResponse("Update đồ ăn thành công", _foodConverter.FoodDTO(food));
		}

		public ResponseObject<DataResponse_Food> DeleteFood(int foodId)
		{
			var food = _context.Foods.FirstOrDefault(x => x.Id == foodId);
			if (food is null)
			{
				throw new ArgumentNullException("Không tìm thấy đồ ăn");
			}
			_context.Foods.Remove(food);
			_context.SaveChanges();
			return _foodResponse.SuccessResponse("Xóa đồ ăn thành công", _foodConverter.FoodDTO(food));
		}

		public ResponseObject<DataResponse_Movie> AddMovie(DataRequest_AddMovie request)
		{
			Movie movie = new Movie
			{
				MovieDuration = request.MovieDuration,
				EndTime = request.EndTime,
				PremiereDate = request.PremiereDate,
				Description = request.Description,
				Director = request.Director,
				Image = request.Image,
				HeroImage = request.HeroImage,
				Language = request.Language,
				MovieTypeId = request.MovieTypeId,
				Name = request.Name,
				RateId = request.RateId,
				Trailer = request.Trailer,
				IsActive = true
			};
			_context.Movies.Add(movie);
			_context.SaveChanges();
			return _movieResponse.SuccessResponse("Thêm phim thành công", _movieConverter.MovieDTO(movie));
		}

		public ResponseObject<DataResponse_Movie> UpdateMovie(int movieId, DataRequest_UpdateMovie request)
		{
			var movie = _context.Movies.FirstOrDefault(x => x.Id == movieId);
			if (movie is null)
			{
				throw new ArgumentNullException("Không tìm thấy phim");
			}
			movie.MovieDuration = request.MovieDuration;
			movie.EndTime = request.EndTime;
			movie.PremiereDate = request.PremiereDate;
			movie.Description = request.Description;
			movie.Director = request.Director;
			movie.Image = request.Image;
			movie.HeroImage = request.HeroImage;
			movie.Language = request.Language;
			movie.MovieTypeId = request.MovieTypeId;
			movie.Name = request.Name;
			movie.RateId = request.RateId;
			movie.Trailer = request.Trailer;
			movie.IsActive = request.IsActive;
			_context.Movies.Update(movie);
			_context.SaveChanges();
			return _movieResponse.SuccessResponse("Update phim thành công", _movieConverter.MovieDTO(movie));
		}

		public ResponseObject<DataResponse_Movie> DeleteMovie(int movieId)
		{
			var movie = _context.Movies.FirstOrDefault(x => x.Id == movieId);
			if (movie is null)
			{
				throw new ArgumentNullException("Không tìm thấy phim");
			}
			_context.Movies.Remove(movie);
			_context.SaveChanges();
			return _movieResponse.SuccessResponse("Update phim thành công", _movieConverter.MovieDTO(movie));
		}

		public ResponseObject<DataResponse_Schedule> AddSchedule(DataRequest_AddSchedule request)
		{
			if(_context.Schedules.Any(x=>x.Code == request.Code))
			{
				return _scheduleResponse.ErrorResponse(StatusCodes.Status400BadRequest,"Code bị trùng lặp",null);
			}
			if (_context.Schedules.Any(x => x.StartAt <= request.StartAt && x.EndAt >= request.StartAt && x.RoomId == request.RoomId))
			{
				return _scheduleResponse.ErrorResponse(StatusCodes.Status400BadRequest,"Bị trùng lặp thời gian xuất chiếu",null);
			}
			if (_context.Schedules.Any(x => x.StartAt <= request.EndAt && x.EndAt >= request.EndAt && x.RoomId == request.RoomId))
			{
				return _scheduleResponse.ErrorResponse(StatusCodes.Status400BadRequest, "Bị trùng lặp thời gian xuất chiếu", null);
			}
			if (_context.Schedules.Any(x => x.StartAt >= request.StartAt && x.EndAt <= request.EndAt && x.RoomId == request.RoomId))
			{
				return _scheduleResponse.ErrorResponse(StatusCodes.Status400BadRequest, "Bị trùng lặp thời gian xuất chiếu", null);
			}
			if (!_context.Movies.Any(x=>x.Id == request.MovieId))
			{
				return _scheduleResponse.ErrorResponse(StatusCodes.Status400BadRequest, "Không tìm thấy phim", null);
			}
			if (!_context.Rooms.Any(x => x.Id == request.RoomId))
			{
				return _scheduleResponse.ErrorResponse(StatusCodes.Status400BadRequest, "Không tìm thấy phòng", null);
			}
			Schedule schedule = new Schedule
			{
				Price = request.Price,
				StartAt = request.StartAt,
				EndAt = request.EndAt,
				Code = request.Code,
				MovieId = request.MovieId,
				Name = request.Name,
				RoomId = request.RoomId,
				IsActive = true
			};
			_context.Schedules.Add(schedule);
			_context.SaveChanges();
			return _scheduleResponse.SuccessResponse("Thêm thành công", _scheduleConverter.ScheduleDTO(schedule));
		}

		public ResponseObject<DataResponse_Schedule> UpdateSchedule(int scheduleId, DataRequest_UpdateSchedule request)
		{
			Schedule schedule = _context.Schedules.FirstOrDefault(x => x.Id == scheduleId);
			if(schedule is null)
			{
				return _scheduleResponse.ErrorResponse(StatusCodes.Status400BadRequest, "Không tìm thấy schedule", null);
			}
			if (!_context.Movies.Any(x => x.Id == request.MovieId))
			{
				return _scheduleResponse.ErrorResponse(StatusCodes.Status400BadRequest, "Không tìm thấy phim", null);
			}
			if (!_context.Rooms.Any(x => x.Id == request.RoomId))
			{
				return _scheduleResponse.ErrorResponse(StatusCodes.Status400BadRequest, "Không tìm thấy phòng", null);
			}
			if (_context.Schedules.Any(x => x.StartAt <= request.StartAt && x.EndAt >= request.StartAt && x.RoomId == request.RoomId && x.Id != scheduleId))
			{
				return _scheduleResponse.ErrorResponse(StatusCodes.Status400BadRequest, "Bị trùng lặp thời gian xuất chiếu", null);
			}
			if (_context.Schedules.Any(x => x.StartAt <= request.EndAt && x.EndAt >= request.EndAt && x.RoomId == request.RoomId && x.Id != scheduleId))
			{
				return _scheduleResponse.ErrorResponse(StatusCodes.Status400BadRequest, "Bị trùng lặp thời gian xuất chiếu", null);
			}
			if (_context.Schedules.Any(x => x.StartAt >= request.StartAt && x.EndAt <= request.EndAt && x.RoomId == request.RoomId && x.Id != scheduleId))
			{
				return _scheduleResponse.ErrorResponse(StatusCodes.Status400BadRequest, "Bị trùng lặp thời gian xuất chiếu", null);
			}
			schedule.Price = request.Price;
			schedule.StartAt = request.StartAt;
			schedule.EndAt = request.EndAt;
			schedule.Name = request.Name;
			schedule.MovieId = request.MovieId;
			schedule.RoomId = request.RoomId;
			schedule.IsActive = request.IsActive;
			_context.Schedules.Update(schedule);
			_context.SaveChanges();
			return _scheduleResponse.SuccessResponse("Update thành công", _scheduleConverter.ScheduleDTO(schedule));
		}

		public ResponseObject<DataResponse_Schedule> DeleteSchedule(int scheduleId)
		{
			Schedule schedule = _context.Schedules.FirstOrDefault(x => x.Id == scheduleId);
			if (schedule is null)
			{
				return _scheduleResponse.ErrorResponse(StatusCodes.Status400BadRequest, "Không tìm thấy schedule", null);
			}
			_context.Schedules.Remove(schedule);
			_context.SaveChanges();
			return _scheduleResponse.SuccessResponse("Delete thành công", _scheduleConverter.ScheduleDTO(schedule));
		}
	}
}
