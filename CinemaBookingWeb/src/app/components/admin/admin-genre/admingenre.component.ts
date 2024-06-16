import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Genre } from 'src/app/models/genre/genre';
import { Movie } from 'src/app/models/movie/movie';
import { GenericService } from 'src/app/services/generic.services';

@Component({
  selector: 'app-admingenre',
  templateUrl: './admingenre.component.html',
  styleUrls: ['../../../../static/assets/css/admin.css']
  
})
export class AdmingenreComponent {
  genreForm!: FormGroup;
  newGenreData: Genre = new Genre()
  movieList: Movie[] = [];
  genreList: Genre[] =[];
  currentGenre: Genre | null = null;
  isEditMode: boolean = false;
  showForm: boolean = false;
  showList: boolean = true;
  
  initForm(genre?: Genre): void {
    this.genreForm = new FormGroup({
      genreID: new FormControl(genre ? genre.genreID : ''),
      genreName: new FormControl(genre ? genre.genreName : '', Validators.required),
    });
  }
  
  constructor(private genreSerice: GenericService<Genre>) {
    this.initForm();
  }
    ngOnInit() {
      this.fetchGenre();
      this.initForm();
    }
  
    fetchGenre() {
      this.genreSerice.getAll("Genre").subscribe(data => {
        this.genreList = data;
      });
      console.log("GenreData: ", this.genreList);
      
    }
  
    editGenre(genre: Genre): void {
      this.currentGenre = genre;
      this.isEditMode = true;
      this.showForm = true;
      this.showList = false;
      this.initForm(genre);
    }
  
    resetForm(): void {
      this.genreForm.reset();
      this.showForm = false;
      this.showList = true;
      this.isEditMode = false;
      this.currentGenre = null;
    }
  
    toggleSave(): void {
      this.isEditMode = false;
      this.currentGenre = null;
      this.initForm();
      this.showForm = !this.showForm;
      this.showList = !this.showForm;
    }
  
    public save(): void {
      if (this.genreForm.valid) {
        const newGenreData = this.genreForm.value;
        const genreId = this.isEditMode ? newGenreData.genreID : 0;
        const genreData = {
          genreID: genreId, 
          genreName: newGenreData.genreName
        };
    
        if (this.isEditMode == true && newGenreData.genreID) {  
          this.genreSerice.update('Genre', genreData, genreData.genreID).subscribe({
            next: (response) => {
              console.log('Genre updated:', response);
              this.resetForm();
              this.fetchGenre();
            },
            error: (error) => {
              debugger
              console.error('Failed to update Genre:', error);
              alert(`Failed to update Genre: ${error.error.title}`);
            }
          });
        }
           else
           {
              this.genreSerice.create('Genre', genreData).subscribe({   
                next: (response) => {
                  console.log('Genre saved:', response);
                  this.genreList.push(response);
                  this.genreForm.reset();
                  this.showForm = false;
                  this.showList = true;
                },
                error: (error) => {
                  console.error('Failed to create genre:', error);
                  alert(`Failed to create genre: ${error.error.title}`);
                }
              });
      }
    }
   }
    
    deleteGenre(genre: Genre) {
      if (genre && genre.genreID !== undefined) {
          this.genreSerice.delete('genre', genre.genreID).subscribe(() => {
              this.fetchGenre();
          });
      }
    }
  }
