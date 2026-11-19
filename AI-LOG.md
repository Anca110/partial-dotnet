# AI-LOG

Write down, in order, what you asked the AI. It is not a punishment: a missing or false log lowers the level D grade.

| Time (approx.) | What you asked (briefly) | What you kept | What you changed or rejected |
|---|---|---|---|
| 9:12 | mutare pe Features | 5 slice-uri cu endpoint si handler | nimic |
| 9:20 |  domain rich, Result în loc de excepții, ICommandHandler, R5 și R6 cu TimeProvider | Room și Booking cu private set și Create(), Result<T> + Error, handlere cu ICommandHandler/IQueryHandler, endpoint-uri subțiri, TimeProvider | AI-ul a șters BusinessException și middleware-ul. JSON invalid dădea 500 și a fost reparat să dea 400. R7 și R8 au rămas pentru pasul următor |
| 9:33 |   EF Core + SQLite, repository + unit of work cu interfețele în domeniu, R7 și R8 | DbContext, EfRoomRepository / EfBookingRepository / EfUnitOfWork, interfetele in Domain, datele raman dupa restart, < strict la suprapunere, doar rezervarile active blocheaza, a doua anulare da 409 |  prima instalare EF a picat (versiunea 11 cere .NET 11), s-a pus 10.0.15. S-au sters repository-urile in memorie. S-a adaugat conversie UTC la datele citite din SQLite. |
