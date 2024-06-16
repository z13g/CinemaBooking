import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Area } from 'src/app/models/area/area';
import { Region } from 'src/app/models/region/region';
import { GenericService } from 'src/app/services/generic.services';

@Component({
  selector: 'app-adminregion',
  templateUrl: './adminregion.component.html',
  styleUrls: ['../../../../static/assets/css/admin.css']
})
export class AdminregionComponent {
  regionForm!: FormGroup;
  regionList: Region[] = [];
  areaList: Area[] =[];
  currentRegion: Region | null = null;
  isEditMode: boolean = false;
  showForm: boolean = false;
  showList: boolean = true;
  
  initForm(region?: Region): void {
    this.regionForm = new FormGroup({
      regionID: new FormControl(region ? region.regionID : ''),
      regionName: new FormControl(region ? region.regionName : '', Validators.required),
    });
  }
  
  constructor(private regionService: GenericService<Region>, private AreaSerice: GenericService<Area>) {
    this.initForm();
  }
    ngOnInit() {
      this.fetchRegion();
      this.initForm();
    }
  
    fetchRegion() {
      this.regionService.getAll("region").subscribe(data => {
        this.regionList = data;
      });
  
      this.AreaSerice.getAll("area").subscribe(data => {
        this.areaList = data;
        console.log(this.areaList);
        
      });
    }
  
    editRegion(region: Region): void {
      this.currentRegion = region;
      this.isEditMode = true;
      this.showForm = true;
      this.showList = false;
      this.initForm(region);
    }
  
    resetForm(): void {
      this.regionForm.reset();
      this.showForm = false;
      this.showList = true;
      this.isEditMode = false;
      this.currentRegion = null;
    }
  
    toggleSave(): void {
      this.isEditMode = false;
      this.currentRegion = null;
      this.initForm();
      this.showForm = !this.showForm;
      this.showList = !this.showForm;
    }
  
    public save(): void {
      if (this.regionForm.valid) {
        const formdata = this.regionForm.value;
        const regionID = this.isEditMode ? formdata.regionID : 0;
  
        const regionData = {
          regionID: regionID,
          regionName: formdata.regionName,
        };
        
        if (this.isEditMode == true && regionData.regionID) {  
          this.regionService.update('region', regionData, regionData.regionID).subscribe({
            next: (response) => {
              console.log('region updated:', response);
              this.resetForm();
              this.fetchRegion();
            },
            error: (error) => {
              console.error('Failed to update region:', error);
              alert(`Failed to update region: ${error.error.title}`);
            }
          });
        }
           else
           {
              this.regionService.create('region', regionData).subscribe({
                next: (response) => {
                  console.log('region saved:', response);
                  this.regionList.push(response);
                  this.regionForm.reset();
                  this.showForm = false;
                  this.showList = true;
                },
                error: (error) => {
                  console.error('Failed to create region:', error);
                  alert(`Failed to create region: ${error.error.title}`);
                }
              });
      }
    }
   }
    
    deleteRegion(region: Region) {
      if (region && region.regionID !== undefined) {
          this.regionService.delete('region', region.regionID).subscribe(() => {
              this.fetchRegion();
          });
      }
    }
  }
