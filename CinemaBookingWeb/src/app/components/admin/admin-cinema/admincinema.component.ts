import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Area } from 'src/app/models/area/area';
import { Cinema } from 'src/app/models/cinema/cinema';
import { GenericService } from 'src/app/services/generic.services';

@Component({
  selector: 'app-admin-cinema',
  templateUrl: './admincinema.component.html',
  styleUrls: ['../../../../static/assets/css/admin.css']
})
export class AdminCinemaComponent {
  cinemaForm!: FormGroup;
  cinemaList: Cinema[] =[];
  areaList: Area[] = [];
  currentCinema: Cinema | null = null;
  isEditMode: boolean = false;
  showForm: boolean = false;
  showList: boolean = true;

  initForm(cinema?: Cinema): void {
    this.cinemaForm = new FormGroup({
      cinemaID: new FormControl(cinema ? cinema.cinemaID : ''),
      name: new FormControl(cinema ? cinema.name : '', Validators.required),
      location: new FormControl(cinema ? cinema.location : '', Validators.required),
      numberOfHalls: new FormControl(cinema ? cinema.numberOfHalls : '', Validators.required),
      areaID: new FormControl(cinema ? cinema.areaID : '', Validators.required),
    });
  }
  
  constructor(private cinemaService: GenericService<Cinema>, private areaSerice: GenericService<Area>) {
    this.initForm();
  }
    ngOnInit() {
      this.fetchcinema();
      this.initForm();
    }
  
    fetchcinema() {
      this.cinemaService.getAll("cinema").subscribe(data => {
        this.cinemaList = data;
      });
  
      this.areaSerice.getAll("area").subscribe(data => {
        this.areaList = data;
        console.log("AreaData: ", this.areaList);
        
      });
    }
  
    editCinema(cinema: Cinema): void {
      this.currentCinema = cinema;
      this.isEditMode = true;
      this.showForm = true;
      this.showList = false;
      this.initForm(cinema);
    }
  
    resetForm(): void {
      this.cinemaForm.reset();
      this.showForm = false;
      this.showList = true;
      this.isEditMode = false;
      this.currentCinema = null;
    }
  
    toggleSave(): void {
      this.isEditMode = false;
      this.currentCinema = null;
      this.initForm();
      this.showForm = !this.showForm;
      this.showList = !this.showForm;
    }
  
    public save(): void {
      if (this.cinemaForm.valid) {
        const formdata = this.cinemaForm.value;
        const cinemaId = this.isEditMode ? formdata.cinemaID : 0;
  
        const cinemaData = {
          cinemaID: cinemaId,
          name: formdata.name,
          location: formdata.location,
          numberOfHalls: parseInt(formdata.numberOfHalls, 10),
          areaID: parseInt(formdata.areaID, 10),
        };
        
        if (this.isEditMode == true && cinemaData.cinemaID) {  
          this.cinemaService.update('Cinema', cinemaData, cinemaData.cinemaID).subscribe({
            next: (response) => {
              console.log('Cinema updated:', response);
              this.resetForm();
              this.fetchcinema();
            },
            error: (error) => {
              console.error('Failed to update Cinema:', error);
              alert(`Failed to update Cinema: ${error.error.title}`);
            }
          });
        }
           else
           {
            console.log("CinemaCreate: ", cinemaData);
            
              this.cinemaService.create('Cinema', cinemaData).subscribe({
                next: (response) => {
                  console.log('cinema saved:', response);
                  this.cinemaList.push(response);
                  this.cinemaForm.reset();
                  this.showForm = false;
                  this.showList = true;
                },
                error: (error) => {
                  console.error('Failed to create cinema:', error);
                  alert(`Failed to create cinema: ${error.error.title}`);
                }
              });
      }
    }
   }
    
    deleteCinema(cinema: Cinema) {
      if (cinema && cinema.cinemaID !== undefined) {
          this.cinemaService.delete('cinema', cinema.cinemaID).subscribe(() => {
              this.fetchcinema();
          });
      }
    }
  }