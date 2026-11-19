using RoomHub.Api.Common;
using RoomHub.Api.Features.Bookings.CancelBooking;
using RoomHub.Api.Features.Bookings.CreateBooking;
using RoomHub.Api.Features.Bookings.GetBooking;
using RoomHub.Api.Features.Rooms.CreateRoom;
using RoomHub.Api.Features.Rooms.GetRoom;
using RoomHub.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Errors that do not come from a handler (bad JSON, unexpected exceptions) are ProblemDetails too.
builder.Services.AddProblemDetails();

// Invalid JSON gives 400, not a 500 (in Development it would throw by default).
builder.Services.Configure<RouteHandlerOptions>(o => o.ThrowOnBadRequest = false);

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddSingleton<RoomRepository>();
builder.Services.AddSingleton<BookingRepository>();

builder.Services.AddScoped<ICommandHandler<CreateRoomCommand, CreateRoomResponse>, CreateRoomHandler>();
builder.Services.AddScoped<IQueryHandler<GetRoomQuery, GetRoomResponse>, GetRoomHandler>();
builder.Services.AddScoped<ICommandHandler<CreateBookingCommand, CreateBookingResponse>, CreateBookingHandler>();
builder.Services.AddScoped<IQueryHandler<GetBookingQuery, GetBookingResponse>, GetBookingHandler>();
builder.Services.AddScoped<ICommandHandler<CancelBookingCommand, Guid>, CancelBookingHandler>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

app.MapGet("/", () => "RoomHub API is running.");

CreateRoomEndpoint.Map(app);
GetRoomEndpoint.Map(app);
CreateBookingEndpoint.Map(app);
GetBookingEndpoint.Map(app);
CancelBookingEndpoint.Map(app);

app.Run();
