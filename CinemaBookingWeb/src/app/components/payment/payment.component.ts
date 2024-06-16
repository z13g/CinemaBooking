import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { BookShow } from 'src/app/models/BookShow/bookshow';
import { BookShowService } from 'src/app/services/bookshow.service';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { GenericService } from 'src/app/services/generic.services';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css']
})
export class PaymentComponent implements OnInit, OnDestroy {
  url: string = environment.apiUrl;
  bookShow: BookShow | null = null;
  isAuthenticated!: boolean;
  userId!: string | null;
  userRole!: string | null;
  email!: string | null;
  name!: string | null;
  totalPrice: number = 0;
  showReservedModal = false;
  hasBooked = false;
  timeLeft: number = 300; // 5 minutes in seconds
  interval: any;

  constructor(
    private bookShowService: BookShowService, 
    private router: Router, 
    private authService: AuthService, 
    private genericService: GenericService<any>
  ) {
    this.isAuthenticated = this.authService.isAuthenticated();
    this.userId = this.authService.getUserId();
    this.userRole = this.authService.getUserRole();
    this.email = this.authService.getEmail();
    this.name = this.authService.getName();
  }

  ngOnInit() {
    this.bookShowService.currentBookShow.subscribe(bookShow => {
      if (bookShow) {
        this.bookShow = bookShow;
        console.log("BookShow in Payment: ", this.bookShow);
        this.seatReserveHandling('Reserved');
        this.calculateTotalPrice();
        this.startTimer();
      } else {
        console.log("No BookShow in Payment");
        this.router.navigate(['404']);
      }
    });
  }

  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any): void {
    if (!this.hasBooked) {
      this.releaseSeats();
    }
  }

  @HostListener('window:unload', ['$event'])
  unloadReleaseSeats($event: any): void {
    if (!this.hasBooked) {
      this.sendBeaconReleaseSeats();
    }
  }

  ngOnDestroy() {
    if (!this.hasBooked) {
      this.releaseSeats();
    }
    clearInterval(this.interval);
  }

  handleReservedModal() {
    this.showReservedModal = !this.showReservedModal;
  }

  bookSeats() {
    this.hasBooked = true;
    this.seatReserveHandling('Booked');
    this.bookShowService.clearBookShow();
    this.router.navigate(['/confirmation']);
  }

  startTimer() {
    this.interval = setInterval(() => {
      if (this.timeLeft > 0) {
        this.timeLeft--;
      } else {
        if (!this.hasBooked) {
          this.releaseSeats();
        }
        clearInterval(this.interval);
      }
    }, 1000);
  }

  formatTime(seconds: number): string {
    const minutes = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${minutes}:${secs < 10 ? '0' : ''}${secs}`;
  }

  seatReserveHandling(status: string) {
    if (!this.bookShow) return;

    const seatList = this.bookShow.selectedSeats?.map(seat => ({
      ...seat,
      seatStatus: status
    }));

    const reserveSeat = {
      showID: this.bookShow.showId,
      seatList: seatList
    };
    console.log("reserveSeat: ", reserveSeat);
    this.genericService.create('Booking/reserve', reserveSeat).subscribe(
      (response: any) => {
        console.log('Reservation successful:', response);
      },
      (error) => {
        console.error('Reservation failed:', error);
      }
    );
  }

  releaseSeats() {
    this.seatReserveHandling('Available');
    this.bookShowService.clearBookShow();
  }

  cancelOrder() {
    this.releaseSeats();
    this.router.navigate(['/']);
  }

  sendBeaconReleaseSeats() {
    if (!this.bookShow) return;

    const seatList = this.bookShow.selectedSeats?.map(seat => ({
      ...seat,
      seatStatus: 'Available'
    }));

    const reserveSeat = {
      showID: this.bookShow.showId,
      seatList: seatList
    };

    try {
      const blob = new Blob([JSON.stringify(reserveSeat)], { type: 'application/json' });
      console.log('Attempting to send beacon for releasing seats:', reserveSeat);
      const success = navigator.sendBeacon(`${this.url}/Booking/reserve`, blob);
      this.bookShowService.clearBookShow();
      console.log('Beacon sent successfully:', success);
      
    } catch (error) {
      console.error('Failed to send beacon for releasing seats:', error);
    }
  }

  calculateTotalPrice() {
    if (this.bookShow && this.bookShow.selectedSeats && this.bookShow.price) {
      this.totalPrice = this.bookShow.selectedSeats.length * this.bookShow.price;
    }
  }
}
