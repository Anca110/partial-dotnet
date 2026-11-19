# DECISIONS

Answer **yourself**, in your own words, in 3–5 sentences per question.
Refer to concrete files and classes from your own repo (for example `Features/Bookings/CreateBooking/...`).
An unanswered question gives 0 points.

## 1. A rule you moved
Pick one business rule (R1–R8). In which file did it live in the starter and in which file does it live now? Why there?

_(R7
inainte Services/BookingService.cs, bucla foreach, acum  Domain/Bookings/Booking.cs, metoda Overlaps
e regula de bussines deci sta in entitate, nu in service sau endpoint)_

## 2. Result instead of an exception
Why is `Result` a better choice than an exception for "the room is occupied"? What happens in your code when an `Error` is returned?

_(Result face esecul vizibil in semnatura si e mai ieftin decat o exeptie
Booking.Create intoarce Error.Conflict, handler ul il da mai departe, iar endpoint ul il transforma in ToProblem())_

## 3. Who knows about EF Core?
Which classes in your project know about EF Core and which do not? What do you gain and what do you pay with Repository + Unit of Work?

_(Infrastructure/Persistence (RoomHubDbContext, EfRoomRepository, EfBookingRepository, EfUnitOfWork), Program.cs
Domain si Features nu)_

## 4. The flow of a request
Draw in text (ASCII arrows) the path of a `POST /api/rooms/{roomId}/bookings` request through your project, with the **real** names of files and classes, from the endpoint to the database.

_()_

## 5. What you rejected from what the AI gave you
Give a concrete example of code or an idea proposed by the AI that you corrected or rejected, and say why.

_(your answer here)_

## 6. If you had 30 more minutes
What would you improve and what risk did you leave in the project?

_(your answer here)_
