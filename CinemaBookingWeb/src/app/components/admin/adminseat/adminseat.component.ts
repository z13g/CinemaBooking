import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Cinemahall } from 'src/app/models/cinemahall/cinemahall';
import { Seat } from 'src/app/models/seat/seat';
import { GenericService } from 'src/app/services/generic.services';

@Component({
  selector: 'app-adminseat',
  templateUrl: './adminseat.component.html',
  styleUrls: ['../../../../static/assets/css/admin.css']
})
export class AdminseatComponent {
  seatForm!: FormGroup;
  seatList: Seat[] =[];
  cinemahallList: Cinemahall[] = [];
  currentSeat: Seat | null = null;
  isEditMode: boolean = false;
  showForm: boolean = false;
  showList: boolean = true;

  initForm(seat?: Seat): void {
    this.seatForm = new FormGroup({
      seatID: new FormControl(seat ? seat.seatID : ''),
      hallID: new FormControl(seat ? seat.hallID : '', Validators.required),
      seatNumber: new FormControl(seat ? seat.seatNumber : '', Validators.required),
      seatRow: new FormControl({ value: seat ? seat.seatRow : '', disabled: !this.isEditMode }, this.isEditMode ? Validators.required: null),
    });
  }
  
  constructor(private seatService: GenericService<Seat>, private cinemahallService: GenericService<Cinemahall>) {
    this.initForm();
  }
    ngOnInit() {
      this.fetchSeat();
      this.initForm();
    }
  
    fetchSeat() {
      this.seatService.getAll("seat").subscribe(data => {
        this.seatList = data;
      });
  
      this.cinemahallService.getAll("cinemahall").subscribe(data => {
        this.cinemahallList = data;
        console.log("CinemahallData: ", this.cinemahallList);
        
      });
    }
  
    editSeat(seat: Seat): void {
      this.currentSeat = seat;
      console.log("CurrentSeat: ", this.currentSeat);
      this.isEditMode = true;
      this.showForm = true;
      this.showList = false;
      this.initForm(seat);
    }
  
    resetForm(): void {
      this.seatForm.reset();
      this.showForm = false;
      this.showList = true;
      this.isEditMode = false;
      this.currentSeat = null;
    }
  
    toggleSave(): void {
      this.isEditMode = false;
      this.currentSeat = null;
      this.initForm();
      this.showForm = !this.showForm;
      this.showList = !this.showForm;
    }
  
    public save(): void {
      if (this.seatForm.valid) {
        const formData = this.seatForm.value;
        console.log("FormData: ", formData);
    
        const seatID = this.isEditMode ? formData.seatID : 0;
    
        if (this.isEditMode && seatID) {
          // Updating a single seat
          const seatData = {
            seatID: seatID,
            hallID: formData.hallID,
            seatNumber: parseInt(formData.seatNumber, 10),
            seatRow: formData.seatRow
          };
    
          this.seatService.update('seat', seatData, seatID).subscribe({
            next: (response) => {
              console.log('Seat updated:', response);
              this.resetForm();
              this.fetchSeat();
            },
            error: (error) => {
              console.error('Failed to update seat:', error);
              alert(`Failed to update seat: ${error.error.title}`);
            }
          });
        } else {

          let totalSeats = parseInt(formData.seatNumber, 10);
          let seatRow = "A";
          let seatNumber = 1;
          let listTemp: Seat[] = [];

          for (let i = 0; i < totalSeats; i++) {
            const seatData: Seat = {
              seatID: 0,
              hallID: parseInt(formData.hallID, 10),
              seatNumber: seatNumber,
              seatRow: seatRow
            };
    
            seatNumber++;
            if (seatNumber > 20) {
              seatNumber = 1;
              seatRow = String.fromCharCode(seatRow.charCodeAt(0) + 1);
            }

            listTemp.push(seatData);
          }
          
          this.seatService.createBulk('seat', listTemp).subscribe({
            next: (response) => {
              this.seatList.push(response);
              this.resetForm();
              this.fetchSeat();
            },
            error: (error) => {
              console.error('Failed to create seat:', error);
              alert(`Failed to create seat: ${error.error.title}`);
            }
          });
        }
      } else {
        alert('Form is not valid, please review the inputs.');
      }
    }
    
    deleteSeat(seat: Seat) {
      if (seat && seat.seatID !== undefined) {
          this.seatService.delete('seat', seat.seatID).subscribe(() => {
              this.fetchSeat();
          });
      }
    }
  }

