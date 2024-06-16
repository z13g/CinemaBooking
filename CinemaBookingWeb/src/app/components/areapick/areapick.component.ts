import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Area } from 'src/app/models/area/area';
import { AreaService } from 'src/app/services/area.service';
import { GenericService } from 'src/app/services/generic.services';
import { LocalStorageGeneric } from 'src/app/services/generic.services';

@Component({
  selector: 'app-areapick',
  templateUrl: './areapick.component.html',
  styleUrls: ['./areapick.component.css']
})
export class AreapickComponent {
  AreaList : Area[] = [];

  constructor(private router: Router, private service: GenericService<Area>, private storageService: LocalStorageGeneric) {}

  ngOnInit() {
    this.service.getAll("Area").subscribe(data => {
      this.AreaList = data;
      console.log("Data: ", data);
      console.log("AreaList: ", this.AreaList);
    })
  }

  onAreaButtonClick(cityName?: string, cityID? : number) {
    if (cityName && cityID !== undefined) {
      const cityData = {
        name: cityName,
        id: cityID
      };
  
      const cityDataString = JSON.stringify(cityData);
      localStorage.setItem('selectedArea', cityDataString);
      this.storageService.handleLocalStorage();

      this.router.navigateByUrl("/");
    }
  }  
}
