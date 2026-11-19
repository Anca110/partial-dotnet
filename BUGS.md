# BUGS

The starter contains behaviors that contradict `ASSIGNMENT.md`. Write here every discrepancy you found.
Fill it in yourself, in your own words. There is no required number: what counts is that they are real.

| Which rule from ASSIGNMENT.md is violated (R1–R8) | Where the problem was in the starter (file, approximate line) | How you discovered it (test, reading, AI) | What you changed |
|---|---|---|---|
| R5 | BookingService.cs, Create |  citire | verificare in Booking.Create |
| R6 | la fel | citire | max 4h in Booking..Create |
| R7 | BookingService.cs, foreach | test | <= devine < |
| R7/R8 | acelasi foreach | citire | doar cele active bloceaza |
| R8 | BookingService.cs, Cancel | test | a doua anulare 409 |