import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Subscriber } from 'rxjs';
import { Costumer } from 'src/app/models/costumer/costumer';
import { CostumerService } from 'src/app/services/costumer.service';

@Component({
  selector: 'app-costumer',
  templateUrl: './costumer.component.html',
  styleUrls: ['./costumer.component.css']
})
export class CostumerComponent {

  costumer : Costumer = {};
  costumerList : Costumer[] = [];

  UserdetailForm: FormGroup = new FormGroup({
    name: new FormControl(''),
    descibtion: new FormControl(''),
    age: new FormControl(''),
  });

  public create(): void{
    console.log(this.UserdetailForm.value);
  }

  ngOnInit(): void {
    // this.costumerList = this.service.getAll();
    }
  }// end ngOnInit


//end Class