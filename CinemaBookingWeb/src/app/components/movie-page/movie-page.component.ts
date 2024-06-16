// Importing necessary modules and components from Angular core, router, and font-awesome icons
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faTicket, faRefresh } from '@fortawesome/free-solid-svg-icons';

// Importing necessary operators and observables from RxJS
import { EMPTY, catchError, switchMap } from 'rxjs';

// Importing models
import { Cinema } from 'src/app/models/cinema/cinema';
import { Movie } from 'src/app/models/movie/movie';
import { Show } from 'src/app/models/show/show';

// Importing services
import { AuthService } from 'src/app/services/auth.service';
import { GenericService, LocalStorageGeneric } from 'src/app/services/generic.services';
import { ShowService } from 'src/app/services/show.service';
import { CinemaShowMap } from '../../services/show.service';

// Importing Angular platform-browser for sanitizing URLs
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

// Importing Area model
import { Area } from 'src/app/models/area/area';

// Importing form building services
import { FormBuilder, FormGroup } from '@angular/forms';

// Interface for route state
interface RouteState {
  movieId: string;
}

// Component decorator with metadata
@Component({
  selector: 'app-movie-page', // CSS selector to identify the component
  templateUrl: './movie-page.component.html', // Path to the component's template file
  styleUrls: ['./movie-page.component.css'] // Path to the component's styles file
})
export class MoviePageComponent {
  // Declaring variables for use in the component
  selectedCityData: string = "";
  faTicket = faTicket; // FontAwesome icon for ticket
  faRefresh = faRefresh; // FontAwesome icon for refresh
  selectedMovie: Movie | undefined; // Selected movie object
  selectedGenreId: string = ""; // Selected genre ID
  selectedAreaID: string = ""; // Selected area ID
  SelectedCityName: string = ""; // Selected city name
  movieId: string | null = null; // Movie ID from route
  selectedCinemaName: string | null = null; // Selected cinema name
  selectedDate: string = ''; // Selected date

  // Toggle states
  toggleDetail: boolean = false;
  toggleBody: boolean = false;
  showLink: boolean = true;
  showBodyLink: boolean = true;

  // Lists for shows, cinemas, movies, and areas
  showsList: Show[] = [];
  cinemaInSelectedArea: Cinema[] = [];
  dayShowsList: any[] = [];
  cinemasList: Cinema[] = [];
  moviesList: Movie[] = [];
  areaList: Area[] = [];
  nextTenDays: Date[] = [];
  showsByDay: { [key: string]: Show[] } = {};
  showsByCinemaAndDate: CinemaShowMap = {};
  showsByCinemaAndDateFiltered: CinemaShowMap = {};

  // Filtered shows list
  filteredShows: Show[] = [];

  // Modal & Form Variables
  loginForm!: FormGroup;
  showModal = false;

  // Error Variables
  error: string = '';
  loginError: string = '';

  // Authentication variables
  isAuthenticated!: boolean;
  userRole!: string | null;
  email!: string | null;
  name!: string | null;

  // Watch list
  watchList: string[] = [];

  // Constructor for dependency injection
  constructor(
    private authService: AuthService, // Authentication service
    private router: Router, // Router for navigation
    private route: ActivatedRoute, // Activated route to access route parameters
    private showService: GenericService<Show>, // Generic service for show
    private showshowService: ShowService, // Show service
    private cinemaService: GenericService<Cinema>, // Generic service for cinema
    private movieService: GenericService<Movie>, // Generic service for movie
    private storageService: LocalStorageGeneric, // Local storage service
    private sanitizer: DomSanitizer, // Sanitizer for URLs
    private formBuilder: FormBuilder, // Form builder service
  ) { }

  // Lifecycle hook for component initialization
  ngOnInit() {
    window.scrollTo(0, 0); // Scroll to top of the page

    // Initialize login form
    this.loginForm = this.formBuilder.group({
      email: '', // Email field
      password: '' // Password field
    });

    // Handle local storage for selected area
    this.storageService.handleLocalStorage().then(areaExists => {
      if (!areaExists) {
        console.log('No valid area selected, user redirected to area pick.');
        this.router.navigate(['/areapick']); // Redirect to area pick if no area is selected
      } else {
        console.log('Valid area selected, user can proceed in the app.');
      }
    }).catch(error => {
      console.error('Error handling in localStorage service:', error);
      this.router.navigate(['/areapick']); // Redirect to area pick on error
    });

    // Retrieve values from local storage
    this.selectedCinemaName = localStorage.getItem('SelectedCinemaName') || '';
    this.selectedDate = localStorage.getItem('SelectedDate') || '';
    this.watchList = JSON.parse(localStorage.getItem('watchList') || '[]');

    // Fetch movies, shows, areas, and cinemas
    this.movieService.getAll("movie").pipe(
      switchMap(movies => {
        this.moviesList = movies;
        console.log("MovieList: ", this.moviesList);

        this.movieId = this.route.snapshot.paramMap.get('movieId');
        if (this.movieId) {
          this.setSelectedMovie(this.movieId);
        } else {
          console.error('movieId is null');
          return EMPTY;
        }

        return this.showService.getAll("show");
      }),
      switchMap(shows => {
        this.showsList = shows;
        console.log("ShowsList: ", this.showsList);
        return this.cinemaService.getAll("area");
      }),
      switchMap(area => {
        this.areaList = area;
        console.log("AreaList: ", this.areaList);
        return this.cinemaService.getAll("cinema");
      }),
      catchError(error => {
        console.error('Error in observable chain:', error);
        return EMPTY;
      })
    ).subscribe(cinemas => {
      this.cinemasList = cinemas;
      console.log("CinemasList: ", this.cinemasList);
      this.FindCinemaBySelectedArea();

      if (this.movieId && this.selectedAreaID) {
        this.fetchFilteredShowsAndSort(parseInt(this.selectedAreaID), parseInt(this.movieId));
      } else {
        console.error('Area ID or movie ID is null');
      }

      this.isAuthenticated = this.authService.isAuthenticated();

      if (this.isAuthenticated) {
        this.name = this.authService.getName() || '';
        this.email = this.authService.getEmail() || '';
        this.userRole = this.authService.getUserRole() || '';
        console.log("User is authenticated on Movie page.");
      } else {
        console.log("User is not authenticated on Movie page.");
      }
    });

    this.filterShowDisplayed();
    this.getSelectedArea();
    this.getNextTenDays();
  }

  // Utility method to get object keys as strings
  objectKeys(obj: any): string[] {
    return Object.keys(obj);
  }

  // Handle area change event
  onAreaChange(event: any): void {
    const selectedAreaID = event.target.value;
    const selectedArea = this.areaList.find(area => area.areaID == selectedAreaID);
    if (selectedArea && selectedAreaID !== undefined) {
      this.resetDropdowns();
      const areaData = { name: selectedArea.areaName, id: selectedAreaID };
      const areaDataString = JSON.stringify(areaData);
      localStorage.setItem('selectedArea', areaDataString);
      this.storageService.handleLocalStorage().then(() => {
        this.getSelectedArea();
        if (this.movieId && selectedAreaID) {
          this.fetchFilteredShowsAndSort(parseInt(selectedAreaID), parseInt(this.movieId));
        }
      });
    }
  }

  // Scroll to specific position on the page
  LookTime() {
    window.scrollTo(0, 880);
  }

  // Toggle watch list status for a movie
  toggleWatchList(movieId: number | undefined) {
    if (!movieId) {
      return;
    }

    const movieIdStr = movieId.toString();
    if (this.isAuthenticated) {
      if (this.watchList.includes(movieIdStr)) {
        // If the movie is already in the watch list, remove it
        this.watchList = this.watchList.filter(id => id !== movieIdStr);
        // Update the watch list in local storage
        localStorage.setItem('watchList', JSON.stringify(this.watchList));
      } else {
        // If the movie is not in the watch list, add it
        this.watchList.push(movieIdStr);
        // Update the watch list in local storage
        localStorage.setItem('watchList', JSON.stringify(this.watchList));
      }
    } else {
      // If not authenticated, show the login modal
      this.toggleModal();
    }
  }

  // Check if a movie is in the watch list
  isInWatchList(movieId: number | undefined): boolean {
    if (!movieId) {
      return false;
    }
    return this.watchList.includes(movieId.toString());
  }

  // Handle login process
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

  // Handle logout process
  logout() {
    this.authService.logout();
    this.isAuthenticated = false;
    this.name = null;
    this.email = null;
    this.userRole = null;
  }

  // Toggle login modal visibility
  toggleModal() {
    this.showModal = !this.showModal;
  }

  // Toggle detail view
  toggleDetails(): void {
    this.showLink = false;  // Hide the link during the transition
    this.toggleDetail = !this.toggleDetail;
    const extraInfoElement = document.querySelector('.extra-info .row2') as HTMLElement;
    if (extraInfoElement) {
      if (this.toggleDetail) {
        // Use setTimeout to ensure the maxHeight is applied after Angular's change detection
        setTimeout(() => {
          extraInfoElement.style.maxHeight = extraInfoElement.scrollHeight + 'px';
          console.log("Height expanded: ", extraInfoElement.style.maxHeight);
        }, 0);
      } else {
        extraInfoElement.style.maxHeight = '0';
        console.log("Height collapsed: ", extraInfoElement.style.maxHeight);
      }
    }

    // Re-show the link after the transition duration
    setTimeout(() => {
      this.showLink = true;
    }, 400);  // Set it 0.1 sec in front of the transaction
  }

  // Toggle body view
toggleBodys(): void {
  this.showBodyLink = false;  // Hide the link during the transition
  this.toggleBody = !this.toggleBody;
  const bodyInfoElement = document.querySelector('.body-info .row2') as HTMLElement;
  if (bodyInfoElement) {
      if (this.toggleBody) {
          // Use setTimeout to ensure the maxHeight is applied after Angular's change detection
          setTimeout(() => {
              bodyInfoElement.style.maxHeight = bodyInfoElement.scrollHeight + 'px';
              console.log("Height expanded: ", bodyInfoElement.style.maxHeight);
          }, 0);
      } else {
          bodyInfoElement.style.maxHeight = '0';
          console.log("Height collapsed: ", bodyInfoElement.style.maxHeight);
      }
  }

  // Re-show the link after the transition duration
  setTimeout(() => {
      this.showBodyLink = true;
  }, 200);  // Set it 0.1 sec in front of the transaction
}


  // Fetch and sort filtered shows based on area and movie ID
  fetchFilteredShowsAndSort(areaId: number, movieId: number): void {
    console.log("Fetching filtered shows for area:", areaId, "and movie:", movieId);
    this.showshowService.getFilteredShows("Show", areaId, movieId).subscribe({
      next: (cinemaShowMap) => {
        for (const cinema in cinemaShowMap) {
          for (const date in cinemaShowMap[cinema]) {
            cinemaShowMap[cinema][date] = cinemaShowMap[cinema][date]
              .sort((a, b) => new Date(a.showDateTime).getTime() - new Date(b.showDateTime).getTime());
          }
        }
        this.showsByCinemaAndDate = cinemaShowMap;
        this.filterShowDisplayed();

        console.log("Sorted and Filtered Shows by Cinema and Date And Movie: ", this.showsByCinemaAndDate);
      },
      error: (error) => console.error('Error fetching filtered shows:', error)
    });
  }

  // Get the next ten days
  getNextTenDays(): void {
    const today = new Date();
    for (let i = 0; i < 10; i++) {
      const nextDay = new Date(today);
      nextDay.setDate(today.getDate() + i);
      this.nextTenDays.push(nextDay);
    }
  }

  // Get abbreviated day name
  getDayAbbreviation(dateStr: string): string {
    const date = new Date(dateStr);
    const days = ['Søn', 'Man', 'Tir', 'Ons', 'Tor', 'Fre', 'Lør'];
    return days[date.getDay()];
  }

  // Handle cinema change event
  onCinemaChange(event: Event): void {
    const element = event.target as HTMLSelectElement;
    if (element) {
      this.selectedCinemaName = element.value;
      localStorage.setItem('SelectedCinemaName', this.selectedCinemaName);
      this.filterShowDisplayed();
    }
  }

  // Handle date change event
  onDateChange(selectedDateString: string): void {
    this.selectedDate = selectedDateString;
    localStorage.setItem('SelectedDate', this.selectedDate);
  }

  // Check if a date should be blurred
  shouldBlur(date: string): boolean {
    return !!this.selectedDate && date !== this.selectedDate;
  }

  // Filter shows to be displayed
  filterShowDisplayed(): void {
    this.showsByCinemaAndDateFiltered = {};

    if (this.selectedCinemaName) {
      if (this.showsByCinemaAndDate[this.selectedCinemaName]) {
        this.showsByCinemaAndDateFiltered[this.selectedCinemaName] = this.showsByCinemaAndDate[this.selectedCinemaName];
      } else {
        console.error('No shows found for the selected cinema:', this.selectedCinemaName);
      }
    } else {
      Object.keys(this.showsByCinemaAndDate).forEach(cinemaName => {
        this.showsByCinemaAndDateFiltered[cinemaName] = this.showsByCinemaAndDate[cinemaName];
      });
    }
  }

  // Get selected area from local storage
  getSelectedArea(): void {
    const selectedCityData = this.storageService.getItem('selectedArea');
    if (selectedCityData) {
      console.log(selectedCityData);
      const selectedArea = selectedCityData;
      this.selectedAreaID = selectedArea.id;
      this.SelectedCityName = selectedArea.name;
      this.FindCinemaBySelectedArea();
    }
  }

  // Set the selected movie based on movie ID
  setSelectedMovie(movieId: string): void {
    if (movieId) {
      this.selectedMovie = this.moviesList.find(movie => movie.movieID?.toString() == movieId);
      console.log("SelectedMovie: ", this.selectedMovie);
    }
  }

  // Sanitize a URL to be used safely in the application
  sanitizeUrl(url: string): SafeResourceUrl {
    return this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }

  // Format minutes into hours and minutes
  formatMinutesToHoursAndMinutes(minutes: number | undefined): string {
    if (minutes === undefined || minutes === null) {
      return "0h 0min";
    }
    const hours = Math.floor(minutes / 60);
    const remainingMinutes = minutes % 60;
    return `${hours}h ${remainingMinutes}min`;
  }

  // Find cinemas by the selected area
  FindCinemaBySelectedArea(): void {
    this.cinemaInSelectedArea = this.cinemasList.filter(cinema => {
      return String(cinema.areaID) === String(this.selectedAreaID);
    });
    console.log("CinemaInSelectedArea: ", this.cinemaInSelectedArea);
  }

  // Reset dropdown selections and local storage items
  resetDropdowns(): void {
    const dropdownKeys = ['SelectedCinemaName', 'SelectedDate'];

    dropdownKeys.forEach(key => {
      this.selectedCinemaName = '';
      this.selectedDate = '';
      this.storageService.removeItem(key);
    });
    this.filterShowDisplayed();
  }
}
