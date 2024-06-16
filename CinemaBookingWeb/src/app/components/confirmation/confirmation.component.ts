import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { GenericService } from 'src/app/services/generic.services';
import { Router } from '@angular/router';

interface Seat {
  hallID: number;
  seatID: number;
  seatNumber: number;
  seatRow: string;
  seatStatus: string;
}

interface Genre {
  genreID: number;
  genreName: string;
}

interface Movie {
  title: string;  
  director: string;
  duration: number;
  genres: Genre[];
  movieID: number;
  movieLink: string;
}

interface Show {
  showID: number;
  hallID: number;
  movieID: number;
  showDateTime: string;
  hallName: string;
  movie: Movie;
}

interface Cinema {
  name: string;
  location: string;
}

interface Booking {
  bookingID: number;
  showID: number;
  numberOfSeats: number;
  price: number;
  isActive: boolean;
  seats: Seat[];
  movie: Movie;
  show: Show;
  showDateTime: string;
  movieTitle: string;
  cinema: Cinema;
}

@Component({
  selector: 'app-confirmation',
  templateUrl: './confirmation.component.html',
  styleUrls: ['./confirmation.component.css']
})
export class ConfirmationComponent implements OnInit {
  currentBooking: Booking | null = null;
  previousBookings: Booking[] = [];
  userId: number | null = null;

  constructor(
    private authService: AuthService,
    private genericService: GenericService<any>,
    private router: Router
  ) {
  }

  ngOnInit(): void {
    this.loadCurrentBooking();
    this.loadPreviousBookings();
  }

  loadCurrentBooking() {
    // Antag at den aktuelle booking gemmes et sted efter betaling
    // Her simulerer vi det ved at hente fra en endpoint
    this.genericService.get('Booking/latest').subscribe({
      next: (data: Booking) => {
        this.currentBooking = data;
        console.log("Current Booking: ", this.currentBooking)

      },
      error: (err) => {
        console.error("Error loading current booking", err);
      }
    });
  }

  loadPreviousBookings() {
    // this.genericService.getById('Booking/previous', this.userId).subscribe({
    //   next: (data: Booking[]) => {
    //     this.previousBookings = data;
    //   },
    //   error: (err) => {
    //     console.error("Error loading previous bookings", err);
    //   }
    // });
  }

  orderMore() {
    if (this.currentBooking?.showID) {
      this.router.navigate(['/booking', this.currentBooking.showID]);
    }
  }

  goToHome() {
    this.router.navigate(['/']);
  }
}
