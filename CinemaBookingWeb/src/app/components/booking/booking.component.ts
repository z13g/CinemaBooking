import { Component, OnInit } from '@angular/core';
import { faCouch } from '@fortawesome/free-solid-svg-icons';
import { faWheelchairMove } from '@fortawesome/free-solid-svg-icons';
import { AuthService } from 'src/app/services/auth.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { GenericService } from 'src/app/services/generic.services';
import { ActivatedRoute, Router } from '@angular/router';
import { BookShowService } from 'src/app/services/bookshow.service';
import { Genre } from 'src/app/models/genre/genre';

interface Seat {
  hallID: number;
  seatID: number;
  seatNumber: number;
  seatRow: string;
  seatStatus: string;  
}

interface Movie {
  director: string;
  duration: number;
  genres: Genre[];
  movieID: number;
  movieLink: string;
  title: string;
}

interface BookShow {
  hallName: string;
  cinemaName: string;
  movie: Movie;
  seats: Seat[];
  showId: number;
  showDateTime: string;
  selectedSeats: Seat[];
  availableSeats: Seat[];
  price: number;
}

@Component({
  selector: 'app-booking',
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.css']
})
export class BookingComponent implements OnInit {
  // Modal & Form Variables
  loginForm!: FormGroup;
  showModal = false;

  // Error Variables
  error: string = '';
  loginError: string = '';

  // Auth Global Variables
  isAuthenticated!: boolean;
  userRole!: string | null;
  email!: string | null;
  name!: string | null;

  // Ticket Variables
  ticketCnt: number = 0;
  basketPrice: number = 0; 

  // BookShow Variable 
  bookShow!: BookShow;

  rows: { row: string, seats: Seat[] }[] = [];

  // Font Awesome Icons
  faCouch = faCouch;
  faWheelchairMove = faWheelchairMove;

  constructor(
    private route: ActivatedRoute, 
    private router: Router, 
    private authService: AuthService, 
    private formBuilder: FormBuilder, 
    private genericService: GenericService<any>,
    private bookShowService: BookShowService
  ) {
    this.isAuthenticated = this.authService.isAuthenticated();
    this.name = this.authService.getName();
    this.email = this.authService.getEmail();
    this.userRole = this.authService.getUserRole();
  }

  ngOnInit() {
    this.handleRouteParam();
    this.loginForm = this.formBuilder.group({
      email: '',
      password: ''
    });
  }

  handleRouteParam() {
    this.route.params.subscribe(params => {
      const showId = params['id'];
      this.genericService.getById('Show/BookInfo', showId).subscribe({
        next: (data: BookShow) => {
          console.log("Data from booking component", data);
          if (data.cinemaName == null) {
            this.router.navigate(['404']);
          }
          this.bookShow = data;
          this.handleGlobalVariables();
          this.organizeRows();
          this.bookShowService.updateBookShow(this.bookShow);
        },
        error: (err) => {
          console.error("Error from booking component", err);
          this.router.navigate(['404']);
        }
      });
    });
  }

  // Handle Login Modal 
  toggleModal() {
    this.showModal = !this.showModal;
  }

  // Handle login
  login() {
    if (this.loginForm.valid) {
      const { email, password } = this.loginForm.value;
      console.log("email:", email, "password:", password);
      this.authService.login(email, password).subscribe({
        next: (success) => {
          if (success) {
            console.log("Login success from login component");
            this.isAuthenticated = true;
            this.name = this.authService.getName();
            this.email = this.authService.getEmail();
            this.userRole = this.authService.getUserRole();
            this.toggleModal();
          } else {
            this.loginError = 'Invalid credentials';
            console.log("Invalid credentials from login component");
          }
        },
        error: (err) => {
          this.loginError = err.message;
        }
      });
    }
  }

  // Handle logout
  logout() {
    this.authService.logout();
    this.isAuthenticated = false;
    this.name = null;
    this.email = null;
    this.userRole = null;
  }

  increaseTicketCnt() {
    // If there are available seats and the ticket count is greater than 0
    if (this.ticketCnt > 0 && this.bookShow.availableSeats.length > 0) {
      // Get the last selected seat
      const lastSelectedSeat = this.bookShow.selectedSeats[this.bookShow.selectedSeats.length - 1];
      // Find the next available seat
      const nextAvailableSeat = this.bookShow.availableSeats.find(seat => seat.seatID === lastSelectedSeat.seatID + 1);
      if (nextAvailableSeat) {
        nextAvailableSeat.seatStatus = 'Selected';
        this.ticketCnt++;
        this.handleGlobalVariables();
      }
    // If there are no selected seats and there are available seats
    } else if (this.ticketCnt == 0 && this.bookShow.availableSeats.length > 0) {
      // Select the first available seat to ticket
      const firstAvailableSeat = this.bookShow.availableSeats[0];
      firstAvailableSeat.seatStatus = 'Selected';
      this.ticketCnt++;
      this.handleGlobalVariables();
    }
  }

  decreaseTicketCnt() {
    console.log("ticket count from decreasteticket", this.ticketCnt);
    console.log("selected seats from decreaseticket", this.bookShow.selectedSeats);
    if (this.ticketCnt > 0 && this.bookShow.selectedSeats.length > 0) {
      const lastSelectedSeat = this.bookShow.selectedSeats[this.bookShow.selectedSeats.length - 1];
      lastSelectedSeat.seatStatus = 'Available';
      this.ticketCnt--;
      this.handleGlobalVariables();
    }
  }

  handleGlobalVariables() {
    const selectedSeats = this.bookShow.seats.filter(seat => seat.seatStatus === 'Selected');
    const availableSeats = this.bookShow.seats.filter(seat => seat.seatStatus === 'Available');
    this.basketPrice = selectedSeats.length * this.bookShow.price;
    this.bookShow.selectedSeats = selectedSeats;
    this.bookShow.availableSeats = availableSeats;
    console.log("selected seats", this.bookShow.selectedSeats);
    // Opdater BookShowService, så data gemmes i sessionStorage
    this.bookShowService.updateBookShow(this.bookShow);
  }

  organizeRows() {
    this.rows = [];
    this.bookShow.seats.forEach(seat => {
      const row = this.rows.find(r => r.row === seat.seatRow);
      if (row) {
        row.seats.push(seat);
      } else {
        this.rows.push({ row: seat.seatRow, seats: [seat] });
      }
    });
  }

  toggleSeat(seat: Seat) {
    console.log(seat);
    switch (seat.seatStatus) {
      case 'Available':
        seat.seatStatus = 'Selected';
        this.ticketCnt++;
        this.handleGlobalVariables();
        break;
      case 'Selected':
        seat.seatStatus = 'Available';
        this.ticketCnt--;
        this.handleGlobalVariables();
        break;
    }
    console.log(seat);
  }

  continueToPayment() {
    if (this.isAuthenticated) {
      if (this.ticketCnt > 0) {
        // send BookShow object to payment component
        this.bookShowService.updateBookShow(this.bookShow);
        this.router.navigate(['payment']);
      } else {
        this.error = 'Please select a seat';
      }
    } else {
      this.toggleModal();
    }
  }
}
