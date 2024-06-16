// src/app/services/visibility.service.ts
// Import necessary modules and dependencies.
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

// Define an interface for mapping string keys to BehaviorSubjects managing visibility states as booleans.
interface ISectionVisibility {
  [key: string]: BehaviorSubject<boolean>;
}

// Mark the service as injectable with a scope of the root application level.
@Injectable({
  providedIn: 'root',
})
export class VisibilityService {
  // Define a private object to track the visibility state of various sections in the application.
  private sections: ISectionVisibility = {
    cinema: new BehaviorSubject<boolean>(false),
    movie: new BehaviorSubject<boolean>(false),
    genre: new BehaviorSubject<boolean>(false),
    cinemahall: new BehaviorSubject<boolean>(false),
    region: new BehaviorSubject<boolean>(false),
    role: new BehaviorSubject<boolean>(false),
    seat: new BehaviorSubject<boolean>(false),
    show: new BehaviorSubject<boolean>(false),
    userDetails: new BehaviorSubject<boolean>(false),
    area: new BehaviorSubject<boolean>(false)
  };

  // Constructor for the VisibilityService.
  constructor() {}

  // Method to reset visibility states of all sections to false.
  resetVisibility(): void {
    Object.values(this.sections).forEach(subject => subject.next(false));
  }

  // Method to toggle the visibility of a specific section to true.
  toggleVisibility(section: string): void {
      this.sections[section].next(true);
  }

  // Method to get an observable that clients can subscribe to, to receive updates on a section's visibility state.
  visibility(section: string) {
    return this.sections[section].asObservable();
  }
}