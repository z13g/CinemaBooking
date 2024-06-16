import * as moment from 'moment-timezone';
import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Cinemahall } from 'src/app/models/cinemahall/cinemahall';
import { Movie } from 'src/app/models/movie/movie';
import { Show } from 'src/app/models/show/show';
import { GenericService } from 'src/app/services/generic.services';

@Component({
  selector: 'app-adminshow',
  templateUrl: './adminshow.component.html',
  styleUrls: ['../../../../static/assets/css/admin.css']
})
export class AdminshowComponent {
    showsForm!: FormGroup;
    showsList: Show[] =[];
    hallList: Cinemahall[] = [];
    movieList: Movie[] = [];
    currentshow: Show | null = null;
    isEditMode: boolean = false;
    showForm: boolean = false;
    showList: boolean = true;
    datetime = new FormControl(new Date().toISOString());
  
    initForm(show?: Show): void {
      this.showsForm = new FormGroup({
        showID: new FormControl(show ? show.showID : ''),
        hallID: new FormControl(show ? show.hallID : '', Validators.required),
        movieID: new FormControl(show ? show.movieID : '', Validators.required),
        price: new FormControl(show ? show.price : '', Validators.required),
        showdatetime: new FormControl(show ? show.showDateTime : '', Validators.required),
      });
    }
    
    constructor(private showService: GenericService<Show>, private cinemahallSerice: GenericService<Cinemahall>, private movieService: GenericService<Movie>) {
      this.initForm();
    }
      ngOnInit() {
        this.fetchshow();
        this.initForm();
      }
    
      fetchshow() {
        this.showService.getAll("show").subscribe(data => {
          console.log("Raw data from server: ", data); 
          this.showsList = data;
          console.log("First item in showlist: ", this.showsList[0]); 
          console.log("showdatetime property: ", this.showsList[0].showDateTime); 
        });
    
        this.cinemahallSerice.getAll("cinemahall").subscribe(data => {
          this.hallList = data;
          console.log("hallList: ", this.hallList);
        });
  
        this.movieService.getAll("movie").subscribe(data => {
          this.movieList = data;
          console.log("Movielist: ", this.movieList);
        });
      }
      onDateTimeChange(event: any) {
        const utcDate = new Date(Date.UTC(event.value.getFullYear(), event.value.getMonth(), event.value.getDate(), event.value.getHours(), event.value.getMinutes()));
        this.datetime.setValue(utcDate.toISOString());
      }
    
      editShow(show: Show): void {
        this.currentshow = show;
        console.log("CurrentShow: ", this.currentshow);
        this.isEditMode = true;
        this.showForm = true;
        this.showList = false;
        this.initForm(show);
      }
    
      resetForm(): void {
        this.showsForm.reset();
        this.showForm = false;
        this.showList = true;
        this.isEditMode = false;
        this.currentshow = null;
      }
    
      toggleSave(): void {
        this.isEditMode = false;
        this.currentshow = null;
        this.initForm();
        this.showForm = !this.showForm;
        this.showList = !this.showForm;
      }
    
      public save(): void {
        if (this.showsForm.valid) {
          const formdata = this.showsForm.value;
          console.log("Formdata: ", formdata);
  
          const tz = 'Europe/Paris';
          const localDateTime = moment(formdata.showdatetime).tz(tz, true);
          
          const showsid = this.isEditMode ? formdata.showID : 0;
    
          const showData = {
            showID: showsid,
            hallID: parseInt(formdata.hallID, 10),
            movieID: parseInt(formdata.movieID, 10),
            price: parseInt(formdata.price, 10),
            showdatetime: localDateTime.format()
          };
  
          if (this.isEditMode == true && showData.showID) {  
            this.showService.update('show', showData, showData.showID).subscribe({
              next: (response) => {
                console.log('show updated:', response);
                this.resetForm();
                this.fetchshow();
              },
              error: (error) => {
                console.error('Failed to update Show:', error);
                alert(`Failed to update Show: ${error.error.title}`);
              }
            });
          }
             else
             {
              console.log("Show Create: ", showData);
              
                this.showService.create('Show', showData).subscribe({
                  next: (response) => {
                    console.log('Show saved:', response);
                    this.showsList.push(response);
                    this.showsForm.reset();
                    this.showForm = false;
                    this.showList = true;
                  },
                  error: (error) => {
                    console.error('Failed to create Show:', error);
                    alert(`Failed to create Show: ${error.error.title}`);
                  }
                });
        }
      }
     }
      
      deleteShow(show: Show) {
        if (show && show.showID !== undefined) {
            this.showService.delete('show', show.showID).subscribe(() => {
                this.fetchshow();
            });
        }
      }
    }
  