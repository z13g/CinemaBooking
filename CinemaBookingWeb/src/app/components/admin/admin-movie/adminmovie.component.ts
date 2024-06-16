import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Genre } from 'src/app/models/genre/genre';
import { Movie } from 'src/app/models/movie/movie';
import { GenericService } from 'src/app/services/generic.services';

@Component({
  selector: 'app-admin-movie',
  templateUrl: './adminmovie.component.html',
  styleUrls: ['../../../../static/assets/css/admin.css']
  
})
export class AdminMovieComponent {
dropdownOpen: Boolean = false;
movieForm!: FormGroup;
movieList: Movie[] = [];
genreList: Genre[] =[];
currentMovie: Movie | null = null;
isEditMode: boolean = false;
showForm: boolean = false;
showList: boolean = true;

initForm(movie?: Movie): void {
  this.movieForm = new FormGroup({
    movieID: new FormControl(movie ? movie.movieID : ''),
    title: new FormControl(movie ? movie.title : '', Validators.required),
    duration: new FormControl(movie ? movie.duration : '', Validators.required),
    director: new FormControl(movie ? movie.director : '', Validators.required),
    movieLink: new FormControl(movie ? movie.movieLink : '', Validators.required),
    trailerLink: new FormControl(movie ? movie.trailerLink : '', Validators.required),
    genreIDs: this.fb.array([])
  });
}

constructor(private fb: FormBuilder, private movieService: GenericService<Movie>, private genreSerice: GenericService<Genre>) {
  this.initForm();
}
  ngOnInit() {
    this.fetchMovies();
    this.initForm();
  }

  fetchMovies() {
    this.movieService.getAll("movie").subscribe(data => {
      this.movieList = data;
    });

    this.genreSerice.getAll("Genre").subscribe(data => {
      this.genreList = data;
      console.log(this.genreList);
      
    });
  }

  editMovie(movie: Movie): void {
    this.currentMovie = movie;
    this.isEditMode = true;
    this.showForm = true;
    this.showList = false;
    this.initForm(movie);
  }

  resetForm(): void {
    this.movieForm.reset();
    this.showForm = false;
    this.showList = true;
    this.isEditMode = false;
    this.currentMovie = null;
  }

  toggleSave(): void {
    this.isEditMode = false;
    this.currentMovie = null;
    this.initForm();
    this.showForm = !this.showForm;
    this.showList = !this.showForm;
  }

  toggleDropdown() {
    this.dropdownOpen = !this.dropdownOpen;   
  }

  onGenreChange(event: Event) {
    const selectElement = event.target as HTMLSelectElement;
    const genreArray: FormArray = this.movieForm.get('genreIDs') as FormArray;
    // Clear the current array
    genreArray.clear();

    // Create a new control for each selected option
    Array.from(selectElement.selectedOptions).forEach((option) => {
      genreArray.push(new FormControl(option.value));
    });
  }

  public save(): void {
    if (this.movieForm.valid) {
      const formdata = this.movieForm.value;

      const selectedGenres = formdata.genreIDs.map((genreID: string) => 
        this.genreList.find(genre => genre.genreID?.toString() === genreID)
      ).filter((genre: Genre | undefined) => genre !== undefined);

      const movieId = this.isEditMode ? formdata.movieID : 0;

      const movieData = {
        movieId: movieId,
        title: formdata.title,
        duration: parseInt(formdata.duration, 10),
        director: formdata.director,
        movieLink: formdata.movieLink,
        trailerLink: formdata.trailerLink,
        genres: selectedGenres.map((genre: Genre) => ({
          genreID: genre.genreID,
          genreName: genre.genreName
        }))
      };
      
      if (this.isEditMode == true && movieData.movieId) {  
        this.movieService.update('movie', movieData, movieData.movieId).subscribe({
          next: (response) => {
            console.log('Movie updated:', response);
            this.resetForm();
            this.fetchMovies();
          },
          error: (error) => {
            console.error('Failed to update movie:', error);
            alert(`Failed to update movie: ${error.error.title}`);
          }
        });
      }
         else
         {
          if (selectedGenres) {
            this.movieService.create('movie/Complex', movieData).subscribe({
              next: (response) => {
                console.log('Complex movie saved:', response);
                this.movieList.push(response);
                this.movieForm.reset();
                this.showForm = false;
                this.showList = true;
              },
              error: (error) => {
                console.error('Failed to create movie:', error);
                alert(`Failed to create movie: ${error.error.title}`);
              }
            });
        } else {
          alert(`It's not valid data in the form`);
        }
    }
  }
 }
  
  deleteMovie(movie: Movie) {
    if (movie && movie.movieID !== undefined) {
        this.movieService.delete('movie', movie.movieID).subscribe(() => {
            this.fetchMovies();
        });
    }
  }
}