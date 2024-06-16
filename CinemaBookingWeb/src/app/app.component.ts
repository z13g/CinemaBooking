import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';
import { Router } from '@angular/router';
import { LocalStorageGeneric } from './services/generic.services';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
//   styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
    isLoggedIn = false;
  
    constructor(private authService: AuthService, private router: Router, private storageService : LocalStorageGeneric) {}

    ngOnInit() {
      console.log("App component initialized")
    }
  }