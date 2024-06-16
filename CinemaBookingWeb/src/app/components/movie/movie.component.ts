import { Component, Input, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Movie } from 'src/app/models/movie/movie';
import { GenericService } from 'src/app/services/generic.services';

@Component({
  selector: 'app-movie',
  templateUrl: './movie.component.html',
  styleUrls: ['./movie.component.css']
})
export class MovieComponent implements OnInit {
  @Input() movie!: Movie; // This is the input, only if you need to display or edit a single movie
  movieList: Movie[] = []; // This should hold the array of movies

  movieForm: FormGroup = new FormGroup({
    movieID: new FormControl(''),
    title: new FormControl(''),
    duration: new FormControl(''),
    director: new FormControl(''),
    genre: new FormGroup({
      genreID: new FormControl(''),
      genreName: new FormControl(''),
    })
  });

  constructor(private genericService: GenericService<Movie>) {}

  ngOnInit() {
    this.genericService.getAll("movies").subscribe(data => {
      this.movieList = data;
      console.log("Movies fetched:", this.movieList);
    });
  }

  public create(): void {
    if (this.movieForm.valid) {
      this.genericService.create('movies', this.movieForm.value).subscribe({
        next: (response) => { 
          console.log('Movie saved:', response);
          this.movieList.push(response); // Assuming response is the newly created movie
          this.movieForm.reset();
        }
      });
    }
  }
}
