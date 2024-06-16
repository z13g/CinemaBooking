import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { switchMap } from 'rxjs';
import { Area } from 'src/app/models/area/area';
import { Cinema } from 'src/app/models/cinema/cinema';
import { Movie } from 'src/app/models/movie/movie';
import { Region } from 'src/app/models/region/region';
import { Show } from 'src/app/models/show/show';
import { AuthService } from 'src/app/services/auth.service';
import { GenericService } from 'src/app/services/generic.services';
import { faTicket, faCheck } from '@fortawesome/free-solid-svg-icons';
import { LocalStorageGeneric } from 'src/app/services/generic.services';
import { Genre } from 'src/app/models/genre/genre';

@Component({
  selector: 'app-frontpage',
  templateUrl: './frontpage.component.html',
  styleUrls: ['./frontpage.component.css']
})
export class FrontpageComponent {
  faTicket = faTicket;
  faCheck = faCheck;
  selectedCinemaId: string = ""
  selectedMovieId: string = ""
  selectedGenreId: string =""
  SelectedCityName: string = "";
  displayedMovies: Movie[] = [];
  cinemaList : Cinema[] = [];
  movieList : Movie[] = [];
  showList : Show[] = [];
  areaList: Area[] = [];
  genreList: Genre[] = [];
  regionList: Region[] = [];
  cinemaInSelectedArea: Cinema[] = [];
  movieInSelectedArea: Movie[] = [];
  cinemasByRegion: { [key: string]: Cinema[] } = {}; 
// Dette er en syntax i Typescript som fungere lidt som et Dictonary. Den bruger regionName som Key og Value fra Cinema som Values
  
  selectedAreaId: string = "";
  chosenCity: string = '';
  showCinemaList: boolean = false;

  //Auth Global Variables
  isAuthenticated!: boolean;
  userRole!: string | null;
  email!: string | null;
  name!: string | null;



  constructor(
    private router: Router,
    private genreService: GenericService<Genre>,
    private cinemaService: GenericService<Cinema>,
    private movieService: GenericService<Movie>,
    private showService: GenericService<Show>,
    private regionService: GenericService<Region>,
    private areaService: GenericService<Area>,
    private authService: AuthService,
    private storageService: LocalStorageGeneric
  ) {}



  ngOnInit() {
    window.scrollTo(0, 0);

    this.storageService.handleLocalStorage().then(areaExists => {
      console.log("handleLocalStorage Result from app component:", areaExists);
      if (!areaExists) {
        console.log('No valid area selected, user redirected to area pick.');
        this.router.navigate(['/areapick']);
      } else {
        console.log('Valid area selected, user can proceed in the app.');
      }
    }).catch(error => {
      console.error('Error handling in localStorage service:', error);
      this.router.navigate(['/areapick']);
    });
    
    this.isAuthenticated = this.authService.isAuthenticated();

    if (this.isAuthenticated) {
      this.name = this.authService.getName() || '';
      this.email = this.authService.getEmail() || '';
      this.userRole = this.authService.getUserRole() || '';
      console.log("User is authenticated on front page.");
    } else {
      console.log("User is not authenticated on front page.");
    }
    console.log("User Role: ", this.userRole);
    console.log("Email: ", this.email);
    console.log("Name: ", this.name);
  this.selectedCinemaId = localStorage.getItem('selectedCinemaId') || '';
  this.selectedMovieId = localStorage.getItem('selectedMovieId') || '';
  this.selectedGenreId = localStorage.getItem('selectedGenreId') || '';

    this.regionService.getAll('Region').pipe(
      switchMap(regions => {
        this.regionList = regions;
        return this.areaService.getAll('area');
      }),
      switchMap(areas => {
        this.areaList = areas;
        return this.genreService.getAll('Genre');
      }),
      switchMap(genre => {
        this.genreList = genre;
        return this.cinemaService.getAll('cinema');
      }),
      switchMap(cinemas => {
        this.cinemaList = cinemas;
        return this.showService.getAll('show');
      }),
      switchMap(shows => {
        this.showList = shows;
        return this.movieService.getAll('movie');
      })
    ).subscribe(movies => {
      this.movieList = movies;
      console.log("Movie: ", this.movieList);
      
      this.mapCinemasToRegions();
      this.FindCinemaBySelectedArea();
      this.filterMoviesDisplayed();
    });
  }

    //Handle logout
  logout() {
    this.authService.logout();
    this.isAuthenticated = false;
    this.name = null;
    this.email = null;
    this.userRole = null;
  }
  
  mapCinemasToRegions() {
    this.cinemasByRegion = {};
    this.cinemaList.forEach(cinema => {
      // Denne finder det Area som matcher AreaID på de forskellige Cinemas
      const area = this.areaList.find(a => a.areaID === cinema.areaID);
      // Hvis den kan finde et Area der matcher, prøver den finde den Region der matcher det area
      if(area) {
        const region = this.regionList.find(r => r.regionID === area.regionID);
        //Hvis den kan finde finde en Region der matcher, laver den et array af cinema hvor RegionName er en key
        if(region) {
          //Hvis den ikke kan finde en Region der matcher, vil den bare lave et tomt CinamasByRegion
          if (!this.cinemasByRegion[region.regionName]) {
            this.cinemasByRegion[region.regionName] = [];
          }
          //Den bruger push, til at putte cinemas under deres Region. Så man har cinemasByRegion, som indeholder alle cinemas men under deres regions
          this.cinemasByRegion[region.regionName].push(cinema);
        }
      }
    });
    console.log("CinemasByRegion: ", this.cinemasByRegion);
  }

  toggleCinemaDisplay(): void {
      this.showCinemaList = !this.showCinemaList;
  }

  goToMoviePage(movieId: string) {
    this.router.navigate(['/moviepage', movieId]);
  }

  truncateTitle(title: string): string {
    return title.length > 26 ? title.substring(0, 26) + '...' : title;
  }

  onAreaChange(event: any): void {
    const selectedAreaID = event.target.value;
    const selectedArea = this.areaList.find(area => area.areaID == selectedAreaID);
    if (selectedArea && selectedAreaID !== undefined) {

      this.resetDropdowns();

      const areaData = { name: selectedArea.areaName, id: selectedAreaID};
      const areaDataString = JSON.stringify(areaData);
      localStorage.setItem('selectedArea', areaDataString);
      this.storageService.handleLocalStorage();
      this.FindCinemaBySelectedArea();
    }
  }

  onCinemaChange(event: Event): void {
    const element = event.target as HTMLSelectElement;
    if (element) {
      this.selectedCinemaId = element.value;
      localStorage.setItem('selectedCinemaId', this.selectedCinemaId);
      this.filterMoviesDisplayed();
    }
  }

  onMovieChange(event: Event): void {
    const element = event.target as HTMLSelectElement;
    if (element) {
      this.selectedMovieId = element.value;
      localStorage.setItem('selectedMovieId', this.selectedMovieId);
      this.filterMoviesDisplayed();
    }
  }

  onGenreChange(event: Event): void {
    const element = event.target as HTMLSelectElement;
    if (element) {
      this.selectedGenreId = element.value;
      localStorage.setItem('selectedGenreId', this.selectedGenreId);
      this.filterMoviesDisplayed();
    }
  }

  resetDropdowns(): void {
    const dropdownKeys = ['selectedCinemaId', 'selectedMovieId', 'selectedGenreId'];
  
    dropdownKeys.forEach(key => {
      this.selectedGenreId = '';
      this.selectedMovieId = '';
      this.selectedCinemaId = '';
      this.storageService.removeItem(key);
    });
    this.filterMoviesDisplayed();
  }
  
  
  FindCinemaBySelectedArea(): void {
    const selectedCityData = this.storageService.getItem('selectedArea');
    if (selectedCityData) {
      console.log(selectedCityData);
      
      const selectedArea = selectedCityData;
      
      const selectedAreaID = selectedArea.id;
      this.SelectedCityName = selectedArea.name;
  
      this.cinemaInSelectedArea = this.cinemaList.filter(cinema => {
        return String(cinema.areaID) === String(selectedAreaID); 
      });
    }
  }  

  filterMoviesDisplayed(): void {
    this.displayedMovies = [...this.movieList];
  
    if (this.selectedMovieId) {
        this.displayedMovies = this.displayedMovies.filter(movie => 
            movie.movieID?.toString() === this.selectedMovieId
        );
    }
    if (this.selectedGenreId) {
        this.displayedMovies = this.displayedMovies.filter(movie => 
            movie.genres && movie.genres.some(genre => genre.genreID?.toString() === this.selectedGenreId)
        );
    }
}

}
