import { Component, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  encapsulation: ViewEncapsulation.None 
})
export class AppComponent {
  title = 'CinemaBooking';
  name: string = 'Cinema Booking';
  age: number = 20;
  sand: boolean = true;
  liste: string[] = ['a', 'b', 'c'];

  ngOnInit(): void {
    console.log('Init method called');
    this.create();
  }
  create(): void {
    console.log('Create method called');
  }

  showPassword(): void {
    const password = document.getElementById('password');
    if (password) {
      if (password.getAttribute('type') === 'password') {
        password.setAttribute('type', 'text');
      } else {
        password.setAttribute('type', 'password');
      }
    }

  }

}
