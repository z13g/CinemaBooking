import { Genre } from "../genre/genre"
import { Seat } from "../seat/seat"
import { Movie } from "../movie/movie"


export class BookShow
{
    hallName?: string;
    cinemaName?: string;
    movie?: Movie;
    seats?: Seat[];
    showId?: number;
    showDateTime?: string;
    selectedSeats?: Seat[];
    availableSeats?: Seat[];
    price?: number;
}
