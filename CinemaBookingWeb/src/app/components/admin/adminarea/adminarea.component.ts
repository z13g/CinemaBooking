import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Area } from 'src/app/models/area/area';
import { Region } from 'src/app/models/region/region';
import { GenericService } from 'src/app/services/generic.services';

@Component({
  selector: 'app-adminarea',
  templateUrl: './adminarea.component.html',
  styleUrls: ['../../../../static/assets/css/admin.css']
})
export class AdminareaComponent {
  areaForm!: FormGroup;
  regionList: Region[] =[];
  areaList: Area[] = [];
  currentArea: Area | null = null;
  isEditMode: boolean = false;
  showForm: boolean = false;
  showList: boolean = true;

  initForm(area?: Area): void {
    this.areaForm = new FormGroup({
      areaID: new FormControl(area ? area.areaID : ''),
      areaName: new FormControl(area ? area.areaName : '', Validators.required),
      regionID: new FormControl(area ? area.regionID : '', Validators.required),
    });
  }
  
  constructor(private regionService: GenericService<Region>, private areaService: GenericService<Area>) {
    this.initForm();
  }
    ngOnInit() {
      this.fetchArea();
      this.initForm();
    }
  
    fetchArea() {
      this.areaService.getAll("area").subscribe(data => {
        this.areaList = data;
      });
  
      this.regionService.getAll("region").subscribe(data => {
        this.regionList = data;
        console.log("RegionData: ", this.regionList);
        
      });
    }
  
    editArea(area: Area): void {
      this.currentArea = area;
      console.log("CurrentArea: ", this.currentArea);
      this.isEditMode = true;
      this.showForm = true;
      this.showList = false;
      this.initForm(area);
    }
  
    resetForm(): void {
      this.areaForm.reset();
      this.showForm = false;
      this.showList = true;
      this.isEditMode = false;
      this.currentArea = null;
    }
  
    toggleSave(): void {
      this.isEditMode = false;
      this.currentArea = null;
      this.initForm();
      this.showForm = !this.showForm;
      this.showList = !this.showForm;
    }
  
    public save(): void {
      if (this.areaForm.valid) {
        const formdata = this.areaForm.value;
        console.log("Formdata: ", formdata);
        
        const areaid = this.isEditMode ? formdata.areaID : 0;
  
        const areaData = {
          areaID: areaid,
          areaName: formdata.areaName,
          regionID: parseInt(formdata.regionID, 10),
        };

        if (this.isEditMode == true && areaData.areaID) {  
          this.areaService.update('area', areaData, areaData.areaID).subscribe({
            next: (response) => {
              console.log('area updated:', response);
              this.resetForm();
              this.fetchArea();
            },
            error: (error) => {
              console.error('Failed to update area:', error);
              alert(`Failed to update area: ${error.error.title}`);
            }
          });
        }
           else
           {
            console.log("area Create: ", areaData);
            
              this.areaService.create('area', areaData).subscribe({
                next: (response) => {
                  console.log('Area saved:', response);
                  this.areaList.push(response);
                  this.areaForm.reset();
                  this.showForm = false;
                  this.showList = true;
                },
                error: (error) => {
                  console.error('Failed to create area:', error);
                  alert(`Failed to create area: ${error.error.title}`);
                }
              });
      }
    }
   }
    
    deleteArea(area: Area) {
      if (area && area.areaID !== undefined) {
          this.areaService.delete('area', area.areaID).subscribe(() => {
              this.fetchArea();
          });
      }
    }
  }
