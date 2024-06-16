import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Cinema } from 'src/app/models/cinema/cinema';
import { Cinemahall } from 'src/app/models/cinemahall/cinemahall';
import { GenericService } from 'src/app/services/generic.services';

@Component({
  selector: 'app-admincinema-hall',
  templateUrl: './admincinemahall.component.html',
  styleUrls: ['../../../../static/assets/css/admin.css']
})
export class AdmincinemaHallComponent {
  cinemahallForm!: FormGroup;
  cinemahallList: Cinemahall[] =[];
  cinemaList: Cinema[] = [];
  currentCinemahall: Cinemahall | null = null;
  isEditMode: boolean = false;
  showForm: boolean = false;
  showList: boolean = true;

  initForm(cinemahall?: Cinemahall): void {
    this.cinemahallForm = new FormGroup({
      hallsID: new FormControl(cinemahall ? cinemahall.hallsID : ''),
      hallName: new FormControl(cinemahall ? cinemahall.hallName : '', Validators.required),
      cinemaID: new FormControl(cinemahall ? cinemahall.cinemaID : '', Validators.required),
    });
  }
  
  constructor(private cinemaService: GenericService<Cinema>, private cinemahallSerice: GenericService<Cinemahall>) {
    this.initForm();
  }
    ngOnInit() {
      this.fetchcinemahall();
      this.initForm();
    }
  
    fetchcinemahall() {
      this.cinemaService.getAll("cinema").subscribe(data => {
        this.cinemaList = data;
      });
  
      this.cinemahallSerice.getAll("cinemahall").subscribe(data => {
        this.cinemahallList = data;
        console.log("CinemaHall: ", this.cinemahallList[0].hallName);
        
      });
    }
  
    editCinemaHall(cinemahall: Cinemahall): void {
      this.currentCinemahall = cinemahall;
      console.log("CurrentHall: ", this.currentCinemahall);
      this.isEditMode = true;
      this.showForm = true;
      this.showList = false;
      this.initForm(cinemahall);
    }
  
    resetForm(): void {
      this.cinemahallForm.reset();
      this.showForm = false;
      this.showList = true;
      this.isEditMode = false;
      this.currentCinemahall = null;
    }
  
    toggleSave(): void {
      this.isEditMode = false;
      this.currentCinemahall = null;
      this.initForm();
      this.showForm = !this.showForm;
      this.showList = !this.showForm;
    }
  
    public save(): void {
      if (this.cinemahallForm.valid) {
        const formdata = this.cinemahallForm.value;
        console.log("Formdata: ", formdata);
        
        const cinemahallId = this.isEditMode ? formdata.hallsID : 0;
  
        const cinemahallData = {
          hallsID: cinemahallId,
          hallname: formdata.hallName,
          cinemaID: parseInt(formdata.cinemaID, 10),
        };

        if (this.isEditMode == true && cinemahallData.hallsID) {  
          this.cinemahallSerice.update('Cinemahall', cinemahallData, cinemahallData.hallsID).subscribe({
            next: (response) => {
              console.log('Cinemahall updated:', response);
              this.resetForm();
              this.fetchcinemahall();
            },
            error: (error) => {
              console.error('Failed to update Cinemahall:', error);
              alert(`Failed to update Cinemahall: ${error.error.title}`);
            }
          });
        }
           else
           {
            console.log("Cinemahall Create: ", cinemahallData);
            
              this.cinemaService.create('CinemaHall', cinemahallData).subscribe({
                next: (response) => {
                  console.log('cinemahall saved:', response);
                  this.cinemahallList.push(response);
                  this.cinemahallForm.reset();
                  this.showForm = false;
                  this.showList = true;
                },
                error: (error) => {
                  console.error('Failed to create cinemahall:', error);
                  alert(`Failed to create cinemahall: ${error.error.title}`);
                }
              });
      }
    }
   }
    
    deleteCinemaHall(cinemahall: Cinemahall) {
      if (cinemahall && cinemahall.hallsID !== undefined) {
          this.cinemaService.delete('cinemahall', cinemahall.hallsID).subscribe(() => {
              this.fetchcinemahall();
          });
      }
    }
  }
